using MacroTrack.Core.Models;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for WeightEntryCard.xaml
    /// </summary>
    public partial class WeightEntryCard : UserControl
    {
        public static readonly DependencyProperty EntryProperty = DependencyProperty.Register(
            nameof(Entry), typeof(WeightEntry), typeof(WeightEntryCard),
            new PropertyMetadata(null)
        );
        public WeightEntry Entry
        {
            get => (WeightEntry)GetValue(EntryProperty);
            set => SetValue(EntryProperty, value);
        }

        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
            nameof(TimeFormat), typeof(string), typeof(WeightEntryCard),
            new PropertyMetadata("yyyy/M/d - HH:mm")
        );
        public string TimeFormat
        {
            get => (string)GetValue(TimeFormatProperty);
            set => SetValue(TimeFormatProperty, value);
        }

        public static readonly DependencyProperty UnitLabelProperty = DependencyProperty.Register(
            nameof(UnitLabel), typeof(string), typeof(WeightEntryCard),
            new PropertyMetadata("")
        );
        public string UnitLabel
        {
            get => (string)GetValue(UnitLabelProperty);
            set => SetValue(UnitLabelProperty, value);
        }

        public bool IsSelected;

        public WeightEntryCard()
        {
            InitializeComponent();
        }
    }
}
