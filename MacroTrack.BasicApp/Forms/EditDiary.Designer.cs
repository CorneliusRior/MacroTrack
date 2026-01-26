namespace MacroTrack.BasicApp.Forms
{
    partial class EditDiary
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
            labelHeader = new Label();
            labelBody = new Label();
            tbBody = new TextBox();
            labelNotes = new Label();
            tbNotes = new TextBox();
            flpFooter = new FlowLayoutPanel();
            btCancel = new Button();
            btEdit = new Button();
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
            tlpBase.Controls.Add(labelHeader, 0, 0);
            tlpBase.Controls.Add(labelBody, 0, 1);
            tlpBase.Controls.Add(tbBody, 0, 2);
            tlpBase.Controls.Add(labelNotes, 0, 3);
            tlpBase.Controls.Add(tbNotes, 0, 4);
            tlpBase.Controls.Add(flpFooter, 0, 5);
            tlpBase.Dock = DockStyle.Fill;
            tlpBase.Location = new Point(0, 0);
            tlpBase.Name = "tlpBase";
            tlpBase.RowCount = 6;
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.Size = new Size(1016, 610);
            tlpBase.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeader.Location = new Point(3, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(480, 38);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Edit entry ## from yyyy/mm/dd hh:ss";
            // 
            // labelBody
            // 
            labelBody.AutoSize = true;
            labelBody.Location = new Point(3, 38);
            labelBody.Name = "labelBody";
            labelBody.Size = new Size(57, 25);
            labelBody.TabIndex = 0;
            labelBody.Text = "Body:";
            // 
            // tbBody
            // 
            tbBody.Dock = DockStyle.Fill;
            tbBody.Location = new Point(3, 66);
            tbBody.Multiline = true;
            tbBody.Name = "tbBody";
            tbBody.ScrollBars = ScrollBars.Vertical;
            tbBody.Size = new Size(1010, 303);
            tbBody.TabIndex = 0;
            // 
            // labelNotes
            // 
            labelNotes.AutoSize = true;
            labelNotes.Location = new Point(3, 372);
            labelNotes.Name = "labelNotes";
            labelNotes.Size = new Size(63, 25);
            labelNotes.TabIndex = 1;
            labelNotes.Text = "Notes:";
            // 
            // tbNotes
            // 
            tbNotes.Dock = DockStyle.Fill;
            tbNotes.Location = new Point(3, 400);
            tbNotes.Multiline = true;
            tbNotes.Name = "tbNotes";
            tbNotes.ScrollBars = ScrollBars.Vertical;
            tbNotes.Size = new Size(1010, 160);
            tbNotes.TabIndex = 1;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(btCancel);
            flpFooter.Controls.Add(btEdit);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 566);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(1010, 41);
            flpFooter.TabIndex = 3;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(895, 3);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(112, 34);
            btCancel.TabIndex = 3;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // btEdit
            // 
            btEdit.Location = new Point(777, 3);
            btEdit.Name = "btEdit";
            btEdit.Size = new Size(112, 34);
            btEdit.TabIndex = 2;
            btEdit.Text = "Edit";
            btEdit.UseVisualStyleBackColor = true;
            btEdit.Click += btEdit_Click;
            // 
            // EditDiary
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1016, 610);
            Controls.Add(tlpBase);
            Name = "EditDiary";
            Text = "Edit Diary Entry";
            tlpBase.ResumeLayout(false);
            tlpBase.PerformLayout();
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpBase;
        private Label labelHeader;
        private Label labelBody;
        private Label labelNotes;
        private FlowLayoutPanel flpFooter;
        private TextBox tbNotes;
        private Button btCancel;
        private Button btEdit;
        private TextBox tbBody;
    }
}