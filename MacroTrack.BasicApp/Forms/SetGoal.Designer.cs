namespace MacroTrack.BasicApp.Forms
{
    partial class SetGoal
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 1D);
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            labelHeader = new Label();
            dtpStart = new DateTimePicker();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel3 = new TableLayoutPanel();
            labelGoalName = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            labelMT1 = new Label();
            labelMT2 = new Label();
            labelMT3 = new Label();
            labelMT4 = new Label();
            labelMT5 = new Label();
            labelMT6 = new Label();
            labelMT7 = new Label();
            labelMTPP = new Label();
            labelMTPG = new Label();
            labelMTPN = new Label();
            labelMTPX = new Label();
            labelMTCP = new Label();
            labelMTCG = new Label();
            labelMTCN = new Label();
            labelMTCX = new Label();
            labelMTFP = new Label();
            labelMTFG = new Label();
            labelMTFN = new Label();
            labelMTFX = new Label();
            labelNotes = new Label();
            pieChartGoal = new System.Windows.Forms.DataVisualization.Charting.Chart();
            labelType = new Label();
            panel1 = new Panel();
            dgvGoals = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonCancel = new Button();
            buttonSet = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pieChartGoal).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGoals).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(splitContainer1, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1262, 653);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanel2.Controls.Add(dtpStart, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(1256, 38);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // labelHeader
            // 
            labelHeader.Anchor = AnchorStyles.Left;
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeader.Location = new Point(3, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(313, 38);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Set Goal for 23/01/2026";
            // 
            // dtpStart
            // 
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(1091, 3);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(162, 31);
            dtpStart.TabIndex = 1;
            dtpStart.ValueChanged += dtpStart_ValueChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 47);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Size = new Size(1256, 557);
            splitContainer1.SplitterDistance = 418;
            splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoScroll = true;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(labelGoalName, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 3);
            tableLayoutPanel3.Controls.Add(labelNotes, 0, 4);
            tableLayoutPanel3.Controls.Add(pieChartGoal, 0, 1);
            tableLayoutPanel3.Controls.Add(labelType, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 5;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(418, 557);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // labelGoalName
            // 
            labelGoalName.AutoSize = true;
            labelGoalName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGoalName.Location = new Point(3, 0);
            labelGoalName.Name = "labelGoalName";
            labelGoalName.Size = new Size(164, 28);
            labelGoalName.TabIndex = 0;
            labelGoalName.Text = "No Goal Selected";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel4.ColumnCount = 5;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Controls.Add(labelMT1, 1, 0);
            tableLayoutPanel4.Controls.Add(labelMT2, 2, 0);
            tableLayoutPanel4.Controls.Add(labelMT3, 3, 0);
            tableLayoutPanel4.Controls.Add(labelMT4, 4, 0);
            tableLayoutPanel4.Controls.Add(labelMT5, 0, 1);
            tableLayoutPanel4.Controls.Add(labelMT6, 0, 2);
            tableLayoutPanel4.Controls.Add(labelMT7, 0, 3);
            tableLayoutPanel4.Controls.Add(labelMTPP, 1, 1);
            tableLayoutPanel4.Controls.Add(labelMTPG, 2, 1);
            tableLayoutPanel4.Controls.Add(labelMTPN, 3, 1);
            tableLayoutPanel4.Controls.Add(labelMTPX, 4, 1);
            tableLayoutPanel4.Controls.Add(labelMTCP, 1, 2);
            tableLayoutPanel4.Controls.Add(labelMTCG, 2, 2);
            tableLayoutPanel4.Controls.Add(labelMTCN, 3, 2);
            tableLayoutPanel4.Controls.Add(labelMTCX, 4, 2);
            tableLayoutPanel4.Controls.Add(labelMTFP, 1, 3);
            tableLayoutPanel4.Controls.Add(labelMTFG, 2, 3);
            tableLayoutPanel4.Controls.Add(labelMTFN, 3, 3);
            tableLayoutPanel4.Controls.Add(labelMTFX, 4, 3);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 362);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(412, 100);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // labelMT1
            // 
            labelMT1.AutoSize = true;
            labelMT1.Location = new Point(81, 0);
            labelMT1.Name = "labelMT1";
            labelMT1.Size = new Size(27, 25);
            labelMT1.TabIndex = 0;
            labelMT1.Text = "%";
            // 
            // labelMT2
            // 
            labelMT2.AutoSize = true;
            labelMT2.Location = new Point(164, 0);
            labelMT2.Name = "labelMT2";
            labelMT2.Size = new Size(23, 25);
            labelMT2.TabIndex = 1;
            labelMT2.Text = "g";
            // 
            // labelMT3
            // 
            labelMT3.AutoSize = true;
            labelMT3.Location = new Point(247, 0);
            labelMT3.Name = "labelMT3";
            labelMT3.Size = new Size(42, 25);
            labelMT3.TabIndex = 2;
            labelMT3.Text = "Min";
            // 
            // labelMT4
            // 
            labelMT4.AutoSize = true;
            labelMT4.Location = new Point(330, 0);
            labelMT4.Name = "labelMT4";
            labelMT4.Size = new Size(45, 25);
            labelMT4.TabIndex = 3;
            labelMT4.Text = "Max";
            // 
            // labelMT5
            // 
            labelMT5.AutoSize = true;
            labelMT5.Location = new Point(3, 25);
            labelMT5.Name = "labelMT5";
            labelMT5.Size = new Size(72, 25);
            labelMT5.TabIndex = 4;
            labelMT5.Text = "Protein:";
            // 
            // labelMT6
            // 
            labelMT6.AutoSize = true;
            labelMT6.Location = new Point(3, 50);
            labelMT6.Name = "labelMT6";
            labelMT6.Size = new Size(61, 25);
            labelMT6.TabIndex = 5;
            labelMT6.Text = "Carbs:";
            // 
            // labelMT7
            // 
            labelMT7.AutoSize = true;
            labelMT7.Location = new Point(3, 75);
            labelMT7.Name = "labelMT7";
            labelMT7.Size = new Size(39, 25);
            labelMT7.TabIndex = 6;
            labelMT7.Text = "Fat:";
            // 
            // labelMTPP
            // 
            labelMTPP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTPP.AutoSize = true;
            labelMTPP.Location = new Point(139, 25);
            labelMTPP.Name = "labelMTPP";
            labelMTPP.Size = new Size(19, 25);
            labelMTPP.TabIndex = 7;
            labelMTPP.Text = "-";
            // 
            // labelMTPG
            // 
            labelMTPG.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTPG.AutoSize = true;
            labelMTPG.Location = new Point(222, 25);
            labelMTPG.Name = "labelMTPG";
            labelMTPG.Size = new Size(19, 25);
            labelMTPG.TabIndex = 9;
            labelMTPG.Text = "-";
            // 
            // labelMTPN
            // 
            labelMTPN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTPN.AutoSize = true;
            labelMTPN.Location = new Point(305, 25);
            labelMTPN.Name = "labelMTPN";
            labelMTPN.Size = new Size(19, 25);
            labelMTPN.TabIndex = 10;
            labelMTPN.Text = "-";
            // 
            // labelMTPX
            // 
            labelMTPX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTPX.AutoSize = true;
            labelMTPX.Location = new Point(390, 25);
            labelMTPX.Name = "labelMTPX";
            labelMTPX.Size = new Size(19, 25);
            labelMTPX.TabIndex = 11;
            labelMTPX.Text = "-";
            // 
            // labelMTCP
            // 
            labelMTCP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTCP.AutoSize = true;
            labelMTCP.Location = new Point(139, 50);
            labelMTCP.Name = "labelMTCP";
            labelMTCP.Size = new Size(19, 25);
            labelMTCP.TabIndex = 12;
            labelMTCP.Text = "-";
            // 
            // labelMTCG
            // 
            labelMTCG.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTCG.AutoSize = true;
            labelMTCG.Location = new Point(222, 50);
            labelMTCG.Name = "labelMTCG";
            labelMTCG.Size = new Size(19, 25);
            labelMTCG.TabIndex = 13;
            labelMTCG.Text = "-";
            // 
            // labelMTCN
            // 
            labelMTCN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTCN.AutoSize = true;
            labelMTCN.Location = new Point(305, 50);
            labelMTCN.Name = "labelMTCN";
            labelMTCN.Size = new Size(19, 25);
            labelMTCN.TabIndex = 14;
            labelMTCN.Text = "-";
            // 
            // labelMTCX
            // 
            labelMTCX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTCX.AutoSize = true;
            labelMTCX.Location = new Point(390, 50);
            labelMTCX.Name = "labelMTCX";
            labelMTCX.Size = new Size(19, 25);
            labelMTCX.TabIndex = 15;
            labelMTCX.Text = "-";
            // 
            // labelMTFP
            // 
            labelMTFP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTFP.AutoSize = true;
            labelMTFP.Location = new Point(139, 75);
            labelMTFP.Name = "labelMTFP";
            labelMTFP.Size = new Size(19, 25);
            labelMTFP.TabIndex = 16;
            labelMTFP.Text = "-";
            // 
            // labelMTFG
            // 
            labelMTFG.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTFG.AutoSize = true;
            labelMTFG.Location = new Point(222, 75);
            labelMTFG.Name = "labelMTFG";
            labelMTFG.Size = new Size(19, 25);
            labelMTFG.TabIndex = 17;
            labelMTFG.Text = "-";
            // 
            // labelMTFN
            // 
            labelMTFN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTFN.AutoSize = true;
            labelMTFN.Location = new Point(305, 75);
            labelMTFN.Name = "labelMTFN";
            labelMTFN.Size = new Size(19, 25);
            labelMTFN.TabIndex = 18;
            labelMTFN.Text = "-";
            // 
            // labelMTFX
            // 
            labelMTFX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelMTFX.AutoSize = true;
            labelMTFX.Location = new Point(390, 75);
            labelMTFX.Name = "labelMTFX";
            labelMTFX.Size = new Size(19, 25);
            labelMTFX.TabIndex = 19;
            labelMTFX.Text = "-";
            // 
            // labelNotes
            // 
            labelNotes.AutoSize = true;
            labelNotes.Location = new Point(3, 465);
            labelNotes.Name = "labelNotes";
            labelNotes.Size = new Size(19, 25);
            labelNotes.TabIndex = 2;
            labelNotes.Text = "-";
            // 
            // pieChartGoal
            // 
            chartArea1.Name = "ChartArea1";
            pieChartGoal.ChartAreas.Add(chartArea1);
            pieChartGoal.Location = new Point(3, 31);
            pieChartGoal.Name = "pieChartGoal";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            pieChartGoal.Series.Add(series1);
            pieChartGoal.Size = new Size(300, 300);
            pieChartGoal.TabIndex = 3;
            pieChartGoal.Text = "chart1";
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Location = new Point(3, 334);
            labelType.Name = "labelType";
            labelType.Size = new Size(122, 25);
            labelType.TabIndex = 4;
            labelType.Text = "Type: No type";
            // 
            // panel1
            // 
            panel1.Controls.Add(dgvGoals);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(834, 557);
            panel1.TabIndex = 0;
            // 
            // dgvGoals
            // 
            dgvGoals.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGoals.Dock = DockStyle.Fill;
            dgvGoals.Location = new Point(0, 0);
            dgvGoals.Margin = new Padding(50);
            dgvGoals.MultiSelect = false;
            dgvGoals.Name = "dgvGoals";
            dgvGoals.ReadOnly = true;
            dgvGoals.RowHeadersWidth = 62;
            dgvGoals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGoals.Size = new Size(834, 557);
            dgvGoals.TabIndex = 0;
            dgvGoals.SelectionChanged += dgvGoals_SelectionChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(buttonCancel);
            flowLayoutPanel1.Controls.Add(buttonSet);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(3, 610);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1256, 40);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(1141, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonSet
            // 
            buttonSet.Location = new Point(1023, 3);
            buttonSet.Name = "buttonSet";
            buttonSet.Size = new Size(112, 34);
            buttonSet.TabIndex = 1;
            buttonSet.Text = "Set";
            buttonSet.UseVisualStyleBackColor = true;
            buttonSet.Click += buttonSet_Click;
            // 
            // SetGoal
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1262, 653);
            Controls.Add(tableLayoutPanel1);
            Name = "SetGoal";
            Text = "Set Goal";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pieChartGoal).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGoals).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label labelHeader;
        private DateTimePicker dtpStart;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonCancel;
        private Button buttonSet;
        private Panel panel1;
        private DataGridView dgvGoals;
        private TableLayoutPanel tableLayoutPanel3;
        private Label labelGoalName;
        private TableLayoutPanel tableLayoutPanel4;
        private Label labelMT1;
        private Label labelMT2;
        private Label labelMT3;
        private Label labelMT4;
        private Label labelMT5;
        private Label labelMT6;
        private Label labelMT7;
        private Label labelMTPP;
        private Label labelMTPG;
        private Label labelMTPN;
        private Label labelMTPX;
        private Label labelMTCP;
        private Label labelMTCG;
        private Label labelMTCN;
        private Label labelMTCX;
        private Label labelMTFP;
        private Label labelMTFG;
        private Label labelMTFN;
        private Label labelMTFX;
        private Label labelNotes;
        private System.Windows.Forms.DataVisualization.Charting.Chart pieChartGoal;
        private Label labelType;
    }
}