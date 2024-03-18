using EvilDICOM.Core;
using EvilDICOM.Core.Extensions;
using EvilDICOM.Network.PDUs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Hatches;
using ScottPlot.Plottables;
using System;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Security.Cryptography;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EvilDICOM.RT.Data.DVH;
using static System.Reflection.Metadata.BlobBuilder;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;
using System.Diagnostics.Metrics;
using OpenTK.Audio.OpenAL;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using Microsoft.VisualBasic.Devices;
using ScottPlot.Panels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;
using OpenTK;

namespace MPPG
{
    public partial class MainForm : Form
    {
        private AscReader.MeasurementData? asc;
        private DcmReader.CalculatedData? dcm;

        public MainForm()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 3 )
            {
                OpenCalculatedFile(args[1]);
                OpenMeasuredFile(args[2]);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClearGraphs()
        {
            drawingPanel.Controls.Clear();
        }

        private void OnOpenCalculatedFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Calculated DICOM Files (*.dcm)|*.dcm";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenCalculatedFile(dlg.FileName);
            };
        }

        private void OpenCalculatedFile(string filePath)
        {
            ClearGraphs();
            Cursor = Cursors.WaitCursor;
            txtDCMFile.Text = Path.GetFileName(filePath);
            var dcmFile = DICOMObject.Read(filePath);
            var dcmSel = dcmFile.GetSelector();
            if (dcmSel.Modality.Data == "RTDOSE")
            {
                var pixelStream = dcmFile.GetPixelStream();
                dcm = DcmReader.Read(dcmSel, pixelStream, filePath);
                if (dcm != null)
                {
                    txtDCMManufacturer.Text = dcm.Value.Manufacturer;
                    var offset = dcm.Value.Offset.Value;
                    txtDCMOffset.Text = string.Format("x: {0:F3}, y: {1:F3}, z: {2:F3}", offset.X, offset.Y, offset.Z);
                    txtDCMStatus.Text = "Ready";
                }
                else
                {
                    txtDCMStatus.Text = "Failed to load DICOM file";
                    txtDCMManufacturer.Text = "";
                    txtDCMOffset.Text = "";
                    dcm = null;
                }
            }
            else
            {
                txtDCMStatus.Text = "Not a DICOM-RT DOSE file";
                txtDCMManufacturer.Text = "";
                txtDCMOffset.Text = "";
                dcm = null;
            }

            Cursor = Cursors.Default;
            runToolStripMenuItem.Enabled = dcm != null && asc != null;
        }

        private void OnOpenMeasuredFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Measured Files (*.asc)|*.asc";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenMeasuredFile(dlg.FileName);
            }
        }

        private void OpenMeasuredFile(string filePath)
        {
            ClearGraphs();
            txtASCFile.Text = Path.GetFileName(filePath);
            asc = AscReader.Read(filePath);
            if (asc != null)
            {
                var numI = 0; // inline
                var numC = 0; // crossline
                var numP = 0; // pdd
                var numO = 0; // other
                foreach (var measurement in asc.Value.Data)
                {
                    switch (measurement.AxisType)
                    {
                        case 'X':
                            numC++;
                            break;
                        case 'Y':
                            numI++;
                            break;
                        case 'Z':
                            numP++;
                            break;
                        default:
                            numO++;
                            break;
                    }
                }
                txtASCStatus.Text = string.Format("{0} inline, {1} crossline, {2} depth-dose, and {3} other profiles", numI, numC, numP, numO);
                txtASCMeasurements.Text = asc.Value.NumberOfMeasurements.ToString();
                txtASCScanner.Text = asc.Value.ScannerSystem;
            }
            else
            {
                txtASCStatus.Text = "Failed to load";
                txtASCMeasurements.Text = "";
                txtASCScanner.Text = "";
                asc = null;
            };

            runToolStripMenuItem.Enabled = dcm != null && asc != null;
        }

        private void OnRun(object sender, EventArgs e)
        {
            if (asc == null || dcm == null)
                return;

            Cursor = Cursors.WaitCursor;
            ClearGraphs();

            var measurement = asc.Value;
            PrepareData(measurement);

            // TODO: Loop
            Plot(measurement.Data[0]);
        }

        private void Plot(AscReader.MeasurementStruct measurement)
        {
            var fileName = Path.GetFileName(txtDCMFile.Text);
            
            var normText = "";
            var horAxisText = "";
            var titleText = "";
            var normLoc = "dmax";

            // Use user preferences to determine normalization location
            switch (measurement.AxisType)
            {
                case 'X':
                    // The x position of the measurement changes, so the profile must have some crossline profile component.
                    // Normalization based on crossline normalization preferences:
                    /*if get(posButProf, 'Value')
                        normLoc = sscanf(get(CrosslinePosProf, 'String'), '%f');
                    else
                        normLoc = 'dmax';
                    end
                    norm_dim = 'X';*/
                    horAxisText = "Crossline Position (X) [cm]";
                    titleText = string.Format("Crossline Profiles at Depth(Y) = {0:F2} cm, Inline Position(Z) = {1:F2} cm",
                        measurement.BeamData.Z[0],
                        measurement.BeamData.Y[0]);
                    break;
                case 'Y':
                    // The depth of the measurement changes, so the profile must have some depth dose component.
                    // Normalization based on PDD normalization preferences:
                    /*if get(depthButPDD, 'Value')
                        normLoc = sscanf(get(DepthPosPDD, 'String'), '%f');
                    else
                        normLoc = 'dmax';
                    end
                    norm_dim = 'Y';*/
                    horAxisText = "Depth (Y) [cm]";
                    titleText = string.Format("Inline Profiles at Depth(Y) = {0:F2} cm, Crossline Position(X) = {1:F2} cm",
                        measurement.BeamData.Z[0],
                        measurement.BeamData.X[0]);
                    break;
                case 'Z':
                    // The y position of the measurement changes, so the profile must have some inline profile component.
                    // Normalization based on inline normalization preferences:
                    /*if get(posButProf, 'Value')
                        normLoc = sscanf(get(InlinePosProf, 'String'), '%f');
                    else
                        normLoc = 'dmax';
                    end
                    norm_dim = 'Z';*/
                    horAxisText = "Inline Position (Z) [cm]";
                    titleText = string.Format("Depth-Dose Profiles at Crossline Position (X) = {0:F2} cm, Inline Position (Z) = {1:F2} cm",
                        measurement.BeamData.X[0],
                        measurement.BeamData.Y[0]);
                    break;
                default: 
                    // This is the case when axis type is not set - meaning it's diagonal
                    // WARNING: Not tested! Test data is needed
                    normText = string.Format("Diagonal Profiles from ({0:F2}, {1:F2}, {2:F2}) to ({3:F2}, {4:F2}, {5:F2})",
                        measurement.BeamData.X[0],
                        measurement.BeamData.Z[0],
                        measurement.BeamData.Y[0],
                        measurement.BeamData.X[^1],
                        measurement.BeamData.Z[^1],
                        measurement.BeamData.Y[^1]);
                        break;
            }

            if (normLoc.Equals("dmax"))
                normText = "Profiles normalized at maximum dose location for each profile";
            else
                normText = string.Format("Profiles normalized at {0} = {1:F2} cm", measurement.AxisType, normLoc);

            txtTitle.Text = string.Format("{0}{1}{2}{1}{3}", fileName, Environment.NewLine, titleText, normText);

            // Relative Dose plot
            var relDosePlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill
            };
            drawingPanel.Controls.Add(relDosePlot, 0, 0);
            relDosePlot.Plot.ShowLegend(Alignment.UpperRight);
            var graph = relDosePlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "Measured";
            graph.Color = ScottPlot.Color.FromARGB(0xff0000ff);
            graph.MarkerStyle = MarkerStyle.None;

            graph = relDosePlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "TPS";
            graph.Color = ScottPlot.Color.FromARGB(0xffff0000);
            graph.LineStyle.Pattern = LinePattern.Dashed;
            graph.MarkerStyle = MarkerStyle.None;

            graph = relDosePlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "Threshold";
            graph.Color = ScottPlot.Color.FromARGB(0xffff00ff);
            graph.LineStyle.Pattern = LinePattern.Dotted;
            graph.MarkerStyle = MarkerStyle.None;

            relDosePlot.Plot.XLabel(horAxisText);
            relDosePlot.Plot.YLabel("Relative Dose");
            relDosePlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            relDosePlot.Refresh();

            var tpsDoseLabel = new System.Windows.Forms.Label()
            {
                Text = string.Format("TPS dose at normalization point is {0:F3} Gy", 1),
                Location = new Point(40, 5),
                BackColor = System.Drawing.Color.White,
                AutoSize = true
            };
            relDosePlot.Controls.Add(tpsDoseLabel);
            tpsDoseLabel.BringToFront();

            // Gamma plot
            var gamaPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill,
            };
            drawingPanel.Controls.Add(gamaPlot, 0, 1);
            graph = gamaPlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "Threshold";
            graph.Color = ScottPlot.Color.FromARGB(0xff0000ff);
            graph.MarkerStyle = MarkerStyle.None;

            gamaPlot.Plot.XLabel(horAxisText);
            gamaPlot.Plot.YLabel("Gamma");
            gamaPlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            gamaPlot.Refresh();

            var passRateLabel = new System.Windows.Forms.Label()
            {
                Text = string.Format("Pass rate: {0:F1}%", 100),
                Location = new Point(40, 5),
                AutoSize = true
            };
            gamaPlot.Controls.Add(passRateLabel);
            passRateLabel.BringToFront();

            // AU plot
            var auPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill,
            };
            drawingPanel.Controls.Add(auPlot, 0, 2);
            auPlot.Plot.ShowLegend(Alignment.UpperRight);
            graph = auPlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "distMinGam";
            graph.Color = ScottPlot.Color.FromARGB(0xff0000ff);
            graph.MarkerStyle = MarkerStyle.None;

            graph = auPlot.Plot.Add.Scatter(measurement.BeamData.X, measurement.BeamData.V);
            graph.Label = "doseMinGam";
            graph.LineStyle.Pattern = LinePattern.Dashed;
            graph.Color = ScottPlot.Color.FromARGB(0xffff0000);
            graph.MarkerStyle = MarkerStyle.None;

            auPlot.Plot.XLabel(horAxisText);
            auPlot.Plot.YLabel("AU");
            auPlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            auPlot.Refresh();
            Cursor = Cursors.Default;
        }

        private static float FindSpacing(List<float> data)
        {
            var maxSpace = 0.0f;
            var minSpace = 1000.0f;
            for (int n = 1; n < data.Count; n++)
            {
                var f = data[n] - data[n - 1];
                if (maxSpace < f) maxSpace = f;
                if (minSpace > f) minSpace = f;
            }
            return maxSpace - minSpace;
        }

        private static void LineSpace(List<float> list)
        {
            // Note that increment can be negative if array is in reverse order
            var inc = (list[^1] - list[0]) / (list.Count - 1);
            var first = list[0];
            for (int i = 0; i < list.Count; i++) 
                list[i] = first + i * inc;
        }

        private static void LineSpace(float[] arr)
        {
            // Note that increment can be negative if array is in reverse order
            var inc = (arr[^1] - arr[0]) / (arr.Length - 1);
            var first = arr[0];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = first + i * inc;
        }

        private static void FillList(List<float> list, float val)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] = val;
        }

        private bool PrepareData(AscReader.MeasurementData measurement)
        {
            // Step 1: Evaluate the calculated dose grid to determine if the measure profile fits inside

            // Check if calculated dose grid is larger than measured for all boundaries

            var measData = measurement.Data[0].BeamData;
            var calData = dcm.Value;

            // WARNING: Sometimes values are arranged in reverse order (from positive to negative) and this code won't work!
            var minx = measData.X[0] < calData.X[0];
            var miny = measData.Z[0] < calData.Y[0];
            var minz = measData.Y[0] < calData.Z[0];
            var maxx = measData.X[^1] > calData.X[^1];
            var maxy = measData.Z[^1] > calData.Y[^1];
            var maxz = measData.Y[^1] > calData.Z[^1];

            // If the measured dose extends outside calculated dose for any dimension, warn the user
            if (minx || maxx || miny || maxy || minz || maxz)
            {
                // WARNING: Not tested, we need data!
                var str = "Calculated dose grid does not fully encompass measured dose profile:";

                var minIndex = new int[3];
                var maxIndex = new int[3];

                if (minx)
                {
                    str += string.Format("{0}Crossline Minimum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.X[0], calData.X[0]);
                    var ind = 0;
                    while (measData.X[ind] < calData.X[0])
                        ind++;
                    minIndex[0] = ind;
                }

                if (maxx)
                {
                    str += string.Format("{0}Crossline Maximum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.X[^1], calData.X[^1]);
                    var ind = measData.X.Count - 1;
                    while (measData.X[ind] > calData.X[^1])
                        ind++;
                    maxIndex[0] = ind;
                }

                if (miny)
                {
                    str += string.Format("{0}Depth Minimum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.Z[0], calData.Y[0]);
                    var ind = 0;
                    while (measData.Z[ind] < calData.Y[0])
                        ind++;
                    minIndex[2] = ind;
                }

                if (maxy)
                {
                    str += string.Format("{0}Depth Maximum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.Z[^1], calData.Y[^1]);
                    var ind = measData.Z.Count - 1;
                    while (measData.Z[ind] > calData.Y[^1])
                        ind++;
                    maxIndex[2] = ind;
                }

                if (minz)
                {
                    str += string.Format("{0}Inline Minimum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.Y[0], calData.Z[0]);
                    var ind = 0;
                    while (measData.Y[ind] < calData.Z[0])
                        ind++;
                    minIndex[1] = ind;
                }

                if (maxz)
                {
                    str += string.Format("{0}Inline Maximum: Measured = {1:F2}, Calculated = {2:F2}",
                        Environment.NewLine, measData.Y[^1], calData.Z[^1]);
                    var ind = measData.Y.Count - 1;
                    while (measData.Y[ind] > calData.Z[^1])
                        ind++;
                    maxIndex[1] = ind;
                }

                // Shrink measured dose parameters
                if (minx || miny || maxz)
                {
                    var rem = minIndex.Max();
                    measData.X = measData.X.Skip(rem).ToList();
                    measData.Y = measData.Y.Skip(rem).ToList();
                    measData.Z = measData.Z.Skip(rem).ToList();
                    measData.V = measData.V.Skip(rem).ToList();
                }

                if (maxx || maxy || maxz)
                {
                    var rem = maxIndex.Min();
                    measData.X = measData.X.SkipLast(rem).ToList();
                    measData.Y = measData.Y.SkipLast(rem).ToList();
                    measData.Z = measData.Z.SkipLast(rem).ToList();
                    measData.V = measData.V.SkipLast(rem).ToList();
                }

                str += string.Format("{0}{0}Measured dose profile has been truncated to allow interpolation of calculated dose data. The full measured profile will not be analyzed. This can be resolved by making the calculation dose grid larger.", Environment.NewLine);
                MessageBox.Show(str, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Step 2: Determine what measured dimension to use for independent variable:

            // Some scanning system export profiles that have meandering position values along axes that are
            // supposed to be fixed. This block of code pre - emptively addresses the problem by:
            // (1) Identifies if the scan is moving along this axis.If the start and end location differ by more than 1 mm
            // it is "moving". If they differ by less that 1 mm, then this axis is assumed to be stationary.
            // (2) If the axis is stationary, all of the values in the vector are set to the mode of that vector.

            // Note: original code was filling stationary axis with their modal but since all values in them are esentially
            // the same we can use the first element
            List<float> idm = measData.X;
            if (Math.Abs(measData.X[^1] - measData.X[0]) < 0.1)
                FillList(measData.X, measData.X[0]);

            if (Math.Abs(measData.Y[^1] - measData.Y[0]) < 0.1)
                FillList(measData.Y, measData.Y[0]);
            else
                idm = measData.Y;

            if (Math.Abs(measData.Z[^1] - measData.Z[0]) < 0.1)
                FillList(measData.Z, measData.Z[0]);
            else
                idm = measData.Z;

            // Check if any points of the measured data are at same location. Get rid of any measured
            // points that are at repeat locations, this will crash interpolation
            /*not_rep_pts = [true; (idm(1:end - 1) - idm(2:end)) ~= 0]; % not repeated points
            idm = idm(not_rep_pts); // remove repeats in the sample positions
            md = md(not_rep_pts);   // remove repeats in measured data*/

            // Step 3: Check for non - uniform dose grid axes. Some TPS(e.g. ViewRay) exports DICOM - RT dose files
            // that have rounding errors in the(X, Y, Z) positions, resulting in a non - uniform dose grid.
            // The following code checks for this condition, determines how large it is and resamples to a uniform spacing.

            // X
            var space = FindSpacing(calData.X);

            if (space > 0.001) // is it larger than 1 / 100 mm?
            {
                MessageBox.Show(string.Format("WARNING: The calculated x-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                LineSpace(calData.X);
            }
            else if (space > 0.0) // is it larger than 0 mm?
            {
                // WARNING: Why???
                // LineSpace(calData.X);
            }

            // Y
            space = FindSpacing(calData.Y);

            if (space > 0.001) // is it larger than 1 / 100 mm?
            {
                MessageBox.Show(string.Format("WARNING: The calculated y-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                LineSpace(calData.Y);
            }
            else if (space > 0.0) // is it larger than 0 mm?
            {
                // WARNING: Why???
                // LineSpace(calData.Y);
            }

            // Z
            space = FindSpacing(calData.Z);

            if (space > 0.001) // is it larger than 1 / 100 mm?
            {
                MessageBox.Show(string.Format("WARNING: The calculated z-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                LineSpace(calData.Z);
            }
            else if (space > 0.0) // is it larger than 0 mm?
            {
                // WARNING: Why???
                // LineSpace(calData.Z);
            }

            // Step 4: Resample for gamma analysis

            // Resample indep with the same range but a finer spacing
            var SAMP_PER_CM = 200; // samples per cm
            var idmRange = Math.Abs(idm[^1] - idm[0]);
            var numberOfPoints = Math.Min(2500, (int)Math.Floor(SAMP_PER_CM * idmRange)); // number of samples needed, max 2500

            var newX = new float[numberOfPoints];
            newX[0] = idm[0];   // Insert first value
            newX[^1] = idm[^1]; // Insert last value

            // This will make it lineary spaced
            LineSpace(newX);

            // Resample md with new indep
            var interpolator = MathNet.Numerics.Interpolation.CubicSpline.InterpolatePchip(idm.Select(n => (double)n), measData.V.Select(n => (double)n));
            var newVals = new float[newX.Length];
            for (var i = 0; i < newX.Length; i++)
                newVals[i] = (float)interpolator.Interpolate(newX[i]);

            idm.EnsureCapacity(newX.Length);
            measData.V.EnsureCapacity(newVals.Length);
            for (var i = 0; i < idm.Count; i++)
            {
                idm[i] = newX[i];
                measData.V[i] = newVals[i];
            }
            for (var i = idm.Count; i < newVals.Length; i++)
            {
                idm.Add(newX[i]);
                measData.V.Add(newVals[i]);
            }

            //md = interp1(idm, md, indep, 'PCHIP');

            // Resample calculated dose with new indep
            //cd = interp3(cx, cy, cz, calcData, linspace(mx(1), mx(end), PTS), linspace(mz(1), mz(end), PTS), linspace(my(1), my(end), PTS), '*cubic');

            // Step 4: Apply normalization preferences:
            /*if strcmp(normLoc, 'dmax')
                md = md / max(md);
                cd_ref = max(cd);
                cd = cd / max(cd);
            else
                md = md / max(md);
                cd_ref = interp1(indep, cd, normLoc);
                cd = (cd / interp1(indep, cd, normLoc)) * interp1(indep, md, normLoc);
            end*/

            return true;
        }

        private void VerifyData(int doseThr)
        {
            // vOut = VerifyData(regMeas, regCalc, plotOn, distThr, doseThr)
            // Perform 1D gamma evaluation
            //
            // Input:
            //   regMeas - col 1 = position(cm), col 2 = measurements
            //   regCalc - col 1 = position(cm), col 2 = calculated dose values
            //   distThr - Gamma calc distance threshold in mm
            //   doseThr - Gamma calc dose threshold in %
            //   globAna - Global vs.Local dose difference analysis
            //   usrThrs - User specified lower threshold for counting gamma results
            //   plotOn - Plot flag to be verbose with plotting
            //
            // Output:
            //   gam - 1D gamma calculation result
            //   distMinGam - DTA component of gamma
            //   doseMinGam - Dose difference component of gamma
            //   gamma_stats - an array with[gamma_max gamma_mean gamma_std
            //   aboveTh aboveThPass passRt]
            //
            //
            // Reference:
            //   D.A.Low and J.F.Dempsey.Evaluation of the gamma dose distribution
            //   comparison method.Medical Physics, 30(5):2455 2464, 2003.

            doseThr = doseThr / 100; // Convert from percent to decimal
            // Compute distance error(in mm)
            /*var len = length(regMeas(:, 1));
            rm = repmat(10 * regMeas(:, 1), 1, len); // convert to mm
            rc = repmat(10 * regCalc(:, 1)',len,1);  // convert to mm
            rE = (rm - rc).^ 2;

            rEThr = rE./ (distThr.^ 2);

            // Compute dose error
            Drm = repmat(regMeas(:, 2), 1, len);
            Drc = repmat(regCalc(:, 2)',len,1);
            if (globAna)
                dE = ((Drm - Drc) / 1).^ 2;
            else
                dE = ((Drm - Drc)./ Drm).^ 2;
            end

            dEThr = dE./ ((doseThr).^ 2);
            gam2 = rEThr + dEThr;

            // take min down columns to get gamma as a function of position
            [gam Ir] = min(gam2); % Ir is the row index where the min gamma was found
            gam = sqrt(gam);

            // get distance error at minimum gamma
            Ic = 1:len; // make column index array
            I = sub2ind(size(rm), Ir, Ic); // convert to linear indices
            distMinGam = sqrt(rEThr(I)); // distance error at minimum gamma position
            doseMinGam = sqrt(dEThr(I)); // dose error at minimum gamma position

            // get distance to minimum dose difference
            [mDose IMDr] = min(dE);

            // Compute the gamma statistics
            aboveTh = 0;
            aboveThPass = 0;
            gamma_max = 0;
            gamma_sum = 0;
            gamma_sum_2 = 0;
            zero_flag = 1;

            for i = 1:length(gam)
                if regMeas(i, 2) >= usrThrs && regCalc(i, 2) > 0

                    // Update maximum gamma
                    if gam(i) > gamma_max, gamma_max = gam(i); end

                    // Update mean and std stats
                    gamma_sum = gamma_sum + gam(i);
                    gamma_sum_2 = gamma_sum_2 + gam(i) * gam(i);

                    // Update counts
                    aboveTh = aboveTh + 1;
                    if gam(i) <= 1
                        aboveThPass = aboveThPass + 1;
                    end
                end
            end

            gamma_mean = gamma_sum / aboveTh;
            gamma_std = sqrt(gamma_sum_2 / aboveTh - gamma_sum * gamma_sum / aboveTh / aboveTh);
            passRt = aboveThPass / aboveTh * 100;
            gamma_stats = [gamma_max gamma_mean gamma_std aboveTh aboveThPass passRt];*/

        }
        private void OnOptions(object sender, EventArgs e)
        {

        }
    }
}
