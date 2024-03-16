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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenCalculatedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Calculated DICOM Files (*.dcm)|*.dcm";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
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

        private void OpenMeasuredFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Measured Files (*.asc)|*.asc";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
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

        private void Run_Click(object sender, EventArgs e)
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };

            var relDosePlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Top | DockStyle.Left,
                Height = 100,
                Name = "Blah1"
            };
            drawingPanel.Controls.Add(relDosePlot);
            relDosePlot.Plot.Add.Scatter(dataX, dataY);
            relDosePlot.Refresh();

            var gamaPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Top | DockStyle.Left,
                Height = 100,
                Name = "Blah2"
            };
            drawingPanel.Controls.Add(gamaPlot);
            gamaPlot.Plot.Add.Scatter(dataX, dataY);
            gamaPlot.Refresh();

            var auPlot = new ScottPlot.WinForms.FormsPlot()
            {
                Dock = DockStyle.Top | DockStyle.Left,
                Height = 100,
                Name = "Blah3"
            };
            drawingPanel.Controls.Add(auPlot);
            auPlot.Plot.Add.Scatter(dataX, dataY);
            auPlot.Refresh();
        }
    }
}
