using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Lightweight
{
    public class LCArrowButton : LCFlatButton
    {
        private LcarsArrowDirection arrowDirection = LcarsArrowDirection.Up;
        public override void Redraw()
        {
            if (HoldDraw) return;
            if (Bounds.Width <= 0) return;
            if (Bounds.Height <= 0) return;

            myBitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(myBitmap);
            SolidBrush myBrush = GetBrush();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Black);
            g.FillRectangle(myBrush, 0, 0, bounds.Width, bounds.Height);
            //Draw arrow
            Point[] myPoints = new Point[3];
            switch (arrowDirection)
            {
                case LcarsArrowDirection.Up:
                    myPoints[0] = new Point(Width / 2, Height / 5);
                    myPoints[1] = new Point(Width / 5, Height - (Height / 5));
                    myPoints[2] = new Point(Width - (Width / 5), Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Down:
                    myPoints[0] = new Point(Width / 5, Height / 5);
                    myPoints[1] = new Point(Width - (Width / 5), Height / 5);
                    myPoints[2] = new Point(Width / 2, Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Left:
                    myPoints[0] = new Point(Width / 5, Height / 2);
                    myPoints[1] = new Point(Width - (Width / 5), Height / 5);
                    myPoints[2] = new Point(Width - (Width / 5), Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Right:
                    myPoints[0] = new Point(Width - (Width / 5), Height / 2);
                    myPoints[1] = new Point(Width / 5, Height / 5);
                    myPoints[2] = new Point(Width / 5, Height - (Height / 5));
                    break;
            }
            g.FillPolygon(Brushes.Black, myPoints);

            ChangeLit(g);
            DoEvent(LightweightEvents.Update);
        }

        public LcarsArrowDirection ArrowDirection
        {
            get { return arrowDirection; }
            set
            {
                arrowDirection = value;
                Redraw();
            }
        }

        #region " Conversion "
        public static implicit operator ArrowButton(LCArrowButton o)
        {
            ArrowButton newButton = new ArrowButton();
            newButton.Text = o.Text;
            newButton.Bounds = o.Bounds;
            newButton.Data = o.Data;
            newButton.Data2 = o.Data2;
            newButton.ColorManager = o.ColorManager;
            newButton.ColorFunction = o.ColorFunction;
            newButton.AutoEllipsis = o.AutoEllipsis;
            newButton.ButtonTextAlign = o.TextAlign;
            newButton.Clickable = o.Clickable;
            newButton.Flash = o.Flashing;
            newButton.FlashInterval = Convert.ToInt32(o.FlashInterval);
            newButton.Font = o.Font;
            newButton.HoldDraw = o.HoldDraw;
            newButton.CustomAlertColor = o.CustomAlertColor;
            newButton.AlertState = o.AlertState;
            newButton.ForceCaps = o.ForceCaps;
            newButton.ArrowDirection = o.ArrowDirection;
            return newButton;
        }
        public static explicit operator LCArrowButton(ArrowButton o)
        {
            LCArrowButton newButton = new LCArrowButton();
            newButton.Text = o.Text;
            newButton.Bounds = o.Bounds;
            newButton.Data = o.Data;
            newButton.Data2 = o.Data2;
            newButton.ColorManager = o.ColorManager;
            newButton.ColorFunction = o.ColorFunction;
            newButton.AutoEllipsis = o.AutoEllipsis;
            newButton.TextAlign = o.ButtonTextAlign;
            newButton.Clickable = o.Clickable;
            newButton.Flashing = o.Flash;
            newButton.FlashInterval = Convert.ToInt32(o.FlashInterval);
            newButton.Font = o.Font;
            newButton.HoldDraw = o.HoldDraw;
            newButton.CustomAlertColor = o.CustomAlertColor;
            newButton.AlertState = o.AlertState;
            newButton.ForceCaps = o.ForceCaps;
            newButton.ArrowDirection = o.ArrowDirection;
            return newButton;
        }
        #endregion
    }
}
