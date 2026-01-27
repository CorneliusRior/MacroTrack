using MacroTrack.Core.Services;
using MacroTrack.Core.Models;
using MacroTrack.Core.Logging;
using MacroTrack.Core.AppModels;
using System.Windows.Forms.DataVisualization.Charting;
using MacroTrack.BasicApp.Forms;


namespace MacroTrack.BasicApp
{
    public partial class Form1 : Form
    {
        private readonly CoreServices _services;
        private bool _isRefreshing;
        private int _timeFrame;

        private int weightMode = 0; // this can be set as 0: Kg (metric), 1: lbs (imperial), 2: st
        private double FEMult = 1; // might be something more elegant we can do but who cares?
        private List<Preset> _presets = new();
        private List<string> _presetCatList = new();

        private readonly System.Windows.Forms.Timer _clockTimer = new();

        public Form1(CoreServices services)
        {
            _services = services;
            _services.MessageLogged += (_, msg) => Print($"{msg.Source} {msg.Message} { (msg.Exception is null ? "" : $"| {msg.Exception.Message}") } ");
            InitializeComponent();
            SetUpClock();
            cbBarTimeFrame.SelectedIndex = 0;
        }


        private void RefreshUI()
        {
            _isRefreshing = true;
            UpdateSummary();
            UpdateWeightGraph(); // Putting it here due to empty existing "start of chart update" comments. We can rearranfe these if you like, idm.
            UpdateCalGraph();

            // Start of history cards:
            flpHistory.SuspendLayout();
            List<FoodEntry>? entries = _services.foodLogService.FromTimes(DateTime.Now.Date.AddDays(-3), DateTime.Now.Date.AddDays(1));
            entries.Reverse();

            try
            {
                flpHistory.Controls.Clear();


                foreach (FoodEntry e in entries)
                {
                    FoodEntryCard card = new FoodEntryCard();
                    card.SetData(e.Id, e.Time, e.ItemName, e.Amount, e.Calories, e.Protein, e.Carbs, e.Fat, e.Notes);

                    card.RequestEdit += (_, id) => EditFoodEntry(id);
                    card.RequestDelete += (_, id) => DeleteFoodEntry(id);

                    flpHistory.Controls.Add(card);
                }
            }
            finally
            {
                flpHistory.ResumeLayout(true);
            }

            flpHistoryWidthAdjust();
            // End history cards

            // Start of task
            flpDT.SuspendLayout();
            List<DailyTask> TaskList = _services.taskService.GetAllStreaks(DateTime.Now, filterInactive: true);

            try
            {
                flpDT.Controls.Clear();
                foreach (DailyTask task in TaskList)
                {
                    taskListItem item = new taskListItem();
                    item.SetData(task.Id, task.Completed, task.Name, task.Streak);

                    item.RequestSetCompleted += (_, id) => TaskSetComplete(id);
                    item.RequestSetIncomplete += (_, id) => TaskSetIncomplete(id);
                    item.RequestPrint += (_, text) => Print($"Taskitem says: \"{text}\"");

                    flpDT.Controls.Add(item);
                }
            }
            finally
            {
                flpDT.ResumeLayout(true);
            }

            flpDTWidthAdjust();
            // End of tasks

            // Preset ComboBox list            
            _presetCatList.Clear();
            _presetCatList.AddRange(["(No Filter)", "(No Category)"]);
            var CatList = _services.presetService.GetCategoryList();
            CatList.Sort();
            _presetCatList.AddRange(CatList);
            UpdatePresetCatList(_presetCatList);

            _presets.Clear();
            _presets = _services.presetService.GetAll();
            UpdatePresetList(_presets);

            _isRefreshing = false;

        }

        private void Print(string text)
        {
            text = text.Replace("\n", Environment.NewLine);
            tbCommandOutput.AppendText(text + Environment.NewLine);
        }

        private void PrintInline(string text)
        {
            tbCommandOutput.AppendText(text);
        }

        private void SetUpClock()
        {
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (_, __) => labelTime.Text = DateTime.Now.ToString("F");
            _clockTimer.Start();

            labelTime.Text = DateTime.Now.ToString("F");
        }

        private void cbBarTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Print($"cbBarTimeFrame changed, selected index is: {cbBarTimeFrame.SelectedIndex}");
            _timeFrame = cbBarTimeFrame.SelectedIndex;
            RefreshUI();
        }

