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
            runToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            txtDCMOffset = new TextBox();
            label8 = new Label();
            txtDCMManufacturer = new TextBox();
            label7 = new Label();
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
            drawingPanel = new Panel();
            mainMenu.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.ImageScalingSize = new Size(20, 20);
            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenuItem, runToolStripMenuItem, optionsToolStripMenuItem, exitMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(1282, 28);
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
            // runToolStripMenuItem
            // 
            runToolStripMenuItem.Enabled = false;
            runToolStripMenuItem.Name = "runToolStripMenuItem";
            runToolStripMenuItem.Size = new Size(48, 24);
            runToolStripMenuItem.Text = "&Run";
            runToolStripMenuItem.Click += Run_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 24);
            optionsToolStripMenuItem.Text = "&Options";
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
            panel1.Size = new Size(1258, 140);
            panel1.TabIndex = 5;
            // 
            // txtDCMOffset
            // 
            txtDCMOffset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDCMOffset.Enabled = false;
            txtDCMOffset.Location = new Point(782, 106);
            txtDCMOffset.Name = "txtDCMOffset";
            txtDCMOffset.ReadOnly = true;
            txtDCMOffset.Size = new Size(470, 27);
            txtDCMOffset.TabIndex = 15;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(632, 109);
            label8.Name = "label8";
            label8.Size = new Size(49, 20);
            label8.TabIndex = 14;
            label8.Text = "Offset";
            // 
            // txtDCMManufacturer
            // 
            txtDCMManufacturer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDCMManufacturer.Enabled = false;
            txtDCMManufacturer.Location = new Point(782, 73);
            txtDCMManufacturer.Name = "txtDCMManufacturer";
            txtDCMManufacturer.ReadOnly = true;
            txtDCMManufacturer.Size = new Size(470, 27);
            txtDCMManufacturer.TabIndex = 13;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(632, 76);
            label7.Name = "label7";
            label7.Size = new Size(97, 20);
            label7.TabIndex = 12;
            label7.Text = "Manufacturer";
            // 
            // txtASCScanner
            // 
            txtASCScanner.Enabled = false;
            txtASCScanner.Location = new Point(148, 106);
            txtASCScanner.Name = "txtASCScanner";
            txtASCScanner.ReadOnly = true;
            txtASCScanner.Size = new Size(470, 27);
            txtASCScanner.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 109);
            label6.Name = "label6";
            label6.Size = new Size(120, 20);
            label6.TabIndex = 6;
            label6.Text = "Scanning System";
            // 
            // txtASCMeasurements
            // 
            txtASCMeasurements.Enabled = false;
            txtASCMeasurements.Location = new Point(148, 73);
            txtASCMeasurements.Name = "txtASCMeasurements";
            txtASCMeasurements.ReadOnly = true;
            txtASCMeasurements.Size = new Size(80, 27);
            txtASCMeasurements.TabIndex = 5;
            // 
            // txtDCMStatus
            // 
            txtDCMStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDCMStatus.Enabled = false;
            txtDCMStatus.Location = new Point(782, 40);
            txtDCMStatus.Name = "txtDCMStatus";
            txtDCMStatus.ReadOnly = true;
            txtDCMStatus.Size = new Size(470, 27);
            txtDCMStatus.TabIndex = 11;
            txtDCMStatus.Text = "Not Loaded";
            // 
            // txtASCStatus
            // 
            txtASCStatus.Enabled = false;
            txtASCStatus.Location = new Point(148, 40);
            txtASCStatus.Name = "txtASCStatus";
            txtASCStatus.ReadOnly = true;
            txtASCStatus.Size = new Size(470, 27);
            txtASCStatus.TabIndex = 3;
            txtASCStatus.Text = "Not Loaded";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 76);
            label5.Name = "label5";
            label5.Size = new Size(129, 20);
            label5.TabIndex = 4;
            label5.Text = "Measurements No";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(632, 43);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 10;
            label4.Text = "Status";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 43);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 2;
            label3.Text = "Status";
            // 
            // txtDCMFile
            // 
            txtDCMFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDCMFile.Enabled = false;
            txtDCMFile.Location = new Point(782, 7);
            txtDCMFile.Name = "txtDCMFile";
            txtDCMFile.ReadOnly = true;
            txtDCMFile.Size = new Size(470, 27);
            txtDCMFile.TabIndex = 9;
            // 
            // txtASCFile
            // 
            txtASCFile.Enabled = false;
            txtASCFile.Location = new Point(148, 7);
            txtASCFile.Name = "txtASCFile";
            txtASCFile.ReadOnly = true;
            txtASCFile.Size = new Size(470, 27);
            txtASCFile.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(632, 10);
            label2.Name = "label2";
            label2.Size = new Size(144, 20);
            label2.TabIndex = 8;
            label2.Text = "Calculated Dose File";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 10);
            label1.Name = "label1";
            label1.Size = new Size(139, 20);
            label1.TabIndex = 0;
            label1.Text = "Measured Dose File";
            // 
            // drawingPanel
            // 
            drawingPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            drawingPanel.BackColor = SystemColors.ControlLight;
            drawingPanel.Location = new Point(12, 189);
            drawingPanel.Name = "drawingPanel";
            drawingPanel.Size = new Size(1258, 482);
            drawingPanel.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1282, 683);
            Controls.Add(drawingPanel);
            Controls.Add(panel1);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            MinimumSize = new Size(1300, 730);
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
        private TextBox txtDCMManufacturer;
        private Label label7;
        private TextBox txtDCMOffset;
        private Label label8;
        private Panel drawingPanel;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
    }
}
