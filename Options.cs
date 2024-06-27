using MPPG.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPPG
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            var Settings = new Settings();

            // Depth Dose normalization
            rdNormDepthMan.Checked = Settings.normDepthMan;
            depthY.Value = (decimal)Settings.depthY;

            // Inline/Crossline Dose normalization
            rdNormInCrossMan.Checked = Settings.NormInCrossMan;
            crosslineX.Value = (decimal)Settings.crossX;
            inlineZ.Value = (decimal)Settings.inlineZ;

            // Gamma
            doseDiff.Value = (decimal)Settings.doseDiff;
            dta.Value = (decimal)Settings.dta;
            chkThreshold.Checked = Settings.useThreshold;
            threshold.Value = (decimal)Settings.threshold;
            rdDoseAnalysisLocal.Checked = Settings.doseAnalysisLocal;

            chkDashDotLines.Checked = Settings.dashedGraphs;
        }

        private void OnNormDepthDMaxChanged(object sender, EventArgs e)
        {
            depthY.Enabled = false;
        }

        private void OnNormDepthManChanged(object sender, EventArgs e)
        {
            depthY.Enabled = true;
        }

        private void OnNormInCrossDMaxChanged(object sender, EventArgs e)
        {
            crosslineX.Enabled = false;
            inlineZ.Enabled = false;
        }

        private void OnNormInCrossManChanged(object sender, EventArgs e)
        {
            crosslineX.Enabled = true;
            inlineZ.Enabled = true;
        }

        private void OnUseThresholdCheck(object sender, EventArgs e)
        {
            threshold.Enabled = chkThreshold.Checked;
        }

        private void OkClick(object sender, EventArgs e)
        {
            var Settings = new Settings();

            // Depth Dose normalization
            Settings.normDepthMan = rdNormDepthMan.Checked;
            Settings.depthY = (float)depthY.Value;

            // Inline/Crossline Dose normalization
            Settings.NormInCrossMan = rdNormInCrossMan.Checked;
            Settings.crossX = (float)crosslineX.Value;
            Settings.inlineZ = (float)inlineZ.Value;

            // Gamma
            Settings.doseDiff = (float)doseDiff.Value;
            Settings.dta = (float)dta.Value;
            Settings.useThreshold = chkThreshold.Checked;
            Settings.threshold = (float)threshold.Value;
            Settings.doseAnalysisLocal = rdDoseAnalysisLocal.Checked;

            Settings.dashedGraphs = chkDashDotLines.Checked;

            Settings.Save();
        }
    }
}
