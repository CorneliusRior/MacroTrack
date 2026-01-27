using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroTrack.BasicApp
{
    public partial class taskListItem : UserControl
    {
        // Copied from PreviousPeriods.cs: We had a bit of difficulty with this for a while: RefreshUI would be counted as a "Check change", so this was ran resursively until every one of the tasks was set as incomplete. We fixed this by adding a _loading bool in taskListItem.cs, which solved everything very quickly, but in looking for the precise location of the issue I found that it's a bit inefficient to have each of the tasks update every single time, so I wrote out a method called "UpdateOneTask(int id)", which would have worked, only we don't have a TaskRepo method of getting the streak in that case. If you want to make one, copying the "get all streak", you can, and then put it here, but for the time being I have deleted the method. Much of the slowdown might have come from all of the "Print" statements I'll get rid of now.
        public int Id { get; private set; }
        public bool Completed { get; private set; }
        private bool _loading = true;
        public event EventHandler<int>? RequestSetCompleted;
        public event EventHandler<int>? RequestSetIncomplete;
        public event EventHandler<string>? RequestPrint;
        public taskListItem()
        {
            InitializeComponent();
        }

        public void SetData(int id, bool completed, string taskName, int streak)
        {
            _loading = true;
            Id = id;
            Completed = completed;
            checkBoxTask.Text = taskName;
            checkBoxTask.Checked = Completed;
            labelStreak.Text = streak.ToString();
            _loading = false;
        }

        private void checkBoxTask_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            if(!Completed)
            {
                RequestSetCompleted?.Invoke(this, Id);
                Completed = true;
            }
            else
            {
                RequestSetIncomplete?.Invoke(this, Id);
                Completed = false;
            }
        }

        private void Print(string text)
        {
            RequestPrint?.Invoke(this, text);
        }
    }
}
