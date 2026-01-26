namespace MacroTrack.BasicApp.Forms
{
    partial class NewPreset
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
            labelTitle = new Label();
            tlpName = new TableLayoutPanel();
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
            flpUnitCategory = new FlowLayoutPanel();
            tlpMass = new TableLayoutPanel();
            labelMass = new Label();
            tbMass = new TextBox();
            cbUnit = new ComboBox();
            tlpCategory = new TableLayoutPanel();
            labelCat = new Label();
            cbCategory = new ComboBox();
            labelNotes = new Label();
            tbNotes = new TextBox();
            flpFooter = new FlowLayoutPanel();
            btCancel = new Button();
            btAdd = new Button();
            tableLayoutPanel1.SuspendLayout();
            tlpName.SuspendLayout();
            flpMacros.SuspendLayout();
            tlpCal.SuspendLayout();
            tlpPro.SuspendLayout();
            tlpCar.SuspendLayout();
            tlpFat.SuspendLayout();
            flpUnitCategory.SuspendLayout();
            tlpMass.SuspendLayout();
            tlpCategory.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(labelTitle, 0, 0);
            tableLayoutPanel1.Controls.Add(tlpName, 0, 1);
            tableLayoutPanel1.Controls.Add(flpMacros, 0, 2);
            tableLayoutPanel1.Controls.Add(flpUnitCategory, 0, 3);
            tableLayoutPanel1.Controls.Add(labelNotes, 0, 4);
            tableLayoutPanel1.Controls.Add(tbNotes, 0, 5);
            tableLayoutPanel1.Controls.Add(flpFooter, 0, 6);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(3, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(216, 38);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Add New Preset";
            // 
            // tlpName
            // 
            tlpName.AutoSize = true;
            tlpName.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpName.ColumnCount = 2;
            tlpName.ColumnStyles.Add(new ColumnStyle());
            tlpName.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpName.Controls.Add(labelName, 0, 0);
            tlpName.Controls.Add(tbName, 1, 0);
            tlpName.Dock = DockStyle.Fill;
            tlpName.Location = new Point(3, 41);
            tlpName.Name = "tlpName";
            tlpName.RowCount = 1;
            tlpName.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpName.Size = new Size(794, 37);
            tlpName.TabIndex = 1;
            // 
            // labelName
            // 
            labelName.Anchor = AnchorStyles.Left;
            labelName.AutoSize = true;
            labelName.Location = new Point(3, 6);
            labelName.Name = "labelName";
            labelName.Size = new Size(63, 25);
            labelName.TabIndex = 0;
            labelName.Text = "Name:";
            // 
            // tbName
            // 
            tbName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbName.Location = new Point(72, 3);
            tbName.Name = "tbName";
            tbName.Size = new Size(719, 31);
            tbName.TabIndex = 1;
            // 
            // flpMacros
            // 
            flpMacros.AutoSize = true;
            flpMacros.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpMacros.Controls.Add(tlpCal);
            flpMacros.Controls.Add(tlpPro);
            flpMacros.Controls.Add(tlpCar);
            flpMacros.Controls.Add(tlpFat);
            flpMacros.Dock = DockStyle.Fill;
            flpMacros.Location = new Point(3, 84);
            flpMacros.Name = "flpMacros";
            flpMacros.Size = new Size(794, 43);
            flpMacros.TabIndex = 2;
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
            tlpCal.Location = new Point(3, 3);
            tlpCal.Name = "tlpCal";
            tlpCal.RowCount = 1;
            tlpCal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCal.Size = new Size(160, 37);
            tlpCal.TabIndex = 2;
            // 
            // labelCal
            // 
            labelCal.Anchor = AnchorStyles.Left;
            labelCal.AutoSize = true;
            labelCal.Location = new Point(3, 6);
            labelCal.Name = "labelCal";
            labelCal.Size = new Size(78, 25);
            labelCal.TabIndex = 0;
            labelCal.Text = "Calories:";
            // 
            // tbCal
            // 
            tbCal.Location = new Point(87, 3);
            tbCal.Name = "tbCal";
            tbCal.Size = new Size(70, 31);
            tbCal.TabIndex = 2;
            tbCal.KeyPress += tbNumeric_Keypress;
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
            tlpPro.Location = new Point(169, 3);
            tlpPro.Name = "tlpPro";
            tlpPro.RowCount = 1;
            tlpPro.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPro.Size = new Size(150, 37);
            tlpPro.TabIndex = 3;
            // 
            // labelPro
            // 
            labelPro.Anchor = AnchorStyles.Left;
            labelPro.AutoSize = true;
            labelPro.Location = new Point(3, 6);
            labelPro.Name = "labelPro";
            labelPro.Size = new Size(68, 25);
            labelPro.TabIndex = 0;
            labelPro.Text = "Protein";
            // 
            // tbPro
            // 
            tbPro.Location = new Point(77, 3);
            tbPro.Name = "tbPro";
            tbPro.Size = new Size(70, 31);
            tbPro.TabIndex = 3;
            tbPro.KeyPress += tbNumeric_Keypress;
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
            tlpCar.Location = new Point(325, 3);
            tlpCar.Name = "tlpCar";
            tlpCar.RowCount = 1;
            tlpCar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCar.Size = new Size(143, 37);
            tlpCar.TabIndex = 4;
            // 
            // labelCar
            // 
            labelCar.Anchor = AnchorStyles.Left;
            labelCar.AutoSize = true;
            labelCar.Location = new Point(3, 6);
            labelCar.Name = "labelCar";
            labelCar.Size = new Size(61, 25);
            labelCar.TabIndex = 0;
            labelCar.Text = "Carbs:";
            // 
            // tbCar
            // 
            tbCar.Location = new Point(70, 3);
            tbCar.Name = "tbCar";
            tbCar.Size = new Size(70, 31);
            tbCar.TabIndex = 4;
            tbCar.KeyPress += tbNumeric_Keypress;
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
            tlpFat.Location = new Point(474, 3);
            tlpFat.Name = "tlpFat";
            tlpFat.RowCount = 1;
            tlpFat.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFat.Size = new Size(121, 37);
            tlpFat.TabIndex = 5;
            // 
            // labelFat
            // 
            labelFat.Anchor = AnchorStyles.Left;
            labelFat.AutoSize = true;
            labelFat.Location = new Point(3, 6);
            labelFat.Name = "labelFat";
            labelFat.Size = new Size(39, 25);
            labelFat.TabIndex = 0;
            labelFat.Text = "Fat:";
            // 
            // tbFat
            // 
            tbFat.Location = new Point(48, 3);
            tbFat.Name = "tbFat";
            tbFat.Size = new Size(70, 31);
            tbFat.TabIndex = 5;
            tbFat.KeyPress += tbNumeric_Keypress;
            // 
            // flpUnitCategory
            // 
            flpUnitCategory.AutoSize = true;
            flpUnitCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpUnitCategory.Controls.Add(tlpMass);
            flpUnitCategory.Controls.Add(tlpCategory);
            flpUnitCategory.Dock = DockStyle.Fill;
            flpUnitCategory.Location = new Point(3, 133);
            flpUnitCategory.Name = "flpUnitCategory";
            flpUnitCategory.Size = new Size(794, 45);
            flpUnitCategory.TabIndex = 6;
            // 
            // tlpMass
            // 
            tlpMass.AutoSize = true;
            tlpMass.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMass.ColumnCount = 3;
            tlpMass.ColumnStyles.Add(new ColumnStyle());
            tlpMass.ColumnStyles.Add(new ColumnStyle());
            tlpMass.ColumnStyles.Add(new ColumnStyle());
            tlpMass.Controls.Add(labelMass, 0, 0);
            tlpMass.Controls.Add(tbMass, 1, 0);
            tlpMass.Controls.Add(cbUnit, 2, 0);
            tlpMass.Location = new Point(3, 3);
            tlpMass.Name = "tlpMass";
            tlpMass.RowCount = 1;
            tlpMass.RowStyles.Add(new RowStyle());
            tlpMass.Size = new Size(190, 39);
            tlpMass.TabIndex = 6;
            // 
            // labelMass
            // 
            labelMass.Anchor = AnchorStyles.Left;
            labelMass.AutoSize = true;
            labelMass.Location = new Point(3, 7);
            labelMass.Name = "labelMass";
            labelMass.Size = new Size(57, 25);
            labelMass.TabIndex = 0;
            labelMass.Text = "Mass:";
            // 
            // tbMass
            // 
            tbMass.Anchor = AnchorStyles.Left;
            tbMass.Location = new Point(66, 4);
            tbMass.Name = "tbMass";
            tbMass.Size = new Size(70, 31);
            tbMass.TabIndex = 6;
            tbMass.KeyPress += tbNumeric_Keypress;
            // 
            // cbUnit
            // 
            cbUnit.Anchor = AnchorStyles.Left;
            cbUnit.FormattingEnabled = true;
            cbUnit.Items.AddRange(new object[] { "g", "ml" });
            cbUnit.Location = new Point(142, 3);
            cbUnit.Name = "cbUnit";
            cbUnit.Size = new Size(45, 33);
            cbUnit.TabIndex = 7;
            // 
            // tlpCategory
            // 
            tlpCategory.AutoSize = true;
            tlpCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCategory.ColumnCount = 2;
            tlpCategory.ColumnStyles.Add(new ColumnStyle());
            tlpCategory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCategory.Controls.Add(labelCat, 0, 0);
            tlpCategory.Controls.Add(cbCategory, 1, 0);
            tlpCategory.Location = new Point(199, 3);
            tlpCategory.Name = "tlpCategory";
            tlpCategory.RowCount = 1;
            tlpCategory.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCategory.Size = new Size(282, 39);
            tlpCategory.TabIndex = 8;
            // 
            // labelCat
            // 
            labelCat.Anchor = AnchorStyles.Left;
            labelCat.AutoSize = true;
            labelCat.Location = new Point(3, 7);
            labelCat.Name = "labelCat";
            labelCat.Size = new Size(88, 25);
            labelCat.TabIndex = 0;
            labelCat.Text = "Category:";
            // 
            // cbCategory
            // 
            cbCategory.Anchor = AnchorStyles.Left;
            cbCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(97, 3);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(182, 33);
            cbCategory.TabIndex = 8;
            // 
            // labelNotes
            // 
            labelNotes.AutoSize = true;
            labelNotes.Location = new Point(3, 181);
            labelNotes.Name = "labelNotes";
            labelNotes.Size = new Size(63, 25);
            labelNotes.TabIndex = 3;
            labelNotes.Text = "Notes:";
            // 
            // tbNotes
            // 
            tbNotes.Dock = DockStyle.Fill;
            tbNotes.Location = new Point(3, 209);
            tbNotes.Multiline = true;
            tbNotes.Name = "tbNotes";
            tbNotes.Size = new Size(794, 192);
            tbNotes.TabIndex = 9;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(btCancel);
            flpFooter.Controls.Add(btAdd);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 407);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(794, 40);
            flpFooter.TabIndex = 10;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(679, 3);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(112, 34);
            btCancel.TabIndex = 11;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // btAdd
            // 
            btAdd.Location = new Point(561, 3);
            btAdd.Name = "btAdd";
            btAdd.Size = new Size(112, 34);
            btAdd.TabIndex = 10;
            btAdd.Text = "Add";
            btAdd.UseVisualStyleBackColor = true;
            btAdd.Click += btAdd_Click;
            // 
            // NewPreset
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "NewPreset";
            Text = "Add New Preset";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tlpName.ResumeLayout(false);
            tlpName.PerformLayout();
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
            flpUnitCategory.ResumeLayout(false);
            flpUnitCategory.PerformLayout();
            tlpMass.ResumeLayout(false);
            tlpMass.PerformLayout();
            tlpCategory.ResumeLayout(false);
            tlpCategory.PerformLayout();
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label labelTitle;
        private FlowLayoutPanel flpMacros;
        private FlowLayoutPanel flpUnitCategory;
        private Label labelNotes;
        private TextBox tbNotes;
        private FlowLayoutPanel flpFooter;
        private Button btCancel;
        private Button btAdd;
        private TableLayoutPanel tlpCal;
        private Label labelCal;
        private TextBox tbCal;
        private TableLayoutPanel tlpPro;
        private Label labelPro;
        private TextBox tbPro;
        private TableLayoutPanel tlpCar;
        private Label labelCar;
        private TextBox tbCar;
        private TableLayoutPanel tlpFat;
        private Label labelFat;
        private TextBox tbFat;
        private TableLayoutPanel tlpMass;
        private TextBox tbMass;
        private Label labelMass;
        private ComboBox cbUnit;
        private TableLayoutPanel tlpCategory;
        private Label labelCat;
        private ComboBox cbCategory;
        private TableLayoutPanel tlpName;
        private Label labelName;
        private TextBox tbName;
    }
}