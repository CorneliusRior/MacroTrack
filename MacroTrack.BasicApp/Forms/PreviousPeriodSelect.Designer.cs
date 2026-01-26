namespace MacroTrack.BasicApp.Forms
{
    partial class PreviousPeriodSelect
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
            labelHeader = new Label();
            panelTimeFrame = new Panel();
            gpTimeFrame = new GroupBox();
            tlpTimeFrame = new TableLayoutPanel();
            rbDay = new RadioButton();
            panelDay = new Panel();
            flpDay = new FlowLayoutPanel();
            dtpDay = new DateTimePicker();
            rbDayCalendar = new RadioButton();
            rbDay24Hours = new RadioButton();
            dtpDayTime = new DateTimePicker();
            rbWeek = new RadioButton();
            panelWeek = new Panel();
            flpWeek = new FlowLayoutPanel();
            dtpWeek = new DateTimePicker();
            rbWeekCalendar = new RadioButton();
            rbWeek7Days = new RadioButton();
            rbMonth = new RadioButton();
            panelMonth = new Panel();
            flpMonth = new FlowLayoutPanel();
            dtpMonth = new DateTimePicker();
            rbMonthCalendar = new RadioButton();
            rbMonth30Days = new RadioButton();
            rbCustom = new RadioButton();
            panelCustom = new Panel();
            flpCustom = new FlowLayoutPanel();
            tlpCustomFrom = new TableLayoutPanel();
            labelCustomFrom = new Label();
            dtpCustomFrom = new DateTimePicker();
            tlpCustomTo = new TableLayoutPanel();
            LabelCustomTo = new Label();
            dtpCustomTo = new DateTimePicker();
            flpFooter = new FlowLayoutPanel();
            buttonCancel = new Button();
            buttonView = new Button();
            gbQuick = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonQYesterday = new Button();
            buttonQWeek = new Button();
            tableLayoutPanel1.SuspendLayout();
            panelTimeFrame.SuspendLayout();
            gpTimeFrame.SuspendLayout();
            tlpTimeFrame.SuspendLayout();
            panelDay.SuspendLayout();
            flpDay.SuspendLayout();
            panelWeek.SuspendLayout();
            flpWeek.SuspendLayout();
            panelMonth.SuspendLayout();
            flpMonth.SuspendLayout();
            panelCustom.SuspendLayout();
            flpCustom.SuspendLayout();
            tlpCustomFrom.SuspendLayout();
            tlpCustomTo.SuspendLayout();
            flpFooter.SuspendLayout();
            gbQuick.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanel1.Controls.Add(panelTimeFrame, 0, 2);
            tableLayoutPanel1.Controls.Add(flpFooter, 0, 3);
            tableLayoutPanel1.Controls.Add(gbQuick, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(739, 642);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.Anchor = AnchorStyles.Left;
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeader.Location = new Point(3, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(291, 38);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Select Previous Period";
            // 
            // panelTimeFrame
            // 
            panelTimeFrame.AutoScroll = true;
            panelTimeFrame.Controls.Add(gpTimeFrame);
            panelTimeFrame.Dock = DockStyle.Fill;
            panelTimeFrame.Location = new Point(3, 137);
            panelTimeFrame.Name = "panelTimeFrame";
            panelTimeFrame.Size = new Size(733, 456);
            panelTimeFrame.TabIndex = 1;
            // 
            // gpTimeFrame
            // 
            gpTimeFrame.Controls.Add(tlpTimeFrame);
            gpTimeFrame.Dock = DockStyle.Fill;
            gpTimeFrame.FlatStyle = FlatStyle.Popup;
            gpTimeFrame.Location = new Point(0, 0);
            gpTimeFrame.Name = "gpTimeFrame";
            gpTimeFrame.Size = new Size(733, 456);
            gpTimeFrame.TabIndex = 1;
            gpTimeFrame.TabStop = false;
            gpTimeFrame.Text = "Timeframe";
            // 
            // tlpTimeFrame
            // 
            tlpTimeFrame.ColumnCount = 1;
            tlpTimeFrame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTimeFrame.Controls.Add(rbDay, 0, 0);
            tlpTimeFrame.Controls.Add(panelDay, 0, 1);
            tlpTimeFrame.Controls.Add(rbWeek, 0, 2);
            tlpTimeFrame.Controls.Add(panelWeek, 0, 3);
            tlpTimeFrame.Controls.Add(rbMonth, 0, 4);
            tlpTimeFrame.Controls.Add(panelMonth, 0, 5);
            tlpTimeFrame.Controls.Add(rbCustom, 0, 6);
            tlpTimeFrame.Controls.Add(panelCustom, 0, 7);
            tlpTimeFrame.Dock = DockStyle.Fill;
            tlpTimeFrame.Location = new Point(3, 27);
            tlpTimeFrame.Name = "tlpTimeFrame";
            tlpTimeFrame.RowCount = 8;
            tlpTimeFrame.RowStyles.Add(new RowStyle());
            tlpTimeFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpTimeFrame.RowStyles.Add(new RowStyle());
            tlpTimeFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpTimeFrame.RowStyles.Add(new RowStyle());
            tlpTimeFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpTimeFrame.RowStyles.Add(new RowStyle());
            tlpTimeFrame.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpTimeFrame.Size = new Size(727, 426);
            tlpTimeFrame.TabIndex = 0;
            // 
            // rbDay
            // 
            rbDay.AutoSize = true;
            rbDay.Location = new Point(3, 3);
            rbDay.Name = "rbDay";
            rbDay.Size = new Size(83, 29);
            rbDay.TabIndex = 0;
            rbDay.Text = "1 Day";
            rbDay.UseVisualStyleBackColor = true;
            rbDay.CheckedChanged += TimeFrameRadio_CheckChanged;
            // 
            // panelDay
            // 
            panelDay.Controls.Add(flpDay);
            panelDay.Dock = DockStyle.Fill;
            panelDay.Enabled = false;
            panelDay.Location = new Point(3, 38);
            panelDay.Name = "panelDay";
            panelDay.Padding = new Padding(10);
            panelDay.Size = new Size(721, 65);
            panelDay.TabIndex = 4;
            // 
            // flpDay
            // 
            flpDay.AutoSize = true;
            flpDay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpDay.Controls.Add(dtpDay);
            flpDay.Controls.Add(rbDayCalendar);
            flpDay.Controls.Add(rbDay24Hours);
            flpDay.Controls.Add(dtpDayTime);
            flpDay.Dock = DockStyle.Fill;
            flpDay.Location = new Point(10, 10);
            flpDay.Name = "flpDay";
            flpDay.Size = new Size(701, 45);
            flpDay.TabIndex = 0;
            // 
            // dtpDay
            // 
            dtpDay.Location = new Point(3, 3);
            dtpDay.Name = "dtpDay";
            dtpDay.Size = new Size(218, 31);
            dtpDay.TabIndex = 0;
            // 
            // rbDayCalendar
            // 
            rbDayCalendar.AutoSize = true;
            rbDayCalendar.Checked = true;
            rbDayCalendar.Location = new Point(227, 3);
            rbDayCalendar.Name = "rbDayCalendar";
            rbDayCalendar.Size = new Size(142, 29);
            rbDayCalendar.TabIndex = 1;
            rbDayCalendar.TabStop = true;
            rbDayCalendar.Text = "Calendar Day";
            rbDayCalendar.UseVisualStyleBackColor = true;
            // 
            // rbDay24Hours
            // 
            rbDay24Hours.AutoSize = true;
            rbDay24Hours.Location = new Point(375, 3);
            rbDay24Hours.Name = "rbDay24Hours";
            rbDay24Hours.Size = new Size(151, 29);
            rbDay24Hours.TabIndex = 2;
            rbDay24Hours.Text = "Next 24 Hours";
            rbDay24Hours.UseVisualStyleBackColor = true;
            rbDay24Hours.CheckedChanged += rbDay24Hours_CheckedChanged;
            // 
            // dtpDayTime
            // 
            dtpDayTime.CustomFormat = "HH:mm";
            dtpDayTime.Enabled = false;
            dtpDayTime.Format = DateTimePickerFormat.Time;
            dtpDayTime.Location = new Point(532, 3);
            dtpDayTime.Name = "dtpDayTime";
            dtpDayTime.ShowUpDown = true;
            dtpDayTime.Size = new Size(100, 31);
            dtpDayTime.TabIndex = 3;
            // 
            // rbWeek
            // 
            rbWeek.AutoSize = true;
            rbWeek.Location = new Point(3, 109);
            rbWeek.Name = "rbWeek";
            rbWeek.Size = new Size(95, 29);
            rbWeek.TabIndex = 1;
            rbWeek.Text = "1 Week";
            rbWeek.UseVisualStyleBackColor = true;
            rbWeek.CheckedChanged += TimeFrameRadio_CheckChanged;
            // 
            // panelWeek
            // 
            panelWeek.Controls.Add(flpWeek);
            panelWeek.Dock = DockStyle.Fill;
            panelWeek.Enabled = false;
            panelWeek.Location = new Point(3, 144);
            panelWeek.Name = "panelWeek";
            panelWeek.Padding = new Padding(10);
            panelWeek.Size = new Size(721, 65);
            panelWeek.TabIndex = 5;
            // 
            // flpWeek
            // 
            flpWeek.AutoSize = true;
            flpWeek.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpWeek.Controls.Add(dtpWeek);
            flpWeek.Controls.Add(rbWeekCalendar);
            flpWeek.Controls.Add(rbWeek7Days);
            flpWeek.Dock = DockStyle.Fill;
            flpWeek.Location = new Point(10, 10);
            flpWeek.Name = "flpWeek";
            flpWeek.Size = new Size(701, 45);
            flpWeek.TabIndex = 1;
            // 
            // dtpWeek
            // 
            dtpWeek.Location = new Point(3, 3);
            dtpWeek.Name = "dtpWeek";
            dtpWeek.Size = new Size(218, 31);
            dtpWeek.TabIndex = 0;
            // 
            // rbWeekCalendar
            // 
            rbWeekCalendar.AutoSize = true;
            rbWeekCalendar.Checked = true;
            rbWeekCalendar.Location = new Point(227, 3);
            rbWeekCalendar.Name = "rbWeekCalendar";
            rbWeekCalendar.Size = new Size(154, 29);
            rbWeekCalendar.TabIndex = 1;
            rbWeekCalendar.TabStop = true;
            rbWeekCalendar.Text = "Calendar Week";
            rbWeekCalendar.UseVisualStyleBackColor = true;
            // 
            // rbWeek7Days
            // 
            rbWeek7Days.AutoSize = true;
            rbWeek7Days.Location = new Point(387, 3);
            rbWeek7Days.Name = "rbWeek7Days";
            rbWeek7Days.Size = new Size(132, 29);
            rbWeek7Days.TabIndex = 2;
            rbWeek7Days.Text = "Next 7 Days";
            rbWeek7Days.UseVisualStyleBackColor = true;
            // 
            // rbMonth
            // 
            rbMonth.AutoSize = true;
            rbMonth.Location = new Point(3, 215);
            rbMonth.Name = "rbMonth";
            rbMonth.Size = new Size(105, 29);
            rbMonth.TabIndex = 2;
            rbMonth.Text = "1 Month";
            rbMonth.UseVisualStyleBackColor = true;
            rbMonth.CheckedChanged += TimeFrameRadio_CheckChanged;
            // 
            // panelMonth
            // 
            panelMonth.Controls.Add(flpMonth);
            panelMonth.Dock = DockStyle.Fill;
            panelMonth.Enabled = false;
            panelMonth.Location = new Point(3, 250);
            panelMonth.Name = "panelMonth";
            panelMonth.Padding = new Padding(10);
            panelMonth.Size = new Size(721, 65);
            panelMonth.TabIndex = 6;
            // 
            // flpMonth
            // 
            flpMonth.AutoSize = true;
            flpMonth.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpMonth.Controls.Add(dtpMonth);
            flpMonth.Controls.Add(rbMonthCalendar);
            flpMonth.Controls.Add(rbMonth30Days);
            flpMonth.Dock = DockStyle.Fill;
            flpMonth.Location = new Point(10, 10);
            flpMonth.Name = "flpMonth";
            flpMonth.Size = new Size(701, 45);
            flpMonth.TabIndex = 1;
            // 
            // dtpMonth
            // 
            dtpMonth.Location = new Point(3, 3);
            dtpMonth.Name = "dtpMonth";
            dtpMonth.Size = new Size(218, 31);
            dtpMonth.TabIndex = 0;
            // 
            // rbMonthCalendar
            // 
            rbMonthCalendar.AutoSize = true;
            rbMonthCalendar.Checked = true;
            rbMonthCalendar.Location = new Point(227, 3);
            rbMonthCalendar.Name = "rbMonthCalendar";
            rbMonthCalendar.Size = new Size(164, 29);
            rbMonthCalendar.TabIndex = 1;
            rbMonthCalendar.TabStop = true;
            rbMonthCalendar.Text = "Calendar Month";
            rbMonthCalendar.UseVisualStyleBackColor = true;
            // 
            // rbMonth30Days
            // 
            rbMonth30Days.AutoSize = true;
            rbMonth30Days.Location = new Point(397, 3);
            rbMonth30Days.Name = "rbMonth30Days";
            rbMonth30Days.Size = new Size(142, 29);
            rbMonth30Days.TabIndex = 2;
            rbMonth30Days.Text = "Next 30 Days";
            rbMonth30Days.UseVisualStyleBackColor = true;
            // 
            // rbCustom
            // 
            rbCustom.AutoSize = true;
            rbCustom.Location = new Point(3, 321);
            rbCustom.Name = "rbCustom";
            rbCustom.Size = new Size(99, 29);
            rbCustom.TabIndex = 3;
            rbCustom.Text = "Custom";
            rbCustom.UseVisualStyleBackColor = true;
            rbCustom.CheckedChanged += TimeFrameRadio_CheckChanged;
            // 
            // panelCustom
            // 
            panelCustom.Controls.Add(flpCustom);
            panelCustom.Dock = DockStyle.Fill;
            panelCustom.Enabled = false;
            panelCustom.Location = new Point(3, 356);
            panelCustom.Name = "panelCustom";
            panelCustom.Padding = new Padding(10);
            panelCustom.Size = new Size(721, 67);
            panelCustom.TabIndex = 7;
            // 
            // flpCustom
            // 
            flpCustom.AutoSize = true;
            flpCustom.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCustom.Controls.Add(tlpCustomFrom);
            flpCustom.Controls.Add(tlpCustomTo);
            flpCustom.Dock = DockStyle.Fill;
            flpCustom.Location = new Point(10, 10);
            flpCustom.Name = "flpCustom";
            flpCustom.Size = new Size(701, 47);
            flpCustom.TabIndex = 0;
            // 
            // tlpCustomFrom
            // 
            tlpCustomFrom.AutoSize = true;
            tlpCustomFrom.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCustomFrom.ColumnCount = 2;
            tlpCustomFrom.ColumnStyles.Add(new ColumnStyle());
            tlpCustomFrom.ColumnStyles.Add(new ColumnStyle());
            tlpCustomFrom.Controls.Add(labelCustomFrom, 0, 0);
            tlpCustomFrom.Controls.Add(dtpCustomFrom, 1, 0);
            tlpCustomFrom.Location = new Point(3, 3);
            tlpCustomFrom.Name = "tlpCustomFrom";
            tlpCustomFrom.RowCount = 1;
            tlpCustomFrom.RowStyles.Add(new RowStyle());
            tlpCustomFrom.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpCustomFrom.Size = new Size(370, 37);
            tlpCustomFrom.TabIndex = 4;
            // 
            // labelCustomFrom
            // 
            labelCustomFrom.Anchor = AnchorStyles.Left;
            labelCustomFrom.AutoSize = true;
            labelCustomFrom.Location = new Point(3, 6);
            labelCustomFrom.Name = "labelCustomFrom";
            labelCustomFrom.Size = new Size(58, 25);
            labelCustomFrom.TabIndex = 2;
            labelCustomFrom.Text = "From:";
            // 
            // dtpCustomFrom
            // 
            dtpCustomFrom.Anchor = AnchorStyles.Left;
            dtpCustomFrom.CustomFormat = "dd MMMM yyyy HH:mm:ss";
            dtpCustomFrom.Format = DateTimePickerFormat.Custom;
            dtpCustomFrom.Location = new Point(67, 3);
            dtpCustomFrom.Name = "dtpCustomFrom";
            dtpCustomFrom.Size = new Size(300, 31);
            dtpCustomFrom.TabIndex = 0;
            // 
            // tlpCustomTo
            // 
            tlpCustomTo.AutoSize = true;
            tlpCustomTo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCustomTo.ColumnCount = 2;
            tlpCustomTo.ColumnStyles.Add(new ColumnStyle());
            tlpCustomTo.ColumnStyles.Add(new ColumnStyle());
            tlpCustomTo.Controls.Add(LabelCustomTo, 0, 0);
            tlpCustomTo.Controls.Add(dtpCustomTo, 1, 0);
            tlpCustomTo.Location = new Point(3, 46);
            tlpCustomTo.Name = "tlpCustomTo";
            tlpCustomTo.RowCount = 1;
            tlpCustomTo.RowStyles.Add(new RowStyle());
            tlpCustomTo.Size = new Size(346, 37);
            tlpCustomTo.TabIndex = 5;
            // 
            // LabelCustomTo
            // 
            LabelCustomTo.Anchor = AnchorStyles.Left;
            LabelCustomTo.AutoSize = true;
            LabelCustomTo.Location = new Point(3, 6);
            LabelCustomTo.Name = "LabelCustomTo";
            LabelCustomTo.Size = new Size(34, 25);
            LabelCustomTo.TabIndex = 2;
            LabelCustomTo.Text = "To:";
            // 
            // dtpCustomTo
            // 
            dtpCustomTo.Anchor = AnchorStyles.Left;
            dtpCustomTo.CustomFormat = "dd MMMM yyyy HH:mm:ss";
            dtpCustomTo.Format = DateTimePickerFormat.Custom;
            dtpCustomTo.Location = new Point(43, 3);
            dtpCustomTo.Name = "dtpCustomTo";
            dtpCustomTo.Size = new Size(300, 31);
            dtpCustomTo.TabIndex = 0;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(buttonCancel);
            flpFooter.Controls.Add(buttonView);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 599);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(733, 40);
            flpFooter.TabIndex = 2;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(618, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonView
            // 
            buttonView.Location = new Point(500, 3);
            buttonView.Name = "buttonView";
            buttonView.Size = new Size(112, 34);
            buttonView.TabIndex = 1;
            buttonView.Text = "View";
            buttonView.UseVisualStyleBackColor = true;
            buttonView.Click += buttonView_Click;
            // 
            // gbQuick
            // 
            gbQuick.AutoSize = true;
            gbQuick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gbQuick.Controls.Add(flowLayoutPanel1);
            gbQuick.Dock = DockStyle.Fill;
            gbQuick.Location = new Point(3, 41);
            gbQuick.Name = "gbQuick";
            gbQuick.Size = new Size(733, 90);
            gbQuick.TabIndex = 3;
            gbQuick.TabStop = false;
            gbQuick.Text = "Quick";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(buttonQYesterday);
            flowLayoutPanel1.Controls.Add(buttonQWeek);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 27);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.Size = new Size(727, 60);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // buttonQYesterday
            // 
            buttonQYesterday.Location = new Point(13, 13);
            buttonQYesterday.Name = "buttonQYesterday";
            buttonQYesterday.Size = new Size(112, 34);
            buttonQYesterday.TabIndex = 0;
            buttonQYesterday.Text = "Yesterday";
            buttonQYesterday.UseVisualStyleBackColor = true;
            buttonQYesterday.Click += buttonQYesterday_Click;
            // 
            // buttonQWeek
            // 
            buttonQWeek.Location = new Point(131, 13);
            buttonQWeek.Name = "buttonQWeek";
            buttonQWeek.Size = new Size(112, 34);
            buttonQWeek.TabIndex = 1;
            buttonQWeek.Text = "This week";
            buttonQWeek.UseVisualStyleBackColor = true;
            buttonQWeek.Click += buttonQWeek_Click;
            // 
            // PreviousPeriodSelect
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(739, 642);
            Controls.Add(tableLayoutPanel1);
            Name = "PreviousPeriodSelect";
            Text = "Previous Periods";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panelTimeFrame.ResumeLayout(false);
            gpTimeFrame.ResumeLayout(false);
            tlpTimeFrame.ResumeLayout(false);
            tlpTimeFrame.PerformLayout();
            panelDay.ResumeLayout(false);
            panelDay.PerformLayout();
            flpDay.ResumeLayout(false);
            flpDay.PerformLayout();
            panelWeek.ResumeLayout(false);
            panelWeek.PerformLayout();
            flpWeek.ResumeLayout(false);
            flpWeek.PerformLayout();
            panelMonth.ResumeLayout(false);
            panelMonth.PerformLayout();
            flpMonth.ResumeLayout(false);
            flpMonth.PerformLayout();
            panelCustom.ResumeLayout(false);
            panelCustom.PerformLayout();
            flpCustom.ResumeLayout(false);
            flpCustom.PerformLayout();
            tlpCustomFrom.ResumeLayout(false);
            tlpCustomFrom.PerformLayout();
            tlpCustomTo.ResumeLayout(false);
            tlpCustomTo.PerformLayout();
            flpFooter.ResumeLayout(false);
            gbQuick.ResumeLayout(false);
            gbQuick.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label labelHeader;
        private GroupBox gpTimeFrame;
        private FlowLayoutPanel flpFooter;
        private Button buttonCancel;
        private Button buttonView;
        private TableLayoutPanel tlpTimeFrame;
        private RadioButton rbDay;
        private RadioButton rbWeek;
        private RadioButton rbMonth;
        private RadioButton rbCustom;
        private Panel panelDay;
        private FlowLayoutPanel flpDay;
        private DateTimePicker dtpDay;
        private Panel panelWeek;
        private FlowLayoutPanel flpWeek;
        private DateTimePicker dtpWeek;
        private Panel panelMonth;
        private FlowLayoutPanel flpMonth;
        private DateTimePicker dtpMonth;
        private Panel panelCustom;
        private RadioButton rbWeekCalendar;
        private RadioButton rbMonthCalendar;
        private RadioButton rbMonth30Days;
        private RadioButton rbDayCalendar;
        private RadioButton rbDay24Hours;
        private DateTimePicker dtpDayTime;
        private RadioButton rbWeek7Days;
        private FlowLayoutPanel flpCustom;
        private DateTimePicker dtpCustomFrom;
        private TableLayoutPanel tlpCustomFrom;
        private Label labelCustomFrom;
        private TableLayoutPanel tlpCustomTo;
        private DateTimePicker dtpCustomTo;
        private Label LabelCustomTo;
        private Panel panelTimeFrame;
        private GroupBox gbQuick;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonQYesterday;
        private Button buttonQWeek;
    }
}