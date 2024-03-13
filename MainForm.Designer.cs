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
            txtDCMFile = new TextBox();
            txtASCFile = new TextBox();
            label2 = new Label();
            label1 = new Label();
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
            mainMenu.Size = new Size(800, 28);
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
            openMeasuredFileToolStripMenuItem.Click += openMeasuredFileToolStripMenuItem_Click;
            // 
            // openCalculatedFileToolStripMenuItem
            // 
            openCalculatedFileToolStripMenuItem.Name = "openCalculatedFileToolStripMenuItem";
            openCalculatedFileToolStripMenuItem.Size = new Size(267, 26);
            openCalculatedFileToolStripMenuItem.Text = "Open &Calculated Dose File";
            openCalculatedFileToolStripMenuItem.Click += openCalculatedFileToolStripMenuItem_Click;
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
            exitMenuItem.Click += exitMenuItem_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(txtDCMFile);
            panel1.Controls.Add(txtASCFile);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(12, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 125);
            panel1.TabIndex = 5;
            // 
            // txtDCMFile
            // 
            txtDCMFile.Location = new Point(153, 61);
            txtDCMFile.Name = "txtDCMFile";
            txtDCMFile.ReadOnly = true;
            txtDCMFile.Size = new Size(278, 27);
            txtDCMFile.TabIndex = 9;
            // 
            // txtASCFile
            // 
            txtASCFile.Location = new Point(153, 10);
            txtASCFile.Name = "txtASCFile";
            txtASCFile.ReadOnly = true;
            txtASCFile.Size = new Size(278, 27);
            txtASCFile.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 64);
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
