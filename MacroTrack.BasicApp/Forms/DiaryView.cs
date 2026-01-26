using MacroTrack.BasicApp.UserControls;
using MacroTrack.Core.Services;
using MacroTrack.Core.Models;
using MacroTrack.BasicApp.Forms;
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
    public partial class DiaryView : Form
    {
        private readonly CoreServices _services;
        private DateTime StartDate;
        private bool ShowAll = false;
        public event EventHandler<DateTime> RequestViewDay;
        public event EventHandler<string> RequestPrint;
        public event EventHandler<string> RequestPrintInline;
        private readonly System.Windows.Forms.Timer _resizeTimer = new() { Interval = 100 };

        public DiaryView(CoreServices services)
        {
            InitializeComponent();
            _services = services;
            cbTimeFrame.SelectedIndex = 1;

            _resizeTimer.Tick += (_, __) =>
            {
                _resizeTimer.Stop();
                flpBodyAdjust();
            };

            flpMain.SizeChanged += (_, __) =>
            {
                _resizeTimer.Stop();
                _resizeTimer.Start();
            };


        }

        private void cbTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTimeFrame.SelectedIndex == 0) StartDate = DateTime.Now.Date.AddDays(-7);
            if (cbTimeFrame.SelectedIndex == 1) StartDate = DateTime.Now.Date.AddDays(-31);
            if (cbTimeFrame.SelectedIndex == 2) StartDate = DateTime.Now.Date.AddDays(-365);
            if (cbTimeFrame.SelectedIndex == 3) ShowAll = true;
            else ShowAll = false;
            labelTitle.Text = $"Diary: {StartDate.ToString("d")} to {DateTime.Now.ToString("d")}";
            LoadEntries();
        }

        private void LoadEntries()
        {
            List<DiaryEntry> Diary = new List<DiaryEntry>();

            if (ShowAll) Diary = _services.diaryService.GetAll();
            else Diary = _services.diaryService.FromTimes(StartDate, DateTime.Now);
            Diary.Reverse();
            flpMain.Controls.Clear();

            foreach (DiaryEntry de in Diary)
            {
                DiaryCard card = new DiaryCard(de);

                card.RequestEdit += (_, id) => EditEntry(id);
                card.RequestDelete += (_, id) => DeleteEntry(id);
                card.RequestViewDay += (_, date) => RequestVD(date);
                flpMain.Controls.Add(card);

                card.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }

            flpBodyAdjust();
        }

        private void EditEntry(int id)
        {
            DiaryEntry? entry;
            try { entry = _services.diaryService.GetEntry(id); }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error getting entry {id}, probably doesn't exist: {ex.Message}");
                return;
            }
            if (entry == null)
            {
                MessageBox.Show($"Error getting entry {id}: null return, probably doesn't exist");
                return;
            }

            using var dlg = new EditDiary(entry);
            dlg.RequestEdit += (_, edits) => EditHandler(edits);
            if (dlg.ShowDialog() == DialogResult.OK) LoadEntries(); 
        }

        private void EditHandler((int Id, string body, string notes) edits)
        {
            try 
            {
                DiaryEntry before = _services.diaryService.GetEntry(edits.Id)!;
                DiaryEntry after = _services.diaryService.EditEntry(edits.Id, edits.body, edits.notes);
                Print($"Edited entry #{edits.Id}:\n\nFrom:\n\"{before.Body}\"\n\nTo:\n\"{after.Body}\"");
                return;
            }
            catch (Exception ex) { MessageBox.Show($"Error editing entry: {ex.Message}"); }
            // After this, dialogue box will close and it will refresh anyway, according to the system here it will have been successful. I would like it if it didn't close the window but I don't know how to do that and don't want to bother. Anyway it does kind of make sense to do it this way: like the only reason this error could happen really is if you opened the edit dialogue, then deleted the entry in another program, then clicked "edit": at which point you might like it if it updated to show you that the entry isn't there anymore. If you would like to have it such that it makes a new diary entry, you can do this:
            // _services.diaryService.AddEntry(edits.body + edits.notes);
            
        }

        private void DeleteEntry(int id)
        {
            _services.diaryService.DeleteEntry(id);
            LoadEntries();
        }

        private void RequestVD(DateTime date)
        {
            RequestViewDay?.Invoke(this, date);
        }

        private void flpBodyAdjust()
        {
            flpMain.SuspendLayout();
            try
            {
                int w = flpMain.ClientSize.Width - (flpMain.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0) - 10;

                foreach (var card in flpMain.Controls.OfType<DiaryCard>())
                {
                    card.SuspendLayout();
                    card.LabelSize(w);
                    card.ResumeLayout(true);
                }
            }
            finally
            {
                flpMain.ResumeLayout(true); 
            }
        }

        private void flpMain_SizeChanged(object sender, EventArgs e)
        {
            //flpBodyAdjust();   
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
