namespace MacroTrack.BasicApp
{
    partial class FoodEntryCard
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tlpButtons = new TableLayoutPanel();
            btEdit = new Button();
            beDelete = new Button();
            tlpMain = new TableLayoutPanel();
            labelTitle = new Label();
            labelMacros = new Label();
            tlpDateRow = new TableLayoutPanel();
            labelTime = new Label();
            labelAmount = new Label();
            labelNotes = new Label();
            panelBasePanel = new Panel();
            tableLayoutPanel1.SuspendLayout();
            tlpButtons.SuspendLayout();
            tlpMain.SuspendLayout();
            tlpDateRow.SuspendLayout();
            panelBasePanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(tlpButtons, 1, 0);
            tableLayoutPanel1.Controls.Add(tlpMain, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(438, 127);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tlpButtons
            // 
            tlpButtons.AutoSize = true;
            tlpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpButtons.ColumnCount = 1;
            tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpButtons.Controls.Add(btEdit, 0, 0);
            tlpButtons.Controls.Add(beDelete, 0, 1);
            tlpButtons.Dock = DockStyle.Fill;
            tlpButtons.Location = new Point(353, 3);
            tlpButtons.Name = "tlpButtons";
            tlpButtons.RowCount = 2;
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpButtons.Size = new Size(82, 121);
            tlpButtons.TabIndex = 0;
            // 
            // btEdit
            // 
            btEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btEdit.AutoSize = true;
            btEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btEdit.Location = new Point(5, 20);
            btEdit.Margin = new Padding(5);
            btEdit.Name = "btEdit";
            btEdit.Size = new Size(72, 35);
            btEdit.TabIndex = 0;
            btEdit.Text = "Edit";
            btEdit.UseVisualStyleBackColor = true;
            btEdit.Click += btEdit_Click;
            // 
            // beDelete
            // 
            beDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            beDelete.AutoSize = true;
            beDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            beDelete.Location = new Point(5, 65);
            beDelete.Margin = new Padding(5);
            beDelete.Name = "beDelete";
            beDelete.Size = new Size(72, 35);
            beDelete.TabIndex = 1;
            beDelete.Text = "Delete";
            beDelete.UseVisualStyleBackColor = true;
            beDelete.Click += beDelete_Click;
            // 
            // tlpMain
            // 
            tlpMain.AutoSize = true;
            tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(labelTitle, 0, 0);
            tlpMain.Controls.Add(labelMacros, 0, 1);
            tlpMain.Controls.Add(tlpDateRow, 0, 2);
            tlpMain.Controls.Add(labelNotes, 0, 3);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(3, 3);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 4;
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.Size = new Size(344, 121);
            tlpMain.TabIndex = 1;
            // 
            // labelTitle
            // 
            labelTitle.Dock = DockStyle.Fill;
            labelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(3, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(338, 32);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Sample Entry (100g)";
            // 
            // labelMacros
            // 
            labelMacros.Dock = DockStyle.Fill;
            labelMacros.Location = new Point(3, 32);
            labelMacros.Name = "labelMacros";
            labelMacros.Size = new Size(338, 25);
            labelMacros.TabIndex = 1;
            labelMacros.Text = "Cal: 100 Pro: 10 Car: 10 Fat: 10";
            // 
            // tlpDateRow
            // 
            tlpDateRow.ColumnCount = 2;
            tlpDateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDateRow.ColumnStyles.Add(new ColumnStyle());
            tlpDateRow.Controls.Add(labelTime, 0, 0);
            tlpDateRow.Controls.Add(labelAmount, 1, 0);
            tlpDateRow.Dock = DockStyle.Fill;
            tlpDateRow.Location = new Point(0, 57);
            tlpDateRow.Margin = new Padding(0);
            tlpDateRow.Name = "tlpDateRow";
            tlpDateRow.RowCount = 1;
            tlpDateRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDateRow.Size = new Size(344, 25);
            tlpDateRow.TabIndex = 2;
            // 
            // labelTime
            // 
            labelTime.Dock = DockStyle.Fill;
            labelTime.Location = new Point(3, 0);
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(278, 25);
            labelTime.TabIndex = 0;
            labelTime.Text = "2026/01/14 22:51";
            // 
            // labelAmount
            // 
            labelAmount.AutoSize = true;
            labelAmount.Dock = DockStyle.Right;
            labelAmount.Location = new Point(287, 0);
            labelAmount.Name = "labelAmount";
            labelAmount.Size = new Size(54, 25);
            labelAmount.TabIndex = 1;
            labelAmount.Text = "(x1.0)";
            labelAmount.TextAlign = ContentAlignment.TopRight;
            // 
            // labelNotes
            // 
            labelNotes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelNotes.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            labelNotes.Location = new Point(3, 82);
            labelNotes.Name = "labelNotes";
            labelNotes.Size = new Size(338, 25);
            labelNotes.TabIndex = 3;
            labelNotes.Text = "Sample Note";
            // 
            // panelBasePanel
            // 
            panelBasePanel.AutoSize = true;
            panelBasePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelBasePanel.BorderStyle = BorderStyle.FixedSingle;
            panelBasePanel.Controls.Add(tableLayoutPanel1);
            panelBasePanel.Dock = DockStyle.Fill;
            panelBasePanel.Location = new Point(0, 0);
            panelBasePanel.Name = "panelBasePanel";
            panelBasePanel.Size = new Size(440, 129);
            panelBasePanel.TabIndex = 1;
            // 
            // FoodEntryCard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelBasePanel);
            Name = "FoodEntryCard";
            Size = new Size(440, 129);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tlpButtons.ResumeLayout(false);
            tlpButtons.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpDateRow.ResumeLayout(false);
            tlpDateRow.PerformLayout();
            panelBasePanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tlpButtons;
        private Button btEdit;
        private Button beDelete;
        private TableLayoutPanel tlpMain;
        private Label labelTitle;
        private Label labelMacros;
        private TableLayoutPanel tlpDateRow;
        private Label labelTime;
        private Label labelAmount;
        private Label labelNotes;
        private Panel panelBasePanel;
    }
}
