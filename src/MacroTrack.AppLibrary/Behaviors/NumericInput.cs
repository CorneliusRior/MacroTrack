using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Behaviors
{
    public static class NumericInput
    {
        private static readonly Regex _regex = new(@"^-?\d*(\.\d+)?$");

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(NumericInput),
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
            p("OnChanged() called.");
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
            int caretIndex = tb.CaretIndex;
            int textLength = tb.GetLineLength(0);
            p($"Tb_PreviewKeyDown() called. e.Key = '{e.Key}', caratIndex = '{caretIndex}', textLength = '{textLength}'");
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                if (double.TryParse(tb.Text, out double value)) tb.Text = (value *= -1).ToString();
                tb.CaretIndex = caretIndex + (tb.GetLineLength(0) - textLength);
                e.Handled = true;
            }
            if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                if (double.TryParse(tb.Text, out double value)) tb.Text = Math.Abs(value).ToString(); e.Handled = true;
                tb.CaretIndex = caretIndex + (tb.GetLineLength(0) - textLength);
                e.Handled = true;
            }
        }

        private static void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {            
            var tb = (TextBox)sender;
            var propsed = tb.Text.Insert(tb.SelectionStart, e.Text + "0");
            e.Handled = !_regex.IsMatch(propsed);
            p($"Tb_PreviewTextInput() called. Proposed = '{tb.Text.Insert(tb.SelectionStart, e.Text + "0")}, e.Handled = {e.Handled}'");
        }

        private static void Tb_Paste(object sender, DataObjectPastingEventArgs e)
        {
            p("Tb_Paste() called.");
            if (!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText)) { e.CancelCommand(); return; }
            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string ?? "";
            if (!_regex.IsMatch(text)) e.CancelCommand();
        }

        /// <summary>
        /// Better debugging printer. Ignore all parameters except "Message", the rest fill in automatically.
        /// </summary>
        /// <param name="message"></param>
        public static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }
    }
}
