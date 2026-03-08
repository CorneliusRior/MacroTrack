namespace MacroTrack.BasicApp
{
    partial class DiaryView
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
            tlpBase = new TableLayoutPanel();
            cbTimeFrame = new ComboBox();
            labelTitle = new Label();
            flpMain = new FlowLayoutPanel();
            tlpBase.SuspendLayout();
            SuspendLayout();
            // 
            // tlpBase
            // 
            tlpBase.AutoSize = true;
            tlpBase.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpBase.ColumnCount = 2;
            tlpBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBase.ColumnStyles.Add(new ColumnStyle());
            tlpBase.Controls.Add(cbTimeFrame, 1, 0);
            tlpBase.Controls.Add(labelTitle, 0, 0);
            tlpBase.Controls.Add(flpMain, 0, 1);
            tlpBase.Dock = DockStyle.Fill;
            tlpBase.Location = new Point(0, 0);
            tlpBase.Name = "tlpBase";
            tlpBase.RowCount = 2;
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBase.Size = new Size(1046, 741);
            tlpBase.TabIndex = 0;
            // 
            // cbTimeFrame
            // 
            cbTimeFrame.Anchor = AnchorStyles.Right;
            cbTimeFrame.FormattingEnabled = true;
            cbTimeFrame.Items.AddRange(new object[] { "Previous Week", "Previous Month", "Previous Year", "All" });
            cbTimeFrame.Location = new Point(861, 7);
            cbTimeFrame.Name = "cbTimeFrame";
            cbTimeFrame.Size = new Size(182, 33);
            cbTimeFrame.TabIndex = 0;
            cbTimeFrame.SelectedIndexChanged += cbTimeFrame_SelectedIndexChanged;
            // 
            // labelTitle
            // 
            labelTitle.Anchor = AnchorStyles.Left;
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(3, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(573, 48);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "Diary: yyyy/mm/dd to yyyy/mm/dd";
            // 
            // flpMain
            // 
            flpMain.AutoScroll = true;
            flpMain.BackColor = SystemColors.GradientActiveCaption;
            tlpBase.SetColumnSpan(flpMain, 2);
            flpMain.Dock = DockStyle.Fill;
            flpMain.FlowDirection = FlowDirection.TopDown;
            flpMain.Location = new Point(3, 51);
            flpMain.Name = "flpMain";
            flpMain.Padding = new Padding(5);
            flpMain.Size = new Size(1040, 687);
            flpMain.TabIndex = 2;
            flpMain.WrapContents = false;
            flpMain.SizeChanged += flpMain_SizeChanged;
            // 
            // DiaryView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1046, 741);
            Controls.Add(tlpBase);
            Name = "DiaryView";
            Text = "Diary";
            tlpBase.ResumeLayout(false);
            tlpBase.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpBase;
        private ComboBox cbTimeFrame;
        private Label labelTitle;
        private FlowLayoutPanel flpMain;
    }
}