using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.BasicApp.Graphs
{

    public class MacroBar : Control
    {
        [Category("Data")]
        [Description($"How much the bar will be filled, relative to \"Target\": this figure is divided by \"Target\" to determine bar length.")]
        public double Actual { get; set; }

        [Category("Data")]
        [Description($"How much constitues a full bar. \"Actual\" is divided by this number to determine bar length.")]
        public double Target { get; set; }

        [Category("Data")]
        [Description($"Threshold below which \"UnderColor\" is used, above which \"AtColor\" is used. Default is 0.9")]
        public double ThresholdLower { get; set; } = 0.9;

        [Category("Data")]
        [Description($"Threshold below which \"AtColor\" is used, above which \"OverColor\" is used. Default is 1.1")]
        public double ThresholeUpper { get; set; } = 1.1;

        [Category("Data")]
        [Description($"Color drawn when under lower threshold, default is yellow.")]
        public Color ColorUnder { get; set; } = Color.Yellow;

        [Category("Data")]
        [Description($"Color drawn when between upper and lower threshold, default is green.")]
        public Color ColorAt { get; set; } = Color.Green;

        [Category("Data")]
        [Description($"Color drawn when over upper threshold, default is red.")]
        public Color ColorOver { get; set; } = Color.Red;

        [Category("Data")]
        [Description($"Enables or disables the \"Overflow\" feature: Blocks to the right of the bar if over 100%.")]
        public bool OverFlow { get; set; } = true;
        
        [Category("Data")]
        [Description($"Ratio of bar width allocated to overflow capacity.")] 
        public double OverFlowRatio { get; set; } = 0.5;

        [Category("Data")]
        [Description($"Gap between the main bar and the overflow section.")]
        public int OverFlowGap { get; set; } = 5;

        [Category("Data")]
        [Description($"Gap between overflow blocks: you can set as 0 for a continuous line.")]
        public int OverFlowBlockGap { get; set; } = 5;

        [Category("Data")]
        [Description($"Width of each overflow block.")]
        public int OverFlowBlockWidth { get; set; } = 10;



        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize { get => base.AutoSize; set => base.AutoSize = value; }


        public MacroBar()
        {
            AutoSize = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            // Set up rectangles
            var full = new Rectangle(0,0, Width -1, Height -1);

            // Set width of main (these will just be 0 if !Overflow)
            int MainW = full.Width - (int)Math.Round((full.Width / (1 + OverFlowRatio)) * OverFlowRatio);
            int OverW = full.Width - MainW - OverFlowGap;
            if (MainW < 1) MainW = 1;

            // Declare the two (if !Overflow, we just don't add the second rectangle)
            var MainRect = new Rectangle(full.X, full.Y, MainW, full.Height);
            var OverRect = new Rectangle(MainRect.Right + OverFlowGap, full.Y, OverW, full.Height);

            // MainRect logic
            double ratio = (Target <= 0) ? 0 : Actual / Target;
            double barRatio = Math.Min(1, ratio);
            int fillW = (int)(barRatio * MainRect.Width);

            // Color formatting (brushes also used in OverRect)
            g.FillRectangle(SystemBrushes.ControlLight, MainRect);
            Brush BrushUnder = new SolidBrush(ColorUnder);
            Brush BrushOver = new SolidBrush(ColorOver);
            Brush BrushAt = new SolidBrush(ColorAt);
            Brush b = ratio > ThresholeUpper ? BrushOver : ratio >= ThresholdLower ? BrushAt : BrushUnder;

            // Define bars
            if (fillW > 0)
            {
                var fillRect = new Rectangle(MainRect.X, MainRect.Y, fillW, MainRect.Height);
                g.FillRectangle(b, fillRect);
                g.DrawRectangle(Pens.Gray, MainRect);
            }
            
            // Overflow:
            if (ratio > 1 && OverFlow && OverRect.Width > 0)
            {
                // Set up overflow area:
                double overFlow = ratio - 1;
                int overFlowFillPx = (int)Math.Round(Math.Min(overFlow * MainRect.Width, OverW));

                // Set up varibales for drawing logic
                int x = OverRect.X; // X coordinate, starts just at OverFlowGap (from var OverRect = ...) and proceed right
                int remaining = overFlowFillPx; // Width of the overflow bar, when we take away from it: width of the overflow bar remaining.

                // Draw overflow blocks:
                while (remaining > 0 && x < OverRect.Right)
                {
                    int w = Math.Min(OverFlowBlockWidth, remaining); // Allows half a block to be drawn, or fractions.
                    if (x + w > OverRect.Right) w = OverRect.Right - x; // Prevents overflowing out of bounds.

                    // Build each
                    var overBlock = new Rectangle(x, OverRect.Y, w, OverRect.Height);
                    g.FillRectangle(b, overBlock);
                    g.DrawRectangle(Pens.Gray, overBlock);

                    // Update the variables
                    remaining -= OverFlowBlockWidth + OverFlowBlockGap;
                    x += OverFlowBlockWidth + OverFlowBlockGap;
                }                               
            }
        } 

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(proposedSize.Width, 10);
        }
    }
}
