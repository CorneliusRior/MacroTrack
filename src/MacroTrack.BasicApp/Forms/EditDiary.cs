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
    public partial class EditDiary : Form
    {
        private IMTLogger _logger;
        private DiaryEntry _entry;
        public event EventHandler<(int id, string body, string notes)>? RequestEdit;

        public EditDiary(CoreServices services, DiaryEntry entry)
        {
            InitializeComponent();
            _logger = services.Logger;
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
            Log("Editing, closing");
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Log("Cancelled, closing");
            Close();
        }

        protected void Log(string message, LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            _logger.Log(this, caller, level, message, ex);
        }
    }
}
