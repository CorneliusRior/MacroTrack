using MacroTrack.AppLibrary.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Controls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePickerOld : UserControl
    {
        //private readonly string format = "d MMMM yyyy hh:mm tt";
        public DateTimePickerOld()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            nameof(Format),
            typeof(string), 
            typeof(DateTimePickerOld), 
            new PropertyMetadata(null)
        );
        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set
            {
                Debug.WriteLine($"Format={Format}");
                SetValue(FormatProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(DateTime?),
                typeof(DateTimePickerOld),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged
                )
        );
        public DateTime? Value
        {
            get => (DateTime?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DateTimePickerOld)d;
            c.PrintValue();
        }

        public void UpdateFormat()
        {
            PrintValue();
        }

        private void PrintValue()
        {
            if (Value is null)
            {
                return;
            }
            Debug.WriteLine($"Printing, format: {Format}");
            var time = Value.Value;
            PART_tb.Text = time.ToString(Format);
            PART_Calendar.SelectedDate = time.Date;
        }

        private void PART_buttonCalendar_Click(object sender, RoutedEventArgs e)
        {
            PART_Popup.IsOpen = !PART_Popup.IsOpen;
        }

        private void SetToCurrent()
        {
            Value = DateTime.Now;
        }

        private void PART_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyCalendarDate();
        }

        private void ApplyCalendarDate()
        {
            DateTime date = PART_Calendar.SelectedDate ?? Value?.Date ?? DateTime.Now;
            int hour = Value?.Hour ?? 0;
            int minute = Value?.Minute ?? 0;
            Value = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
        }

        private void Commit()
        {
            var text = PART_tb.Text?.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                Value = null;
                return;
            }
            if (!DateTime.TryParse(text, out DateTime dateTime))
            {
                string t = text.ToLowerInvariant();
                if (t == "yesterday" || t == "y" || t == "yesterdat")
                {
                    Value = DateTime.Now.Date.AddMinutes(-1);
                    return;
                }
                if (t == "tomorrow" || t == "t" || t == "tommorrow")
                {
                    Value = DateTime.Now.Date.AddDays(1);
                    return;
                }


                Value = null;
                return;
            }
            else
            {
                Value = dateTime;
            }
        }

        private void PART_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                Commit();
                e.Handled = true;
            }
            if (e.Key == Key.Tab)
            {
                Commit(); // We don't say handled = true;
            }
        }

        private void PART_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            Commit();
        }
    }
}
