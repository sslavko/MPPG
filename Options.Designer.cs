namespace MPPG
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOk = new Button();
            btnCancel = new Button();
            groupBox1 = new GroupBox();
            depthY = new NumericUpDown();
            label3 = new Label();
            label2 = new Label();
            rdNormDepthMan = new RadioButton();
            rdNormDepthDMax = new RadioButton();
            groupBox2 = new GroupBox();
            inlineZ = new NumericUpDown();
            label7 = new Label();
            label8 = new Label();
            crosslineX = new NumericUpDown();
            label5 = new Label();
            rdNormInCrossMan = new RadioButton();
            label6 = new Label();
            rdNormInCrossDMax = new RadioButton();
            groupBox3 = new GroupBox();
            rdDoseAnalysisLocal = new RadioButton();
            label12 = new Label();
            rdDoseAnalysisGlobal = new RadioButton();
            threshold = new NumericUpDown();
            label11 = new Label();
            chkThreshold = new CheckBox();
            dta = new NumericUpDown();
            label9 = new Label();
            label10 = new Label();
            doseDiff = new NumericUpDown();
            label4 = new Label();
            label1 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)depthY).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inlineZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)crosslineX).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)threshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)doseDiff).BeginInit();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(519, 279);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(94, 29);
            btnOk.TabIndex = 3;
            btnOk.Text = "&OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += OkClick;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(619, 279);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(depthY);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(rdNormDepthMan);
            groupBox1.Controls.Add(rdNormDepthDMax);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(326, 102);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Normalize Depth-Dose Profile To";
            // 
            // depthY
            // 
            depthY.DecimalPlaces = 1;
            depthY.Enabled = false;
            depthY.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            depthY.Location = new Point(196, 57);
            depthY.Name = "depthY";
            depthY.Size = new Size(82, 27);
            depthY.TabIndex = 3;
            depthY.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(284, 59);
            label3.Name = "label3";
            label3.Size = new Size(33, 20);
            label3.TabIndex = 4;
            label3.Text = "cm ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(118, 59);
            label2.Name = "label2";
            label2.Size = new Size(72, 20);
            label2.TabIndex = 2;
            label2.Text = "Depth (Y)";
            // 
            // rdNormDepthMan
            // 
            rdNormDepthMan.AutoSize = true;
            rdNormDepthMan.Location = new Point(9, 57);
            rdNormDepthMan.Name = "rdNormDepthMan";
            rdNormDepthMan.Size = new Size(93, 24);
            rdNormDepthMan.TabIndex = 1;
            rdNormDepthMan.Text = "Depth (Y)";
            rdNormDepthMan.UseVisualStyleBackColor = true;
            rdNormDepthMan.CheckedChanged += OnNormDepthManChanged;
            // 
            // rdNormDepthDMax
            // 
            rdNormDepthDMax.AutoSize = true;
            rdNormDepthDMax.Checked = true;
            rdNormDepthDMax.Location = new Point(9, 27);
            rdNormDepthDMax.Name = "rdNormDepthDMax";
            rdNormDepthDMax.Size = new Size(69, 24);
            rdNormDepthDMax.TabIndex = 0;
            rdNormDepthDMax.TabStop = true;
            rdNormDepthDMax.Text = "Dmax";
            rdNormDepthDMax.UseVisualStyleBackColor = true;
            rdNormDepthDMax.CheckedChanged += OnNormDepthDMaxChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(inlineZ);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(crosslineX);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(rdNormInCrossMan);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(rdNormInCrossDMax);
            groupBox2.Location = new Point(344, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(369, 102);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Normalize Inline and Crossline Profile To";
            // 
            // inlineZ
            // 
            inlineZ.DecimalPlaces = 1;
            inlineZ.Enabled = false;
            inlineZ.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            inlineZ.Location = new Point(244, 57);
            inlineZ.Name = "inlineZ";
            inlineZ.Size = new Size(82, 27);
            inlineZ.TabIndex = 6;
            inlineZ.TextAlign = HorizontalAlignment.Right;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(332, 59);
            label7.Name = "label7";
            label7.Size = new Size(33, 20);
            label7.TabIndex = 7;
            label7.Text = "cm ";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(170, 59);
            label8.Name = "label8";
            label8.Size = new Size(68, 20);
            label8.TabIndex = 5;
            label8.Text = "Inline (Z)";
            // 
            // crosslineX
            // 
            crosslineX.DecimalPlaces = 1;
            crosslineX.Enabled = false;
            crosslineX.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            crosslineX.Location = new Point(244, 27);
            crosslineX.Name = "crosslineX";
            crosslineX.Size = new Size(82, 27);
            crosslineX.TabIndex = 3;
            crosslineX.TextAlign = HorizontalAlignment.Right;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(332, 29);
            label5.Name = "label5";
            label5.Size = new Size(33, 20);
            label5.TabIndex = 4;
            label5.Text = "cm ";
            // 
            // rdNormInCrossMan
            // 
            rdNormInCrossMan.AutoSize = true;
            rdNormInCrossMan.Location = new Point(12, 57);
            rdNormInCrossMan.Name = "rdNormInCrossMan";
            rdNormInCrossMan.Size = new Size(121, 24);
            rdNormInCrossMan.TabIndex = 1;
            rdNormInCrossMan.Text = "Position (X, Z)";
            rdNormInCrossMan.UseVisualStyleBackColor = true;
            rdNormInCrossMan.CheckedChanged += OnNormInCrossManChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(147, 29);
            label6.Name = "label6";
            label6.Size = new Size(91, 20);
            label6.TabIndex = 2;
            label6.Text = "Crossline (X)";
            // 
            // rdNormInCrossDMax
            // 
            rdNormInCrossDMax.AutoSize = true;
            rdNormInCrossDMax.Checked = true;
            rdNormInCrossDMax.Location = new Point(12, 27);
            rdNormInCrossDMax.Name = "rdNormInCrossDMax";
            rdNormInCrossDMax.Size = new Size(69, 24);
            rdNormInCrossDMax.TabIndex = 0;
            rdNormInCrossDMax.TabStop = true;
            rdNormInCrossDMax.Text = "Dmax";
            rdNormInCrossDMax.UseVisualStyleBackColor = true;
            rdNormInCrossDMax.CheckedChanged += OnNormInCrossDMaxChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(rdDoseAnalysisLocal);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(rdDoseAnalysisGlobal);
            groupBox3.Controls.Add(threshold);
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(chkThreshold);
            groupBox3.Controls.Add(dta);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(label10);
            groupBox3.Controls.Add(doseDiff);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(label1);
            groupBox3.Location = new Point(12, 120);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(701, 133);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Gamma Analysis";
            // 
            // rdDoseAnalysisLocal
            // 
            rdDoseAnalysisLocal.AutoSize = true;
            rdDoseAnalysisLocal.Location = new Point(576, 66);
            rdDoseAnalysisLocal.Name = "rdDoseAnalysisLocal";
            rdDoseAnalysisLocal.Size = new Size(65, 24);
            rdDoseAnalysisLocal.TabIndex = 11;
            rdDoseAnalysisLocal.Text = "Local";
            rdDoseAnalysisLocal.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(368, 66);
            label12.Name = "label12";
            label12.Size = new Size(100, 20);
            label12.TabIndex = 9;
            label12.Text = "Dose Analysis";
            // 
            // rdDoseAnalysisGlobal
            // 
            rdDoseAnalysisGlobal.AutoSize = true;
            rdDoseAnalysisGlobal.Checked = true;
            rdDoseAnalysisGlobal.Location = new Point(474, 66);
            rdDoseAnalysisGlobal.Name = "rdDoseAnalysisGlobal";
            rdDoseAnalysisGlobal.Size = new Size(74, 24);
            rdDoseAnalysisGlobal.TabIndex = 10;
            rdDoseAnalysisGlobal.TabStop = true;
            rdDoseAnalysisGlobal.Text = "Global";
            rdDoseAnalysisGlobal.UseVisualStyleBackColor = true;
            // 
            // threshold
            // 
            threshold.DecimalPlaces = 1;
            threshold.Enabled = false;
            threshold.Location = new Point(477, 28);
            threshold.Name = "threshold";
            threshold.Size = new Size(82, 27);
            threshold.TabIndex = 7;
            threshold.TextAlign = HorizontalAlignment.Right;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(565, 30);
            label11.Name = "label11";
            label11.Size = new Size(21, 20);
            label11.TabIndex = 8;
            label11.Text = "%";
            // 
            // chkThreshold
            // 
            chkThreshold.AutoSize = true;
            chkThreshold.Location = new Point(347, 29);
            chkThreshold.Name = "chkThreshold";
            chkThreshold.Size = new Size(124, 24);
            chkThreshold.TabIndex = 6;
            chkThreshold.Text = "Use Threshold";
            chkThreshold.UseVisualStyleBackColor = true;
            chkThreshold.CheckedChanged += OnUseThresholdCheck;
            // 
            // dta
            // 
            dta.Location = new Point(87, 64);
            dta.Name = "dta";
            dta.Size = new Size(82, 27);
            dta.TabIndex = 4;
            dta.TextAlign = HorizontalAlignment.Right;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(175, 66);
            label9.Name = "label9";
            label9.Size = new Size(35, 20);
            label9.TabIndex = 5;
            label9.Text = "mm";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(45, 66);
            label10.Name = "label10";
            label10.Size = new Size(36, 20);
            label10.TabIndex = 3;
            label10.Text = "DTA";
            // 
            // doseDiff
            // 
            doseDiff.Location = new Point(87, 31);
            doseDiff.Name = "doseDiff";
            doseDiff.Size = new Size(82, 27);
            doseDiff.TabIndex = 1;
            doseDiff.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(175, 33);
            label4.Name = "label4";
            label4.Size = new Size(21, 20);
            label4.TabIndex = 2;
            label4.Text = "%";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 33);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 0;
            label1.Text = "Dose Diff";
            // 
            // Options
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(723, 320);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Options";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Options";
            Load += OnLoad;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)depthY).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)inlineZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)crosslineX).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)threshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)dta).EndInit();
            ((System.ComponentModel.ISupportInitialize)doseDiff).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private GroupBox groupBox1;
        private RadioButton rdNormDepthMan;
        private RadioButton rdNormDepthDMax;
        private GroupBox groupBox2;
        private NumericUpDown depthY;
        private Label label3;
        private Label label2;
        private GroupBox groupBox3;
        private NumericUpDown inlineZ;
        private Label label7;
        private Label label8;
        private NumericUpDown crosslineX;
        private Label label5;
        private RadioButton rdNormInCrossMan;
        private Label label6;
        private RadioButton rdNormInCrossDMax;
        private NumericUpDown doseDiff;
        private Label label4;
        private Label label1;
        private Label label12;
        private NumericUpDown threshold;
        private Label label11;
        private CheckBox chkThreshold;
        private NumericUpDown dta;
        private Label label9;
        private Label label10;
        private RadioButton rdDoseAnalysisLocal;
        private RadioButton rdDoseAnalysisGlobal;
    }
}