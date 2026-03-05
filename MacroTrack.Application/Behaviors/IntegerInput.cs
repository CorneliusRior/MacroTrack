using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Behaviors
{
    public static class IntegerInput
    {
        private static readonly Regex _regex = new(@"^-?\d*?$");

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(IntegerInput),
            new PropertyMetadata(false, OnChanged)
        );

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;

            if ((bool)e.NewValue)
            {
                tb.PreviewTextInput += Tb_PreviewTextInput;
                tb.PreviewKeyDown += Tb_PreviewKeyDown;
                DataObject.AddPastingHandler(tb, Tb_Paste);
            }
            else
            {
                tb.PreviewTextInput -= Tb_PreviewTextInput;
                tb.PreviewKeyDown -= Tb_PreviewKeyDown;
                DataObject.RemovePastingHandler(tb, Tb_Paste);
            }
        }

        private static void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = (TextBox)sender;
            int caratIndex = tb.CaretIndex;
            int textLength = tb.GetLineLength(0);
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                if (int.TryParse(tb.Text, out int value)) tb.Text = (value *= -1).ToString();
                tb.CaretIndex = caratIndex + (tb.GetLineLength(0) - textLength);
                e.Handled = true;
            }
            if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                if (int.TryParse(tb.Text, out int value)) tb.Text = Math.Abs(value).ToString(); e.Handled = true;
                tb.CaretIndex = caratIndex + (tb.GetLineLength(0) - textLength);
                e.Handled = true;
            }
        }

        private static void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // This is a remnant of double formatting, but it doesn't break anything, this should be fine.
            var tb = (TextBox)sender;
            var propsed = tb.Text.Insert(tb.SelectionStart, e.Text + "0");
            e.Handled = !_regex.IsMatch(propsed);
        }

        private static void Tb_Paste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText)) { e.CancelCommand(); return; }
            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string ?? "";
            if (!_regex.IsMatch(text)) e.CancelCommand();
        }
    }
}
