using EvilDICOM.Core;
using EvilDICOM.Core.Extensions;
using ScottPlot;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Accessibility;
using System.Diagnostics;
using Microsoft.VisualBasic.Logging;
using Microsoft.Extensions.Logging;

namespace MPPG
{
    public partial class MainForm : Form
    {
        private AscReader.Measurements? asc;
        private DcmReader.CalculatedData? dcm;
        private int pageNum = 1;

        public MainForm()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 3)
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
            txtTitle.Text = "";
        }

        private void ClearPages()
        {
            pageNum = 1;
            txtPageNum.Text = pageNum.ToString();
            txtPageNum.Enabled = false;
            btnLeftPageNo.Enabled = false;
            btnRightPageNo.Enabled = false;
        }

        private void OnOpenCalculatedFile(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Calculated DICOM Files (*.dcm)|*.dcm"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenCalculatedFile(dlg.FileName);
            };
        }

        private void OpenCalculatedFile(string filePath)
        {
            ClearGraphs();
            exportPDFToolStripMenuItem.Enabled = false;
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
                    var offset = dcm.Value.Offset.Value;
                    txtDCMOffset.Text = string.Format("x: {0:F3}, y: {1:F3}, z: {2:F3}", offset.X, offset.Y, offset.Z);
                    txtDCMStatus.Text = "Ready";
                }
                else
                {
                    txtDCMStatus.Text = "Failed to load DICOM file";
                    txtDCMOffset.Text = "";
                    dcm = null;
                }
            }
            else
            {
                txtDCMStatus.Text = "Not a DICOM-RT DOSE file";
                txtDCMOffset.Text = "";
                dcm = null;
            }

            Cursor = Cursors.Default;
            runToolStripMenuItem.Enabled = dcm != null && asc != null;
        }

        private void OnOpenMeasuredFile(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Measured Files (*.asc)|*.asc"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenMeasuredFile(dlg.FileName);
            }
        }

        private void OpenMeasuredFile(string filePath)
        {
            ClearPages();
            ClearGraphs();
            exportPDFToolStripMenuItem.Enabled = false;
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
            }
            else
            {
                txtASCStatus.Text = "Failed to load";
                txtASCMeasurements.Text = "";
                asc = null;
            };

            runToolStripMenuItem.Enabled = dcm != null && asc != null;
        }

        private void OnOptions(object sender, EventArgs e)
        {
            var opts = new Options();
            if (opts.ShowDialog(this) == DialogResult.OK)
            {
                ClearPages();
                ClearGraphs();
            }
        }

        private void CheckPageButtons()
        {
            btnLeftPageNo.Enabled = pageNum > 1;
            btnRightPageNo.Enabled = pageNum < asc.Value.NumberOfMeasurements;
        }

        private void OnPrevPage(object sender, EventArgs e)
        {
            if (pageNum > 1)
            {
                pageNum--;
                txtPageNum.Text = pageNum.ToString();
            }
        }

        private void OnNextPage(object sender, EventArgs e)
        {
            if (pageNum < asc.Value.NumberOfMeasurements)
            {
                pageNum++;
                txtPageNum.Text = pageNum.ToString();
            }
        }

        private void OnPageNumChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtPageNum.Text, out int val) && val >= 1 && val <= asc.Value.NumberOfMeasurements)
            {
                Cursor = Cursors.WaitCursor;
                pageNum = val;
                ClearGraphs();
                var plotData = PrepareData(asc.Value.Data[pageNum - 1]);
                VerifyData(ref plotData);
                Plot(plotData, drawingPanel, true);
                CheckPageButtons();
                Cursor = Cursors.Default;
            }

            txtPageNum.Text = pageNum.ToString();
        }

        private void OnRun(object sender, EventArgs e)
        {
            if (asc == null || dcm == null)
                return;

            Cursor = Cursors.WaitCursor;

            ClearPages();
            ClearGraphs();
            exportPDFToolStripMenuItem.Enabled = true;

            var measurements = asc.Value;

            for (int i = 0; i < measurements.NumberOfMeasurements; i++)
                CheckData(measurements.Data[0], i + 1);

            PlotData plotData = PrepareData(measurements.Data[0]);
            VerifyData(ref plotData);
            Plot(plotData, drawingPanel, true);

            txtPageNum.Enabled = true;
            btnRightPageNo.Enabled = measurements.NumberOfMeasurements > 1;

            Cursor = Cursors.Default;
        }

        private struct PlotData
        {
            internal float[] positions;
            internal float[] measuredValues;
            internal float[] calculatedValues;
            internal float[] gamma;
            internal float[] distMinGamma;
            internal float[] doseMinGamma;
            internal float cdRef;
            internal float? normLoc; // Normalization location if provided, or null if Dmax is used
            internal string plotTitle;
            internal string horAxisText;
            internal float threshold;
            internal float passRate;
        }

        private void Plot(PlotData plotData, TableLayoutPanel panel, bool screenDrawing)
        {
            if (screenDrawing)
                txtTitle.Text = plotData.plotTitle;

            var settings = new Properties.Settings();
            var drawDashedGraphs = settings.dashedGraphs;

            // Relative Dose plot
            var relDosePlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill
            };
            panel.Controls.Add(relDosePlot, 0, 0);
            relDosePlot.Plot.ShowLegend(Alignment.UpperRight);
            var graph = relDosePlot.Plot.Add.Scatter(plotData.positions, plotData.measuredValues);
            graph.Label = "Measured";
            graph.Color = ScottPlot.Colors.Blue;
            graph.MarkerStyle = MarkerStyle.None;

            graph = relDosePlot.Plot.Add.Scatter(plotData.positions, plotData.calculatedValues);
            graph.Label = "TPS";
            graph.Color = ScottPlot.Colors.Red;
            if (drawDashedGraphs)
                graph.LineStyle.Pattern = LinePattern.Dashed;

            graph.MarkerStyle = MarkerStyle.None;

            var threshold = new float[plotData.calculatedValues.Length];
            for (int i = 0; i < threshold.Length; i++)
                threshold[i] = plotData.threshold;

            graph = relDosePlot.Plot.Add.Scatter(plotData.positions, threshold);
            graph.Label = "Threshold";
            graph.Color = ScottPlot.Colors.Magenta;
            graph.LineStyle.Pattern = LinePattern.Dotted;
            graph.MarkerStyle = MarkerStyle.None;

            relDosePlot.Plot.XLabel(plotData.horAxisText);
            relDosePlot.Plot.Axes.SetLimitsX(plotData.positions[0], plotData.positions[^1]);
            relDosePlot.Plot.YLabel("Relative Dose");
            relDosePlot.Plot.FigureBackground.Color = ScottPlot.Colors.White;
            relDosePlot.Refresh();

            var tpsDoseLabel = new System.Windows.Forms.Label()
            {
                Text = string.Format("TPS dose at normalization point is {0:F3} Gy", plotData.cdRef),
                Location = new Point(55, 17),
                BackColor = System.Drawing.Color.White,
                AutoSize = true
            };
            relDosePlot.Controls.Add(tpsDoseLabel);
            if (screenDrawing)
                tpsDoseLabel.BringToFront();

            // Gamma plot
            var gamaPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill,
            };
            panel.Controls.Add(gamaPlot, 0, 1);

            graph = gamaPlot.Plot.Add.Scatter(plotData.positions, plotData.gamma);
            graph.Label = "Threshold";
            graph.Color = ScottPlot.Colors.Blue;
            graph.MarkerStyle = MarkerStyle.None;

            gamaPlot.Plot.XLabel(plotData.horAxisText);
            gamaPlot.Plot.Axes.SetLimitsX(plotData.positions[0], plotData.positions[^1]);
            gamaPlot.Plot.YLabel("Gamma");
            gamaPlot.Plot.Axes.SetLimitsY(0, 1.5);
            gamaPlot.Plot.FigureBackground.Color = ScottPlot.Colors.White;
            gamaPlot.Refresh();

            var passRateLabel = new System.Windows.Forms.Label()
            {
                Text = string.Format("Pass rate: {0:F1}%", plotData.passRate),
                Location = new Point(55, 17),
                BackColor = System.Drawing.Color.White,
                AutoSize = true
            };
            gamaPlot.Controls.Add(passRateLabel);
            if (screenDrawing)
                passRateLabel.BringToFront();

            // AU plot
            var auPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill,
            };
            panel.Controls.Add(auPlot, 0, 2);
            auPlot.Plot.ShowLegend(Alignment.UpperRight);

            graph = auPlot.Plot.Add.Scatter(plotData.positions, plotData.distMinGamma);
            graph.Label = "distMinGam";
            graph.Color = ScottPlot.Colors.Blue;
            graph.MarkerStyle = MarkerStyle.None;

            graph = auPlot.Plot.Add.Scatter(plotData.positions, plotData.doseMinGamma);
            graph.Label = "doseMinGam";
            if (drawDashedGraphs)
                graph.LineStyle.Pattern = LinePattern.Dashed;

            graph.Color = ScottPlot.Colors.Red;
            graph.MarkerStyle = MarkerStyle.None;

            auPlot.Plot.XLabel(plotData.horAxisText);
            auPlot.Plot.Axes.SetLimitsX(plotData.positions[0], plotData.positions[^1]);
            auPlot.Plot.YLabel("AU");
            auPlot.Plot.Axes.SetLimitsY(0, 1.5);
            auPlot.Plot.FigureBackground.Color = ScottPlot.Colors.White;
            auPlot.Refresh();
        }

        private static float FindSpacing(float[] data)
        {
            var maxSpace = 0.0f;
            var minSpace = 1000.0f;
            for (int n = 1; n < data.Length; n++)
            {
                var f = data[n] - data[n - 1];
                if (maxSpace < f) maxSpace = f;
                if (minSpace > f) minSpace = f;
            }
            return maxSpace - minSpace;
        }

        private static void FillLinearlySpaced(float[] arr)
        {
            // Note: increment can be negative if array is in reverse order
            var inc = (arr[^1] - arr[0]) / (arr.Length - 1);
            var first = arr[0];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = first + i * inc;
        }

        private static void FillArray(float[] list, float val)
        {
            for (int i = 0; i < list.Length; i++)
                list[i] = val;
        }

        private static float FindArrayMode(float[] arr)
        {
            var mode = arr.GroupBy(i => i)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

            return mode;
        }

        private static int FindNearestIndex(float[] arr, float val)
        {
            if (arr[0] < arr[^1])
            {
                // Ascending array
                if (val <= arr[0])
                    return 0;

                if (val >= arr[^1])
                    return arr.Length - 1;

                for (int i = 0; i < arr.Length - 1; i++)
                    if (val >= arr[i] && val < arr[i + 1])
                    {
                        if (val - arr[i] < arr[i + 1] - val)
                            return i;
                        else
                            return i + 1;
                    }
            }
            else
            {
                // Descending array
                if (val >= arr[0])
                    return 0;

                if (val <= arr[^1])
                    return arr.Length - 1;

                for (int i = 0; i < arr.Length - 1; i++)
                    if (val <= arr[i] && val > arr[i + 1])
                    {
                        if (arr[i] - val < val - arr[i + 1])
                            return i;
                        else
                            return i + 1;
                    }
            }

            return -1;
        }

        /* Evaluate the calculated dose grid to determine if the measure profile fits inside
         * Check if calculated dose grid is larger than measured for all boundaries
         */
        private void CheckData(AscReader.SingleMeasurement measurement, int measurementNumber)
        {
            var measData = measurement.BeamData;
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
                // WARNING: Not tested, we need data for this use case!
                var str = string.Format("Measurement number {0}{1}", measurementNumber, Environment.NewLine);
                str += "Calculated dose grid does not fully encompass measured dose profile:";

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

                    var ind = measData.X.Length - 1;
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

                    var ind = measData.Z.Length - 1;
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

                    var ind = measData.Y.Length - 1;
                    while (measData.Y[ind] > calData.Z[^1])
                        ind++;
                    maxIndex[1] = ind;
                }

                // Shrink measured dose parameters
                if (minx || miny || maxz)
                {
                    var rem = minIndex.Max();
                    measData.X = measData.X.Skip(rem).ToArray();
                    measData.Y = measData.Y.Skip(rem).ToArray();
                    measData.Z = measData.Z.Skip(rem).ToArray();
                    measData.V = measData.V.Skip(rem).ToArray();
                }

                if (maxx || maxy || maxz)
                {
                    var rem = maxIndex.Min();
                    measData.X = measData.X.SkipLast(rem).ToArray();
                    measData.Y = measData.Y.SkipLast(rem).ToArray();
                    measData.Z = measData.Z.SkipLast(rem).ToArray();
                    measData.V = measData.V.SkipLast(rem).ToArray();
                }

                str += string.Format("{0}{0}Measured dose profile has been truncated to allow interpolation of calculated dose data. The full measured profile will not be analyzed. This can be resolved by making the calculation dose grid larger.", Environment.NewLine);
                MessageBox.Show(str, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /* Prepare data for drawing on graphs
         */
        private PlotData PrepareData(AscReader.SingleMeasurement measurement)
        {
            var measData = measurement.BeamData;
            var calcData = dcm.Value;

            // Some scanning system export profiles that have meandering position values along axes that are
            // supposed to be fixed. This block of code pre-emptively addresses the problem by:
            // (1) Identifies if the scan is moving along this axis. If the start and end location differ by more than 1 mm
            // it is "moving". If they differ by less that 1 mm, then this axis is assumed to be stationary.
            // (2) If the axis is stationary, all of the values in the vector are set to the mode of that vector.

            var idm = measData.X;
            switch (measurement.AxisType)
            {
                case 'X':
                    FillArray(measData.Y, FindArrayMode(measData.Y));
                    FillArray(measData.Z, FindArrayMode(measData.Z));
                    break;
                case 'Y':
                    idm = measData.Y;
                    FillArray(measData.X, FindArrayMode(measData.X));
                    FillArray(measData.Z, FindArrayMode(measData.Z));
                    break;
                case 'Z':
                    idm = measData.Z;
                    FillArray(measData.X, FindArrayMode(measData.X));
                    FillArray(measData.Y, FindArrayMode(measData.Y));
                    break;
            }

            // Check if any points of the measured data are at same location. Get rid of any measured
            // points that are at repeat locations, this will crash interpolation
            // TODO: Move this code to AscReader
            for (int i = 1; i < idm.Length; i++)
            {
                if (idm[i] - idm[i - 1] == 0)
                {
                    Debug.WriteLine("*** Duplicate found ***");
                    // TODO: Remove duplicated measurement
                    /*not_rep_pts = [true; (idm(1:end - 1) - idm(2:end)) ~= 0]; % not repeated points
                    idm = idm(not_rep_pts); // remove repeats in the sample positions
                    md = md(not_rep_pts);   // remove repeats in measured data*/
                }
            }

            // Check for non-uniform dose grid axes. Some TPS(e.g. ViewRay) exports DICOM - RT dose files
            // that have rounding errors in the (X, Y, Z) positions, resulting in a non-uniform dose grid.
            // The following code checks for this condition, determines how large it is and resamples to a uniform spacing.
            // TODO: Move this code to DcmReader and do it only once after file is read

            var spacingTolerance = 0.00001;

            // X
            var space = FindSpacing(calcData.X);
            if (space > spacingTolerance)
            {
                // Apply new linearly spaced values for positions
                if (space > 0.001) // Warn user if maximum spacing is larger than 1 / 100 mm
                {
                    MessageBox.Show(string.Format("WARNING: The calculated x-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                FillLinearlySpaced(calcData.X);
            }

            // Y
            space = FindSpacing(calcData.Y);
            if (space > spacingTolerance)
            {
                if (space > 0.001) // Warn user if maximum spacing is larger than 1 / 100 mm
                {
                    MessageBox.Show(string.Format("WARNING: The calculated y-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                FillLinearlySpaced(calcData.Y);
            }

            // Z
            space = FindSpacing(calcData.Z);
            if (space > spacingTolerance)
            {
                if (space > 0.001) // Warn user if maximum spacing is larger than 1 / 100 mm
                {
                    MessageBox.Show(string.Format("WARNING: The calculated z-axis values are not uniformly spaced. The maximum discrepancy is {0} cm.", space),
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                FillLinearlySpaced(calcData.Z);
            }

            // *** Resample for gamma analysis ***

            // Resample indep with the same range but a finer spacing
            var SAMP_PER_CM = 200; // samples per cm
            var idmRange = Math.Abs(idm[^1] - idm[0]);
            var numberOfPoints = Math.Min(2500, (int)Math.Floor(SAMP_PER_CM * idmRange)); // number of samples needed, max 2500

            var plotData = new PlotData
            {
                positions = new float[numberOfPoints]
            };
            plotData.positions[0] = idm[0];   // Insert first value
            plotData.positions[^1] = idm[^1]; // Insert last value

            // This will fill array of positions with linearly spaced values between first and last values
            FillLinearlySpaced(plotData.positions);

            // Interpolation functions require arrays of doubles
            var measuredPositions = idm.Select(n => (double)n);
            var measuredValues = measData.V.Select(n => (double)n);

            // Create interpolator with measured positions and values (real measured data)
            var interpolatorM = MathNet.Numerics.Interpolation.CubicSpline.InterpolatePchip(
                measuredPositions,
                measuredValues);

            // Find interpolated measured values for each new position
            plotData.measuredValues = new float[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
                plotData.measuredValues[i] = (float)interpolatorM.Interpolate(plotData.positions[i]);

            // Prepare calculated data for interpolation
            int xIndex, yIndex, zIndex;
            var calculatedPositions = Array.Empty<double>();
            var calculatedValues = Array.Empty<double>();

            switch (measurement.AxisType)
            {
                case 'X':
                    yIndex = FindNearestIndex(calcData.Y, measurement.BeamData.Z[0]);
                    zIndex = FindNearestIndex(calcData.Z, measurement.BeamData.Y[0]);

                    // Extract values along calculated X axis
                    calculatedValues = new double[calcData.X.Length];
                    for (int i = 0; i < calcData.X.Length; i++)
                        calculatedValues[i] = calcData.V[i, yIndex, zIndex];

                    calculatedPositions = calcData.X.Select(n => (double)n).ToArray();
                    break;
                case 'Z':
                    xIndex = FindNearestIndex(calcData.X, measurement.BeamData.X[0]);
                    zIndex = FindNearestIndex(calcData.Z, measurement.BeamData.Y[0]);

                    // Extract values along calculated Y axis
                    calculatedValues = new double[calcData.Y.Length];
                    for (int i = 0; i < calculatedValues.Length; i++)
                        calculatedValues[i] = calcData.V[xIndex, i, zIndex];

                    calculatedPositions = calcData.Y.Select(n => (double)n).ToArray();
                    break;
                case 'Y':
                    xIndex = FindNearestIndex(calcData.X, measurement.BeamData.X[0]);
                    yIndex = FindNearestIndex(calcData.Y, measurement.BeamData.Z[0]);

                    // Extract values along calculated Z axis
                    calculatedValues = new double[calcData.Z.Length];
                    for (int i = 0; i < calculatedValues.Length; i++)
                        calculatedValues[i] = calcData.V[xIndex, yIndex, i];

                    calculatedPositions = calcData.Z.Select(n => (double)n).ToArray();
                    break;
                case 'D':
                    // Assumption is that measured diagonal data is always in XY plane, Z position is fixed
                    yIndex = FindNearestIndex(calcData.Y, measurement.BeamData.Z[0]);

                    // Extract values along diagonal
                    var startX = FindNearestIndex(calcData.X, measurement.BeamData.X[0]);
                    var endX = FindNearestIndex(calcData.X, measurement.BeamData.X[^1]);
                    var startZ = FindNearestIndex(calcData.Z, measurement.BeamData.Y[0]);
                    var endZ = FindNearestIndex(calcData.Z, measurement.BeamData.Y[^1]);

                    calculatedPositions = new double[endX - startX + 1];
                    for (int i = 0; i < calculatedPositions.Length; i++)
                        calculatedPositions[i] = calcData.X[startX + i];

                    calculatedValues = new double[calculatedPositions.Length];
                    for (int i = 0; i < calculatedValues.Length; i++)
                    {
                        xIndex = startX + i;
                        zIndex = startZ + i;
                        calculatedValues[i] = calcData.V[xIndex, yIndex, zIndex];
                    }
                    break;
            }

            // Create interpolator with calculated positions and values (real calculated data)
            var interpolatorC = MathNet.Numerics.Interpolation.CubicSpline.InterpolateNatural(
                calculatedPositions,
                calculatedValues);

            // Find interpolated calculated values for each new position
            plotData.calculatedValues = new float[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
                plotData.calculatedValues[i] = (float)interpolatorC.Interpolate(plotData.positions[i]);

            // *** Normalization ***

            // Measured data are always normalized with maximum value
            var maxVal = plotData.measuredValues.Max();
            for (int i = 0; i < plotData.measuredValues.Length; i++)
                plotData.measuredValues[i] /= maxVal;

            // Apply normalization to calculated values, use user preferences to determine normalization location
            var settings = new Properties.Settings();
            plotData.normLoc = null;
            string normText = "";
            switch (measurement.AxisType)
            {
                case 'X':
                    // The x position of the measurement changes, so the profile must have some crossline profile component.
                    // Normalization based on crossline normalization preferences:
                    if (settings.NormInCrossMan)
                    {
                        plotData.normLoc = settings.crossX;
                        normText = string.Format("Profiles normalized at X = {0:F2} cm", plotData.normLoc);
                    }

                    plotData.horAxisText = "Crossline Position (X) [cm]";
                    plotData.plotTitle = string.Format("Crossline Profiles at Depth(Y) = {0:F2} cm, Inline Position(Z) = {1:F2} cm",
                        measurement.BeamData.Z[0],
                        measurement.BeamData.Y[0]);
                    break;
                case 'Y':
                    // The y position of the measurement changes, so the profile must have some inline profile component.
                    // Normalization based on inline normalization preferences:
                    if (settings.NormInCrossMan)
                    {
                        plotData.normLoc = settings.inlineZ;
                        normText = string.Format("Profiles normalized at Z = {0:F2} cm", plotData.normLoc);
                    }

                    plotData.horAxisText = "Inline Position (Z) [cm]";
                    plotData.plotTitle = string.Format("Inline Profiles at Depth(Y) = {0:F2} cm, Crossline Position(X) = {1:F2} cm",
                        measurement.BeamData.Z[0],
                        measurement.BeamData.X[0]);
                    break;
                case 'Z':
                    // The depth of the measurement changes, so the profile must have some depth dose component.
                    // Normalization based on PDD normalization preferences:
                    if (settings.normDepthMan)
                    {
                        plotData.normLoc = settings.depthY;
                        normText = string.Format("Profiles normalized at Y = {0:F2} cm", plotData.normLoc);
                    }

                    plotData.horAxisText = "Depth (Y) [cm]";
                    plotData.plotTitle = string.Format("Depth-Dose Profiles at Crossline Position (X) = {0:F2} cm, Inline Position (Z) = {1:F2} cm",
                        measurement.BeamData.X[0],
                        measurement.BeamData.Y[0]);
                    break;
                case 'D':
                    // This is the case when axis type is diagonal. We will still use X axis
                    // Normalization based on crossline normalization preferences:
                    if (settings.NormInCrossMan)
                    {
                        plotData.normLoc = settings.crossX;
                        normText = string.Format("Profiles normalized at X = {0:F2} cm", plotData.normLoc);
                    }
                    plotData.horAxisText = "Crossline Position (X) [cm]";
                    plotData.plotTitle = string.Format("Diagonal Profiles from ({0:F2}, {1:F2}, {2:F2}) to ({3:F2}, {4:F2}, {5:F2})",
                        measurement.BeamData.X[0],
                        measurement.BeamData.Z[0],
                        measurement.BeamData.Y[0],
                        measurement.BeamData.X[^1],
                        measurement.BeamData.Z[^1],
                        measurement.BeamData.Y[^1]);
                    break;
            }

            if (plotData.normLoc.HasValue)
            {
                // Normalization position is specified by user
                plotData.cdRef = (float)interpolatorC.Interpolate(plotData.normLoc.Value);

                // QUESTION: Can we use interpolatorM instead of creating new one for large set of already interpolated data?
                var interpolatorMNorm = MathNet.Numerics.Interpolation.LinearSpline.Interpolate(
                    plotData.positions.Select(n => (double)n),
                    plotData.measuredValues.Select(n => (double)n));

                // QUESTION: Why do we multiply with measured value? Is it to make sure we don't go over value of 1?
                var m = (float)interpolatorMNorm.Interpolate(plotData.normLoc.Value);
                var q = plotData.cdRef * m;
                for (int i = 0; i < plotData.calculatedValues.Length; i++)
                    plotData.calculatedValues[i] /= q;
            }
            else
            {
                // Use maximum value for normalization
                normText = "Profiles normalized at maximum dose location for each profile";
                plotData.cdRef = plotData.calculatedValues.Max();
                for (int i = 0; i < plotData.calculatedValues.Length; i++)
                    plotData.calculatedValues[i] /= plotData.cdRef;
            }

            if (settings.useThreshold)
                plotData.threshold = settings.threshold / 100;
            else
                plotData.threshold = 0;

            plotData.gamma = new float[numberOfPoints];

            plotData.plotTitle = string.Format("{0}{1}{2}{1}{3}",
                                    Path.GetFileNameWithoutExtension(txtDCMFile.Text),
                                    Environment.NewLine,
                                    plotData.plotTitle,
                                    normText);

            return plotData;
        }

        /*
         * Reference:
         *   D.A.Low and J.F.Dempsey.Evaluation of the gamma dose distribution
         *   comparison method.Medical Physics, 30(5):2455 2464, 2003.
         */
        private static void VerifyData(ref PlotData plotData)
        {
            var settings = new Properties.Settings();

            var useLocalAnalysis = settings.doseAnalysisLocal;

            // Distance threshold
            var distThr = settings.dta;
            var distThrSquared = distThr * distThr;

            // Dose threshold
            var doseThr = settings.doseDiff / 100; // Convert from percent to decimal
            var doseThrSquared = doseThr * doseThr;

            var len = plotData.positions.Length;

            // Array to hold minimum squared gamma values
            var gammaSquaredMinColumn = new float[len];
            for (int i = 0; i < len; i++)
                gammaSquaredMinColumn[i] = float.PositiveInfinity;

            plotData.distMinGamma = new float[len];
            plotData.doseMinGamma = new float[len];

            for (int col = 0; col < len; col++)
            {
                var minDistErr = 0f;
                var minDoseErr = 0f;
                for (int row = 0; row < len; row++)
                {
                    // Compute distance error (in mm)
                    var d = (plotData.positions[col] - plotData.positions[row]) * 10; // convert to mm
                    var distErr = d * d / distThrSquared;

                    // Compute dose error
                    d = plotData.measuredValues[col] - plotData.calculatedValues[row];
                    if (useLocalAnalysis)
                        d /= plotData.measuredValues[col];

                    var doseErr = d * d / doseThrSquared;

                    var gammaSquared = distErr + doseErr;
                    if (gammaSquared < gammaSquaredMinColumn[col])
                    {
                        gammaSquaredMinColumn[col] = gammaSquared;
                        minDistErr = distErr;
                        minDoseErr = doseErr;
                    }
                }
                plotData.distMinGamma[col] = (float)Math.Sqrt(minDistErr);
                plotData.doseMinGamma[col] = (float)Math.Sqrt(minDoseErr);
            }

            var aboveTh = 0;
            var aboveThPass = 0;
            for (int i = 0; i < len; i++)
            {
                plotData.gamma[i] = (float)Math.Sqrt(gammaSquaredMinColumn[i]);
                if (plotData.measuredValues[i] > plotData.threshold && plotData.calculatedValues[i] > 0)
                {
                    aboveTh++;
                    if (plotData.gamma[i] <= 1)
                        aboveThPass++;
                }
            }
            plotData.passRate = (float)aboveThPass / aboveTh * 100;
        }

        private void OnExportPDF(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(txtDCMFile.Text),
                Filter = "PDF file (*.pdf)|*.pdf"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                var doc = new Document();
                var bmpWidth = (int)(16 / 2.54 * 300);  // 16cm at 300dpi
                var bmpHeight = (int)(10 / 2.54 * 300); // 10cm at 300dpi

                var measurements = asc.Value;
                var tempFiles = new List<string>();

                // Prepare off-screen panel for drawing
                var panel = new TableLayoutPanel
                {
                    Width = bmpWidth,
                    Height = bmpHeight,
                    RowCount = 3
                };
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));

                // Insert first page in PDF with detailed information about this calculation
                var section = doc.AddSection();
                var settings = new Properties.Settings();

                var table = section.AddTable();
                table.Borders.Color = MigraDoc.DocumentObjectModel.Colors.Black;

                table.AddColumn("7cm");
                table.AddColumn("10cm");

                var row = table.AddRow();
                row.Cells[0].AddParagraph("Measurement file");
                row.Cells[1].AddParagraph(txtASCFile.Text);

                row = table.AddRow();
                row.Cells[0].AddParagraph("DICOM-RT DOSE file");
                row.Cells[1].AddParagraph(txtDCMFile.Text);

                row = table.AddRow();
                row.Cells[0].AddParagraph("DICOM offset");
                row.Cells[1].AddParagraph(txtDCMOffset.Text);

                row = table.AddRow();
                row.Cells[0].AddParagraph("Normalize depth dose profile to");
                if (settings.normDepthMan)
                    row.Cells[1].AddParagraph(settings.depthY + "cm");
                else
                    row.Cells[1].AddParagraph("Dmax");

                row = table.AddRow();
                row.Cells[0].AddParagraph(string.Format("Normalize Inline and Crossline profiles to"));
                if (settings.NormInCrossMan)
                    row.Cells[1].AddParagraph(string.Format("Crossline (X) {0}cm, Inline (Z) {1}cm", settings.crossX, settings.inlineZ));
                else
                    row.Cells[1].AddParagraph("Dmax");

                row = table.AddRow();
                row.Cells[0].AddParagraph("Dose difference");
                row.Cells[1].AddParagraph(settings.doseDiff.ToString() + "%");

                row = table.AddRow();
                row.Cells[0].AddParagraph("DTA");
                row.Cells[1].AddParagraph(settings.dta.ToString() + "mm");

                row = table.AddRow();
                row.Cells[0].AddParagraph("Threshold");
                row.Cells[1].AddParagraph(settings.useThreshold ? settings.threshold.ToString() : "-");

                row = table.AddRow();
                row.Cells[0].AddParagraph("Dose analysis");
                row.Cells[1].AddParagraph(settings.doseAnalysisLocal ? "Local" : "Global");

                for (int i = 0; i < measurements.NumberOfMeasurements; i++)
                {
                    PlotData plotData = PrepareData(measurements.Data[i]);
                    VerifyData(ref plotData);

                    // Add page break after every second graph
                    if (i % 2 == 0)
                        section.AddPageBreak();
                    else
                        section.AddParagraph();

                    section.AddParagraph(plotData.plotTitle).Format.Alignment = ParagraphAlignment.Center;

                    Plot(plotData, panel, false);
                    var bmp = new Bitmap(bmpWidth, bmpHeight);
                    bmp.SetResolution(300, 300);
                    panel.DrawToBitmap(bmp, new Rectangle(0, 0, bmpWidth, bmpHeight));
                    panel.Controls.Clear();
                    var bmpFile = Path.GetTempFileName() + ".png";
                    bmp.Save(bmpFile, System.Drawing.Imaging.ImageFormat.Png);
                    section.AddImage(bmpFile);
                    tempFiles.Add(bmpFile);
                }
                var pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(dlg.FileName);

                // Delete temporary image files
                foreach (var tempFile in tempFiles)
                    File.Delete(tempFile);

                Cursor = Cursors.Default;
            }
        }

        private void OnAbout(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
