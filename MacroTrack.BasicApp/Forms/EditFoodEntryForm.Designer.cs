namespace MacroTrack.BasicApp
{
    partial class EditFoodEntryForm
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
            tlpMain = new TableLayoutPanel();
            labelTitle = new Label();
            tlpTimeName = new TableLayoutPanel();
            labelTime = new Label();
            dtpFood = new DateTimePicker();
            btNow = new Button();
            labelName = new Label();
            tbName = new TextBox();
            flpMacros = new FlowLayoutPanel();
            tlpCal = new TableLayoutPanel();
            labelCal = new Label();
            tbCal = new TextBox();
            tlpPro = new TableLayoutPanel();
            labelPro = new Label();
            tbPro = new TextBox();
            tlpCar = new TableLayoutPanel();
            labelCar = new Label();
            tbCar = new TextBox();
            tlpFat = new TableLayoutPanel();
            labelFat = new Label();
            tbFat = new TextBox();
            tlpMult = new TableLayoutPanel();
            labelMult = new Label();
            spinMult = new NumericUpDown();
            labelNotes = new Label();
            tbNotes = new TextBox();
            flpFooter = new FlowLayoutPanel();
            btSave = new Button();
            btCancel = new Button();
            tlpMain.SuspendLayout();
            tlpTimeName.SuspendLayout();
            flpMacros.SuspendLayout();
            tlpCal.SuspendLayout();
            tlpPro.SuspendLayout();
            tlpCar.SuspendLayout();
            tlpFat.SuspendLayout();
            tlpMult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinMult).BeginInit();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.AutoSize = true;
            tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(labelTitle, 0, 0);
            tlpMain.Controls.Add(tlpTimeName, 0, 1);
            tlpMain.Controls.Add(flpMacros, 0, 2);
            tlpMain.Controls.Add(labelNotes, 0, 3);
            tlpMain.Controls.Add(tbNotes, 0, 4);
            tlpMain.Controls.Add(flpFooter, 0, 5);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 6;
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMain.Size = new Size(714, 428);
            tlpMain.TabIndex = 0;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(3, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(576, 45);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Editing entry ID ## - yyyy/mm/dd hh:ss";
            // 
            // tlpTimeName
            // 
            tlpTimeName.AutoSize = true;
            tlpTimeName.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpTimeName.ColumnCount = 3;
            tlpTimeName.ColumnStyles.Add(new ColumnStyle());
            tlpTimeName.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTimeName.ColumnStyles.Add(new ColumnStyle());
            tlpTimeName.Controls.Add(labelTime, 0, 0);
            tlpTimeName.Controls.Add(dtpFood, 1, 0);
            tlpTimeName.Controls.Add(btNow, 2, 0);
            tlpTimeName.Controls.Add(labelName, 0, 1);
            tlpTimeName.Controls.Add(tbName, 1, 1);
            tlpTimeName.Dock = DockStyle.Fill;
            tlpTimeName.Location = new Point(3, 48);
            tlpTimeName.Name = "tlpTimeName";
            tlpTimeName.RowCount = 2;
            tlpTimeName.RowStyles.Add(new RowStyle());
            tlpTimeName.RowStyles.Add(new RowStyle());
            tlpTimeName.Size = new Size(708, 78);
            tlpTimeName.TabIndex = 0;
            // 
            // labelTime
            // 
            labelTime.Anchor = AnchorStyles.Left;
            labelTime.AutoSize = true;
            labelTime.Location = new Point(3, 8);
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(54, 25);
            labelTime.TabIndex = 0;
            labelTime.Text = "Time:";
            // 
            // dtpFood
            // 
            dtpFood.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpFood.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFood.Format = DateTimePickerFormat.Custom;
            dtpFood.Location = new Point(72, 5);
            dtpFood.Name = "dtpFood";
            dtpFood.RightToLeft = RightToLeft.Yes;
            dtpFood.Size = new Size(504, 31);
            dtpFood.TabIndex = 0;
            // 
            // btNow
            // 
            btNow.Anchor = AnchorStyles.Right;
            btNow.AutoSize = true;
            btNow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btNow.Location = new Point(582, 3);
            btNow.Name = "btNow";
            btNow.Size = new Size(123, 35);
            btNow.TabIndex = 1;
            btNow.Text = "Current Time";
            btNow.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            labelName.Anchor = AnchorStyles.Left;
            labelName.AutoSize = true;
            labelName.Location = new Point(3, 47);
            labelName.Name = "labelName";
            labelName.Size = new Size(63, 25);
            labelName.TabIndex = 2;
            labelName.Text = "Name:";
            // 
            // tbName
            // 
            tbName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpTimeName.SetColumnSpan(tbName, 2);
            tbName.Location = new Point(72, 44);
            tbName.Name = "tbName";
            tbName.Size = new Size(633, 31);
            tbName.TabIndex = 2;
            // 
            // flpMacros
            // 
            flpMacros.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flpMacros.AutoSize = true;
            flpMacros.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpMacros.Controls.Add(tlpCal);
            flpMacros.Controls.Add(tlpPro);
            flpMacros.Controls.Add(tlpCar);
            flpMacros.Controls.Add(tlpFat);
            flpMacros.Controls.Add(tlpMult);
            flpMacros.Location = new Point(3, 132);
            flpMacros.Name = "flpMacros";
            flpMacros.Size = new Size(708, 37);
            flpMacros.TabIndex = 3;
            flpMacros.TabStop = true;
            // 
            // tlpCal
            // 
            tlpCal.AutoSize = true;
            tlpCal.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCal.ColumnCount = 2;
            tlpCal.ColumnStyles.Add(new ColumnStyle());
            tlpCal.ColumnStyles.Add(new ColumnStyle());
            tlpCal.Controls.Add(labelCal, 0, 0);
            tlpCal.Controls.Add(tbCal, 1, 0);
            tlpCal.Location = new Point(0, 0);
            tlpCal.Margin = new Padding(0);
            tlpCal.Name = "tlpCal";
            tlpCal.RowCount = 1;
            tlpCal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCal.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpCal.Size = new Size(122, 37);
            tlpCal.TabIndex = 0;
            // 
            // labelCal
            // 
            labelCal.Anchor = AnchorStyles.Left;
            labelCal.AutoSize = true;
            labelCal.Location = new Point(3, 6);
            labelCal.Name = "labelCal";
            labelCal.Size = new Size(40, 25);
            labelCal.TabIndex = 4;
            labelCal.Text = "Cal:";
            labelCal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tbCal
            // 
            tbCal.Anchor = AnchorStyles.Left;
            tbCal.Location = new Point(49, 3);
            tbCal.MaximumSize = new Size(70, 0);
            tbCal.Name = "tbCal";
            tbCal.Size = new Size(70, 31);
            tbCal.TabIndex = 0;
            tbCal.KeyPress += tbNumeric_KeyPress;
            // 
            // tlpPro
            // 
            tlpPro.AutoSize = true;
            tlpPro.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPro.ColumnCount = 2;
            tlpPro.ColumnStyles.Add(new ColumnStyle());
            tlpPro.ColumnStyles.Add(new ColumnStyle());
            tlpPro.Controls.Add(labelPro, 0, 0);
            tlpPro.Controls.Add(tbPro, 1, 0);
            tlpPro.Location = new Point(122, 0);
            tlpPro.Margin = new Padding(0);
            tlpPro.Name = "tlpPro";
            tlpPro.RowCount = 1;
            tlpPro.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPro.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpPro.Size = new Size(125, 37);
            tlpPro.TabIndex = 1;
            // 
            // labelPro
            // 
            labelPro.Anchor = AnchorStyles.Left;
            labelPro.AutoSize = true;
            labelPro.Location = new Point(3, 6);
            labelPro.Name = "labelPro";
            labelPro.Size = new Size(43, 25);
            labelPro.TabIndex = 4;
            labelPro.Text = "Pro:";
            labelPro.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tbPro
            // 
            tbPro.Anchor = AnchorStyles.Left;
            tbPro.Location = new Point(52, 3);
            tbPro.MaximumSize = new Size(70, 0);
            tbPro.Name = "tbPro";
            tbPro.Size = new Size(70, 31);
            tbPro.TabIndex = 1;
            tbPro.KeyPress += tbNumeric_KeyPress;
            // 
            // tlpCar
            // 
            tlpCar.AutoSize = true;
            tlpCar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCar.ColumnCount = 2;
            tlpCar.ColumnStyles.Add(new ColumnStyle());
            tlpCar.ColumnStyles.Add(new ColumnStyle());
            tlpCar.Controls.Add(labelCar, 0, 0);
            tlpCar.Controls.Add(tbCar, 1, 0);
            tlpCar.Location = new Point(247, 0);
            tlpCar.Margin = new Padding(0);
            tlpCar.Name = "tlpCar";
            tlpCar.RowCount = 1;
            tlpCar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpCar.Size = new Size(124, 37);
            tlpCar.TabIndex = 2;
            // 
            // labelCar
            // 
            labelCar.Anchor = AnchorStyles.Left;
            labelCar.AutoSize = true;
            labelCar.Location = new Point(3, 6);
            labelCar.Name = "labelCar";
            labelCar.Size = new Size(42, 25);
            labelCar.TabIndex = 4;
            labelCar.Text = "Car:";
            labelCar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tbCar
            // 
            tbCar.Anchor = AnchorStyles.Left;
            tbCar.Location = new Point(51, 3);
            tbCar.MaximumSize = new Size(70, 0);
            tbCar.Name = "tbCar";
            tbCar.Size = new Size(70, 31);
            tbCar.TabIndex = 2;
            tbCar.KeyPress += tbNumeric_KeyPress;
            // 
            // tlpFat
            // 
            tlpFat.AutoSize = true;
            tlpFat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpFat.ColumnCount = 2;
            tlpFat.ColumnStyles.Add(new ColumnStyle());
            tlpFat.ColumnStyles.Add(new ColumnStyle());
            tlpFat.Controls.Add(labelFat, 0, 0);
            tlpFat.Controls.Add(tbFat, 1, 0);
            tlpFat.Location = new Point(371, 0);
            tlpFat.Margin = new Padding(0);
            tlpFat.Name = "tlpFat";
            tlpFat.RowCount = 1;
            tlpFat.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFat.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpFat.Size = new Size(121, 37);
            tlpFat.TabIndex = 3;
            // 
            // labelFat
            // 
            labelFat.Anchor = AnchorStyles.Left;
            labelFat.AutoSize = true;
            labelFat.Location = new Point(3, 6);
            labelFat.Name = "labelFat";
            labelFat.Size = new Size(39, 25);
            labelFat.TabIndex = 4;
            labelFat.Text = "Fat:";
            labelFat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tbFat
            // 
            tbFat.Anchor = AnchorStyles.Left;
            tbFat.Location = new Point(48, 3);
            tbFat.MaximumSize = new Size(70, 0);
            tbFat.Name = "tbFat";
            tbFat.Size = new Size(70, 31);
            tbFat.TabIndex = 3;
            tbFat.KeyPress += tbNumeric_KeyPress;
            // 
            // tlpMult
            // 
            tlpMult.AutoSize = true;
            tlpMult.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMult.ColumnCount = 2;
            tlpMult.ColumnStyles.Add(new ColumnStyle());
            tlpMult.ColumnStyles.Add(new ColumnStyle());
            tlpMult.Controls.Add(labelMult, 0, 0);
            tlpMult.Controls.Add(spinMult, 1, 0);
            tlpMult.Location = new Point(492, 0);
            tlpMult.Margin = new Padding(0);
            tlpMult.Name = "tlpMult";
            tlpMult.RowCount = 1;
            tlpMult.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMult.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMult.Size = new Size(112, 37);
            tlpMult.TabIndex = 4;
            // 
            // labelMult
            // 
            labelMult.Anchor = AnchorStyles.Left;
            labelMult.AutoSize = true;
            labelMult.Location = new Point(3, 6);
            labelMult.Name = "labelMult";
            labelMult.Size = new Size(20, 25);
            labelMult.TabIndex = 4;
            labelMult.Text = "x";
            labelMult.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // spinMult
            // 
            spinMult.DecimalPlaces = 2;
            spinMult.Location = new Point(29, 3);
            spinMult.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            spinMult.Name = "spinMult";
            spinMult.Size = new Size(80, 31);
            spinMult.TabIndex = 4;
            spinMult.TextAlign = HorizontalAlignment.Right;
            spinMult.Value = new decimal(new int[] { 1, 0, 0, 0 });
            spinMult.KeyDown += spinMult_KeyDown;
            // 
            // labelNotes
            // 
            labelNotes.AutoSize = true;
            labelNotes.Location = new Point(3, 172);
            labelNotes.Name = "labelNotes";
            labelNotes.Size = new Size(63, 25);
            labelNotes.TabIndex = 5;
            labelNotes.Text = "Notes:";
            // 
            // tbNotes
            // 
            tbNotes.Dock = DockStyle.Fill;
            tbNotes.Location = new Point(3, 200);
            tbNotes.Multiline = true;
            tbNotes.Name = "tbNotes";
            tbNotes.Size = new Size(708, 179);
            tbNotes.TabIndex = 4;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(btSave);
            flpFooter.Controls.Add(btCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 385);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(708, 40);
            flpFooter.TabIndex = 4;
            // 
            // btSave
            // 
            btSave.Location = new Point(593, 3);
            btSave.Name = "btSave";
            btSave.Size = new Size(112, 34);
            btSave.TabIndex = 6;
            btSave.Text = "Save";
            btSave.UseVisualStyleBackColor = true;
            btSave.Click += btSave_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(475, 3);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(112, 34);
            btCancel.TabIndex = 5;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // EditFoodEntryForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 428);
            Controls.Add(tlpMain);
            Name = "EditFoodEntryForm";
            Text = "Edit Food Entry";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            tlpTimeName.ResumeLayout(false);
            tlpTimeName.PerformLayout();
            flpMacros.ResumeLayout(false);
            flpMacros.PerformLayout();
            tlpCal.ResumeLayout(false);
            tlpCal.PerformLayout();
            tlpPro.ResumeLayout(false);
            tlpPro.PerformLayout();
            tlpCar.ResumeLayout(false);
            tlpCar.PerformLayout();
            tlpFat.ResumeLayout(false);
            tlpFat.PerformLayout();
            tlpMult.ResumeLayout(false);
            tlpMult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinMult).EndInit();
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label labelTitle;
        private FlowLayoutPanel flpMacros;
        private FlowLayoutPanel flpFooter;
        private Label labelNotes;
        private Label labelCal;
        private Label labelPro;
        private Label labelCar;
        private Label labelFat;
        private Button btNow;
        private TableLayoutPanel tlpTimeName;
        private Label labelName;
        private TextBox tbName;
        private Label labelTime;
        private Button btSave;
        private Button btCancel;
        private DateTimePicker dtpFood;
        private TextBox tbNotes;
        private TableLayoutPanel tlpCal;
        private TextBox tbCal;
        private TableLayoutPanel tlpPro;
        private TextBox tbPro;
        private TableLayoutPanel tlpCar;
        private TextBox tbCar;
        private TableLayoutPanel tlpFat;
        private TextBox tbFat;
        private TableLayoutPanel tlpMult;
        private NumericUpDown spinMult;
        private Label labelMult;
    }
}