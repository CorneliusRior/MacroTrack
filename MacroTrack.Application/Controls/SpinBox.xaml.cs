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
    /// Interaction logic for SpinBox.xaml
    /// </summary>
    public partial class SpinBox : UserControl
    {
        private bool _updating = false;
        public SpinBox()
        {
            InitializeComponent();
            tbValue.TextChanged += TbValue_TextChanged;
            tbValue.PreviewKeyDown += TbValue_PreviewKeyDown;
            tbValue.PreviewMouseWheel += TbValue_PreviewMouseWheel;
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
            get => (double)GetValue(DefaultProperty);
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
            sb._updating = true;
            if (e.NewValue is null) sb.tbValue.Text = string.Empty;
            else sb.tbValue.Text = ((double)e.NewValue).ToString("0.00");
            sb._updating = false;
        }

        private void TbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating) return;
            if (double.TryParse(tbValue.Text, out var value)) Value = value;
            else if (string.IsNullOrWhiteSpace(tbValue.Text)) Value = null;
        }

        private void ChangeValue(double delta)
        {
            var v = Value ?? 0;
            Value = Math.Round(v + delta, 2);
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
            int ci = tbValue.CaretIndex;
            int ll = tbValue.Text.Length;
            if (e.Key == Key.Up)
            {
                ChangeValue(Step);
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                ChangeValue(-Step);
                e.Handled = true;
            }
            tbValue.CaretIndex = ci + (tbValue.Text.Length - ll);
        }
        private void TbValue_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            int ci = tbValue.CaretIndex;
            int ll = tbValue.Text.Length;
            if (e.Delta > 0) ChangeValue(SmallStep);
            if (e.Delta < 0) ChangeValue(-SmallStep);
            tbValue.CaretIndex = ci + (tbValue.Text.Length - ll);
            e.Handled = true;
        }
    }
}
