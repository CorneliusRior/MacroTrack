using MacroTrack.Core.Services;
using MacroTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroTrack.BasicApp
{
    public partial class EditFoodEntryForm : Form
    {
        private readonly CoreServices _services;
        private readonly int _id;
        private double LastMult;
        private FoodEntry entry;
        public event EventHandler<string> RequestPrint;
        public event EventHandler<string> RequestPrintInline;
        public EditFoodEntryForm(CoreServices services, int id)
        {
            InitializeComponent();
            _services = services;
            _id = id;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            entry = _services.foodLogService.GetEntry(_id);

            labelTitle.Text = $"Editing entry ID #{entry.Id} - {entry.Time.ToString("u")}";
            dtpFood.Value = entry.Time;
            tbName.Text = entry.ItemName;
            tbCal.Text = entry.Calories.ToString();
            tbPro.Text = entry.Protein.ToString();
            tbCar.Text = entry.Carbs.ToString();
            tbFat.Text = entry.Fat.ToString();
            spinMult.Value = (decimal)entry.Amount;
            LastMult = entry.Amount;
            tbNotes.Text = entry.Notes ?? "";
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            // Check to see if everything is okay!!!
            bool error = false;
            string errorMessage = "Error: Invalid formatting:\n";
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                error = true;
                errorMessage += $" - \"Name\" is blank, must be populated.\n";
            }
            if (!double.TryParse(tbCal.Text, out double calories))
            {
                error = true;
                errorMessage += $" = \"Cal\" cannot be parsed as a double, must be a number.";
            }
            if (!double.TryParse(tbPro.Text, out double protein))
            {
                error = true;
                errorMessage += $" = \"Pro\" cannot be parsed as a double, must be a number.";
            }
            if (!double.TryParse(tbCar.Text, out double carbs))
            {
                error = true;
                errorMessage += $" = \"Car\" cannot be parsed as a double, must be a number.";
            }
            if (!double.TryParse(tbFat.Text, out double fat))
            {
                error = true;
                errorMessage += $" = \"Fat\" cannot be parsed as a double, must be a number.";
            }
            if (error)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            string notes = string.IsNullOrWhiteSpace(tbNotes.Text) ? string.Empty : tbNotes.Text;

            _services.foodLogService.EditEntry(_id, dtpFood.Value, tbName.Text, (double)spinMult.Value, calories, protein, carbs, fat, null, notes);
            Print($"Edited entry #{_id}");
            DialogResult = DialogResult.OK;
            Close();
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

        private void spinMult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double mult = (double)spinMult.Value;

                if (LastMult == 0)
                {
                    tbCal.Text = entry.Calories.ToString();
                    tbPro.Text = entry.Protein.ToString();
                    tbCar.Text = entry.Carbs.ToString();
                    tbFat.Text = entry.Fat.ToString();
                    LastMult = entry.Amount;
                }

                if (double.TryParse(tbCal.Text, out double cal) && cal != 0) tbCal.Text = ((cal / LastMult) * mult).ToString("0.##");
                if (double.TryParse(tbPro.Text, out double pro) && pro != 0) tbPro.Text = ((pro / LastMult) * mult).ToString("0.##");
                if (double.TryParse(tbCar.Text, out double car) && car != 0) tbCar.Text = ((car / LastMult) * mult).ToString("0.##");
                if (double.TryParse(tbFat.Text, out double fat) && fat != 0) tbFat.Text = ((fat / LastMult) * mult).ToString("0.##");
                LastMult = mult;
            }

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
