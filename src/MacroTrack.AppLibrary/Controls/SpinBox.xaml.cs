using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    /// Interaction logic for SpinBox.xaml
    /// </summary>
    public partial class SpinBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public SpinBox()
        {
            InitializeComponent();
            Value = DefaultValue;
        }

        // Value:
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double?), typeof(SpinBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public double? Value
        {
            get => (double?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (SpinBox)d;
            c.RefreshDisplayString();
        }

        // Display String:

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

        // Default Value:
        public static DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue), typeof(double?), typeof(SpinBox),
            new PropertyMetadata(1.00));
        public double? DefaultValue
        {
            get => (double?)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        // Step (big step):
        public static DependencyProperty StepProperty = DependencyProperty.Register(
            nameof(Step), typeof(double?), typeof(SpinBox),
            new PropertyMetadata(1.00));
        public double? Step
        {
            get => (double?)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        // Small Step:
        public static DependencyProperty SmallStepProperty = DependencyProperty.Register(
            nameof(SmallStep), typeof(double?), typeof(SpinBox),
            new PropertyMetadata(0.10));
        public double? SmallStep
        {
            get => (double?)GetValue(SmallStepProperty);
            set => SetValue(SmallStepProperty, value);
        }

        // Format string:
        public static DependencyProperty FormatStringProperty = DependencyProperty.Register(
            nameof(FormatString), typeof(string), typeof(SpinBox),
            new PropertyMetadata("0.00"));
        public string FormatString
        {
            get => (string)GetValue(FormatStringProperty);
            set => SetValue(FormatStringProperty, value);
        }

        // Formatting values:

        // Outer Border Thickness:
        public static DependencyProperty OuterBorderThicknessProperty = DependencyProperty.Register(
            nameof(OuterBorderThickness), typeof(Thickness), typeof(SpinBox), 
            new PropertyMetadata(new Thickness(1)));
        public Thickness OuterBorderThickness
        {
            get => (Thickness)GetValue(OuterBorderThicknessProperty);
            set => SetValue(OuterBorderThicknessProperty, value);
        }

        // Outer Border Brush:
        public static DependencyProperty OuterBorderBrushProperty = DependencyProperty.Register(
            nameof(OuterBorderBrush), typeof(Brush), typeof(SpinBox), 
            new PropertyMetadata(Brushes.Transparent));
        public Brush OuterBorderBrush
        {
            get => (Brush)GetValue(OuterBorderBrushProperty);
            set => SetValue(OuterBorderBrushProperty, value);
        }

        // Hover Outer Border Brush:
        public static DependencyProperty HoverOuterBorderBrushProperty = DependencyProperty.Register(
            nameof(HoverOuterBorderBrush), typeof(Brush), typeof(SpinBox),
            new PropertyMetadata(Brushes.Transparent));
        public Brush HoverOuterBorderBrush
        {
            get => (Brush)GetValue(HoverOuterBorderBrushProperty);
            set => SetValue(HoverOuterBorderBrushProperty, value);
        }

        // Selected Outer Border Brush:
        public static DependencyProperty SelectedOuterBorderBrushProperty = DependencyProperty.Register(
            nameof(SelectedOuterBorderBrush), typeof(Brush), typeof(SpinBox),
            new PropertyMetadata(Brushes.Transparent));
        public Brush SelectedOuterBorderBrush
        {
            get => (Brush)GetValue(SelectedOuterBorderBrushProperty);
            set => SetValue(SelectedOuterBorderBrushProperty, value);
        }

        // Button Width:
        public static DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
            nameof(ButtonWidth), typeof(int), typeof(SpinBox),
            new PropertyMetadata(20));
        public int ButtonWidth
        {
            get => (int)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }


        // Function:

        private void RefreshDisplayString()
        {
            DisplayString = Value?.ToString(FormatString) ?? "";
        }

        private void Commit()
        {
            string? text = DisplayString?.Trim();
            if (double.TryParse(text, out double v))
            {
                Value = Math.Round(v, 2);
            }
            RefreshDisplayString();
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e) => Value += Step;
        private void ButtonDown_Click(object sender, RoutedEventArgs e) => Value -= Step;

        private void tbValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) Value += Step;
            if (e.Key == Key.Down) Value -= Step;
            if (e.Key == Key.Enter || e.Key == Key.Escape || e.Key == Key.Tab) Commit();
        }

        private void tbValue_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Delta > 0) Value += 0.01;
                if (e.Delta < 0) Value -= 0.01;
            }
            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                if (e.Delta > 0) Value += 0.05;
                if (e.Delta < 0) Value -= 0.05;
            }
            else
            {
                if (e.Delta > 0) Value += SmallStep;
                if (e.Delta < 0) Value -= SmallStep;
            }

            e.Handled = true;
        }

        private void tbValue_LostFocus(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        // Old code: can get rid of this if you like in a bit:
        /*
        private bool _updating = false;
        public SpinBox()
        {
            InitializeComponent();
            tbValue.TextChanged += TbValue_TextChanged;
            tbValue.PreviewKeyDown += TbValue_PreviewKeyDown;
            tbValue.PreviewMouseWheel += TbValue_PreviewMouseWheel;
            tbValue.LostFocus += TbValue_LostFocus;
        }

        

        // Operational values:
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), 
                typeof(double?), 
                typeof(SpinBox), 
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged
                )
            );
        public double? Value
        {
            get => (double?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.Register(
                nameof(DefaultValue),
                typeof(double?),
                typeof(SpinBox),
                new PropertyMetadata()
            );
        public double? DefaultValue
        {
            get => (double?)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                nameof(Step),
                typeof(double),
                typeof(SpinBox),
                new PropertyMetadata()
            );
        public double Step
        {
            get => (double)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty SmallStepProperty =
            DependencyProperty.Register(
                nameof(SmallStep),
                typeof(double),
                typeof(SpinBox),
                new PropertyMetadata(0.1)
            );
        public double SmallStep
        {
            get => (double)GetValue(SmallStepProperty);
            set => SetValue(SmallStepProperty, value);
        }

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            nameof(FormatString), typeof(string), typeof(SpinBox),
            new PropertyMetadata("0.00")            
        );
        public string FormatString
        {
            get => (string)GetValue(FormatStringProperty);
            set => SetValue(FormatStringProperty, value);
        }

        // Formatting values:
        public static readonly DependencyProperty OuterBorderThicknessProperty =
            DependencyProperty.Register(
                nameof(OuterBorderThickness),
                typeof(Thickness),
                typeof(SpinBox),
                new PropertyMetadata(new Thickness(1))
            );
        public Thickness OuterBorderThickness
        {
            get => (Thickness)GetValue(OuterBorderThicknessProperty);
            set => SetValue(OuterBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty OuterBorderBrushProperty =
            DependencyProperty.Register(
                nameof(OuterBorderBrush),
                typeof(Brush),
                typeof(SpinBox),
                new PropertyMetadata(Brushes.Transparent)
            );
        public Brush OuterBorderBrush
        {
            get => (SolidColorBrush)GetValue(OuterBorderBrushProperty);
            set => SetValue(OuterBorderBrushProperty, value);
        }

        public static readonly DependencyProperty HoverOuterBorderBrushProperty =
            DependencyProperty.Register(
                nameof(HoverOuterBorderBrush),
                typeof(Brush),
                typeof(SpinBox),
                new PropertyMetadata(Brushes.Transparent)
            );
        public Brush HoverOuterBorderBrush
        {
            get => (SolidColorBrush)GetValue(HoverOuterBorderBrushProperty);
            set => SetValue(HoverOuterBorderBrushProperty, value);
        }

        public static readonly DependencyProperty SelectedOuterBorderBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedOuterBorderBrush),
                typeof(Brush),
                typeof(SpinBox),
                new PropertyMetadata(Brushes.Transparent)
            );
        public Brush SelectedOuterBorderBrush
        {
            get => (SolidColorBrush)GetValue(SelectedOuterBorderBrushProperty);
            set => SetValue(SelectedOuterBorderBrushProperty, value);
        }

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register(
                nameof(ButtonWidth),
                typeof(int),
                typeof(SpinBox),
                new PropertyMetadata(20)
            );
        public int ButtonWidth
        {
            get => (int)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        } 


        private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var sb = (SpinBox)d;
            if (sb._updating) return;
            sb._updating = true;
            if (e.NewValue is double newVal) sb.tbValue.Text = newVal.ToString(sb.FormatString);
            else sb.tbValue.Text = "";
            sb._updating = false;
            if (!sb.tbValue.IsFocused) sb.OnExitFormat();
        }

        private void TbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating) return;
            if (double.TryParse(tbValue.Text, out var value)) Value = value;
            else if (string.IsNullOrWhiteSpace(tbValue.Text)) Value = null;
        }

        private void OnExitFormat()
        {
            if (_updating) return;
            if (!double.TryParse(tbValue.Text, out var value)) return;
            tbValue.Text = value.ToString(FormatString);
            tbValue.CaretIndex = tbValue.Text.Length;
            
        }

        private int CaretPositionBefore(string s, int caretPos)
        {
            int count = 0;
            for (int i=0; i < Math.Min(s.Length, caretPos); i++)
            {
                if (char.IsDigit(s[i]) || char.Equals(s[i], '.')) count++;
            }
            return count;
        }

        private int CaretPositionAfter(string s, int caretPos)
        {
            if (s.Length <= 0) return 0;
            if (s.Length < caretPos) return s.Length;
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]) || char.Equals(s[i], '.'))
                {
                    count++;
                    if (count == caretPos) return i + 1; 
                }
            }
            return s.Length;
        }

        private void ChangeValue(double delta)
        {
            var v = Value ?? 0;
            Value = Math.Round(v + delta, 2);
        }

        private void ChangeValueMaintainCaret(double delta)
        {
            // Method to get number length, converts double n into a formatted string, if we want to change formate we need to change this.
            int GetNumberLength(double n)
            {
                string s = n.ToString(FormatString);
                int count = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (Char.IsDigit(s[i])) count++;
                }
                return count;
            }

            // Get pre-change caret position and numberonly length
            int ci = CaretPositionBefore(tbValue.Text, tbValue.CaretIndex);
            int startNL = GetNumberLength(Math.Round(Value ?? 0, 2 ));

            // Change value
            ChangeValue(delta); // I feel like we should call this instead of changing it manually here, idk.

            // Get end numberlength and compare.
            int endNL = GetNumberLength(Value ?? 0); // We don't need to round it again.
            int diff = endNL - startNL;

            // Format and put caret back in place with adjustment for diff:
            OnExitFormat();
            tbValue.CaretIndex = CaretPositionAfter(tbValue.Text, ci + diff);
        }

        private void SetToDefault()
        {
            if (DefaultValue != null) Value = DefaultValue;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(Step);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue(-Step);
        }
        private void TbValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                ChangeValueMaintainCaret(Step);
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                ChangeValueMaintainCaret(-Step);
                e.Handled = true;
            }
            if (e.Key == Key.Enter || e.Key == Key.Escape || e.Key == Key.Tab)
            {
                OnExitFormat();
            }
        }
        private void TbValue_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Delta > 0) ChangeValueMaintainCaret(0.01);
                if (e.Delta < 0) ChangeValueMaintainCaret(-0.01);
            }
            else
            {
                if (e.Delta > 0) ChangeValueMaintainCaret(SmallStep);
                if (e.Delta < 0) ChangeValueMaintainCaret(-SmallStep);
            }
            
            e.Handled = true;
        }

        private void TbValue_LostFocus(object sender, RoutedEventArgs e)
        {
            OnExitFormat();
        }
        */
    }
}
