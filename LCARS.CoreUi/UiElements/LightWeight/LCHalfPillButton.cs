using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Lightweight
{
    /// <summary>
    /// A lightweight implementation of the <see cref="HalfPillButton">HalfPillButton</see>
    /// </summary>
    public class LCHalfPillButton : LCStandardButton
    {
        /// <summary>
        /// Redraws the control
        /// </summary>
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
            g.Clear(Color.Black);
            //Draw basic shape
            RectangleF textArea = default(RectangleF);
            if (pillDirection == LcarsHalfPillButtonStyles.Left)
            {
                g.FillEllipse(myBrush, 0, 0, Height, Height);
                textArea = new RectangleF(Height / 2, 0, Width - Height / 2, Height);
            }
            else
            {
                g.FillEllipse(myBrush, new RectangleF(Width - Height, 0, Height, Height));
                textArea = new RectangleF(0, 0, Width - Height / 2, Height);
            }
            g.FillRectangle(myBrush, textArea);
            //Draw text
            DrawText(textArea, g);
            ChangeLit(g);
            g.Dispose();
            DoEvent(LightweightEvents.Update);
        }

        /// <summary>
        /// The side of the control to be rounded
        /// </summary>
        /// <remarks>
        /// Setting this property to its current value will not trigger a redraw
        /// </remarks>
        public LcarsHalfPillButtonStyles PillDirection
        {
            get { return pillDirection; }
            set
            {
                if (value == pillDirection) return;
                pillDirection = value;
                Redraw();
            }
        }
        private LcarsHalfPillButtonStyles pillDirection = LcarsHalfPillButtonStyles.Left;

        #region " Conversion "
        public static implicit operator HalfPillButton(LCHalfPillButton o)
        {
            HalfPillButton newButton = new HalfPillButton();
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
            newButton.ButtonStyle = o.PillDirection;
            return newButton;
        }

        public static explicit operator LCHalfPillButton(HalfPillButton o)
        {
            LCHalfPillButton newButton = new LCHalfPillButton();
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
            newButton.PillDirection = o.ButtonStyle;
            return newButton;
        }
        #endregion
    }
}
