using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MacroTrack.Core.Models;

namespace MacroTrack.BasicApp.UserControls
{
    public partial class DiaryCard : UserControl
    {
        private DiaryEntry _entry;
        public event EventHandler<int>? RequestEdit;
        public event EventHandler<int>? RequestDelete;
        public event EventHandler<DateTime>? RequestViewDay;
        private bool _VDEnabled = true;

        public DiaryCard(DiaryEntry entry)
        {
            InitializeComponent();
            _entry = entry;
            Populate();
            DoubleBuffered = true;
        }

        private void Populate()
        {
            labelHeader.Text = $"Entry [{_entry.Id}]: {_entry.Time.ToString("u")}";
            labelBody.Text = _entry.Body;
        }

        public void LabelSize(int width)
        {
            labelBody.MaximumSize = new Size(width, 0);
        }

        public void disableViewDay()
        {
            _VDEnabled = !_VDEnabled;
            if (!_VDEnabled) btViewDay.Visible = false;
            else btViewDay.Visible = true;
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            RequestEdit?.Invoke(this, _entry.Id);
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Delete entry #{_entry.Id}?",
                "Delete Entry",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if ( result == DialogResult.Yes ) RequestDelete?.Invoke(this, _entry.Id);
        }

        private void btViewDay_Click(object sender, EventArgs e)
        {
            RequestViewDay?.Invoke(this, _entry.Time);
        }
    }
}
