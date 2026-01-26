using MacroTrack.BasicApp.UserControls;
using MacroTrack.Core.AppModels;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MacroTrack.BasicApp.Forms
{
    public partial class PreviousPeriods : Form
    {
        private DateTime StartDate;
        private DateTime EndDate;
        private CoreServices _services;
        public event EventHandler<int> RequestEdit;
        public event EventHandler<int> RequestDelete;
        public event EventHandler RequestRefresh;
        public event EventHandler<string> RequestPrint;
        public event EventHandler<string> RequestPrintInline;
        public PreviousPeriods(DateTime startDate, DateTime endDate, CoreServices services)
        {
            StartDate = startDate;
            EndDate = endDate;
            _services = services;
            InitializeComponent();
            this.KeyPreview = true;
            Populate();
        }

        private void Populate()
        {
            // See if it is one day.
            if (StartDate != StartDate.Date || EndDate != EndDate.Date)
            {
                if (StartDate.Date == EndDate.Date) labelTimeFrame.Text = $"{StartDate.DayOfWeek.ToString()}, {StartDate.ToString("d")}, from {StartDate.ToString("t")} to {EndDate.ToString("t")}";
                else labelTimeFrame.Text = $"From {StartDate.ToString("t")} on {StartDate.DayOfWeek.ToString()}, {StartDate.ToString("d")} to {EndDate.ToString("t")} on {EndDate.DayOfWeek.ToString()}, {StartDate.ToString("d")}";
            }
            else if (EndDate - StartDate == TimeSpan.FromDays(1)) labelTimeFrame.Text = $"{StartDate.DayOfWeek.ToString()}, {StartDate.ToString("D")}";
            else labelTimeFrame.Text = $"From {StartDate.DayOfWeek.ToString()}, {StartDate.ToString("d")} to {EndDate.DayOfWeek.ToString()}, {StartDate.ToString("d")}";

            Text = labelTimeFrame.Text;
            UpdateSummary();
            UpdateHistory();
            flpHistoryWidthAdjust();
            UpdateDiary();
            UpdateTasks();
        }

        // Visual data, bars, summary, charts:
        private void UpdateSummary()
        {
            MacroSummary Summary = _services.dataService.GetMacroSummary(StartDate, EndDate);

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

            MakePieGoal(Summary.NoGoal, Summary.Target.Calories, Summary.Target.Protein, Summary.Target.Carbs, Summary.Target.Fat);
            MakePieActual(Summary.Actual.Calories, Summary.Actual.Protein, Summary.Actual.Carbs, Summary.Actual.Fat);
            UpdateWeightGraph();
            UpdateWeightLabel();
            UpdateCalGraph();

            // Debugging thing: I just wanted to know what a macrosummary actually looked like, added bool NoGoal after this. I'd delete this, but it could be handy to have lying around:
            // MessageBox.Show($"This is a moderately crude way of doing it, but here is the present MacroSummary:\nFrom: {Summary.From.ToString("u")}\nTo: {Summary.To.ToString("u")}\nGoal Name: {Summary.GoalName}\n\nTarget:\n{Summary.Target.Calories.ToString()} // {Summary.Target.Protein.ToString()} // {Summary.Target.Carbs.ToString()} // {Summary.Target.Fat.ToString()}\n\nActual:\n{Summary.Actual.Calories.ToString()} // {Summary.Actual.Protein.ToString()} // {Summary.Actual.Carbs.ToString()} // {Summary.Actual.Fat.ToString()}\n\nRemaining:\n{Summary.Remaining.Calories.ToString()} // {Summary.Remaining.Protein.ToString()} // {Summary.Remaining.Carbs.ToString()} // {Summary.Remaining.Fat.ToString()}");
        }

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

            // Title
            pieChartActual.Titles.Add("Actual");

            // Inside annotation ("Calories: ####")
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

            // Some more formatting
            s["DoughnutRadius"] = "50";
            s.ChartType = SeriesChartType.Doughnut;
            s.Label = empty ? " " : "#VALX:\n#PERCENT{P0}";

            // Add it
            pieChartActual.Series.Add(s);
        }

        private void UpdateWeightGraph()
        {
            DateTime GraphEndDate = EndDate.AddDays(15);
            DateTime GraphStartDate = StartDate.AddDays(-15);

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
            ca.AxisX.Minimum = GraphStartDate.ToOADate();
            ca.AxisX.Maximum = GraphEndDate.ToOADate();
            ca.AxisX.LabelStyle.Format = "dd/MM";
            ca.AxisY.MajorGrid.Enabled = true;

            // Set up series
            var s = new StripLine
            {
                Interval = 0,
                IntervalOffset = StartDate.ToOADate(),
                StripWidth = EndDate.ToOADate() - StartDate.ToOADate(),
                BackColor = Color.Gold,
                BackHatchStyle = ChartHatchStyle.BackwardDiagonal

            };

            var w = new Series("Weight")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Double,
                BorderWidth = 4,
                Color = Color.Black
            };

            // Give it data
            List<WeightEntry> pts = _services.dataService.GetWeightEntries(GraphStartDate, GraphEndDate).OrderBy(pts => pts.Time).ToList();
            foreach (WeightEntry p in pts) w.Points.AddXY(p.Time, p.Weight);

            // Add
            ca.AxisX.StripLines.Add(s);
            lineChartWeight.Series.Add(w);
        }

        private void UpdateWeightLabel()
        {
            List<WeightEntry> WeightEntries = _services.dataService.GetWeightEntries(StartDate, EndDate).OrderBy(pts => pts.Time).ToList();
            if (WeightEntries.Count == 0) labelWeight.Text = "No entries for this period";
            else 
            {
                string WeightString;
                if (WeightEntries.Count == 1) WeightString = "There was 1 entry during this period:";
                else WeightString = $"There were {WeightEntries.Count} entries during this period:";
                foreach (WeightEntry e in WeightEntries)
                {
                    WeightString += $"\n[{e.Id}] @{e.Time.ToString("g")}: {e.Weight}Kg";
                }
                labelWeight.Text = WeightString;
            }
        }

        private void UpdateCalGraph()
        {
            DateTime GraphEndDate = EndDate.AddDays(15);
            DateTime GraphStartDate = StartDate.AddDays(-15);

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
            ca.AxisX.Minimum = GraphStartDate.ToOADate();
            ca.AxisX.Maximum = GraphEndDate.ToOADate();
            ca.AxisX.LabelStyle.Format = "dd/MM";
            ca.AxisY.MajorGrid.Enabled = true;

            // Set up series
            var s = new StripLine
            {
                Interval = 0,
                IntervalOffset = StartDate.ToOADate(),
                StripWidth = EndDate.ToOADate() - StartDate.ToOADate(),
                BackColor = Color.Gold,
                BackHatchStyle = ChartHatchStyle.BackwardDiagonal
                
            };

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
            List<GoalSeriesPoint> goalCal = _services.dataService.GetGoalSeries(GraphStartDate, GraphEndDate);
            foreach (GoalSeriesPoint p in goalCal) g.Points.AddXY(p.Date, p.Calories);
            //foreach (GoalSeriesPoint p in goalCat) Print($"Point: [{p.Id}]: {p.Date} Cal: {p.Calories}");

            List<CalSeriesPoint> actualCal = _services.dataService.GetCalSeries(GraphStartDate, GraphEndDate);
            foreach (CalSeriesPoint p in actualCal) c.Points.AddXY(p.Date, p.Calories);
            //foreach (CalSeriesPoint p in actualCal) Print($"Point: [{p.Id}]: {p.Date} Cal: {p.Calories}");

            // Add
            ca.AxisX.StripLines.Add(s);
            lineChartCal.Series.Add(g);
            lineChartCal.Series.Add(c);
        }

        // Food entries:
        private void UpdateHistory()
        {
            flpHistory.SuspendLayout();
            List<FoodEntry>? entries = _services.foodLogService.FromTimes(StartDate, EndDate);
            entries.Reverse();
            flpHistory.Controls.Clear();

            try
            {
                if (entries.Count == 0)
                {
                    Label NoEntryLabel = new Label
                    {
                        Text = "No entries for this period",
                        //AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(10, 10, 10, 10),
                    };
                    flpHistory.Controls.Add(NoEntryLabel);
                }
                else
                {
                    foreach (FoodEntry e in entries)
                    {
                        FoodEntryCard card = new FoodEntryCard();
                        card.SetData(e.Id, e.Time, e.ItemName, e.Amount, e.Calories, e.Protein, e.Carbs, e.Fat, e.Notes);

                        card.RequestEdit += (_, id) => EditFoodEntry(id);
                        card.RequestDelete += (_, id) => DeleteFoodEntry(id);

                        flpHistory.Controls.Add(card);
                    }
                }
            }
            finally
            {
                flpHistory.ResumeLayout();
            }
        }

        private void EditFoodEntry(int id)
        {
            RequestEdit?.Invoke(this, id);
            Populate();
        }

        private void DeleteFoodEntry(int id)
        {
            RequestDelete?.Invoke(this, id);
            Populate();
        }

        private void flpHistoryWidthAdjust()
        {
            foreach (Control c in flpHistory.Controls)
            {
                c.Width = flpHistory.Width - 40;
            }
        }

        private void flpHistory_SizeChanged(object sender, EventArgs e)
        {
            flpHistoryWidthAdjust();
        }

        //Diary:
        private void UpdateDiary()
        {
            flpDiary.SuspendLayout();
            List<DiaryEntry> Diary = _services.diaryService.FromTimes(StartDate, EndDate);
            flpDiary.Controls.Clear();

            try
            {          
                if (Diary.Count == 0) // this is meant to be Diary.Count == 0, but changing this to test something.
                {
                    Label NoEntryLabel = new Label
                    {
                        Text = "No entries for this period",
                        //AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(10, 10, 10, 10),
                    };                    
                    flpDiary.Controls.Add(NoEntryLabel);
                }
                else
                {
                    foreach (DiaryEntry e in Diary)
                    {
                        DiaryCard card = new DiaryCard(e);
                        card.RequestEdit += (_, id) => EditDiaryEntry(id);
                        card.RequestDelete += (_, id) => DeleteDiaryEntry(id);
                        card.disableViewDay(); // due to the fact that we are already viewing the day, we don't need this button.

                        flpDiary.Controls.Add(card);
                        card.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
                    
                }
            finally
            {
                flpDiary.ResumeLayout();
            }
            FlpDiaryWidthAdjust();
            
        }

        private void EditDiaryEntry(int id)
        {
            DiaryEntry? entry;
            try { entry = _services.diaryService.GetEntry(id); }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting entry {id}, probably doesn't exist: {ex.Message}");
                return;
            }
            if (entry == null)
            {
                MessageBox.Show($"Error getting entry {id}: null return, probably doesn't exist");
                return;
            }

            using var dlg = new EditDiary(entry);
            dlg.RequestEdit += (_, edits) => DiaryEditHandler(edits);
            if (dlg.ShowDialog() == DialogResult.OK) UpdateDiary();
        }

        private void DiaryEditHandler((int Id, string body, string notes) edits)
        {
            try
            {
                _services.diaryService.EditEntry(edits.Id, edits.body, edits.notes);
                UpdateDiary();
            }
            catch (Exception ex) { MessageBox.Show($"Error editing entry: {ex.Message}"); }
            // Had a long coment in Form1.EditHandler(), which I copied this from. We already have Diaries being edited in DiaryView.cs instead of Form1, because diary entries don't actually appear on form1, but food entries do, which is why we need to pass food edits back there but not diary edits.
        }

        private void DeleteDiaryEntry(int id)
        {
            _services.diaryService.DeleteEntry(id);
            UpdateDiary();
        }

        private void FlpDiaryWidthAdjust()
        {
            int w = flpDiary.ClientSize.Width - (flpDiary.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0) - 20;
            foreach (var card in flpDiary.Controls.OfType<DiaryCard>())
            {
                card.Width = w;
                card.SuspendLayout();
                card.LabelSize(w);
                card.ResumeLayout(true);
            }
            foreach (var lbl in flpDiary.Controls.OfType<Label>())
            {
                lbl.Width = w;
            }
        }

        private void flpDiary_SizeChanged(object sender, EventArgs e)
        {
            FlpDiaryWidthAdjust();
        }

        // Tasks
        private void UpdateTasks()
        {
            // We had a bit of difficulty with this for a while: RefreshUI would be counted as a "Check change", so this was ran resursively until every one of the tasks was set as incomplete. We fixed this by adding a _loading bool in taskListItem.cs, which solved everything very quickly, but in looking for the precise location of the issue I found that it's a bit inefficient to have each of the tasks update every single time, so I wrote out a method called "UpdateOneTask(int id)", which would have worked, only we don't have a TaskRepo method of getting the streak in that case. If you want to make one, copying the "get all streak", you can, and then put it here, but for the time being I have deleted the method. Much of the slowdown might have come from all of the "Print" statements I'll get rid of now.

            flpTasks.SuspendLayout();
            List<DailyTask> TaskList = _services.taskService.GetAllStreaks(StartDate, filterInactive: true);
            flpTasks.Controls.Clear();

            try
            {
                foreach (DailyTask t in TaskList)
                {
                    taskListItem i = new taskListItem();
                    i.SetData(t.Id, t.Completed, t.Name, t.Streak);

                    i.RequestSetCompleted += (_, id) => TaskSetComplete(id);
                    i.RequestSetIncomplete += (_, id) => TaskSetIncomplete(id);
                    i.RequestPrint += (_, text) => TaskItemPrint(text);

                    flpTasks.Controls.Add(i);
                }
            }
            finally
            {
                flpTasks.ResumeLayout(true);
            }
            flpTasksWidthAdjust();
        }

        private void TaskItemPrint(string text)
        {
            Print($"A task item says: \"{text}\" ");
        }

        private void TaskSetComplete(int id)
        {
            _services.taskService.SetComplete(id, StartDate);
            RequestRefresh?.Invoke(this, EventArgs.Empty);
            UpdateTasks();
        }

        private void TaskSetIncomplete(int id)
        {
            _services.taskService.SetIncomplete(id, StartDate);
            RequestRefresh?.Invoke(this, EventArgs.Empty);
            UpdateTasks();
        }

        private void flpTasksWidthAdjust()
        {
            foreach (Control c in flpTasks.Controls)
            {
                c.Width = flpTasks.Width - 40;
            }
        }

        private void flpTasks_SizeChanged(object sender, EventArgs e)
        {
            flpTasksWidthAdjust();
        }

        // Previous & Next buttons
        private void btPrevious_Click(object sender, EventArgs e)
        {
            Previous();            
        }
                
        private void btNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void Previous()
        {
            TimeSpan delta = EndDate - StartDate;
            StartDate = StartDate - delta;
            EndDate = EndDate - delta;
            Populate();
        }

        private void Next()
        {
            // This will be done by taking the difference between the two and subtracting it. Crude maybe and weird when you have times, but whatever.
            TimeSpan delta = EndDate - StartDate;
            StartDate = StartDate + delta;
            EndDate = EndDate + delta;
            Populate();
        }

        private void PreviousPeriods_KeyDown(object sender, KeyEventArgs e)
        {
                        
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                Previous();
            }
            else if (keyData == Keys.Right)
            {
                Next();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Print(string text)
        {
            RequestPrint?.Invoke(this, text);
        }

        private void PrintInline(string text)
        {
            RequestPrint?.Invoke(this, text);
        }
    }
}
