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
    public partial class EditDiary : Form
    {
        private DiaryEntry _entry;
        public event EventHandler<(int id, string body, string notes)> RequestEdit;
        public EditDiary(DiaryEntry entry)
        {
            InitializeComponent();
            _entry = entry;
            Populate();
        }

        private void Populate()
        {
            labelHeader.Text = $"Edit entry #{_entry.Id} from {_entry.Time.ToString("g")}";
            //tbBody.Text = _entry.Body.Replace("\n", Environment.NewLine);
            tbBody.Text = _entry.Body;
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            RequestEdit?.Invoke(this, (_entry.Id, tbBody.Text, tbNotes.Text));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
