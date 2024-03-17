using EvilDICOM.Core;
using EvilDICOM.Core.Extensions;
using ScottPlot.Plottables;

namespace MPPG
{
    public partial class MainForm : Form
    {
        private AscReader.MeasurementData? asc;
        private DcmReader.CalculatedData? dcm;

        public MainForm()
        {
            InitializeComponent();
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
                ClearGraphs();
                Cursor = Cursors.WaitCursor;
                txtDCMFile.Text = dlg.SafeFileName;
                var dcmFile = DICOMObject.Read(dlg.FileName);
                var dcmSel = dcmFile.GetSelector();
                if (dcmSel.Modality.Data == "RTDOSE")
                {
                    var pixelStream = dcmFile.GetPixelStream();
                    dcm = DcmReader.Read(dcmSel, pixelStream, dlg.FileName);
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
            };
        }

        private void OnOpenMeasuredFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Measured Files (*.asc)|*.asc";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ClearGraphs();
                txtASCFile.Text = dlg.SafeFileName;
                asc = AscReader.Read(dlg.FileName);
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
            };
        }

        private void OnRun(object sender, EventArgs e)
        {
            ClearGraphs();

            var fileName = Path.GetFileName(txtDCMFile.Text);
            var titleText = string.Format("Crossline Profiles at Depth (Y) = {0:F2} cm, Inline Position (Z) = {1:F2} cm", 1, 2);
            var normText = string.Format("asdsad");
            var horAxisText = string.Format("Inline Position (Z) [cm]");
            txtTitle.Text = string.Format("{0}{1}{2}{1}{3}", fileName, Environment.NewLine, titleText, normText);

            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            // Relative Dose plot
            var relDosePlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Fill
            };
            drawingPanel.Controls.Add(relDosePlot, 0, 0);
            relDosePlot.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
            relDosePlot.Plot.Add.Scatter(dataX, dataY).Label = "Measured"; //'TPS','Threshold'
            relDosePlot.Plot.XLabel(horAxisText);
            relDosePlot.Plot.YLabel("Relative Dose");
            relDosePlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            relDosePlot.Refresh();

            var tpsDoseLabel = new Label()
            {
                Text = string.Format("TPS dose at normalization point is {0:F3} Gy", 1),
                Location = new Point(40, 5),
                BackColor = Color.White,
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
            gamaPlot.Plot.Add.Scatter(dataX, dataY);
            gamaPlot.Plot.XLabel(horAxisText);
            gamaPlot.Plot.YLabel("Gamma");
            gamaPlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            gamaPlot.Refresh();

            var passRateLabel = new Label()
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
            auPlot.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
            auPlot.Plot.Add.Scatter(dataX, dataY).Label = "distMinGam"; // 'doseMinGam'
            auPlot.Plot.XLabel(horAxisText);
            auPlot.Plot.YLabel("AU");
            auPlot.Plot.FigureBackground = ScottPlot.Color.FromARGB(0xffffffff);
            auPlot.Refresh();
        }

        private void OnOptions(object sender, EventArgs e)
        {

        }
    }
}
