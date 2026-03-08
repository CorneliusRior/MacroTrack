using MacroTrack.Core.Logging;
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
    public partial class PreviousPeriodSelect : Form
    {
        private IMTLogger _logger;

        public event EventHandler<(DateTime, DateTime)>? RequestPreviousPeriod;
        public bool CloseOnView = false; // this can be set true if we want the window to close once we click "view".

        public event EventHandler<string>? RequestPrint;
        public event EventHandler<string>? RequestPrintInline;

        public PreviousPeriodSelect(CoreServices services)
        {
            InitializeComponent();
            _logger = services.Logger;
            SetToDefault();
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

        // Make sure all of the dateTimePickers have the right time, instead of giving the current time of when I put those controls in:
        public void SetToDefault()
        {
            dtpDay.Value = DateTime.Now.Date;
            dtpDayTime.Value = DateTime.Now;
            dtpWeek.Value = DateTime.Now.Date;
            dtpMonth.Value = DateTime.Now.Date;
            dtpCustomFrom.Value = DateTime.Now.Date.AddDays(-1);
            dtpCustomTo.Value = DateTime.Now.Date;
        }

        
        // Invoke previous period event:
        private void CallPreviousPeriod(DateTime startTime, DateTime endTime)
        {
            RequestPreviousPeriod?.Invoke(this, (startTime, endTime));
        }

        
        // "View" button functionality, and most of the logic:
        private void buttonView_Click(object sender, EventArgs e)
        {
            Print("View Button Clicked, doing so and so");
            // The logic will be done in here. Note also that these will be done by requesting a previous period from Form1, this select window can't do that, nor should it, given the number of Events which Previous Periods can invoke, which we would need to pass through this back to Form1, let's just do it once.
            DateTime startTime;
            DateTime endTime;

            if (rbDay.Checked)
            {
                if (rbDayCalendar.Checked)
                {
                    startTime = dtpDay.Value.Date;
                    endTime = startTime.AddDays(1);
                }
                else
                {
                    startTime = dtpDay.Value.Date + dtpDayTime.Value.TimeOfDay;
                    endTime = startTime.AddDays(1);
                }
            }
            else if (rbWeek.Checked)
            {
                if (rbWeekCalendar.Checked)
                {
                    startTime = dtpWeek.Value.AddDays(-(((int)dtpWeek.Value.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7)).Date;
                    endTime = startTime.AddDays(7);
                }
                else
                {
                    startTime = dtpWeek.Value;
                    endTime = startTime.AddDays(7);
                }

            }
            else if (rbMonth.Checked)
            {
                if (rbMonthCalendar.Checked)
                {
                    startTime = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1).Date;
                    endTime = startTime.AddMonths(1);
                }
                else
                {
                    startTime = dtpMonth.Value;
                    endTime = startTime.AddDays(30);
                }
            }
            else if (rbCustom.Checked)
            {
                startTime = dtpCustomFrom.Value;
                endTime = dtpCustomTo.Value;
            }
            else
            {
                MessageBox.Show("Error: No time frame selected.", "No time frame.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Make sure one is after the other. I'm actually a little bit curious what happens if we don't do this, probably it just returns nothing, but let't not do that:
            if (startTime.CompareTo(endTime) > 0)
            {
                MessageBox.Show($"Error: startTime ({startTime.ToString("g")}) is after endTime ({endTime.ToString("g")}).\nMust be the other way around.", "Start time after end time", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            Print($"Now showing previous period for times: {startTime.ToString("G")} to {endTime.ToString("G")}");
            CallPreviousPeriod(startTime, endTime);
            if (CloseOnView) Close();
        }

        
        // Radio Button Handling:
        private void TimeFrameRadio_CheckChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton rb || !rb.Checked) return;

            panelDay.Enabled = rbDay.Checked;
            panelWeek.Enabled = rbWeek.Checked;
            panelMonth.Enabled = rbMonth.Checked;
            panelCustom.Enabled = rbCustom.Checked;
        }
        
        private void rbDay24Hours_CheckedChanged(object sender, EventArgs e)
        {
            dtpDayTime.Enabled = rbDay24Hours.Checked;
        }
        

        // Quick buttons:
        private void buttonQYesterday_Click(object sender, EventArgs e)
        {
            CallPreviousPeriod(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);
        }

        private void buttonQWeek_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now.AddDays(-(((int)dtpWeek.Value.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7)).Date;
            DateTime endTime = startTime.AddDays(7);
            CallPreviousPeriod(startTime, endTime);
        }


        // "Cancel" button:
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
