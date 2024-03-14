using EvilDICOM.Core;
using EvilDICOM.Core.Helpers;

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
                txtDCMFile.Text = dlg.SafeFileName;
                var dcmSel = DICOMObject.Read(dlg.FileName).GetSelector();
                if (dcmSel.Modality.Data == "RTDOSE")
                {
                    dcm = DcmReader.Read(dcmSel, dlg.FileName);
                    if (dcm != null)
                    {
                        txtDCMManufacturer.Text = dcm.Value.Manufacturer;
                        var offset = dcm.Value.Offset.Value;
                        txtDCMOffset.Text = string.Format("x: {0}, y: {1}, z: {2}", offset.X, offset.Y, offset.Z);
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
                    dcm = null;
                }

                btnRun.Enabled = dcm != null && asc != null;
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
                    txtASCStatus.Text = "Ready";
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

                btnRun.Enabled = dcm != null && asc != null;
            };
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {

        }
    }
}
