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

namespace MacroTrack.BasicApp.Forms
{
    public partial class NewPreset : Form
    {
        private readonly CoreServices _services;
        public event EventHandler<string> RequestPrint;
        public event EventHandler<string> RequestPrintInline;
        public NewPreset(CoreServices services)
        {
            InitializeComponent();
            _services = services;

            cbUnit.SelectedIndex = 0;
            cbCategory.SelectedIndex = -1;

            List<string> CatList = _services.presetService.GetCategoryList();
            CatList.Order();
            cbCategory.BeginUpdate(); // I feel like doing all of this is a bit overkill, we odn't need to do live updates after all, do we?
            try
            {
                cbCategory.DataSource = null;
                cbCategory.DataSource = CatList;
                cbCategory.SelectedIndex = -1;
                cbCategory.Text = "";
            }
            finally
            {
                cbCategory.EndUpdate();
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            // Make sure the results are formatted somewhat correctly:
            bool error = false;
            string errorMessage = $"Could not add preset, invalid formatting:\n";
            string name = "";
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                error = true;
                errorMessage += $" - \"Name\" field is blank: give each preset a name.\n";
            }
            else name = tbName.Text;
            if (!double.TryParse(tbCal.Text, out double cal))
            {
                error = true;
                errorMessage += $" - Could not parse calorie amount {tbCal.Text}, must be a number.\n";
            }
            if (!double.TryParse(tbPro.Text, out double pro))
            {
                error = true;
                errorMessage += $" - Could not parse protein amount {tbPro.Text}, must be a number.\n";
            }
            if (!double.TryParse(tbCar.Text, out double car))
            {
                error = true;
                errorMessage += $" - Could not parse carb amount {tbCar.Text}, must be a number.\n";
            }
            if (!double.TryParse(tbFat.Text, out double fat))
            {
                error = true;
                errorMessage += $" - Could not parse fat amount {tbFat.Text}, must be a number.\n";
            }
            double? mass = null;
            string? unit = null;
            string? category = null;
            string? notes = null;

            if (!string.IsNullOrWhiteSpace(tbMass.Text))
            {
                if (!Double.TryParse(tbMass.Text, out double massParsed))
                {
                    error = true;
                    errorMessage += $" - Could not parse mass {tbMass.Text}, must be a number (or leave blank for none)\n";
                }
                else
                {
                    mass = massParsed;
                    unit = cbUnit.Text;
                }
            }

            category = string.IsNullOrWhiteSpace(cbCategory.Text) ? null : cbCategory.Text;
            notes = string.IsNullOrWhiteSpace(tbNotes.Text) ? null : tbNotes.Text;
            
            if (error)
            {
                MessageBox.Show(errorMessage);
            }
            else
            {
                try
                { 
                    _services.presetService.AddEntry(name, cal, pro, car, fat, mass, unit, category, notes);
                    Print($"Added preset \"{name}\".");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    Print($"Error adding preset:\n{ex.Message}");
                    MessageBox.Show($"Error adding preset:\n{ex.Message}");
                }
            }
        }

        private void tbNumeric_Keypress(object sender, KeyPressEventArgs e)
        {
            var tb = (TextBox)sender;
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == '.' && !tb.Text.Contains('.')) return;
            if (e.KeyChar == '-' && !tb.Text.Contains("-")) return;
            e.Handled = true;
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
