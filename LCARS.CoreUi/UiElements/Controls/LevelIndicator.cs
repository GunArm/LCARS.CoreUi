using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System;
using System.ComponentModel;
using System.Drawing;

namespace LCARS.CoreUi.UiElements.Controls
{
    public class LevelIndicator : LcarsButtonBase
    {
        [DefaultValue(100)]
        public LcarsColorFunction ColorFunction2
        {
            get { return colorFunction2; }
            set
            {
                colorFunction2 = value;
                DrawAllButtons();
            }
        }
        LcarsColorFunction colorFunction2;

        [DefaultValue(100)]
        public int Max
        {
            get { return intMax; }
            set
            {
                if (value > intMin)
                {
                    intMax = value;
                    DrawAllButtons();
                }
                else
                {
                    throw new Exception("The Max value MUST be greater than the Min value.");
                }
            }
        }
        int intMax = 100;

        [DefaultValue(0)]
        public int Min
        {
            get { return intMin; }
            set
            {
                if (value < intMax)
                {
                    intMin = value;
                    DrawAllButtons();
                }
                else
                {
                    throw new Exception("The Min value MUST be lesser than the Max value.");
                }
            }
        }
        int intMin;

        [DefaultValue(0)]
        public int Value
        {
            get { return intVal; }
            set
            {
                if (value >= intMin & value <= Max)
                {
                    intVal = value;
                    if (intMax - intMin > 0)
                    {
                        ButtonText = ((intVal / (intMax - intMin)) * 100) + "%";
                    }
                    else
                    {
                        ButtonText = "0%";
                    }

                    DrawAllButtons();
                }
                else
                {
                    throw new Exception("The Value MUST be between Min and Max.");
                }
            }
        }
        int intVal;

        public LevelIndicator()
        {
            Clickable = false;
            ButtonTextAlign = ContentAlignment.MiddleCenter;
        }

        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(mybitmap);
            SolidBrush myBrush = new SolidBrush(GetButtonColor());
            SolidBrush myBrush2 = new SolidBrush(ColorManager.GetColor(colorFunction2));
            int valHeight = 0;

            if (intVal > 0)
            {
                valHeight = ((Height - 20) * intVal) / intMax;
            }
            else
            {
                valHeight = 0;
            }

            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width, Height));

            g.FillRectangle(myBrush, new Rectangle(20, 10, Width - 40, Height - 20));

            g.FillRectangle(myBrush2, new Rectangle(20, ((Height - 10) - valHeight), Width - 40, valHeight));

            int myStep = (Height - 22) / 20;
            if (myStep < 1)
            {
                myStep = 1;
            }
            for (int intloop = 10; intloop <= Height - 12; intloop += myStep)
            {
                if (intloop % 5 == 0)
                {
                    g.FillRectangle(myBrush, new Rectangle(5, (Height - intloop) - 2, 15, 2));
                    g.FillRectangle(myBrush, new Rectangle(Width - 20, (Height - intloop) - 2, 15, 2));

                }
                else
                {
                    g.FillRectangle(Brushes.Red, new Rectangle(10, (Height - intloop) - 2, 10, 2));
                    g.FillRectangle(Brushes.Red, new Rectangle(Width - 20, (Height - intloop) - 2, 10, 2));

                }

            }
            TextSize = Size;
            g.Dispose();

            return mybitmap;
        }
    }
}
