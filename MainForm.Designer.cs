namespace MPPG
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainMenu = new MenuStrip();
            fileMenuItem = new ToolStripMenuItem();
            openMeasuredFileToolStripMenuItem = new ToolStripMenuItem();
            openCalculatedFileToolStripMenuItem = new ToolStripMenuItem();
            exportPDFToolStripMenuItem = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            txtDCMManufacturer = new TextBox();
            label7 = new Label();
            btnRun = new Button();
            txtASCScanner = new TextBox();
            label6 = new Label();
            txtASCMeasurements = new TextBox();
            txtDCMStatus = new TextBox();
            txtASCStatus = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            txtDCMFile = new TextBox();
            txtASCFile = new TextBox();
            label2 = new Label();
            label1 = new Label();
            label8 = new Label();
            txtDCMOffset = new TextBox();
            mainMenu.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.ImageScalingSize = new Size(20, 20);
            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenuItem, exitMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(1196, 28);
            mainMenu.TabIndex = 4;
            mainMenu.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openMeasuredFileToolStripMenuItem, openCalculatedFileToolStripMenuItem, exportPDFToolStripMenuItem });
            fileMenuItem.Name = "fileMenuItem";
            fileMenuItem.Size = new Size(46, 24);
            fileMenuItem.Text = "&File";
            // 
            // openMeasuredFileToolStripMenuItem
            // 
            openMeasuredFileToolStripMenuItem.Name = "openMeasuredFileToolStripMenuItem";
            openMeasuredFileToolStripMenuItem.Size = new Size(267, 26);
            openMeasuredFileToolStripMenuItem.Text = "Open &Measured Dose File";
            openMeasuredFileToolStripMenuItem.Click += OpenMeasuredFileToolStripMenuItem_Click;
            // 
            // openCalculatedFileToolStripMenuItem
            // 
            openCalculatedFileToolStripMenuItem.Name = "openCalculatedFileToolStripMenuItem";
            openCalculatedFileToolStripMenuItem.Size = new Size(267, 26);
            openCalculatedFileToolStripMenuItem.Text = "Open &Calculated Dose File";
            openCalculatedFileToolStripMenuItem.Click += OpenCalculatedFileToolStripMenuItem_Click;
            // 
            // exportPDFToolStripMenuItem
            // 
            exportPDFToolStripMenuItem.Enabled = false;
            exportPDFToolStripMenuItem.Name = "exportPDFToolStripMenuItem";
            exportPDFToolStripMenuItem.Size = new Size(267, 26);
            exportPDFToolStripMenuItem.Text = "Export PDF";
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(47, 24);
            exitMenuItem.Text = "&Exit";
            exitMenuItem.Click += ExitMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(txtDCMOffset);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(txtDCMManufacturer);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(btnRun);
            panel1.Controls.Add(txtASCScanner);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtASCMeasurements);
            panel1.Controls.Add(txtDCMStatus);
            panel1.Controls.Add(txtASCStatus);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtDCMFile);
            panel1.Controls.Add(txtASCFile);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(12, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(1172, 153);
            panel1.TabIndex = 5;
            // 
            // txtDCMManufacturer
            // 
            txtDCMManufacturer.Enabled = false;
            txtDCMManufacturer.Location = new Point(692, 76);
            txtDCMManufacturer.Name = "txtDCMManufacturer";
            txtDCMManufacturer.ReadOnly = true;
            txtDCMManufacturer.Size = new Size(278, 27);
            txtDCMManufacturer.TabIndex = 20;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(542, 76);
            label7.Name = "label7";
            label7.Size = new Size(97, 20);
            label7.TabIndex = 19;
            label7.Text = "Manufacturer";
            // 
            // btnRun
            // 
            btnRun.Anchor = AnchorStyles.Top;
            btnRun.Enabled = false;
            btnRun.Location = new Point(1075, 121);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(94, 29);
            btnRun.TabIndex = 18;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += BtnRun_Click;
            // 
            // txtASCScanner
            // 
            txtASCScanner.Enabled = false;
            txtASCScanner.Location = new Point(148, 106);
            txtASCScanner.Name = "txtASCScanner";
            txtASCScanner.ReadOnly = true;
            txtASCScanner.Size = new Size(278, 27);
            txtASCScanner.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 109);
            label6.Name = "label6";
            label6.Size = new Size(120, 20);
            label6.TabIndex = 16;
            label6.Text = "Scanning System";
            // 
            // txtASCMeasurements
            // 
            txtASCMeasurements.Enabled = false;
            txtASCMeasurements.Location = new Point(148, 73);
            txtASCMeasurements.Name = "txtASCMeasurements";
            txtASCMeasurements.ReadOnly = true;
            txtASCMeasurements.Size = new Size(47, 27);
            txtASCMeasurements.TabIndex = 15;
            // 
            // txtDCMStatus
            // 
            txtDCMStatus.Enabled = false;
            txtDCMStatus.Location = new Point(692, 40);
            txtDCMStatus.Name = "txtDCMStatus";
            txtDCMStatus.ReadOnly = true;
            txtDCMStatus.Size = new Size(278, 27);
            txtDCMStatus.TabIndex = 14;
            txtDCMStatus.Text = "Not Loaded";
            // 
            // txtASCStatus
            // 
            txtASCStatus.Enabled = false;
            txtASCStatus.Location = new Point(148, 40);
            txtASCStatus.Name = "txtASCStatus";
            txtASCStatus.ReadOnly = true;
            txtASCStatus.Size = new Size(278, 27);
            txtASCStatus.TabIndex = 13;
            txtASCStatus.Text = "Not Loaded";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 76);
            label5.Name = "label5";
            label5.Size = new Size(105, 20);
            label5.TabIndex = 12;
            label5.Text = "Measurements";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(542, 43);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 11;
            label4.Text = "Status";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 43);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 10;
            label3.Text = "Status";
            // 
            // txtDCMFile
            // 
            txtDCMFile.Enabled = false;
            txtDCMFile.Location = new Point(692, 7);
            txtDCMFile.Name = "txtDCMFile";
            txtDCMFile.ReadOnly = true;
            txtDCMFile.Size = new Size(278, 27);
            txtDCMFile.TabIndex = 9;
            // 
            // txtASCFile
            // 
            txtASCFile.Enabled = false;
            txtASCFile.Location = new Point(148, 7);
            txtASCFile.Name = "txtASCFile";
            txtASCFile.ReadOnly = true;
            txtASCFile.Size = new Size(278, 27);
            txtASCFile.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(542, 10);
            label2.Name = "label2";
            label2.Size = new Size(144, 20);
            label2.TabIndex = 7;
            label2.Text = "Calculated Dose File";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 10);
            label1.Name = "label1";
            label1.Size = new Size(139, 20);
            label1.TabIndex = 6;
            label1.Text = "Measured Dose File";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(542, 116);
            label8.Name = "label8";
            label8.Size = new Size(49, 20);
            label8.TabIndex = 21;
            label8.Text = "Offset";
            // 
            // txtDCMOffset
            // 
            txtDCMOffset.Enabled = false;
            txtDCMOffset.Location = new Point(692, 113);
            txtDCMOffset.Name = "txtDCMOffset";
            txtDCMOffset.ReadOnly = true;
            txtDCMOffset.Size = new Size(278, 27);
            txtDCMOffset.TabIndex = 22;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1196, 646);
            Controls.Add(panel1);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "MPPG Compare";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenuItem;
        private ToolStripMenuItem openMeasuredFileToolStripMenuItem;
        private ToolStripMenuItem openCalculatedFileToolStripMenuItem;
        private ToolStripMenuItem exportPDFToolStripMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private Panel panel1;
        private Label label1;
        private TextBox txtDCMFile;
        private TextBox txtASCFile;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label4;
        private TextBox txtASCScanner;
        private Label label6;
        private TextBox txtASCMeasurements;
        private TextBox txtDCMStatus;
        private TextBox txtASCStatus;
        private Button btnRun;
        private TextBox txtDCMManufacturer;
        private Label label7;
        private TextBox txtDCMOffset;
        private Label label8;
    }
}