        private void UpdateSummary()
        {
            DateTime SummaryStartTime;
            MacroSummary Summary = _services.dataService.GetMacroSummary(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            if (_timeFrame == 0)
            {
                SummaryStartTime = DateTime.Now.Date;
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryStartTime.AddDays(1));
            }
            if (_timeFrame == 1)
            {
                SummaryStartTime = DateTime.Now.AddDays(-1);
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryStartTime.AddDays(1));
            }
            if (_timeFrame == 2)
            {
                int diff = ((int)DateTime.Now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                //Print($"DateTime.Now: {DateTime.Now}");
                //Print($"diff: {diff}");
                SummaryStartTime = DateTime.Now.Date.AddDays(-diff);
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryStartTime.AddDays(7));
            }
            if (_timeFrame == 3)
            {
                SummaryStartTime = DateTime.Now.Date.AddDays(-7);
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryStartTime.AddDays(7));
            }
            if (_timeFrame == 4)
            {
                SummaryStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var SummaryEndTime = SummaryStartTime.AddMonths(1); // I don't know if this does actually go to the next calendar month of just 30 days.
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryEndTime);
            }
            if (_timeFrame == 5)
            {
                SummaryStartTime = DateTime.Now.Date.AddDays(-30);
                Summary = _services.dataService.GetMacroSummary(SummaryStartTime, SummaryStartTime.AddDays(30));
            }
            if (_timeFrame < 0 || _timeFrame > 5)
            {
                Print("Error refreshing: invalid _timeFrame");
                MessageBox.Show("Error refreshing: invalid _timeFrame");
                return;
            }

            labelBarBannerGoal.Text = Summary.GoalName;

            double CalPct = Summary.Target.Calories == 0 ? 0 : (Summary.Actual.Calories / Summary.Target.Calories) * 100;
            labelBarsFractionCal.Text = $"{Summary.Actual.Calories:0.0}/{Summary.Target.Calories:0.0} ({CalPct.ToString("0.0")}%)";
            double ProPct = Summary.Target.Protein == 0 ? 0 : (Summary.Actual.Protein / Summary.Target.Protein) * 100;
            labelBarsFractionPro.Text = $"{Summary.Actual.Protein:0.0}/{Summary.Target.Protein:0.0} ({ProPct.ToString("0.0")}%)";
            double CarPct = Summary.Target.Carbs == 0 ? 0 : (Summary.Actual.Carbs / Summary.Target.Carbs) * 100;
            labelBarsFractionCar.Text = $"{Summary.Actual.Carbs:0.0}/{Summary.Target.Carbs:0.0} ({CarPct.ToString("0.0")}%)";
            double FatPct = Summary.Target.Fat == 0 ? 0 : (Summary.Actual.Fat / Summary.Target.Fat) * 100;
            labelBarsFractionFat.Text = $"{Summary.Actual.Fat:0.0}/{Summary.Target.Fat:0.0} ({FatPct.ToString("0.0")}%)";

            labelBarsRemainingCal.Text = $"{Summary.Remaining.Calories:0.0}";
            labelBarsRemainingPro.Text = $"{Summary.Remaining.Protein:0.0}";
            labelBarsRemainingCar.Text = $"{Summary.Remaining.Carbs:0.0}";
            labelBarsRemainingFat.Text = $"{Summary.Remaining.Fat:0.0}";

            macroBarCal.Actual = Summary.Actual.Calories;
            macroBarCal.Target = Summary.Target.Calories;
            macroBarCal.Invalidate();
            macroBarPro.Actual = Summary.Actual.Protein;
            macroBarPro.Target = Summary.Target.Protein;
            macroBarPro.Invalidate();
            macroBarCar.Actual = Summary.Actual.Carbs;
            macroBarCar.Target = Summary.Target.Carbs;
            macroBarCar.Invalidate();
            macroBarFat.Actual = Summary.Actual.Fat;
            macroBarFat.Target = Summary.Target.Fat;
            macroBarFat.Invalidate();

            // we put these here because they use summary and were in RefreshUI earlier.
            MakePieGoal(Summary.NoGoal, Summary.Target.Calories, Summary.Target.Protein, Summary.Target.Carbs, Summary.Target.Fat);
            MakePieActual(Summary.Actual.Calories, Summary.Actual.Protein, Summary.Actual.Carbs, Summary.Actual.Fat);
        }

        private void UpdateWeightGraph()
        {
            DateTime EndDate = DateTime.Now.Date.AddDays(1);
            DateTime StartDate = DateTime.Now.Date.AddDays(-30);

            // Clear graph
            lineChartWeight.Series.Clear();
            lineChartWeight.ChartAreas.Clear();
            lineChartWeight.Legends.Clear();

            // Set up chart area
            var ca = new ChartArea("Main");
            lineChartWeight.ChartAreas.Add(ca);

            // Set up grid lines
            ca.AxisX.IntervalType = DateTimeIntervalType.Weeks;
            ca.AxisX.Interval = 1;
            ca.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Weeks;
            ca.AxisX.MajorGrid.Interval = 1;
            ca.AxisX.Minimum = StartDate.ToOADate();
            ca.AxisX.Maximum = EndDate.ToOADate();
            ca.AxisX.LabelStyle.Format = "dd/MM";
            ca.AxisY.MajorGrid.Enabled = true;

            // Set up series
            var s = new Series("Weight")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Double,
                BorderWidth = 4,
                Color = Color.Black
            };

            // Give it data
            List<WeightEntry> pts = _services.dataService.GetWeightEntries(StartDate, EndDate).OrderBy(pts => pts.Time).ToList();

            foreach (WeightEntry p in pts) s.Points.AddXY(p.Time, p.Weight);

            //Add
            lineChartWeight.Series.Add(s);
        }

        private void UpdateCalGraph()
        {
            DateTime EndDate = DateTime.Now.Date;
            DateTime StartDate = DateTime.Now.Date.AddDays(-30);

            // Clear graph
            lineChartCal.Series.Clear();
            lineChartCal.ChartAreas.Clear();
            lineChartCal.Legends.Clear();

            // Set up chart area
            var ca = new ChartArea("Main");
            lineChartCal.ChartAreas.Add(ca);

            // Set up grid lines
            ca.AxisX.IntervalType = DateTimeIntervalType.Weeks;
            ca.AxisX.Interval = 1;
            ca.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Weeks;
            ca.AxisX.MajorGrid.Interval = 1;
            ca.AxisX.Minimum = StartDate.ToOADate();
            ca.AxisX.Maximum = EndDate.ToOADate();
            ca.AxisX.LabelStyle.Format = "dd/MM";
            ca.AxisY.MajorGrid.Enabled = true;

            // Set up series
            var g = new Series("Goal")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Double,
                BorderWidth = 4,
                Color = Color.Red
            };

            var c = new Series("Actual")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Double,
                BorderWidth = 4,
                Color = Color.Black
            };

            // Give them data:
            List<GoalSeriesPoint> goalCal = _services.dataService.GetGoalSeries(StartDate, EndDate);
            foreach (GoalSeriesPoint p in goalCal) g.Points.AddXY(p.Date, p.Calories);
            //foreach (GoalSeriesPoint p in goalCat) Print($"Point: [{p.Id}]: {p.Date} Cal: {p.Calories}");

            List<CalSeriesPoint> actualCal = _services.dataService.GetCalSeries(StartDate, EndDate);
            foreach (CalSeriesPoint p in actualCal) c.Points.AddXY(p.Date, p.Calories);
            //foreach (CalSeriesPoint p in actualCal) Print($"Point: [{p.Id}]: {p.Date} Cal: {p.Calories}");

            // Add
            lineChartCal.Series.Add(g);
            lineChartCal.Series.Add(c);
        }

        private void macroBar_SizeChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void UpdatePresetList(List<Preset> set)
        {
            cbFEItem.BeginUpdate();
            try
            {
                cbFEItem.DataSource = null;

                cbFEItem.DataSource = set;
                cbFEItem.DisplayMember = "PresetName";
                cbFEItem.ValueMember = "Id";
                cbFEItem.SelectedIndex = -1;
                cbFEItem.Text = "";
            }
            finally
            {
                cbFEItem.EndUpdate();
            }
        }

        private void UpdatePresetCatList(List<string> set)
        {
            cbFEFilter.BeginUpdate();
            try
            {
                cbFEFilter.DataSource = null;
                cbFEFilter.DataSource = set;

            }
            finally
            {
                cbFEFilter.EndUpdate();
            }
        }

        private void cbFEItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbFEItem.SelectedItem is not Preset p) return;

            tbFECal.Text = p.Calories.ToString();
            tbFEPro.Text = p.Protein.ToString();
            tbFECar.Text = p.Carbs.ToString();
            tbFEFat.Text = p.Fat.ToString();

            spinFEMult.Value = 1;
        }

        private void cbFEFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbFEFilter.SelectedIndex == 0)
            {
                _presets = _services.presetService.GetAll();
            }
            else if (cbFEFilter.SelectedIndex == 1)
            {
                _presets = _services.presetService.GetAllCategory(null);
            }
            else
            {
                _presets = _services.presetService.GetAllCategory(_presetCatList[cbFEFilter.SelectedIndex]);
            }

            UpdatePresetList(_presets);
        }

        private void cbFEItem_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void spinFEMult_ValueChanged(object sender, EventArgs e)
        {
            double LastMult = FEMult;
            FEMult = (double)spinFEMult.Value;

            if (FEMult == 0)
            {
                tbFECal.Text = "0";
                tbFEPro.Text = "0";
                tbFECar.Text = "0";
                tbFEFat.Text = "0";
                return;
            }

            if (LastMult == 0)
            {
                if (cbFEItem.SelectedItem is not Preset p) return;

                tbFECal.Text = (p.Calories * FEMult).ToString("0.##");
                tbFEPro.Text = (p.Protein * FEMult).ToString("0.##");
                tbFECar.Text = (p.Carbs * FEMult).ToString("0.##");
                tbFEFat.Text = (p.Fat * FEMult).ToString("0.##");
                return;
            }

            if (double.TryParse(tbFECal.Text, out double cal) && cal != 0) tbFECal.Text = ((cal / LastMult) * FEMult).ToString("0.##");
            if (double.TryParse(tbFEPro.Text, out double pro) && pro != 0) tbFEPro.Text = ((pro / LastMult) * FEMult).ToString("0.##");
            if (double.TryParse(tbFECar.Text, out double car) && car != 0) tbFECar.Text = ((car / LastMult) * FEMult).ToString("0.##");
            if (double.TryParse(tbFEFat.Text, out double fat) && fat != 0) tbFEFat.Text = ((fat / LastMult) * FEMult).ToString("0.##");
        }

        private void btFEAdd_Click(object sender, EventArgs e)
        {
            bool inputError = false;
            string errorString = "";
            DateTime time = dtpFood.Value;
            string itemName = cbFEItem.Text;
            double amount = (double)spinFEMult.Value;
            if (!double.TryParse(tbFECal.Text, out double calories))
            {
                inputError = true;
                errorString += $"Could not parse calorie amount '{tbFECal.Text}'\n";
            }
            if (!double.TryParse(tbFEPro.Text, out double protein))
            {
                inputError = true;
                errorString += $"Could not parse calorie amount '{tbFEPro.Text}'\n";
            }
            if (!double.TryParse(tbFECar.Text, out double carbs))
            {
                inputError = true;
                errorString += $"Could not parse calorie amount '{tbFECar.Text}'\n";
            }
            if (!double.TryParse(tbFEFat.Text, out double fat))
            {
                inputError = true;
                errorString += $"Could not parse calorie amount '{tbFEFat.Text}'\n";
            }
            string? category = string.IsNullOrEmpty(cbFEFilter.Text) ? null : cbFEFilter.Text;
            string? notes = string.IsNullOrEmpty(tbFENotes.Text) ? null : tbFENotes.Text;

            if (inputError)
            {
                MessageBox.Show("Error adding food entry:\n" + errorString);
            }
            else
            {
                var entry = _services.foodLogService.AddEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
                if (entry == null)
                {
                    MessageBox.Show("There has been some error adding entry.");
                }
                else
                {
                    Print($"Added entry: ({entry.Time}) \"{entry.ItemName}\" (x{entry.Amount:0.00}): {entry.Calories}Kcal, {entry.Protein}g protein, {entry.Carbs}g carbs, {entry.Fat}g fat, Notes: {entry.Notes}");
                    FEClear();
                    RefreshUI();
                }

            }
        }

        private void EditFoodEntry(int id)
        {
            try { _services.foodLogService.GetEntry(id); }
            catch (Exception ex) { Print($"Error editing entry {id}: {ex.Message}"); return; }
            using var dlg = new EditFoodEntryForm(_services, id);
            dlg.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            dlg.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshUI();
            }
        }

        private void DeleteFoodEntry(int id)
        {
            FoodEntry? entry = _services.foodLogService.DeleteEntry(id);
            if (entry != null)
            {
                Print($"Deleted entry [{entry.Id}] ({entry.Time}) \"{entry.ItemName}\" (x{entry.Amount:0.00}): {entry.Calories}Kcal, {entry.Protein}g protein, {entry.Carbs}g carbs, {entry.Fat}g fat, Notes: {entry.Notes}");
                RefreshUI();
            }
            else
            {
                Print($"Error deleting entry ID '{id}': Probably does not exist.");
            }

        }

        private void btFEClear_Click(object sender, EventArgs e)
        {
            FEClear();
        }

        private void FEClear()
        {
            cbFEItem.Text = string.Empty;
            spinFEMult.Value = 1;
            cbFEFilter.SelectedIndex = 0;
            tbFECal.Text = string.Empty;
            tbFEPro.Text = string.Empty;
            tbFECar.Text = string.Empty;
            tbFEFat.Text = string.Empty;
            tbFENotes.Text = string.Empty;
        }

        private void btFECurrentTime_Click(object sender, EventArgs e)
        {
            dtpFood.Value = DateTime.Now;
        }

        private void btFENewPreset_Click(object sender, EventArgs e)
        {
            using var dlg = new NewPreset(_services);
            dlg.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            dlg.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Print($"Added entry, we don't have anything called \"Get last ID\", so you'll just have to trust me for now :)");
                RefreshUI();
            }
        }

        private void btWECurrentTime_Click(object sender, EventArgs e)
        {
            dtpWeight.Value = DateTime.Now;
        }

        private void btWEAdd_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(tbWeight.Text, out var weight))
            {
                MessageBox.Show($"Error adding weight:\nCould not parse '{tbWeight.Text}' as double: must be a number");
            }
            else
            {
                _services.weightLogService.AddEntry(dtpWeight.Value, weight);
                Print($"Added weight entry: {weight}" + (weightMode == 0 ? "kg" : weightMode == 1 ? "lbs" : "st"));
                tbWeight.Text = string.Empty;
            }
            UpdateWeightGraph();
        }

        private void btDEAdd_Click(object sender, EventArgs e)
        {
            if (_services.diaryService.AddEntry(tbDEDiary.Text) == null)
            {
                MessageBox.Show("Error adding diary entry, idk.");
            }
            else
            {
                Print($"Added Diary Entry: \n\n{tbDEDiary.Text}");
                tbDEDiary.Text = string.Empty;
            }

            try
            {
                DiaryEntry entry = _services.diaryService.AddEntry(tbDEDiary.Text);
                Print($"Added Diary Entry: \n\n{entry.Body}");
                tbDEDiary.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding diary entry: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private void btDEClear_Click(object sender, EventArgs e)
        {
            tbDEDiary.Text = string.Empty;
        }

        private void btDEViewDiary_Click(object sender, EventArgs e)
        {
            Print("Open diary...");
            var f = new DiaryView(_services);
            f.RequestViewDay += (_, date) => PreviousPeriod(date.Date, date.Date.AddDays(1));
            f.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            f.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            f.Show();
        }
      
        private void btBannerYesterday_Click(object sender, EventArgs e)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            PreviousPeriod(yesterday, yesterday.AddDays(1));
        }

        private void PreviousPeriod(DateTime startDate, DateTime endDate)
        {
            // Not implemented fully yet but this will eventually be a screen where you can view previous periods, from a single day to everything I guess. It will use DateTime because that's less finnicky than DateOnly in my experience, and maybe we would like to see stuff like, what did we eat from 6pm this day to 6am that day and stuff like that who knows?
            var f = new PreviousPeriods(startDate, endDate, _services);
            f.RequestEdit += (_, id) => EditFoodEntry(id);
            f.RequestDelete += (_, id) => DeleteFoodEntry(id);
            f.RequestRefresh += (_, _) => RefreshUI();
            f.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            f.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            f.Show();
        }

        private void tbNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb = (TextBox)sender;
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == '.' && !tb.Text.Contains('.')) return;
            if (e.KeyChar == '-' && !tb.Text.Contains("-")) return;
            e.Handled = true;
        }

        private void spinNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            var sb = (NumericUpDown)sender;
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == '.') return;
            if (e.KeyChar == '-' && sb.Value >= 0)
            {
                sb.Value *= -1;
                e.Handled = true;
            }
            if (e.KeyChar == '+' && sb.Value <= 0)
            {
                sb.Value *= -1;
                e.Handled = true;
            }
            e.Handled = true;
        }

        private void tbWeight_TextChanged(object sender, EventArgs e)
        {
            /* We put this in "TextChanged" because 1. That's actually what we are detecting here, it just sort of seems right.
               2. We already have a handler for KeyPresses, which is also used with something else.
               You might also notice that we do division for some conversions instead of multiplying them by other large specific decimals, this is to keep consistency.*/



            if (weightMode == 0)
            {
                if (!double.TryParse(tbWeight.Text, out double weight) || string.IsNullOrWhiteSpace(tbWeight.Text))
                {
                    labelWEUnitConvert.Text = "(0 lbs, 0 st)";

                }
                else
                {
                    double lbs = weight * 2.2046226218;
                    double st = lbs / 14;
                    labelWEUnitConvert.Text = $"({lbs:0.0} lbs, {st:0.0} st)";
                }
            }
            if (weightMode == 1)
            {
                if (!double.TryParse(tbWeight.Text, out double weight))
                {
                    labelWEUnitConvert.Text = "(0 st, 0 lbs)";
                }
                else
                {
                    double st = weight / 14;
                    double kg = weight / 2.2046226218;
                    labelWEUnitConvert.Text = $"({st:0.0} st, {kg:0.0} kg)";
                }
            }
            if (weightMode == 2)
            {
                if (!double.TryParse(tbWeight.Text, out double weight))
                {
                    labelWEUnitConvert.Text = "(0 lbs, 0 kg)";
                }
                else
                {
                    double lbs = weight * 14;
                    double kg = lbs * 2.2046226218;
                    labelWEUnitConvert.Text = $"({lbs:0.0} lbs, {kg:0.0} kg)";
                }
            }
            // else I guess nothing happens.
        }

        private void flpHistory_Resize(object sender, EventArgs e)
        {
            flpHistoryWidthAdjust();
        }

        private void flpHistoryWidthAdjust()
        {
            foreach (Control c in flpHistory.Controls)
            {
                c.Width = flpHistory.Width - 30;
            }
        }

        private void flpDT_Resize(object sender, EventArgs e)
        {
            flpDTWidthAdjust();
        }

        private void flpDTWidthAdjust()
        {
            foreach (Control c in flpDT.Controls)
            {
                c.Width = flpDT.Width - 40;
            }
        }

        private void TaskSetComplete(int id)
        {
            _services.taskService.SetComplete(id);
            Print($"Set task [{id}] complete");
            RefreshUI();
        }

        private void TaskSetIncomplete(int id)
        {
            _services.taskService.SetIncomplete(id);
            Print($"Set task [{id}] incomplete");
            RefreshUI();
        }

        private void btDTAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbDTNew.Text))
            {
                _services.taskService.AddTask(tbDTNew.Text);
                RefreshUI();
            }
        }

        //I know I should be doing graphing stuff outside of this but I am beyond caring for the time being:
        private void MakePieGoal(bool noGoal, double cal, double pro, double car, double fat)
        {
            // Clear everything and add the chart
            pieChartGoal.Series.Clear();
            pieChartGoal.ChartAreas.Clear();
            pieChartGoal.Legends.Clear();
            pieChartGoal.Titles.Clear();
            pieChartGoal.Annotations.Clear();

            pieChartGoal.ChartAreas.Add(new ChartArea("Main"));

            // Format Chart area
            var ca = pieChartGoal.ChartAreas["Main"];
            ca.Position.Auto = false;
            ca.Position = new ElementPosition(0, 0, 100, 100);
            ca.InnerPlotPosition.Auto = true;

            // Define series
            var s = new Series("Macros")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = noGoal ? false : true,
                LabelFormat = "0"
            };

            // Add data, only if they're over 0, so no empty sections, grey if no goal.
            if (car > 0) s.Points.AddXY("Carbs", car);
            if (fat > 0) s.Points.AddXY("Fat", fat);
            if (pro > 0) s.Points.AddXY("Protein", pro);
            if (noGoal)
            {
                var none = s.Points.AddXY("None", 1);
                s.Points[none].Color = Color.Gray;
            }

            // Title
            pieChartGoal.Titles.Add("Goal");

            // Inside annotation, "Calories: ####"
            var a = new TextAnnotation
            {
                Text = noGoal ? "No Goal" : $"Calories:\n{cal.ToString("0")}",
                AnchorX = 50,
                AnchorY = 50,
                Alignment = ContentAlignment.MiddleCenter,
                AnchorAlignment = ContentAlignment.MiddleCenter,
                ClipToChartArea = "Main",
                IsSizeAlwaysRelative = true,
            };
            pieChartGoal.Annotations.Add(a);
            s.SmartLabelStyle.Enabled = false;
            s["PieLabelStyle"] = "Inside";
            s.Font = new Font("SegoeUI", 8, FontStyle.Bold);

            // Some more formatting
            s["DoughnutRadius"] = "50";
            s.ChartType = SeriesChartType.Doughnut;
            s.Label = noGoal ? " " : "#VALX:\n#PERCENT{P0}";

            // Add it
            pieChartGoal.Series.Add(s);
        }

        private void MakePieActual(double cal, double pro, double car, double fat)
        {
            // For a more well commented version, see "PreviousPeriods.cs"
            // Clear everything and add the chart
            pieChartActual.Series.Clear();
            pieChartActual.ChartAreas.Clear();
            pieChartActual.Legends.Clear();
            pieChartActual.Titles.Clear();
            pieChartActual.Annotations.Clear();

            pieChartActual.ChartAreas.Add(new ChartArea("Main"));

            // this is the only kind of data formatting we're going here
            bool empty = false;
            if (car <= 0 && fat <= 0 && pro <= 0) empty = true;

            // I would call this "format chart", but everything here is formatting the chart.
            var ca = pieChartActual.ChartAreas["Main"];
            ca.Position.Auto = false;
            ca.Position = new ElementPosition(0, 0, 100, 100);
            ca.InnerPlotPosition.Auto = true;

            var s = new Series("Macros")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0"
            };

            // Ensure they are non-zero before adding, grey if nothing.
            if (car > 0) s.Points.AddXY("Carbs", car);
            if (fat > 0) s.Points.AddXY("Fat", fat);
            if (pro > 0) s.Points.AddXY("Protein", pro);
            if (empty)
            {
                var none = s.Points.AddXY("None", 1);
                s.Points[none].Color = Color.Gray;
            }

            pieChartActual.Titles.Add("Actual");

            var a = new TextAnnotation
            {
                // In the goal one, we have this replaced with "No Goal" if there's none. We could do something similar here, but 0 Calories seems fine.
                Text = $"Calories:\n{cal.ToString("0")}",
                AnchorX = 50,
                AnchorY = 50,
                Alignment = ContentAlignment.MiddleCenter,
                AnchorAlignment = ContentAlignment.MiddleCenter,
                ClipToChartArea = "Main",
                IsSizeAlwaysRelative = true,
            };
            pieChartActual.Annotations.Add(a);
            s.SmartLabelStyle.Enabled = false;
            s["PieLabelStyle"] = "Inside";
            s.Font = new Font("SegoeUI", 8, FontStyle.Bold);

            s["DoughnutRadius"] = "50";
            s.ChartType = SeriesChartType.Doughnut;
            s.Label = empty ? " " : "#VALX:\n#PERCENT{P0}";

            pieChartActual.Series.Add(s);
        }

        private void btBannerSetGoal_Click(object sender, EventArgs e)
        {
            var f = new SetGoal(_services.goalService);
            f.RequestRefresh += (_, _) => RefreshUI();
            f.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            f.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            f.Show();
        }

        private void btBannerPreviousPerioriod_Click(object sender, EventArgs e)
        {
            var f = new PreviousPeriodSelect();
            f.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            f.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            f.RequestPreviousPeriod += (sender, times) => PreviousPeriod(times.Item1, times.Item2);
            f.Show();
        }

        private void buttonBannerNewGoal_Click(object sender, EventArgs e)
        {
            var f = new NewGoal(_services);
            f.RequestPrint += (sender, text) => Print($"{((Control)sender!).Name}: {text}");
            f.RequestPrintInline += (sender, text) => PrintInline($"{((Control)sender!).Name}: {text}");
            f.Show();
        }
    }
}
