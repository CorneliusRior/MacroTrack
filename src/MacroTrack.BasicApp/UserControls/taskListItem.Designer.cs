namespace MacroTrack.BasicApp
{
    partial class taskListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            checkBoxTask = new CheckBox();
            labelStreak = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // checkBoxTask
            // 
            checkBoxTask.AutoSize = true;
            checkBoxTask.Location = new Point(3, 3);
            checkBoxTask.Name = "checkBoxTask";
            checkBoxTask.Size = new Size(91, 29);
            checkBoxTask.TabIndex = 0;
            checkBoxTask.Text = "ItemName";
            checkBoxTask.UseVisualStyleBackColor = true;
            checkBoxTask.CheckedChanged += checkBoxTask_CheckedChanged;
            // 
            // labelStreak
            // 
            labelStreak.Anchor = AnchorStyles.Right;
            labelStreak.AutoSize = true;
            labelStreak.Location = new Point(100, 5);
            labelStreak.Name = "labelStreak";
            labelStreak.Size = new Size(22, 25);
            labelStreak.TabIndex = 1;
            labelStreak.Text = "0";
            labelStreak.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(checkBoxTask, 0, 0);
            tableLayoutPanel1.Controls.Add(labelStreak, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(125, 35);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // taskListItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(tableLayoutPanel1);
            Name = "taskListItem";
            Size = new Size(125, 35);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBoxTask;
        private Label labelStreak;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
