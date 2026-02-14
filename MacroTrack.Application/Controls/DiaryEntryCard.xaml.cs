using MacroTrack.Core.Models;
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
    /// Interaction logic for DiaryEntryCard.xaml
    /// </summary>
    public partial class DiaryEntryCard : UserControl
    {
        // DPs:
        public static readonly DependencyProperty EntryProperty = DependencyProperty.Register(
            nameof(Entry),
            typeof(DiaryEntry),
            typeof(DiaryEntryCard), 
            new PropertyMetadata(null)
        );
        public DiaryEntry? Entry
        {
            get => (DiaryEntry?)GetValue(EntryProperty);
            set => SetValue(EntryProperty, value);
        }

        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
            nameof(TimeFormat),
            typeof(string),
            typeof(DiaryEntryCard),
            new PropertyMetadata("yyyy/M/d - HH:mm")
        );
        public string TimeFormat
        {
            get => (string)GetValue(TimeFormatProperty);
            set => SetValue(TimeFormatProperty, value);
        }

        // Functions:
        public DiaryEntryCard()
        {
            InitializeComponent();
        }
    }
}
