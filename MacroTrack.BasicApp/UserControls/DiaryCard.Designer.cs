namespace MacroTrack.BasicApp.UserControls
{
    partial class DiaryCard
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
            tlpBase = new TableLayoutPanel();
            labelBody = new Label();
            labelHeader = new Label();
            flpFooter = new FlowLayoutPanel();
            btEdit = new Button();
            btDelete = new Button();
            btViewDay = new Button();
            tlpBase.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tlpBase
            // 
            tlpBase.AutoSize = true;
            tlpBase.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpBase.ColumnCount = 1;
            tlpBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBase.Controls.Add(labelBody, 0, 1);
            tlpBase.Controls.Add(labelHeader, 0, 0);
            tlpBase.Controls.Add(flpFooter, 0, 2);
            tlpBase.Dock = DockStyle.Fill;
            tlpBase.Location = new Point(0, 0);
            tlpBase.Name = "tlpBase";
            tlpBase.RowCount = 3;
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.Size = new Size(354, 130);
            tlpBase.TabIndex = 0;
            // 
            // labelBody
            // 
            labelBody.AutoSize = true;
            labelBody.Location = new Point(10, 55);
            labelBody.Margin = new Padding(10);
            labelBody.Name = "labelBody";
            labelBody.Size = new Size(320, 25);
            labelBody.TabIndex = 1;
            labelBody.Text = "This will be the body of the diary entry.";
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHeader.Location = new Point(10, 10);
            labelHeader.Margin = new Padding(10);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(272, 25);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Entry [#]: yyyy/mm/dd hh:mm";
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(btEdit);
            flpFooter.Controls.Add(btDelete);
            flpFooter.Controls.Add(btViewDay);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.Location = new Point(0, 90);
            flpFooter.Margin = new Padding(0);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(354, 40);
            flpFooter.TabIndex = 1;
            flpFooter.WrapContents = false;
            // 
            // btEdit
            // 
            btEdit.Location = new Point(3, 3);
            btEdit.Name = "btEdit";
            btEdit.Size = new Size(112, 34);
            btEdit.TabIndex = 0;
            btEdit.Text = "Edit";
            btEdit.UseVisualStyleBackColor = true;
            btEdit.Click += btEdit_Click;
            // 
            // btDelete
            // 
            btDelete.Location = new Point(121, 3);
            btDelete.Name = "btDelete";
            btDelete.Size = new Size(112, 34);
            btDelete.TabIndex = 1;
            btDelete.Text = "Delete";
            btDelete.UseVisualStyleBackColor = true;
            btDelete.Click += btDelete_Click;
            // 
            // btViewDay
            // 
            btViewDay.Location = new Point(239, 3);
            btViewDay.Name = "btViewDay";
            btViewDay.Size = new Size(112, 34);
            btViewDay.TabIndex = 2;
            btViewDay.Text = "View Day";
            btViewDay.UseVisualStyleBackColor = true;
            btViewDay.Click += btViewDay_Click;
            // 
            // DiaryCard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.Window;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(tlpBase);
            Name = "DiaryCard";
            Size = new Size(354, 130);
            tlpBase.ResumeLayout(false);
            tlpBase.PerformLayout();
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpBase;
        private Label labelHeader;
        private FlowLayoutPanel flpFooter;
        private Button btEdit;
        private Button btDelete;
        private Button btViewDay;
        private Label labelBody;
    }
}
