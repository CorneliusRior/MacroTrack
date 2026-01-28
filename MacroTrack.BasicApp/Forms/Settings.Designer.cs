namespace MacroTrack.BasicApp.Forms
{
    partial class Settings
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            labelTItle = new Label();
            buttonDefault = new Button();
            buttonRevert = new Button();
            flpFooter = new FlowLayoutPanel();
            buttonCancel = new Button();
            buttonApply = new Button();
            panelMain = new Panel();
            gpLogging = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            tlpLogREPL = new TableLayoutPanel();
            labelLogREPLHeader = new Label();
            rbREPLDebug = new RadioButton();
            rbREPLInfo = new RadioButton();
            rbREPLWarning = new RadioButton();
            rbREPLError = new RadioButton();
            tlpLogLog = new TableLayoutPanel();
            labelLogLogHeader = new Label();
            rbLogLogDebug = new RadioButton();
            rbLogLogInfo = new RadioButton();
            rbLogLogWarning = new RadioButton();
            rbLogLogError = new RadioButton();
            gpWeightFormat = new GroupBox();
            tlpWeightFormat = new TableLayoutPanel();
            tlpWeightFormatOptions = new TableLayoutPanel();
            labelWeightFormatHeader = new Label();
            rbWFKg = new RadioButton();
            rbWFLbs = new RadioButton();
            rbWFSt = new RadioButton();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            flpFooter.SuspendLayout();
            panelMain.SuspendLayout();
            gpLogging.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tlpLogREPL.SuspendLayout();
            tlpLogLog.SuspendLayout();
            gpWeightFormat.SuspendLayout();
            tlpWeightFormat.SuspendLayout();
            tlpWeightFormatOptions.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(flpFooter, 0, 2);
            tableLayoutPanel1.Controls.Add(panelMain, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(800, 730);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(labelTItle, 0, 0);
            tableLayoutPanel3.Controls.Add(buttonDefault, 2, 0);
            tableLayoutPanel3.Controls.Add(buttonRevert, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(794, 40);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // labelTItle
            // 
            labelTItle.AutoSize = true;
            labelTItle.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTItle.Location = new Point(3, 0);
            labelTItle.Name = "labelTItle";
            labelTItle.Size = new Size(116, 38);
            labelTItle.TabIndex = 0;
            labelTItle.Text = "Settings";
            // 
            // buttonDefault
            // 
            buttonDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDefault.Location = new Point(679, 3);
            buttonDefault.Name = "buttonDefault";
            buttonDefault.RightToLeft = RightToLeft.No;
            buttonDefault.Size = new Size(112, 34);
            buttonDefault.TabIndex = 1;
            buttonDefault.Text = "Default";
            buttonDefault.UseVisualStyleBackColor = true;
            buttonDefault.Click += buttonDefault_Click;
            // 
            // buttonRevert
            // 
            buttonRevert.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRevert.Location = new Point(561, 3);
            buttonRevert.Name = "buttonRevert";
            buttonRevert.Size = new Size(112, 34);
            buttonRevert.TabIndex = 2;
            buttonRevert.Text = "Revert";
            buttonRevert.UseVisualStyleBackColor = true;
            buttonRevert.Click += buttonRevert_Click;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(buttonCancel);
            flpFooter.Controls.Add(buttonApply);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 687);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(794, 40);
            flpFooter.TabIndex = 1;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(679, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonApply
            // 
            buttonApply.Location = new Point(561, 3);
            buttonApply.Name = "buttonApply";
            buttonApply.Size = new Size(112, 34);
            buttonApply.TabIndex = 1;
            buttonApply.Text = "Apply";
            buttonApply.UseVisualStyleBackColor = true;
            buttonApply.Click += buttonApply_Click;
            // 
            // panelMain
            // 
            panelMain.Controls.Add(gpLogging);
            panelMain.Controls.Add(gpWeightFormat);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(3, 49);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(794, 632);
            panelMain.TabIndex = 2;
            // 
            // gpLogging
            // 
            gpLogging.AutoSize = true;
            gpLogging.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gpLogging.Controls.Add(tableLayoutPanel2);
            gpLogging.Dock = DockStyle.Top;
            gpLogging.Location = new Point(0, 166);
            gpLogging.Name = "gpLogging";
            gpLogging.Size = new Size(794, 372);
            gpLogging.TabIndex = 0;
            gpLogging.TabStop = false;
            gpLogging.Text = "Logging";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(tlpLogREPL, 0, 0);
            tableLayoutPanel2.Controls.Add(tlpLogLog, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 27);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(788, 342);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tlpLogREPL
            // 
            tlpLogREPL.AutoSize = true;
            tlpLogREPL.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpLogREPL.ColumnCount = 1;
            tlpLogREPL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLogREPL.Controls.Add(labelLogREPLHeader, 0, 0);
            tlpLogREPL.Controls.Add(rbREPLDebug, 0, 1);
            tlpLogREPL.Controls.Add(rbREPLInfo, 0, 2);
            tlpLogREPL.Controls.Add(rbREPLWarning, 0, 3);
            tlpLogREPL.Controls.Add(rbREPLError, 0, 4);
            tlpLogREPL.Location = new Point(3, 3);
            tlpLogREPL.Name = "tlpLogREPL";
            tlpLogREPL.RowCount = 5;
            tlpLogREPL.RowStyles.Add(new RowStyle());
            tlpLogREPL.RowStyles.Add(new RowStyle());
            tlpLogREPL.RowStyles.Add(new RowStyle());
            tlpLogREPL.RowStyles.Add(new RowStyle());
            tlpLogREPL.RowStyles.Add(new RowStyle());
            tlpLogREPL.Size = new Size(145, 165);
            tlpLogREPL.TabIndex = 0;
            // 
            // labelLogREPLHeader
            // 
            labelLogREPLHeader.AutoSize = true;
            labelLogREPLHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelLogREPLHeader.Location = new Point(3, 0);
            labelLogREPLHeader.Name = "labelLogREPLHeader";
            labelLogREPLHeader.Size = new Size(139, 25);
            labelLogREPLHeader.TabIndex = 0;
            labelLogREPLHeader.Text = "REPL Threshold";
            // 
            // rbREPLDebug
            // 
            rbREPLDebug.AutoSize = true;
            rbREPLDebug.Location = new Point(3, 28);
            rbREPLDebug.Name = "rbREPLDebug";
            rbREPLDebug.Size = new Size(91, 29);
            rbREPLDebug.TabIndex = 1;
            rbREPLDebug.TabStop = true;
            rbREPLDebug.Text = "Debug";
            rbREPLDebug.UseVisualStyleBackColor = true;
            // 
            // rbREPLInfo
            // 
            rbREPLInfo.AutoSize = true;
            rbREPLInfo.Location = new Point(3, 63);
            rbREPLInfo.Name = "rbREPLInfo";
            rbREPLInfo.Size = new Size(69, 29);
            rbREPLInfo.TabIndex = 2;
            rbREPLInfo.TabStop = true;
            rbREPLInfo.Text = "Info";
            rbREPLInfo.UseVisualStyleBackColor = true;
            // 
            // rbREPLWarning
            // 
            rbREPLWarning.AutoSize = true;
            rbREPLWarning.Location = new Point(3, 98);
            rbREPLWarning.Name = "rbREPLWarning";
            rbREPLWarning.Size = new Size(103, 29);
            rbREPLWarning.TabIndex = 3;
            rbREPLWarning.TabStop = true;
            rbREPLWarning.Text = "Warning";
            rbREPLWarning.UseVisualStyleBackColor = true;
            // 
            // rbREPLError
            // 
            rbREPLError.AutoSize = true;
            rbREPLError.Location = new Point(3, 133);
            rbREPLError.Name = "rbREPLError";
            rbREPLError.Size = new Size(75, 29);
            rbREPLError.TabIndex = 4;
            rbREPLError.TabStop = true;
            rbREPLError.Text = "Error";
            rbREPLError.UseVisualStyleBackColor = true;
            // 
            // tlpLogLog
            // 
            tlpLogLog.AutoSize = true;
            tlpLogLog.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpLogLog.ColumnCount = 1;
            tlpLogLog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLogLog.Controls.Add(labelLogLogHeader, 0, 0);
            tlpLogLog.Controls.Add(rbLogLogDebug, 0, 1);
            tlpLogLog.Controls.Add(rbLogLogInfo, 0, 2);
            tlpLogLog.Controls.Add(rbLogLogWarning, 0, 3);
            tlpLogLog.Controls.Add(rbLogLogError, 0, 4);
            tlpLogLog.Location = new Point(3, 174);
            tlpLogLog.Name = "tlpLogLog";
            tlpLogLog.RowCount = 5;
            tlpLogLog.RowStyles.Add(new RowStyle());
            tlpLogLog.RowStyles.Add(new RowStyle());
            tlpLogLog.RowStyles.Add(new RowStyle());
            tlpLogLog.RowStyles.Add(new RowStyle());
            tlpLogLog.RowStyles.Add(new RowStyle());
            tlpLogLog.Size = new Size(136, 165);
            tlpLogLog.TabIndex = 1;
            // 
            // labelLogLogHeader
            // 
            labelLogLogHeader.AutoSize = true;
            labelLogLogHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelLogLogHeader.Location = new Point(3, 0);
            labelLogLogHeader.Name = "labelLogLogHeader";
            labelLogLogHeader.Size = new Size(130, 25);
            labelLogLogHeader.TabIndex = 0;
            labelLogLogHeader.Text = "Log Threshold";
            // 
            // rbLogLogDebug
            // 
            rbLogLogDebug.AutoSize = true;
            rbLogLogDebug.Location = new Point(3, 28);
            rbLogLogDebug.Name = "rbLogLogDebug";
            rbLogLogDebug.Size = new Size(91, 29);
            rbLogLogDebug.TabIndex = 1;
            rbLogLogDebug.TabStop = true;
            rbLogLogDebug.Text = "Debug";
            rbLogLogDebug.UseVisualStyleBackColor = true;
            // 
            // rbLogLogInfo
            // 
            rbLogLogInfo.AutoSize = true;
            rbLogLogInfo.Location = new Point(3, 63);
            rbLogLogInfo.Name = "rbLogLogInfo";
            rbLogLogInfo.Size = new Size(69, 29);
            rbLogLogInfo.TabIndex = 2;
            rbLogLogInfo.TabStop = true;
            rbLogLogInfo.Text = "Info";
            rbLogLogInfo.UseVisualStyleBackColor = true;
            // 
            // rbLogLogWarning
            // 
            rbLogLogWarning.AutoSize = true;
            rbLogLogWarning.Location = new Point(3, 98);
            rbLogLogWarning.Name = "rbLogLogWarning";
            rbLogLogWarning.Size = new Size(103, 29);
            rbLogLogWarning.TabIndex = 3;
            rbLogLogWarning.TabStop = true;
            rbLogLogWarning.Text = "Warning";
            rbLogLogWarning.UseVisualStyleBackColor = true;
            // 
            // rbLogLogError
            // 
            rbLogLogError.AutoSize = true;
            rbLogLogError.Location = new Point(3, 133);
            rbLogLogError.Name = "rbLogLogError";
            rbLogLogError.Size = new Size(75, 29);
            rbLogLogError.TabIndex = 4;
            rbLogLogError.TabStop = true;
            rbLogLogError.Text = "Error";
            rbLogLogError.UseVisualStyleBackColor = true;
            // 
            // gpWeightFormat
            // 
            gpWeightFormat.AutoSize = true;
            gpWeightFormat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gpWeightFormat.Controls.Add(tlpWeightFormat);
            gpWeightFormat.Dock = DockStyle.Top;
            gpWeightFormat.Location = new Point(0, 0);
            gpWeightFormat.Name = "gpWeightFormat";
            gpWeightFormat.Size = new Size(794, 166);
            gpWeightFormat.TabIndex = 1;
            gpWeightFormat.TabStop = false;
            gpWeightFormat.Text = "Weight format";
            // 
            // tlpWeightFormat
            // 
            tlpWeightFormat.AutoSize = true;
            tlpWeightFormat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpWeightFormat.ColumnCount = 1;
            tlpWeightFormat.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpWeightFormat.Controls.Add(tlpWeightFormatOptions, 0, 0);
            tlpWeightFormat.Dock = DockStyle.Fill;
            tlpWeightFormat.Location = new Point(3, 27);
            tlpWeightFormat.Name = "tlpWeightFormat";
            tlpWeightFormat.RowCount = 1;
            tlpWeightFormat.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpWeightFormat.Size = new Size(788, 136);
            tlpWeightFormat.TabIndex = 3;
            // 
            // tlpWeightFormatOptions
            // 
            tlpWeightFormatOptions.AutoSize = true;
            tlpWeightFormatOptions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpWeightFormatOptions.ColumnCount = 1;
            tlpWeightFormatOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpWeightFormatOptions.Controls.Add(labelWeightFormatHeader, 0, 0);
            tlpWeightFormatOptions.Controls.Add(rbWFKg, 0, 1);
            tlpWeightFormatOptions.Controls.Add(rbWFLbs, 0, 2);
            tlpWeightFormatOptions.Controls.Add(rbWFSt, 0, 3);
            tlpWeightFormatOptions.Location = new Point(3, 3);
            tlpWeightFormatOptions.Name = "tlpWeightFormatOptions";
            tlpWeightFormatOptions.RowCount = 5;
            tlpWeightFormatOptions.RowStyles.Add(new RowStyle());
            tlpWeightFormatOptions.RowStyles.Add(new RowStyle());
            tlpWeightFormatOptions.RowStyles.Add(new RowStyle());
            tlpWeightFormatOptions.RowStyles.Add(new RowStyle());
            tlpWeightFormatOptions.RowStyles.Add(new RowStyle());
            tlpWeightFormatOptions.Size = new Size(149, 130);
            tlpWeightFormatOptions.TabIndex = 2;
            // 
            // labelWeightFormatHeader
            // 
            labelWeightFormatHeader.AutoSize = true;
            labelWeightFormatHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelWeightFormatHeader.Location = new Point(3, 0);
            labelWeightFormatHeader.Name = "labelWeightFormatHeader";
            labelWeightFormatHeader.Size = new Size(136, 25);
            labelWeightFormatHeader.TabIndex = 0;
            labelWeightFormatHeader.Text = "Weight Format";
            // 
            // rbWFKg
            // 
            rbWFKg.AutoSize = true;
            rbWFKg.Location = new Point(3, 28);
            rbWFKg.Name = "rbWFKg";
            rbWFKg.Size = new Size(122, 29);
            rbWFKg.TabIndex = 1;
            rbWFKg.TabStop = true;
            rbWFKg.Text = "Metric (Kg)";
            rbWFKg.UseVisualStyleBackColor = true;
            // 
            // rbWFLbs
            // 
            rbWFLbs.AutoSize = true;
            rbWFLbs.Location = new Point(3, 63);
            rbWFLbs.Name = "rbWFLbs";
            rbWFLbs.Size = new Size(143, 29);
            rbWFLbs.TabIndex = 2;
            rbWFLbs.TabStop = true;
            rbWFLbs.Text = "Imperial (Lbs)";
            rbWFLbs.UseVisualStyleBackColor = true;
            // 
            // rbWFSt
            // 
            rbWFSt.AutoSize = true;
            rbWFSt.Location = new Point(3, 98);
            rbWFSt.Name = "rbWFSt";
            rbWFSt.Size = new Size(131, 29);
            rbWFSt.TabIndex = 3;
            rbWFSt.TabStop = true;
            rbWFSt.Text = "Imperial (St)";
            rbWFSt.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 730);
            Controls.Add(tableLayoutPanel1);
            Name = "Settings";
            Text = "Settings";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            flpFooter.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            gpLogging.ResumeLayout(false);
            gpLogging.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tlpLogREPL.ResumeLayout(false);
            tlpLogREPL.PerformLayout();
            tlpLogLog.ResumeLayout(false);
            tlpLogLog.PerformLayout();
            gpWeightFormat.ResumeLayout(false);
            gpWeightFormat.PerformLayout();
            tlpWeightFormat.ResumeLayout(false);
            tlpWeightFormat.PerformLayout();
            tlpWeightFormatOptions.ResumeLayout(false);
            tlpWeightFormatOptions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label labelTItle;
        private FlowLayoutPanel flpFooter;
        private Button buttonCancel;
        private Button buttonApply;
        private Panel panelMain;
        private GroupBox gpLogging;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tlpLogREPL;
        private Label labelLogREPLHeader;
        private RadioButton rbREPLDebug;
        private RadioButton rbREPLInfo;
        private RadioButton rbREPLWarning;
        private TableLayoutPanel tlpLogLog;
        private Label labelLogLogHeader;
        private RadioButton rbLogLogDebug;
        private RadioButton rbLogLogInfo;
        private RadioButton rbLogLogWarning;
        private RadioButton rbLogLogError;
        private RadioButton rbREPLError;
        private GroupBox gpWeightFormat;
        private TableLayoutPanel tlpWeightFormat;
        private TableLayoutPanel tlpWeightFormatOptions;
        private Label labelWeightFormatHeader;
        private RadioButton rbWFKg;
        private RadioButton rbWFLbs;
        private RadioButton rbWFSt;
        private TableLayoutPanel tableLayoutPanel3;
        private Button buttonDefault;
        private Button buttonRevert;
    }
}