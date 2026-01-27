using MacroTrack.BasicApp.Forms;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MacroTrack.BasicApp.Forms
{
    public partial class SetGoal : Form
    {
        private CoreServices Services;
        private IMTLogger _logger;
        public event EventHandler<string>? RequestPrint;
        public event EventHandler<string>? RequestPrintInline;

        private List<Goal> GoalReg;
        private bool _loading = true;
        public event EventHandler? RequestRefresh;

        public SetGoal(CoreServices services)
        {
            InitializeComponent();
            Services = services;
            _logger = Services.Logger;
            Populate();
            GoalReg = Services.goalService.GetAllGoals();
            dtpStart_ValueChanged(this, EventArgs.Empty);
            buttonCancel.Focus();
        }

        protected void Print(string text)
        {
            RequestPrint?.Invoke(this, text);
        }

        protected void PrintInline(string text)
        {
            RequestPrintInline?.Invoke(this, text);
        }
        private void Log(string message, LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            _logger.Log(this, caller, level, message, ex);
        }

        private void Populate()
        {            
            dgvGoals.AutoGenerateColumns = false;
            dgvGoals.Columns.Clear();

            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GoalName",
                HeaderText = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Calories",
                HeaderText = "Calories (g)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Protein",
                HeaderText = "Protein (g)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Carbs",
                HeaderText = "Carbs (g)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fat",
                HeaderText = "Fat (g)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            
            dgvGoals.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Notes",
                HeaderText = "Notes",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            
            dgvGoals.DataSource = GoalReg;

            UpdatePieChart(true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dgvGoals.ClearSelection();
            dgvGoals.CurrentCell = null;
            _loading = false;
        }

        private void dgvGoals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGoals.CurrentRow?.DataBoundItem is Goal g && !_loading)
            {
                labelGoalName.Text = g.GoalName;
                // Update chart now
                UpdatePieChart(false, g.Calories, g.Protein, g.Carbs, g.Fat);

                // Fill in table.

                labelType.Text = g.GoalType == null ? "Type: No type" : $"Type: {g.GoalType}";

                labelMTPP.Text = g.Calories <= 0 ? "-%" : (((g.Protein * 4) / g.Calories)).ToString("0.0%");
                labelMTCP.Text = g.Calories <= 0 ? "-%" : (((g.Carbs * 4) / g.Calories)).ToString("0.0%");
                labelMTFP.Text = g.Calories <= 0 ? "-%" : (((g.Fat * 9) / g.Calories)).ToString("0.0%");

                labelMTPG.Text = $"{g.Protein.ToString("0.#")}g";
                labelMTCG.Text = $"{g.Carbs.ToString("0.#")}g";
                labelMTFG.Text = $"{g.Fat.ToString("0.#")}g";

                labelMTPN.Text = g.MinPro.HasValue ? $"{g.MinPro.ToString()}g" : "-";
                labelMTCN.Text = g.MinCar.HasValue ? $"{g.MinCar.ToString()}g" : "-";
                labelMTFN.Text = g.MinFat.HasValue ? $"{g.MinFat.ToString()}g" : "-";

                labelMTPX.Text = g.MaxPro.HasValue ? $"{g.MaxPro.ToString()}g" : "-";
                labelMTCX.Text = g.MaxCar.HasValue ? $"{g.MaxCar.ToString()}g" : "-";
                labelMTFX.Text = g.MaxFat.HasValue ? $"{g.MaxFat.ToString()}g" : "-";

                labelNotes.Text = g.Notes == null ? "-" : g.Notes;
            }
        }

        private void UpdatePieChart(bool noGoal, double cal = 0, double pro = 0, double car = 0, double fat = 0)
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            if (dgvGoals.CurrentRow?.DataBoundItem is Goal g)
            {
                string MessageString = $"Set goal to {g.GoalName} ({g.Calories}) from:\n";
                DateTime FromDate = dtpStart.Value;

                GoalActivation? NextGoal = Services.goalService.GetNextGoal(DateOnly.FromDateTime(FromDate));
                if (NextGoal != null)
                {
                    DateTime ToDate = NextGoal.ActivatedAt.ToDateTime(TimeOnly.MinValue);
                    MessageString += $"{FromDate.ToString("D")}, to {ToDate.ToString("D")}.\n";
                }
                else
                {
                    MessageString += $"{FromDate.ToString("D")}, onwards.\n";
                }

                var confirm = MessageBox.Show(
                    $"{MessageString}Are you sure?",
                    "Confirm",
                    MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    Services.goalService.ActivateGoal(g.Id, DateOnly.FromDateTime(dtpStart.Value));
                    Print(MessageString);
                    RequestRefresh?.Invoke(this, e);
                    Close(); // Alternatively, it could stay open, so that you can set up multiple.
                }
            }
            else
            {
                MessageBox.Show("No goal selected, select goal or click \"Cancal\" to leave.");
                Print("This is to affirm that print requests work!!!");
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            // Essentially, update header.
            DateTime FromDate = dtpStart.Value;
            GoalActivation? NextGoal = Services.goalService.GetNextGoal(DateOnly.FromDateTime(FromDate));
            if (NextGoal != null)
            {
                DateTime ToDate = NextGoal.ActivatedAt.ToDateTime(TimeOnly.MinValue);
                labelHeader.Text = $"Set Goal from {FromDate.ToString("d")} to {ToDate.ToString("d")}";
            }
            else
            {
                labelHeader.Text = $"Set Goal for {FromDate.ToString("d")}";
            }
        }
    }
}
