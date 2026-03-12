using DocumentFormat.OpenXml.Office2013.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for DateTimePicker2.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public DateTimePicker()
        {
            InitializeComponent();
            Value = DateTime.Now;
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(DateTime?), typeof(DateTimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public DateTime? Value
        {
            get => (DateTime?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DateTimePicker)d;
            c.RefreshDisplayString();
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            nameof(Format), typeof(string), typeof(DateTimePicker),
            new PropertyMetadata("d MMMM yyyy hh:mm tt", OnFormatChanged));
        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }
        private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (DateTimePicker)d;
            c.RefreshDisplayString();
        }

        private string _displayString = "";
        public string DisplayString
        {
            get => _displayString;
            set
            {
                if (_displayString == value) return;
                _displayString = value;
                OnPropertyChanged();
            }
        }        
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }        

        private void RefreshDisplayString()
        {
            //if (Value is not null) DisplayString = Value.Value.ToString(Format);
            DisplayString = Value?.ToString(Format) ?? "";
        }

        private void Commit()
        {
            string? text = DisplayString?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                Value = null;
                return;
            }

            string t = text.ToLowerInvariant();

            if (DateTime.TryParse(DisplayString, out DateTime dt)) Value = dt;
            else Value = DisplayString switch
            {
                "t"         => DateTime.Today.AddDays(1),
                "tomorrow"  => DateTime.Today.AddDays(1),
                "tommorrow" => DateTime.Today.AddDays(1),
                "y"         => DateTime.Today.AddMinutes(-1),
                "yesterday" => DateTime.Today.AddMinutes(-1),
                "yesterdat" => DateTime.Today.AddMinutes(-1),
                "now"       => DateTime.Now,
                _ => null
            };
            RefreshDisplayString();
            return;
        }

        private void PART_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                Commit();
                e.Handled = true;
            }
            if (e.Key == Key.Tab) Commit(); // We don't say handled = true;
        }

        private void PART_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void PART_btCalendar_Click(object sender, RoutedEventArgs e)
        {
            PART_Popup.IsOpen = !PART_Popup.IsOpen;
        }

        private void PART_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PART_Calendar.SelectedDate is DateTime date)
            {
                int hour = Value?.Hour ?? 0;
                int minute = Value?.Minute ?? 0;
                Value = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
                PART_Popup.IsOpen = false;
            }
        }

        private void PART_tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) Value = Value?.AddHours(1);
            if (e.Key == Key.Down) Value = Value?.AddHours(-1);
        }

        private void PART_tb_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) 
            {
                if (e.Delta > 0) Value = Value?.AddMinutes(1);
                if (e.Delta < 0) Value = Value?.AddMinutes(-1);
            }
            else
            {
                if (e.Delta > 0) Value = Value?.AddMinutes(10);
                if (e.Delta < 0) Value = Value?.AddMinutes(-10);
            }
            
            e.Handled = true;
        }

    }
}
