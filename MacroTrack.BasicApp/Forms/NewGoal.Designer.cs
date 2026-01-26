namespace MacroTrack.BasicApp.Forms
{
    partial class NewGoal
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
            gbGeneral = new GroupBox();
            tlpGeneral = new TableLayoutPanel();
            labelName = new Label();
            tbName = new TextBox();
            labelCalories = new Label();
            flpCalories = new FlowLayoutPanel();
            tlpTotalCal = new TableLayoutPanel();
            spinTotalCal = new NumericUpDown();
            tlpMinCal = new TableLayoutPanel();
            spinMinCal = new NumericUpDown();
            checkMinCal = new CheckBox();
            tlpMaxCal = new TableLayoutPanel();
            spinMaxCal = new NumericUpDown();
            checkMaxCal = new CheckBox();
            labelType = new Label();
            cbType = new ComboBox();
            tlpSliders = new TableLayoutPanel();
            labelProteinSlider = new Label();
            labelCarbsSlider = new Label();
            labelFatSlider = new Label();
            sliderProtein = new TrackBar();
            sliderCarbs = new TrackBar();
            sliderFat = new TrackBar();
            labelPro = new Label();
            labelCar = new Label();
            labelFat = new Label();
            tlpClampPro = new TableLayoutPanel();
            spinProMax = new NumericUpDown();
            spinProSet = new NumericUpDown();
            spinProMin = new NumericUpDown();
            checkProMax = new CheckBox();
            checkProSet = new CheckBox();
            checkProMin = new CheckBox();
            tlpClampCar = new TableLayoutPanel();
            spinCarMax = new NumericUpDown();
            spinCarSet = new NumericUpDown();
            spinCarMin = new NumericUpDown();
            checkCarMax = new CheckBox();
            checkCarSet = new CheckBox();
            checkCarMin = new CheckBox();
            tlpClampFat = new TableLayoutPanel();
            spinFatMax = new NumericUpDown();
            spinFatSet = new NumericUpDown();
            spinFatMin = new NumericUpDown();
            checkFatMax = new CheckBox();
            checkFatSet = new CheckBox();
            checkFatMin = new CheckBox();
            gpNotes = new GroupBox();
            tbNotes = new TextBox();
            flpFooter = new FlowLayoutPanel();
            buttonCancel = new Button();
            buttonAdd = new Button();
            tlpBase.SuspendLayout();
            gbGeneral.SuspendLayout();
            tlpGeneral.SuspendLayout();
            flpCalories.SuspendLayout();
            tlpTotalCal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinTotalCal).BeginInit();
            tlpMinCal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinMinCal).BeginInit();
            tlpMaxCal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinMaxCal).BeginInit();
            tlpSliders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sliderProtein).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sliderCarbs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sliderFat).BeginInit();
            tlpClampPro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinProMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinProSet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinProMin).BeginInit();
            tlpClampCar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinCarMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinCarSet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinCarMin).BeginInit();
            tlpClampFat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinFatMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinFatSet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinFatMin).BeginInit();
            gpNotes.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tlpBase
            // 
            tlpBase.ColumnCount = 1;
            tlpBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBase.Controls.Add(labelHeader, 0, 0);
            tlpBase.Controls.Add(gbGeneral, 0, 1);
            tlpBase.Controls.Add(tlpSliders, 0, 2);
            tlpBase.Controls.Add(gpNotes, 0, 3);
            tlpBase.Controls.Add(flpFooter, 0, 4);
            tlpBase.Dock = DockStyle.Fill;
            tlpBase.Location = new Point(0, 0);
            tlpBase.Name = "tlpBase";
            tlpBase.RowCount = 5;
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBase.RowStyles.Add(new RowStyle());
            tlpBase.Size = new Size(944, 855);
            tlpBase.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeader.Location = new Point(3, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(195, 38);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Add New Goal";
            // 
            // gbGeneral
            // 
            gbGeneral.AutoSize = true;
            gbGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gbGeneral.Controls.Add(tlpGeneral);
            gbGeneral.Dock = DockStyle.Fill;
            gbGeneral.Location = new Point(3, 41);
            gbGeneral.Name = "gbGeneral";
            gbGeneral.Size = new Size(938, 169);
            gbGeneral.TabIndex = 0;
            gbGeneral.TabStop = false;
            gbGeneral.Text = "General";
            // 
            // tlpGeneral
            // 
            tlpGeneral.AutoSize = true;
            tlpGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpGeneral.ColumnCount = 2;
            tlpGeneral.ColumnStyles.Add(new ColumnStyle());
            tlpGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpGeneral.Controls.Add(labelName, 0, 0);
            tlpGeneral.Controls.Add(tbName, 1, 0);
            tlpGeneral.Controls.Add(labelCalories, 0, 1);
            tlpGeneral.Controls.Add(flpCalories, 1, 1);
            tlpGeneral.Controls.Add(labelType, 0, 2);
            tlpGeneral.Controls.Add(cbType, 1, 2);
            tlpGeneral.Dock = DockStyle.Fill;
            tlpGeneral.Location = new Point(3, 27);
            tlpGeneral.Name = "tlpGeneral";
            tlpGeneral.RowCount = 3;
            tlpGeneral.RowStyles.Add(new RowStyle());
            tlpGeneral.RowStyles.Add(new RowStyle());
            tlpGeneral.RowStyles.Add(new RowStyle());
            tlpGeneral.Size = new Size(932, 139);
            tlpGeneral.TabIndex = 0;
            tlpGeneral.TabStop = true;
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
            tbName.Location = new Point(129, 3);
            tbName.Name = "tbName";
            tbName.Size = new Size(800, 31);
            tbName.TabIndex = 0;
            // 
            // labelCalories
            // 
            labelCalories.Anchor = AnchorStyles.Left;
            labelCalories.AutoSize = true;
            labelCalories.Location = new Point(3, 56);
            labelCalories.Name = "labelCalories";
            labelCalories.Size = new Size(120, 25);
            labelCalories.TabIndex = 1;
            labelCalories.Text = "Total Calories:";
            // 
            // flpCalories
            // 
            flpCalories.AutoSize = true;
            flpCalories.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCalories.Controls.Add(tlpTotalCal);
            flpCalories.Controls.Add(tlpMinCal);
            flpCalories.Controls.Add(tlpMaxCal);
            flpCalories.Dock = DockStyle.Fill;
            flpCalories.Location = new Point(129, 40);
            flpCalories.Name = "flpCalories";
            flpCalories.Size = new Size(800, 57);
            flpCalories.TabIndex = 1;
            // 
            // tlpTotalCal
            // 
            tlpTotalCal.AutoSize = true;
            tlpTotalCal.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpTotalCal.ColumnCount = 1;
            tlpTotalCal.ColumnStyles.Add(new ColumnStyle());
            tlpTotalCal.Controls.Add(spinTotalCal, 0, 0);
            tlpTotalCal.Location = new Point(10, 10);
            tlpTotalCal.Margin = new Padding(10);
            tlpTotalCal.Name = "tlpTotalCal";
            tlpTotalCal.RowCount = 1;
            tlpTotalCal.RowStyles.Add(new RowStyle());
            tlpTotalCal.Size = new Size(94, 37);
            tlpTotalCal.TabIndex = 1;
            // 
            // spinTotalCal
            // 
            spinTotalCal.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            spinTotalCal.Location = new Point(3, 3);
            spinTotalCal.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinTotalCal.Name = "spinTotalCal";
            spinTotalCal.Size = new Size(88, 31);
            spinTotalCal.TabIndex = 1;
            spinTotalCal.TextAlign = HorizontalAlignment.Center;
            spinTotalCal.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            spinTotalCal.ValueChanged += spinTotalCal_ValueChanged;
            // 
            // tlpMinCal
            // 
            tlpMinCal.AutoSize = true;
            tlpMinCal.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMinCal.ColumnCount = 2;
            tlpMinCal.ColumnStyles.Add(new ColumnStyle());
            tlpMinCal.ColumnStyles.Add(new ColumnStyle());
            tlpMinCal.Controls.Add(checkMinCal, 0, 0);
            tlpMinCal.Controls.Add(spinMinCal, 1, 0);
            tlpMinCal.Location = new Point(124, 10);
            tlpMinCal.Margin = new Padding(10);
            tlpMinCal.Name = "tlpMinCal";
            tlpMinCal.RowCount = 1;
            tlpMinCal.RowStyles.Add(new RowStyle());
            tlpMinCal.Size = new Size(244, 37);
            tlpMinCal.TabIndex = 2;
            // 
            // spinMinCal
            // 
            spinMinCal.Increment = new decimal(new int[] { 25, 0, 0, 0 });
            spinMinCal.Location = new Point(153, 3);
            spinMinCal.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinMinCal.Name = "spinMinCal";
            spinMinCal.Size = new Size(88, 31);
            spinMinCal.TabIndex = 3;
            spinMinCal.TextAlign = HorizontalAlignment.Center;
            spinMinCal.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // checkMinCal
            // 
            checkMinCal.AutoSize = true;
            checkMinCal.Location = new Point(3, 3);
            checkMinCal.Name = "checkMinCal";
            checkMinCal.Size = new Size(144, 29);
            checkMinCal.TabIndex = 2;
            checkMinCal.Text = "Set minimum";
            checkMinCal.UseVisualStyleBackColor = true;
            checkMinCal.CheckedChanged += Check_CheckChanged;
            // 
            // tlpMaxCal
            // 
            tlpMaxCal.AutoSize = true;
            tlpMaxCal.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMaxCal.ColumnCount = 2;
            tlpMaxCal.ColumnStyles.Add(new ColumnStyle());
            tlpMaxCal.ColumnStyles.Add(new ColumnStyle());
            tlpMaxCal.Controls.Add(checkMaxCal, 0, 0);
            tlpMaxCal.Controls.Add(spinMaxCal, 1, 0);
            tlpMaxCal.Location = new Point(388, 10);
            tlpMaxCal.Margin = new Padding(10);
            tlpMaxCal.Name = "tlpMaxCal";
            tlpMaxCal.RowCount = 1;
            tlpMaxCal.RowStyles.Add(new RowStyle());
            tlpMaxCal.Size = new Size(247, 37);
            tlpMaxCal.TabIndex = 2;
            // 
            // spinMaxCal
            // 
            spinMaxCal.Increment = new decimal(new int[] { 25, 0, 0, 0 });
            spinMaxCal.Location = new Point(156, 3);
            spinMaxCal.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinMaxCal.Name = "spinMaxCal";
            spinMaxCal.Size = new Size(88, 31);
            spinMaxCal.TabIndex = 5;
            spinMaxCal.TextAlign = HorizontalAlignment.Center;
            spinMaxCal.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // checkMaxCal
            // 
            checkMaxCal.AutoSize = true;
            checkMaxCal.Location = new Point(3, 3);
            checkMaxCal.Name = "checkMaxCal";
            checkMaxCal.Size = new Size(147, 29);
            checkMaxCal.TabIndex = 4;
            checkMaxCal.Text = "Set maximum";
            checkMaxCal.UseVisualStyleBackColor = true;
            checkMaxCal.Click += Check_CheckChanged;
            // 
            // labelType
            // 
            labelType.Anchor = AnchorStyles.Left;
            labelType.AutoSize = true;
            labelType.Location = new Point(3, 107);
            labelType.Name = "labelType";
            labelType.Size = new Size(53, 25);
            labelType.TabIndex = 2;
            labelType.Text = "Type:";
            // 
            // cbType
            // 
            cbType.FormattingEnabled = true;
            cbType.Items.AddRange(new object[] { "(None)", "Cut", "Maintenance", "Bulk" });
            cbType.Location = new Point(129, 103);
            cbType.Name = "cbType";
            cbType.Size = new Size(214, 33);
            cbType.TabIndex = 6;
            // 
            // tlpSliders
            // 
            tlpSliders.AutoSize = true;
            tlpSliders.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSliders.ColumnCount = 4;
            tlpSliders.ColumnStyles.Add(new ColumnStyle());
            tlpSliders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSliders.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tlpSliders.ColumnStyles.Add(new ColumnStyle());
            tlpSliders.Controls.Add(labelProteinSlider, 0, 0);
            tlpSliders.Controls.Add(labelCarbsSlider, 0, 1);
            tlpSliders.Controls.Add(labelFatSlider, 0, 2);
            tlpSliders.Controls.Add(sliderProtein, 1, 0);
            tlpSliders.Controls.Add(sliderCarbs, 1, 1);
            tlpSliders.Controls.Add(sliderFat, 1, 2);
            tlpSliders.Controls.Add(labelPro, 2, 0);
            tlpSliders.Controls.Add(labelCar, 2, 1);
            tlpSliders.Controls.Add(labelFat, 2, 2);
            tlpSliders.Controls.Add(tlpClampPro, 3, 0);
            tlpSliders.Controls.Add(tlpClampCar, 3, 1);
            tlpSliders.Controls.Add(tlpClampFat, 3, 2);
            tlpSliders.Dock = DockStyle.Top;
            tlpSliders.Location = new Point(20, 233);
            tlpSliders.Margin = new Padding(20);
            tlpSliders.Name = "tlpSliders";
            tlpSliders.RowCount = 3;
            tlpSliders.RowStyles.Add(new RowStyle());
            tlpSliders.RowStyles.Add(new RowStyle());
            tlpSliders.RowStyles.Add(new RowStyle());
            tlpSliders.Size = new Size(904, 351);
            tlpSliders.TabIndex = 1;
            tlpSliders.TabStop = true;
            // 
            // labelProteinSlider
            // 
            labelProteinSlider.Anchor = AnchorStyles.Left;
            labelProteinSlider.AutoSize = true;
            labelProteinSlider.Location = new Point(3, 46);
            labelProteinSlider.Name = "labelProteinSlider";
            labelProteinSlider.Size = new Size(72, 25);
            labelProteinSlider.TabIndex = 0;
            labelProteinSlider.Text = "Protein:";
            // 
            // labelCarbsSlider
            // 
            labelCarbsSlider.Anchor = AnchorStyles.Left;
            labelCarbsSlider.AutoSize = true;
            labelCarbsSlider.Location = new Point(3, 163);
            labelCarbsSlider.Name = "labelCarbsSlider";
            labelCarbsSlider.Size = new Size(61, 25);
            labelCarbsSlider.TabIndex = 1;
            labelCarbsSlider.Text = "Carbs:";
            // 
            // labelFatSlider
            // 
            labelFatSlider.Anchor = AnchorStyles.Left;
            labelFatSlider.AutoSize = true;
            labelFatSlider.Location = new Point(3, 280);
            labelFatSlider.Name = "labelFatSlider";
            labelFatSlider.Size = new Size(39, 25);
            labelFatSlider.TabIndex = 2;
            labelFatSlider.Text = "Fat:";
            // 
            // sliderProtein
            // 
            sliderProtein.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            sliderProtein.Location = new Point(81, 24);
            sliderProtein.Maximum = 100;
            sliderProtein.Name = "sliderProtein";
            sliderProtein.Size = new Size(518, 69);
            sliderProtein.TabIndex = 0;
            // 
            // sliderCarbs
            // 
            sliderCarbs.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            sliderCarbs.Location = new Point(81, 141);
            sliderCarbs.Maximum = 100;
            sliderCarbs.Name = "sliderCarbs";
            sliderCarbs.Size = new Size(518, 69);
            sliderCarbs.TabIndex = 2;
            // 
            // sliderFat
            // 
            sliderFat.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            sliderFat.Location = new Point(81, 258);
            sliderFat.Maximum = 100;
            sliderFat.Name = "sliderFat";
            sliderFat.Size = new Size(518, 69);
            sliderFat.TabIndex = 4;
            // 
            // labelPro
            // 
            labelPro.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelPro.AutoSize = true;
            labelPro.Location = new Point(605, 0);
            labelPro.Name = "labelPro";
            labelPro.Size = new Size(119, 117);
            labelPro.TabIndex = 6;
            labelPro.Text = "33%\r\n660 Kcal\r\n165g";
            labelPro.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelCar
            // 
            labelCar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelCar.AutoSize = true;
            labelCar.Location = new Point(605, 117);
            labelCar.Name = "labelCar";
            labelCar.Size = new Size(119, 117);
            labelCar.TabIndex = 7;
            labelCar.Text = "33%\r\n660 Kcal\r\n165g";
            labelCar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelFat
            // 
            labelFat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelFat.AutoSize = true;
            labelFat.Location = new Point(605, 234);
            labelFat.Name = "labelFat";
            labelFat.Size = new Size(119, 117);
            labelFat.TabIndex = 8;
            labelFat.Text = "33%\r\n660 Kcal\r\n165g";
            labelFat.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpClampPro
            // 
            tlpClampPro.AutoSize = true;
            tlpClampPro.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpClampPro.ColumnCount = 2;
            tlpClampPro.ColumnStyles.Add(new ColumnStyle());
            tlpClampPro.ColumnStyles.Add(new ColumnStyle());
            tlpClampPro.Controls.Add(spinProMax, 0, 0);
            tlpClampPro.Controls.Add(spinProSet, 0, 1);
            tlpClampPro.Controls.Add(spinProMin, 0, 2);
            tlpClampPro.Controls.Add(checkProMax, 1, 0);
            tlpClampPro.Controls.Add(checkProSet, 1, 1);
            tlpClampPro.Controls.Add(checkProMin, 1, 2);
            tlpClampPro.Location = new Point(730, 3);
            tlpClampPro.Name = "tlpClampPro";
            tlpClampPro.RowCount = 3;
            tlpClampPro.RowStyles.Add(new RowStyle());
            tlpClampPro.RowStyles.Add(new RowStyle());
            tlpClampPro.RowStyles.Add(new RowStyle());
            tlpClampPro.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpClampPro.Size = new Size(171, 111);
            tlpClampPro.TabIndex = 1;
            tlpClampPro.TabStop = true;
            // 
            // spinProMax
            // 
            spinProMax.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinProMax.Location = new Point(3, 3);
            spinProMax.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinProMax.Name = "spinProMax";
            spinProMax.Size = new Size(88, 31);
            spinProMax.TabIndex = 1;
            spinProMax.TextAlign = HorizontalAlignment.Right;
            // 
            // spinProSet
            // 
            spinProSet.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinProSet.Location = new Point(3, 40);
            spinProSet.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinProSet.Name = "spinProSet";
            spinProSet.Size = new Size(88, 31);
            spinProSet.TabIndex = 3;
            spinProSet.TextAlign = HorizontalAlignment.Right;
            spinProSet.ValueChanged += spinMinMaxClamp_ValueChanged;
            // 
            // spinProMin
            // 
            spinProMin.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinProMin.Location = new Point(3, 77);
            spinProMin.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinProMin.Name = "spinProMin";
            spinProMin.Size = new Size(88, 31);
            spinProMin.TabIndex = 5;
            spinProMin.TextAlign = HorizontalAlignment.Right;
            // 
            // checkProMax
            // 
            checkProMax.AutoSize = true;
            checkProMax.Location = new Point(97, 3);
            checkProMax.Name = "checkProMax";
            checkProMax.Size = new Size(71, 29);
            checkProMax.TabIndex = 0;
            checkProMax.Text = "Max";
            checkProMax.UseVisualStyleBackColor = true;
            checkProMax.Click += Check_CheckChanged;
            // 
            // checkProSet
            // 
            checkProSet.AutoSize = true;
            checkProSet.Location = new Point(97, 40);
            checkProSet.Name = "checkProSet";
            checkProSet.Size = new Size(63, 29);
            checkProSet.TabIndex = 2;
            checkProSet.Text = "Set";
            checkProSet.UseVisualStyleBackColor = true;
            checkProSet.Click += Check_CheckChanged;
            // 
            // checkProMin
            // 
            checkProMin.AutoSize = true;
            checkProMin.Location = new Point(97, 77);
            checkProMin.Name = "checkProMin";
            checkProMin.Size = new Size(68, 29);
            checkProMin.TabIndex = 4;
            checkProMin.Text = "Min";
            checkProMin.UseVisualStyleBackColor = true;
            checkProMin.Click += Check_CheckChanged;
            // 
            // tlpClampCar
            // 
            tlpClampCar.AutoSize = true;
            tlpClampCar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpClampCar.ColumnCount = 2;
            tlpClampCar.ColumnStyles.Add(new ColumnStyle());
            tlpClampCar.ColumnStyles.Add(new ColumnStyle());
            tlpClampCar.Controls.Add(spinCarMax, 0, 0);
            tlpClampCar.Controls.Add(spinCarSet, 0, 1);
            tlpClampCar.Controls.Add(spinCarMin, 0, 2);
            tlpClampCar.Controls.Add(checkCarMax, 1, 0);
            tlpClampCar.Controls.Add(checkCarSet, 1, 1);
            tlpClampCar.Controls.Add(checkCarMin, 1, 2);
            tlpClampCar.Location = new Point(730, 120);
            tlpClampCar.Name = "tlpClampCar";
            tlpClampCar.RowCount = 3;
            tlpClampCar.RowStyles.Add(new RowStyle());
            tlpClampCar.RowStyles.Add(new RowStyle());
            tlpClampCar.RowStyles.Add(new RowStyle());
            tlpClampCar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpClampCar.Size = new Size(171, 111);
            tlpClampCar.TabIndex = 3;
            tlpClampCar.TabStop = true;
            // 
            // spinCarMax
            // 
            spinCarMax.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinCarMax.Location = new Point(3, 3);
            spinCarMax.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinCarMax.Name = "spinCarMax";
            spinCarMax.Size = new Size(88, 31);
            spinCarMax.TabIndex = 1;
            spinCarMax.TextAlign = HorizontalAlignment.Right;
            // 
            // spinCarSet
            // 
            spinCarSet.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinCarSet.Location = new Point(3, 40);
            spinCarSet.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinCarSet.Name = "spinCarSet";
            spinCarSet.Size = new Size(88, 31);
            spinCarSet.TabIndex = 3;
            spinCarSet.TextAlign = HorizontalAlignment.Right;
            spinCarSet.ValueChanged += spinMinMaxClamp_ValueChanged;
            // 
            // spinCarMin
            // 
            spinCarMin.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinCarMin.Location = new Point(3, 77);
            spinCarMin.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinCarMin.Name = "spinCarMin";
            spinCarMin.Size = new Size(88, 31);
            spinCarMin.TabIndex = 5;
            spinCarMin.TextAlign = HorizontalAlignment.Right;
            // 
            // checkCarMax
            // 
            checkCarMax.AutoSize = true;
            checkCarMax.Location = new Point(97, 3);
            checkCarMax.Name = "checkCarMax";
            checkCarMax.Size = new Size(71, 29);
            checkCarMax.TabIndex = 0;
            checkCarMax.Text = "Max";
            checkCarMax.UseVisualStyleBackColor = true;
            checkCarMax.Click += Check_CheckChanged;
            // 
            // checkCarSet
            // 
            checkCarSet.AutoSize = true;
            checkCarSet.Location = new Point(97, 40);
            checkCarSet.Name = "checkCarSet";
            checkCarSet.Size = new Size(63, 29);
            checkCarSet.TabIndex = 2;
            checkCarSet.Text = "Set";
            checkCarSet.UseVisualStyleBackColor = true;
            checkCarSet.Click += Check_CheckChanged;
            // 
            // checkCarMin
            // 
            checkCarMin.AutoSize = true;
            checkCarMin.Location = new Point(97, 77);
            checkCarMin.Name = "checkCarMin";
            checkCarMin.Size = new Size(68, 29);
            checkCarMin.TabIndex = 4;
            checkCarMin.Text = "Min";
            checkCarMin.UseVisualStyleBackColor = true;
            checkCarMin.Click += Check_CheckChanged;
            // 
            // tlpClampFat
            // 
            tlpClampFat.AutoSize = true;
            tlpClampFat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpClampFat.ColumnCount = 2;
            tlpClampFat.ColumnStyles.Add(new ColumnStyle());
            tlpClampFat.ColumnStyles.Add(new ColumnStyle());
            tlpClampFat.Controls.Add(spinFatMax, 0, 0);
            tlpClampFat.Controls.Add(spinFatSet, 0, 1);
            tlpClampFat.Controls.Add(spinFatMin, 0, 2);
            tlpClampFat.Controls.Add(checkFatMax, 1, 0);
            tlpClampFat.Controls.Add(checkFatSet, 1, 1);
            tlpClampFat.Controls.Add(checkFatMin, 1, 2);
            tlpClampFat.Location = new Point(730, 237);
            tlpClampFat.Name = "tlpClampFat";
            tlpClampFat.RowCount = 3;
            tlpClampFat.RowStyles.Add(new RowStyle());
            tlpClampFat.RowStyles.Add(new RowStyle());
            tlpClampFat.RowStyles.Add(new RowStyle());
            tlpClampFat.Size = new Size(171, 111);
            tlpClampFat.TabIndex = 5;
            tlpClampFat.TabStop = true;
            // 
            // spinFatMax
            // 
            spinFatMax.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinFatMax.Location = new Point(3, 3);
            spinFatMax.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinFatMax.Name = "spinFatMax";
            spinFatMax.Size = new Size(88, 31);
            spinFatMax.TabIndex = 1;
            spinFatMax.TextAlign = HorizontalAlignment.Right;
            // 
            // spinFatSet
            // 
            spinFatSet.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinFatSet.Location = new Point(3, 40);
            spinFatSet.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinFatSet.Name = "spinFatSet";
            spinFatSet.Size = new Size(88, 31);
            spinFatSet.TabIndex = 3;
            spinFatSet.TextAlign = HorizontalAlignment.Right;
            spinFatSet.ValueChanged += spinMinMaxClamp_ValueChanged;
            // 
            // spinFatMin
            // 
            spinFatMin.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            spinFatMin.Location = new Point(3, 77);
            spinFatMin.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            spinFatMin.Name = "spinFatMin";
            spinFatMin.Size = new Size(88, 31);
            spinFatMin.TabIndex = 5;
            spinFatMin.TextAlign = HorizontalAlignment.Right;
            // 
            // checkFatMax
            // 
            checkFatMax.AutoSize = true;
            checkFatMax.Location = new Point(97, 3);
            checkFatMax.Name = "checkFatMax";
            checkFatMax.Size = new Size(71, 29);
            checkFatMax.TabIndex = 0;
            checkFatMax.Text = "Max";
            checkFatMax.UseVisualStyleBackColor = true;
            checkFatMax.Click += Check_CheckChanged;
            // 
            // checkFatSet
            // 
            checkFatSet.AutoSize = true;
            checkFatSet.Location = new Point(97, 40);
            checkFatSet.Name = "checkFatSet";
            checkFatSet.Size = new Size(63, 29);
            checkFatSet.TabIndex = 2;
            checkFatSet.Text = "Set";
            checkFatSet.UseVisualStyleBackColor = true;
            checkFatSet.Click += Check_CheckChanged;
            // 
            // checkFatMin
            // 
            checkFatMin.AutoSize = true;
            checkFatMin.Location = new Point(97, 77);
            checkFatMin.Name = "checkFatMin";
            checkFatMin.Size = new Size(68, 29);
            checkFatMin.TabIndex = 4;
            checkFatMin.Text = "Min";
            checkFatMin.UseVisualStyleBackColor = true;
            checkFatMin.Click += Check_CheckChanged;
            // 
            // gpNotes
            // 
            gpNotes.Controls.Add(tbNotes);
            gpNotes.Dock = DockStyle.Fill;
            gpNotes.Location = new Point(3, 607);
            gpNotes.Name = "gpNotes";
            gpNotes.Size = new Size(938, 199);
            gpNotes.TabIndex = 2;
            gpNotes.TabStop = false;
            gpNotes.Text = "Notes";
            // 
            // tbNotes
            // 
            tbNotes.Dock = DockStyle.Fill;
            tbNotes.Location = new Point(3, 27);
            tbNotes.Multiline = true;
            tbNotes.Name = "tbNotes";
            tbNotes.Size = new Size(932, 169);
            tbNotes.TabIndex = 2;
            // 
            // flpFooter
            // 
            flpFooter.AutoSize = true;
            flpFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFooter.Controls.Add(buttonCancel);
            flpFooter.Controls.Add(buttonAdd);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(3, 812);
            flpFooter.Name = "flpFooter";
            flpFooter.Size = new Size(938, 40);
            flpFooter.TabIndex = 3;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(823, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(705, 3);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(112, 34);
            buttonAdd.TabIndex = 4;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // NewGoal
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 855);
            Controls.Add(tlpBase);
            Name = "NewGoal";
            Text = "New Goal";
            tlpBase.ResumeLayout(false);
            tlpBase.PerformLayout();
            gbGeneral.ResumeLayout(false);
            gbGeneral.PerformLayout();
            tlpGeneral.ResumeLayout(false);
            tlpGeneral.PerformLayout();
            flpCalories.ResumeLayout(false);
            flpCalories.PerformLayout();
            tlpTotalCal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spinTotalCal).EndInit();
            tlpMinCal.ResumeLayout(false);
            tlpMinCal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinMinCal).EndInit();
            tlpMaxCal.ResumeLayout(false);
            tlpMaxCal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinMaxCal).EndInit();
            tlpSliders.ResumeLayout(false);
            tlpSliders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sliderProtein).EndInit();
            ((System.ComponentModel.ISupportInitialize)sliderCarbs).EndInit();
            ((System.ComponentModel.ISupportInitialize)sliderFat).EndInit();
            tlpClampPro.ResumeLayout(false);
            tlpClampPro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinProMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinProSet).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinProMin).EndInit();
            tlpClampCar.ResumeLayout(false);
            tlpClampCar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinCarMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinCarSet).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinCarMin).EndInit();
            tlpClampFat.ResumeLayout(false);
            tlpClampFat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinFatMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinFatSet).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinFatMin).EndInit();
            gpNotes.ResumeLayout(false);
            gpNotes.PerformLayout();
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpBase;
        private Label labelHeader;
        private FlowLayoutPanel flpFooter;
        private Button buttonCancel;
        private Button buttonAdd;
        private GroupBox gpNotes;
        private TextBox tbNotes;
        private GroupBox gbGeneral;
        private TableLayoutPanel tlpGeneral;
        private Label labelName;
        private Label labelCalories;
        private Label labelType;
        private TextBox tbName;
        private FlowLayoutPanel flpCalories;
        private TableLayoutPanel tlpMinCal;
        private NumericUpDown spinMinCal;
        private CheckBox checkMinCal;
        private TableLayoutPanel tlpTotalCal;
        private NumericUpDown spinTotalCal;
        private TableLayoutPanel tlpMaxCal;
        private NumericUpDown spinMaxCal;
        private CheckBox checkMaxCal;
        private ComboBox cbType;
        private TableLayoutPanel tlpSliders;
        private Label labelProteinSlider;
        private Label labelCarbsSlider;
        private Label labelFatSlider;
        private TrackBar sliderProtein;
        private TrackBar sliderCarbs;
        private TrackBar sliderFat;
        private Label labelPro;
        private Label labelCar;
        private Label labelFat;
        private TableLayoutPanel tlpClampPro;
        private TableLayoutPanel tlpClampFat;
        private NumericUpDown spinFatMin;
        private NumericUpDown spinFatSet;
        private NumericUpDown spinFatMax;
        private CheckBox checkFatMax;
        private CheckBox checkFatSet;
        private CheckBox checkFatMin;
        private TableLayoutPanel tlpClampCar;
        private NumericUpDown spinCarMin;
        private NumericUpDown spinCarSet;
        private NumericUpDown spinCarMax;
        private CheckBox checkCarMax;
        private CheckBox checkCarSet;
        private CheckBox checkCarMin;
        private NumericUpDown spinProMin;
        private NumericUpDown spinProSet;
        private NumericUpDown spinProMax;
        private CheckBox checkProMax;
        private CheckBox checkProSet;
        private CheckBox checkProMin;
    }
}