using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Lightweight
{
    /// <summary>
    /// Lightweight implementation of the <see cref="Controls.FlatButton">FlatButton</see>
    /// </summary>
    public class LCStandardButton : LCFlatButton
    {
        /// <summary>
        /// Causes the control to redraw
        /// </summary>
        /// <remarks>
        /// This sub will completely refresh the bitmap of the control, then raise the <see cref="Update">Update</see> event.
        /// It should be overridden in child classes.
        /// </remarks>
        public override void Redraw()
        {
            if (holdDraw) return;
            if (bounds.Height <= 0) return;
            if (bounds.Width <= 0) return;

            myBitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(myBitmap);
            SolidBrush myBrush = GetBrush();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Transparent);
            //Draw basic shape
            RectangleF textRect = default(RectangleF);
            if (buttonStyle == LcarsButtonStyles.Pill)
            {
                g.FillEllipse(myBrush, new Rectangle(0, 0, bounds.Height, bounds.Height));
                g.FillEllipse(myBrush, new Rectangle(bounds.Width - bounds.Height, 0, bounds.Height, bounds.Height));
                textRect = new RectangleF(Convert.ToSingle(bounds.Height / 2), 0, bounds.Width - bounds.Height, bounds.Height);
                g.FillRectangle(myBrush, textRect);
            }
            else
            {
                float diameter = Height / 4;
                g.FillEllipse(myBrush, new Rectangle(0, 0,(int) diameter, (int) diameter));
                g.FillEllipse(myBrush, new RectangleF(0, Height - diameter, diameter, diameter));
                g.FillEllipse(myBrush, new RectangleF(Width - diameter, 0, diameter, diameter));
                g.FillEllipse(myBrush, new RectangleF(Width - diameter, Height - diameter, diameter, diameter));
                textRect = new RectangleF(diameter / 2, 0, Width - diameter, Height);
                g.FillRectangle(myBrush, textRect);
                g.FillRectangle(myBrush, new RectangleF(0, diameter / 2, Width, Height - diameter));
                if (buttonStyle == LcarsButtonStyles.RoundedSquareSlant ||
                    buttonStyle == LcarsButtonStyles.RoundedSquareBackSlant)
                {
                    Bitmap slant = new Bitmap(myBitmap);
                    Point[] mypoints = new Point[3];
                    if (buttonStyle == LcarsButtonStyles.RoundedSquareSlant)
                    {
                        mypoints[0] = new Point(Width / 4, 0);
                        mypoints[1] = new Point(Width, 0);
                        mypoints[2] = new Point(0, Height);
                    }
                    else
                    {
                        mypoints[0] = new Point(0, 0);
                        mypoints[1] = new Point(Width - (Width / 4), 0);
                        mypoints[2] = new Point(Width / 4, Height);
                    }
                    GraphicsUnit gu = GraphicsUnit.Pixel;
                    g.FillRectangle(Brushes.Black, myBitmap.GetBounds(ref gu));
                    g.DrawImage(slant, mypoints);
                    textRect = new RectangleF(Width / 4, 0, Width / 2, Height);
                }

            }
            //Draw text
            DrawText(textRect, g);
            ChangeLit(g);
            g.Dispose();
            DoEvent(LightweightEvents.Update);
        }

        /// <summary>
        /// Visual type of the button
        /// </summary>
        /// <value>New visual type</value>
        /// <returns>Current visual type</returns>
        public LcarsButtonStyles ButtonStyle
        {
            get { return buttonStyle; }
            set
            {
                if (buttonStyle == value) return;
                buttonStyle = value;
                Redraw();
            }
        }
        LcarsButtonStyles buttonStyle = LcarsButtonStyles.Pill;

        #region " Conversion "
        public static implicit operator StandardButton(LCStandardButton o)
        {
            StandardButton newButton = new StandardButton();
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
            newButton.ButtonStyle = o.ButtonStyle;
            return newButton;
        }
        public static explicit operator LCStandardButton(StandardButton o)
        {
            LCStandardButton newButton = new LCStandardButton();
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
            newButton.ButtonStyle = o.ButtonStyle;
            return newButton;
        }
        #endregion
    }
}
