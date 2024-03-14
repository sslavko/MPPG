using EvilDICOM.Core;
using EvilDICOM.Core.Helpers;

namespace MPPG
{
    public partial class MainForm : Form
    {
        private AscReader? asc;
        private DcmReader? dcm;

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
                var dcmFile = DICOMObject.Read(dlg.FileName);
                var modality = dcmFile.FindFirst(TagHelper.Modality).DData as string;
                if (modality == "RTDOSE")
                {
                    var dcm = new DcmReader();
                    if (dcm.Read(dcmFile))
                    {
                        txtDCMManufacturer.Text = dcm.Manufacturer;
                        txtDCMStatus.Text = "Ready";
                    }
                    else
                    {
                        txtDCMStatus.Text = "Failed to load DICOM file";
                        txtDCMManufacturer.Text = "";
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
                asc = new AscReader();
                if (asc.Read(dlg.FileName))
                {
                    txtASCStatus.Text = "Ready";
                    txtASCMeasurements.Text = asc.NumberOfMeasurements.ToString();
                    txtASCScanner.Text = asc.ScannerSystem;
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
