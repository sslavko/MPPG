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
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            txtDCMStatus = new TextBox();
            label3 = new Label();
            txtDCMFile = new TextBox();
            label5 = new Label();
            label4 = new Label();
            txtASCFile = new TextBox();
            txtASCMeasurements = new TextBox();
            label2 = new Label();
            txtASCStatus = new TextBox();
            label8 = new Label();
            txtDCMOffset = new TextBox();
            drawingPanel = new TableLayoutPanel();
            txtTitle = new TextBox();
            btnLeftPageNo = new Button();
            btnRightPageNo = new Button();
            txtPageNum = new TextBox();
            mainMenu.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.ImageScalingSize = new Size(20, 20);
            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenuItem, runToolStripMenuItem, optionsToolStripMenuItem, exitMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(1182, 28);
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
            openMeasuredFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.M;
            openMeasuredFileToolStripMenuItem.Size = new Size(320, 26);
            openMeasuredFileToolStripMenuItem.Text = "Open &Measured Dose File";
            openMeasuredFileToolStripMenuItem.Click += OnOpenMeasuredFile;
            // 
            // openCalculatedFileToolStripMenuItem
            // 
            openCalculatedFileToolStripMenuItem.Name = "openCalculatedFileToolStripMenuItem";
            openCalculatedFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            openCalculatedFileToolStripMenuItem.Size = new Size(320, 26);
            openCalculatedFileToolStripMenuItem.Text = "Open &Calculated Dose File";
            openCalculatedFileToolStripMenuItem.Click += OnOpenCalculatedFile;
            // 
            // exportPDFToolStripMenuItem
            // 
            exportPDFToolStripMenuItem.Enabled = false;
            exportPDFToolStripMenuItem.Name = "exportPDFToolStripMenuItem";
            exportPDFToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            exportPDFToolStripMenuItem.Size = new Size(320, 26);
            exportPDFToolStripMenuItem.Text = "Export PDF";
            exportPDFToolStripMenuItem.Click += OnExportPDF;
            // 
            // runToolStripMenuItem
            // 
            runToolStripMenuItem.Enabled = false;
            runToolStripMenuItem.Name = "runToolStripMenuItem";
            runToolStripMenuItem.Size = new Size(48, 24);
            runToolStripMenuItem.Text = "&Run";
            runToolStripMenuItem.Click += OnRun;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 24);
            optionsToolStripMenuItem.Text = "&Options";
            optionsToolStripMenuItem.Click += OnOptions;
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(47, 24);
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += OnExit;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(txtDCMStatus, 3, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 1);
            tableLayoutPanel1.Controls.Add(txtDCMFile, 3, 0);
            tableLayoutPanel1.Controls.Add(label5, 0, 2);
            tableLayoutPanel1.Controls.Add(label4, 2, 1);
            tableLayoutPanel1.Controls.Add(txtASCFile, 1, 0);
            tableLayoutPanel1.Controls.Add(txtASCMeasurements, 1, 2);
            tableLayoutPanel1.Controls.Add(label2, 2, 0);
            tableLayoutPanel1.Controls.Add(txtASCStatus, 1, 1);
            tableLayoutPanel1.Controls.Add(label8, 2, 2);
            tableLayoutPanel1.Controls.Add(txtDCMOffset, 3, 2);
            tableLayoutPanel1.Location = new Point(12, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(0, 7, 0, 0);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.Size = new Size(1158, 113);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(3, 14);
            label1.Name = "label1";
            label1.Size = new Size(139, 20);
            label1.TabIndex = 0;
            label1.Text = "Measured Dose File";
            // 
            // txtDCMStatus
            // 
            txtDCMStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDCMStatus.Enabled = false;
            txtDCMStatus.Location = new Point(732, 46);
            txtDCMStatus.Name = "txtDCMStatus";
            txtDCMStatus.ReadOnly = true;
            txtDCMStatus.Size = new Size(423, 27);
            txtDCMStatus.TabIndex = 11;
            txtDCMStatus.Text = "Not Loaded";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(3, 49);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 2;
            label3.Text = "Status";
            // 
            // txtDCMFile
            // 
            txtDCMFile.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDCMFile.Enabled = false;
            txtDCMFile.Location = new Point(732, 11);
            txtDCMFile.Name = "txtDCMFile";
            txtDCMFile.ReadOnly = true;
            txtDCMFile.Size = new Size(423, 27);
            txtDCMFile.TabIndex = 9;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(3, 85);
            label5.Name = "label5";
            label5.Size = new Size(105, 20);
            label5.TabIndex = 4;
            label5.Text = "Measurements";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(582, 49);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 10;
            label4.Text = "Status";
            // 
            // txtASCFile
            // 
            txtASCFile.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtASCFile.Enabled = false;
            txtASCFile.Location = new Point(153, 11);
            txtASCFile.Name = "txtASCFile";
            txtASCFile.ReadOnly = true;
            txtASCFile.Size = new Size(423, 27);
            txtASCFile.TabIndex = 1;
            // 
            // txtASCMeasurements
            // 
            txtASCMeasurements.Anchor = AnchorStyles.Left;
            txtASCMeasurements.Enabled = false;
            txtASCMeasurements.Location = new Point(153, 81);
            txtASCMeasurements.Name = "txtASCMeasurements";
            txtASCMeasurements.ReadOnly = true;
            txtASCMeasurements.Size = new Size(80, 27);
            txtASCMeasurements.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(582, 14);
            label2.Name = "label2";
            label2.Size = new Size(144, 20);
            label2.TabIndex = 8;
            label2.Text = "Calculated Dose File";
            // 
            // txtASCStatus
            // 
            txtASCStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtASCStatus.Enabled = false;
            txtASCStatus.Location = new Point(153, 46);
            txtASCStatus.Name = "txtASCStatus";
            txtASCStatus.ReadOnly = true;
            txtASCStatus.Size = new Size(423, 27);
            txtASCStatus.TabIndex = 3;
            txtASCStatus.Text = "Not Loaded";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Left;
            label8.AutoSize = true;
            label8.Location = new Point(582, 85);
            label8.Name = "label8";
            label8.Size = new Size(49, 20);
            label8.TabIndex = 14;
            label8.Text = "Offset";
            // 
            // txtDCMOffset
            // 
            txtDCMOffset.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDCMOffset.Enabled = false;
            txtDCMOffset.Location = new Point(732, 81);
            txtDCMOffset.Name = "txtDCMOffset";
            txtDCMOffset.ReadOnly = true;
            txtDCMOffset.Size = new Size(423, 27);
            txtDCMOffset.TabIndex = 15;
            // 
            // drawingPanel
            // 
            drawingPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            drawingPanel.BackColor = SystemColors.Control;
            drawingPanel.ColumnCount = 1;
            drawingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            drawingPanel.Location = new Point(12, 235);
            drawingPanel.Name = "drawingPanel";
            drawingPanel.RowCount = 3;
            drawingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            drawingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            drawingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            drawingPanel.Size = new Size(1158, 456);
            drawingPanel.TabIndex = 6;
            // 
            // txtTitle
            // 
            txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitle.BackColor = SystemColors.Control;
            txtTitle.BorderStyle = BorderStyle.None;
            txtTitle.Enabled = false;
            txtTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtTitle.Location = new Point(165, 150);
            txtTitle.Multiline = true;
            txtTitle.Name = "txtTitle";
            txtTitle.ReadOnly = true;
            txtTitle.Size = new Size(851, 79);
            txtTitle.TabIndex = 10;
            txtTitle.TextAlign = HorizontalAlignment.Center;
            // 
            // btnLeftPageNo
            // 
            btnLeftPageNo.Enabled = false;
            btnLeftPageNo.Location = new Point(12, 150);
            btnLeftPageNo.Name = "btnLeftPageNo";
            btnLeftPageNo.Size = new Size(27, 27);
            btnLeftPageNo.TabIndex = 7;
            btnLeftPageNo.Text = "<";
            btnLeftPageNo.UseVisualStyleBackColor = true;
            btnLeftPageNo.Click += OnPrevPage;
            // 
            // btnRightPageNo
            // 
            btnRightPageNo.Enabled = false;
            btnRightPageNo.Location = new Point(91, 150);
            btnRightPageNo.Name = "btnRightPageNo";
            btnRightPageNo.Size = new Size(27, 27);
            btnRightPageNo.TabIndex = 9;
            btnRightPageNo.Text = ">";
            btnRightPageNo.UseVisualStyleBackColor = true;
            btnRightPageNo.Click += OnNextPage;
            // 
            // txtPageNum
            // 
            txtPageNum.Enabled = false;
            txtPageNum.Location = new Point(45, 150);
            txtPageNum.Name = "txtPageNum";
            txtPageNum.Size = new Size(40, 27);
            txtPageNum.TabIndex = 8;
            txtPageNum.Text = "1";
            txtPageNum.TextAlign = HorizontalAlignment.Center;
            txtPageNum.TextChanged += OnPageNumChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 703);
            Controls.Add(txtPageNum);
            Controls.Add(btnRightPageNo);
            Controls.Add(btnLeftPageNo);
            Controls.Add(txtTitle);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(drawingPanel);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            MinimumSize = new Size(700, 600);
            Name = "MainForm";
            Text = "MPPG Compare";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
        private Label label1;
        private TextBox txtDCMFile;
        private TextBox txtASCFile;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label4;
        private TextBox txtASCMeasurements;
        private TextBox txtDCMStatus;
        private TextBox txtASCStatus;
        private TextBox txtDCMOffset;
        private Label label8;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private TableLayoutPanel drawingPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox txtTitle;
        private Button btnLeftPageNo;
        private Button btnRightPageNo;
        private TextBox txtPageNum;
    }
}
