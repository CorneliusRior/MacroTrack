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
    public partial class FoodEntryCard : UserControl
    {
        public int Id { get; private set; }
        public event EventHandler<int>? RequestEdit;
        public event EventHandler<int>? RequestDelete;
        public FoodEntryCard()
        {
            InitializeComponent();
        }

        public void SetData(int id, DateTime time, string itemname, double amount, double calories, double protein, double carbs, double fat, string? notes)
        {
            Id = id;
            labelTitle.Text = itemname;
            labelMacros.Text = $"Cal: {calories:0} Pro: {protein:0} Car: {carbs:0} Fat: {fat:0}";
            labelTime.Text = time.ToString("g");
            labelAmount.Text = $"(x{amount:0.00})";
            labelNotes.Text = notes ?? "";
        }

        private void beDelete_Click(object sender, EventArgs e)
        {
            RequestDelete?.Invoke(this, Id);
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            RequestEdit?.Invoke(this, Id);
        }
    }
}
