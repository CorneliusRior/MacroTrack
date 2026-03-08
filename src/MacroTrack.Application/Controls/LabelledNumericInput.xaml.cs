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
    /// Interaction logic for LabelledNumericInput.xaml
    /// </summary>
    public partial class LabelledNumericInput : UserControl
    {
        public LabelledNumericInput()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                nameof(LabelText),
                typeof(string),
                typeof(LabelledNumericInput),
                new PropertyMetadata("Label")
            );
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty UnitLabelTextProperty =
            DependencyProperty.Register(
                nameof(UnitLabelText),
                typeof(string),
                typeof(LabelledNumericInput),
                new PropertyMetadata("")
            );
        public string UnitLabelText
        {
            get => (string)GetValue(UnitLabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty InputWidthProperty =
            DependencyProperty.Register(
                nameof(InputWidth),
                typeof(int),
                typeof(LabelledNumericInput),
                new PropertyMetadata(30)                
            );
        public int InputWidth
        {
            get => (int)GetValue(InputWidthProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(int),
                typeof(LabelledNumericInput),
                new FrameworkPropertyMetadata(0, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set
            {
                if (Value == value) return;
                if (Max != null && Value > Max) { SetValue(ValueProperty, Max); return; }
                if (Min != null && Value < Min) { SetValue(ValueProperty, Min); return; }
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(
                nameof(Min),
                typeof(int?),
                typeof(LabelledNumericInput),
                new PropertyMetadata(null)
            );
        public int? Min
        {
            get => (int)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(
                nameof(Max),
                typeof(int?),
                typeof(LabelledNumericInput),
                new PropertyMetadata(null)
            );
        public int? Max
        {
            get => (int)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }
    }
}
