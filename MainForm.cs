using EvilDICOM.Core;

namespace MPPG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openCalculatedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Calculated DICOM Files (*.dcm)|*.dcm";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDCMFile.Text = dlg.SafeFileName;
                var dcm = DICOMObject.Read(dlg.FileName);
            };
        }

        private void openMeasuredFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Measured Files (*.asc)|*.asc";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtASCFile.Text = dlg.SafeFileName;
                var asc = new AscReader();
                asc.Read(dlg.FileName);
            };
        }
    }
}
