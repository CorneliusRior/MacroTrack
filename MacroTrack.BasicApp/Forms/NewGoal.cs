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

namespace MacroTrack.BasicApp.Forms
{
    public partial class NewGoal : Form
    {
        private CoreServices Services;
        private IMTLogger _logger;
        public event EventHandler<string>? RequestPrint;
        public event EventHandler<string>? RequestPrintInline;

        private decimal Calories = 2000;        
        private bool _updatingSliders;

        public NewGoal(CoreServices services)
        {
            InitializeComponent();
            Services = services;
            _logger = Services.Logger;
            SetToDefault();

            sliderProtein.ValueChanged += MacroSlider_ValueChanged;
            sliderCarbs.ValueChanged += MacroSlider_ValueChanged;
            sliderFat.ValueChanged += MacroSlider_ValueChanged;
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

        // Default (onload basically)
        public void SetToDefault()
        {
            tbName.Text = string.Empty;
            spinTotalCal.Value = 2000;

            checkMinCal.Checked = false;
            spinMinCal.Enabled = false;
            spinMinCal.Value = 1900;
            checkMaxCal.Checked = false;
            spinMaxCal.Enabled = false;
            spinMaxCal.Value = 2100;

            cbType.SelectedIndex = 0;

            sliderProtein.Enabled = true;
            sliderProtein.Value = 35;
            labelPro.Text = $"-%\n- Kcal\n-g";
            spinProMax.Enabled = false;
            spinProSet.Enabled = false;
            spinProMin.Enabled = false;

            sliderCarbs.Enabled = true;
            sliderCarbs.Value = 35;
            labelCar.Text = $"-%\n- Kcal\n-g";
            spinCarMax.Enabled = false;
            spinCarSet.Enabled = false;
            spinCarMin.Enabled = false;

            sliderFat.Enabled = true;
            sliderFat.Value = 30;
            labelFat.Text = $"-%\n- Kcal\n-g";
            spinFatMax.Enabled = false;
            spinFatSet.Enabled = false;
            spinFatMin.Enabled = false;

            spinTotalCal_ValueChanged(this, EventArgs.Empty);
            //sliderProtein_ValueChanged(this, EventArgs.Empty);
        }

        private void spinTotalCal_ValueChanged(object sender, EventArgs e)
        {
            Calories = spinTotalCal.Value;
            UpdateDisplayLabels();

        }

        // Check handling:
        private void Check_CheckChanged(object sender, EventArgs e)
        {
            // Enable/disable spins for Min & Max Cals:
            spinMinCal.Enabled = checkMinCal.Checked;
            spinMaxCal.Enabled = checkMaxCal.Checked;

            // Enable/disable set buttons when two are clicked, so that we won't have three:
            checkProSet.Enabled = !(checkCarSet.Checked && checkFatSet.Checked);
            checkCarSet.Enabled = !(checkProSet.Checked && checkFatSet.Checked);
            checkFatSet.Enabled = !(checkProSet.Checked && checkCarSet.Checked);

            // Enable/disable sliders and spins for each Macro:
            sliderProtein.Enabled = !checkProSet.Checked;
            spinProSet.Enabled = checkProSet.Checked;
            spinProMin.Enabled = checkProMin.Checked;
            spinProMax.Enabled = checkProMax.Checked;

            sliderCarbs.Enabled = !checkCarSet.Checked;
            spinCarSet.Enabled = checkCarSet.Checked;
            spinCarMin.Enabled = checkCarMin.Checked;
            spinCarMax.Enabled = checkCarMax.Checked;

            sliderFat.Enabled = !checkFatSet.Checked;
            spinFatSet.Enabled = checkFatSet.Checked;
            spinFatMin.Enabled = checkFatMin.Checked;
            spinFatMax.Enabled = checkFatMax.Checked;

            // Enable/disable set buttons when two are clicked, so that we won't have three:
            checkProSet.Enabled = !(checkCarSet.Checked && checkFatSet.Checked);
            checkCarSet.Enabled = !(checkProSet.Checked && checkFatSet.Checked);
            checkFatSet.Enabled = !(checkProSet.Checked && checkCarSet.Checked);

        }

        private void MacroSlider_ValueChanged(object? sender, EventArgs e)
        {
            if (_updatingSliders) return;
            if (sender is not TrackBar changed) return;

            _updatingSliders = true;
            try
            {
                if (changed == sliderProtein) Redistribute(changed: sliderProtein, otherA: sliderCarbs, otherB: sliderFat);
                else if (changed == sliderCarbs) Redistribute(changed: sliderCarbs, otherA: sliderProtein, otherB: sliderFat);
                else if (changed == sliderFat) Redistribute(changed: sliderFat, otherA: sliderProtein, otherB: sliderCarbs);

                UpdateDisplayLabels();
            }
            finally
            {
                _updatingSliders = false;
            }
        }

        private void Redistribute(TrackBar changed, TrackBar otherA, TrackBar otherB)
        {
            int x = changed.Value;
            int remaining = 100 - x;
            int a = otherA.Value;
            int b = otherB.Value;
            int aNew;
            int bNew;

            if (remaining < 0)
            {
                changed.Value = 100;
                otherA.Value = 0;
                otherB.Value = 0;
                return;
            }

            int sum = a + b;
            if (sum == 0)
            {
                otherA.Value = remaining / 2;
                otherB.Value = remaining / 2;
                return;
            }

            if (!otherA.Enabled && !otherB.Enabled)
            {
                changed.Value = Math.Max(100 - otherA.Value - otherB.Value, 0);
                return;
            }

            if (!otherA.Enabled)
            {
                otherB.Value = Math.Max(remaining - otherA.Value, 0);
                changed.Value = Math.Min(changed.Value, 100 - otherA.Value);
                return;
            }

            if (!otherB.Enabled)
            {
                otherA.Value = Math.Max(remaining - otherA.Value, 0);
                changed.Value = Math.Min(changed.Value, 100 - otherA.Value);
                return;
            }

            // Actual scaling now:
            aNew = (int)Math.Round(remaining * (a / (double)sum));
            aNew = Math.Clamp(aNew, 0, remaining);
            bNew = remaining - aNew;

            otherA.Value = aNew;
            otherB.Value = bNew;
        }

        private void UpdateDisplayLabels()
        {
            // In which we update the three line strings
            int pPct = sliderProtein.Value;
            int cPct = sliderCarbs.Value;
            int fPct = sliderFat.Value;

            decimal pCal = Calories * pPct / 100m;
            decimal cCal = Calories * cPct / 100m;
            decimal fCal = Calories * fPct / 100m;

            decimal pG = pCal / 4m;
            decimal cG = cCal / 4m;
            decimal fG = fCal / 9m;

            labelPro.Text = $"{pPct.ToString("0.#")}%\n{pCal.ToString("0.#")} Kcal\n{pG.ToString("0.#")}g";
            labelCar.Text = $"{cPct.ToString("0.#")}%\n{cCal.ToString("0.#")} Kcal\n{cG.ToString("0.#")}g";
            labelFat.Text = $"{fPct.ToString("0.#")}%\n{fCal.ToString("0.#")} Kcal\n{fG.ToString("0.#")}g";

            spinProSet.Value = pG;
            spinCarSet.Value = cG;
            spinFatSet.Value = fG;

            if (!checkProMax.Checked) spinProMax.Value = pG + 40;
            if (!checkCarMax.Checked) spinCarMax.Value = cG + 40;
            if (!checkFatMax.Checked) spinFatMax.Value = fG + 20;

            if (!checkProMin.Checked) spinProMin.Value = Math.Max(pG + 40, 0);
            if (!checkCarMin.Checked) spinCarMin.Value = Math.Max(cG + 40, 0);
            if (!checkFatMin.Checked) spinFatMin.Value = Math.Max(fG + 20, 0);
        }

        /*
        private void sliderProtein_ValueChanged(object sender, EventArgs e)
        {
            decimal pct = sliderProtein.Value;
            decimal dec = pct / 100;
            labelPro.Text = $"{pct}%\n{(Calories * dec).ToString("0")} Kcal\n{(MaxProP * dec).ToString("0")}g";
            // We just put it on a sliding scale from 0 to 1 for the calories part.

            spinProSet.Value = MaxProP * dec;
            if (!spinProMin.Enabled) spinProMin.Value = Math.Max(spinProSet.Value - 40, 0);
            if (!spinProMax.Enabled) spinProMax.Value = spinProSet.Value + 40;

            // Change proportions of others, first, assuming one of them is locked. If both were locked, this would be disabled! 
            if (!sliderCarbs.Enabled) sliderFat.Value = 100 - sliderProtein.Value - sliderCarbs.Value;
            if (!sliderFat.Enabled) sliderCarbs.Value = 100 - sliderProtein.Value - sliderFat.Value;


        }*/

        private void spinMinMaxClamp_ValueChanged(object sender, EventArgs e)
        {
            if (spinProSet.Enabled) sliderProtein.Value = (int)Math.Round((spinProSet.Value * 400) / Calories);
            if (spinCarSet.Enabled) sliderCarbs.Value = (int)Math.Round((spinCarSet.Value * 400) / Calories);
            if (spinFatSet.Enabled) sliderFat.Value = (int)Math.Round((spinFatSet.Value * 900) / Calories);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Close out of \"New Goal\" window, are you sure?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // Try to parse everything into something...
            string name = tbName.Text;
            string? type = cbType.SelectedIndex == 0 ? null : cbType.SelectedItem!.ToString();

            double totalCalories = Math.Round((double)spinTotalCal.Value, 1);
            double? minCal = checkMinCal.Checked ? Math.Round((double)spinMinCal.Value, 1) : null;
            double? maxCal = checkMaxCal.Checked ? Math.Round((double)spinMaxCal.Value, 1) : null;

            double protein = Math.Round((double)spinProSet.Value, 1);
            double? minPro = checkProMin.Checked ? Math.Round((double)spinProMin.Value, 1) : null;
            double? maxPro = checkProMax.Checked ? Math.Round((double)spinProMax.Value, 1) : null;

            double carbs = Math.Round((double)spinCarSet.Value, 1);
            double? minCar = checkCarMin.Checked ? Math.Round((double)spinCarMin.Value, 1) : null;
            double? maxCar = checkCarMax.Checked ? Math.Round((double)spinCarMax.Value, 1) : null;

            double fat = Math.Round((double)spinFatSet.Value, 1);
            double? minFat = checkFatMin.Checked ? Math.Round((double)spinFatMin.Value, 1) : null;
            double? maxFat = checkFatMax.Checked ? Math.Round((double)spinFatMax.Value, 1) : null;

            string? notes = string.IsNullOrWhiteSpace(tbNotes.Text) ? null : tbNotes.Text;

            // Throw errors here. Usually we are stricter with formatting, but we can just leave it at names:
            if (string.IsNullOrWhiteSpace(name))
            {
                var result = MessageBox.Show("You are attempting to make a goal with a blank/empty name. You can do that if you want to, but are you sure?", "Blank name", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
            }

            bool _error = false;
            string errorString = $"Warning: a goal by the name of \"{name}\" already exists.";
            List<Goal> GoalList = Services.goalService.GetAllGoals();
            foreach (Goal g in GoalList)
            {
                if (g.GoalName == name) 
                { 
                    _error = true;
                    errorString += $"\n[{g.Id}]: \"{g.GoalName}\"";
                }
            }
            if (_error)
            {
                var result = MessageBox.Show($"{errorString}\nYou can still add another goal with that same name, and they will still be distinct due to assigned IDs, but are you sure?", "Dupliate name", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
            }

            try 
            { 
                Goal goal = Services.goalService.AddGoal(name, totalCalories, protein, carbs, fat, type, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
                Print($"Added goal: ID = [{goal.Id}] GoalName = '{goal.GoalName}', Calories = '{goal.Calories}', Protein = '{goal.Protein}', Carbs = '{goal.Carbs}', fat: '{goal.Fat}', type = '{goal.GoalType}', notes: '{goal.Notes}'");
                Close();
            }
            catch (Exception ex)
            {
                Print($"Error creating goal: {ex.Message}");
                MessageBox.Show($"Error creating goal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

        }
    }
}
