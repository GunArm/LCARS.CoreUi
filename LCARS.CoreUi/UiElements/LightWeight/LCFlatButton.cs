using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using LCARS.CoreUi.Interfaces;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Lightweight
{
    /// <summary>
    /// Lightweight implementation of the <see cref="Controls.FlatButton">FlatButton</see>
    /// </summary>
    /// <remarks>
    /// Several other controls derive from this one by overriding <see cref="LCFlatButton.Redraw">Redraw</see> and adding properties.
    /// </remarks>
    public class LCFlatButton : ILightweightControl, IAlertable, IDisposable, ISounding, IColorable
    {

        /// <summary>
        /// The bitmap of the control's interface
        /// </summary>
        /// <remarks>This is used as a buffer to draw to, which is then passed to the
        /// parent control to be drawn to the screen.
        /// </remarks>
        protected Bitmap myBitmap = new Bitmap(1, 1);

        protected string currentTextScrollRotation;

        protected bool isPressed = false;
        protected bool flashOn = false;

        private System.Timers.Timer textScrollTimer = new System.Timers.Timer(100);
        private System.Timers.Timer flashTimer = new System.Timers.Timer(1000);

        protected RectangleF textArea;
        protected Control parent;

        public event EventHandler Update;

        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event EventHandler MouseLeave;
        public event EventHandler DoubleClick;

        /// <summary>
        /// Returns current bitmap for the button's interface
        /// </summary>
        /// <returns>Bitmap for the button's interface</returns>
        /// <remarks>
        /// Calling this function does not cause a redraw, unless the bitmap is not drawn yet. In such a case, the control will redraw
        /// itself, then return the bitmap.
        /// </remarks>
        public Bitmap GetBitmap()
        {
            if (myBitmap == null)
            {
                HoldDraw = false;
            }
            return myBitmap;
        }

        /// <summary>
        /// Causes the control to redraw
        /// </summary>
        /// <remarks>
        /// This sub will completely refresh the bitmap of the control, then raise the <see cref="Update">Update</see> event.
        /// It should be overridden in child classes.
        /// </remarks>
        public virtual void Redraw()
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
            g.FillRectangle(myBrush, new Rectangle(0, 0, bounds.Width, bounds.Height));
            //Draw text
            DrawText(new Rectangle(0, 0, this.Width, this.Height), g);
            ChangeLit(g);
            g.Dispose();
            Update?.Invoke(this, null);
        }
        /// <summary>
        /// Returns the standard color brush.
        /// </summary>
        /// <returns>Solid Brush according to standard color rules</returns>
        /// <remarks>This should be used with the Color property to save rewriting code.</remarks>
        protected SolidBrush GetBrush()
        {
            SolidBrush myBrush = default(SolidBrush);
            if (isPressed)
            {
                myBrush = new SolidBrush(Color.White);
            }
            else
            {
                if (alertState == LcarsAlert.Red)
                {
                    myBrush = new SolidBrush(Color.Red);
                }
                else if (alertState == LcarsAlert.Yellow)
                {
                    myBrush = new SolidBrush(Color.Yellow);
                }
                else if (alertState == LcarsAlert.White)
                {
                    myBrush = new SolidBrush(Color.White);
                }
                else if (alertState == LcarsAlert.Custom)
                {
                    myBrush = new SolidBrush(customAlertColor);
                }
                else
                {
                    myBrush = new SolidBrush(ColorManager.GetColor(colorFunction));
                }
            }
            return myBrush;
        }

        /// <summary>
        /// Darkens the control if not lit
        /// </summary>
        /// <param name="g">Graphics object being used to draw the control</param>
        /// <remarks>
        /// The entire control will be darkened, so do not use this if you need something else.
        /// </remarks>
        protected void ChangeLit(Graphics g)
        {
            if ((flashing && flashOn) || ((!Flashing) && (!isLit)))
            {
                SolidBrush darkBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
                g.FillRectangle(darkBrush, 0, 0, this.Width, this.Height);
            }
        }

        /// <summary>
        /// Draws the text of the control in specified area.
        /// </summary>
        /// <param name="area">Valid area to draw text in.</param>
        /// <param name="g">Graphics object being used to draw the control.</param>
        /// <remarks>This sub will handle all auto-ellipsis functionality and alignment.
        /// </remarks>
        protected void DrawText(RectangleF area, Graphics g)
        {
            textArea = area;
            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.NoWrap;
            if (textScrollTimer.Enabled | !autoEllipsis)
            {
                if (ellipsisMode == EllipsisModes.Character) format.Trimming = StringTrimming.Character;
                else format.Trimming = StringTrimming.Word;
            }
            else
            {
                if (ellipsisMode == EllipsisModes.Character) format.Trimming = StringTrimming.EllipsisCharacter;
                else format.Trimming = StringTrimming.EllipsisWord;
            }
            if (textScrollTimer.Enabled) format.Alignment = StringAlignment.Near;
            else
            {
                if (textAlign == ContentAlignment.BottomCenter | textAlign == ContentAlignment.MiddleCenter | textAlign == ContentAlignment.TopCenter)
                {
                    format.Alignment = StringAlignment.Center;
                }
                else if (textAlign == ContentAlignment.BottomRight | textAlign == ContentAlignment.MiddleRight | textAlign == ContentAlignment.TopRight)
                {
                    format.Alignment = StringAlignment.Far;
                }
                else if (textAlign == ContentAlignment.BottomLeft | textAlign == ContentAlignment.MiddleLeft | textAlign == ContentAlignment.TopLeft)
                {
                    format.Alignment = StringAlignment.Near;
                }
            }
            if (textAlign == ContentAlignment.TopCenter | textAlign == ContentAlignment.TopLeft | textAlign == ContentAlignment.TopRight)
            {
                format.LineAlignment = StringAlignment.Near;
            }
            else if (textAlign == ContentAlignment.MiddleCenter | textAlign == ContentAlignment.MiddleLeft | textAlign == ContentAlignment.MiddleRight)
            {
                format.LineAlignment = StringAlignment.Center;
            }
            else if (textAlign == ContentAlignment.BottomCenter | textAlign == ContentAlignment.BottomLeft | textAlign == ContentAlignment.BottomRight)
            {
                format.LineAlignment = StringAlignment.Far;
            }
            if (forceCaps) g.DrawString(currentTextScrollRotation.ToUpper(), font, Brushes.Black, area, format);
            else g.DrawString(currentTextScrollRotation, font, Brushes.Black, area, format);
        }
        /// <summary>
        /// Raises the click event of the control
        /// </summary>
        /// <remarks>If the control has detected the mouse as down, this sub will reset it to normal and redraw the control.</remarks>
        public void DoClick()
        {
            if (canClick)
            {
                Click?.Invoke(this, new EventArgs());
                if (!isPressed) return;
                isPressed = false;
                Redraw();
            }
        }

        //Handles text scrolling
        private void TextScrollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            currentTextScrollRotation = currentTextScrollRotation.Substring(1) + currentTextScrollRotation.Substring(0, 1);
            Redraw();
        }

        //Handles button flashing
        private void FlashTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            flashOn = !flashOn;
            Redraw();
        }

        /// <summary>
        /// Handles most events of the control
        /// </summary>
        /// <param name="eventName">Type of event to raise</param>
        /// <remarks></remarks>
        public void DoEvent(LightweightEvents eventName)
        {
            switch (eventName)
            {
                case LightweightEvents.Click:
                    DoClick();
                    break;
                case LightweightEvents.MouseDown:
                    if (canClick)
                    {
                        isPressed = true;
                        Redraw();
                    }
                    MouseDown?.Invoke(this, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

                    break;
                case LightweightEvents.MouseUp:
                    if (canClick)
                    {
                        isPressed = false;
                        Redraw();
                    }
                    MouseUp?.Invoke(this, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

                    break;
                case LightweightEvents.MouseEnter:
                    Scrolling = true;
                    MouseEnter?.Invoke(this, new EventArgs());

                    break;
                case LightweightEvents.MouseLeave:
                    Scrolling = false;
                    MouseLeave?.Invoke(this, new EventArgs());

                    break;
                case LightweightEvents.Update:
                    Update?.Invoke(this, null);

                    break;
                case LightweightEvents.DoubleClick:
                    DoubleClick?.Invoke(this, new EventArgs());

                    break;
                case LightweightEvents.MouseMove:
                    MouseMove?.Invoke(this, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

                    break;
                default:
                    throw new NotImplementedException("The requested event for this control does not exist");
            }
        }

        private void Beep(object sender, System.EventArgs e)
        {
            if (!doesBeep) return;
            LcarsSound.Play(soundAsset);
        }

        public void SetParent(Control NewParent)
        {
            parent = NewParent;
            flashTimer.SynchronizingObject = parent;
            textScrollTimer.SynchronizingObject = parent;
        }

        private void ReloadColors(object sender, EventArgs e)
        {
            Redraw();
        }

        #region " Properties "
        /// <summary>
        /// The bounds of the control
        /// </summary>
        /// <value>The current bounds of the control</value>
        /// <returns>The new bounds of the control</returns>
        /// <remarks>
        /// These bounds are in pixels for a parent control's client area. A redraw will only be triggered if the new value is
        /// different from the current value.
        /// </remarks>
        public Rectangle Bounds
        {
            get { return bounds; }
            set
            {
                if (bounds == value) return;
                bounds = value;
                Redraw();
            }
        }
        protected Rectangle bounds;

        /// <summary>
        /// The height of the control.
        /// </summary>
        /// <value>The new height for the control</value>
        /// <returns>The current height of the control</returns>
        /// <remarks>
        /// This is an alias for Bounds.Height, and is included to make handling lightweight controls more similar to standard
        /// controls. A redraw will only be triggered if the new value is different from the current value.
        /// </remarks>
        public int Height
        {
            get { return bounds.Height; }
            set
            {
                if (bounds.Height == value) return;
                bounds.Height = value;
                Redraw();
            }
        }

        /// <summary>
        /// The width of the control.
        /// </summary>
        /// <value>The current width of the control</value>
        /// <returns>The new width for the control</returns>
        /// <remarks>
        /// This is an alias for Bounds.Width, and is included to make handling lightweight controls more similar to standard
        /// controls. A redraw will only be triggered if the new value is different from the current value.
        /// </remarks>
        public int Width
        {
            get { return bounds.Width; }
            set
            {
                if (bounds.Width == value) return;
                bounds.Width = value;
                Redraw();
            }
        }

        /// <summary>
        /// The text of the control
        /// </summary>
        /// <value>New text for the control</value>
        /// <returns>Current text of the control</returns>
        /// <remarks>
        /// A redraw will always be triggered when this text is changed.
        /// </remarks>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                currentTextScrollRotation = value;
                Redraw();
            }
        }
        protected string text = "LCARS";

        /// <summary>
        /// The alignment for the control text
        /// </summary>
        /// <remarks>
        /// In derived controls, this refers to the alignment within the valid text area
        /// only, not the entire control.
        /// </remarks>
        public ContentAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                Redraw();
            }
        }
        protected ContentAlignment textAlign = ContentAlignment.TopLeft;

        /// <summary>
        /// Prevents the control from redrawing
        /// </summary>
        /// <remarks>
        /// Setting this property to true will trigger a redraw, regardless of its previous
        /// value.
        /// </remarks>
        public bool HoldDraw
        {
            get { return holdDraw; }
            set
            {
                holdDraw = value;
                if (!HoldDraw) Redraw();
                else myBitmap = null;
            }
        }
        protected bool holdDraw = false;

        /// <summary>
        /// The left position of the control, relative to its parent container
        /// </summary>
        public int Left
        {
            get { return bounds.Left; }
            set
            {
                if (value == bounds.Left) return;
                bounds = new Rectangle(value, bounds.Top, bounds.Width, bounds.Height);
                Redraw();
            }
        }

        /// <summary>
        /// The top position of the control, relative to its parent container
        /// </summary>
        public int Top
        {
            get { return bounds.Top; }
            set
            {
                if (value == bounds.Top) return;
                bounds = new Rectangle(bounds.Left, value, bounds.Width, bounds.Height);
                Redraw();
            }
        }

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
        object data;

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data2
        {
            get { return data2; }
            set { data2 = value; }
        }
        object data2;

        /// <summary>
        /// The color to display if RedAlert is set to Custom
        /// </summary>
        public Color CustomAlertColor
        {
            get { return customAlertColor; }
            set
            {
                customAlertColor = value;
                if (alertState == LcarsAlert.Custom)
                {
                    Redraw();
                }
            }
        }
        protected Color customAlertColor = new Color();

        /// <summary>
        /// The alert state of the control
        /// </summary>
        public LcarsAlert AlertState
        {
            get { return alertState; }
            set
            {
                if (value == alertState) return;
                alertState = value;
                Redraw();
            }
        }
        protected LcarsAlert alertState = LcarsAlert.Normal;

        /// <summary>
        /// Sets the text scrolling  for the control
        /// </summary>
        /// <remarks>
        /// This should only change in response to mouseover events
        /// </remarks>
        protected bool Scrolling
        {
            get { return textScrollTimer.Enabled; }
            set
            {
                if (textScrollTimer.Enabled == value) return;
                if (value)
                {
                    Bitmap temp = new Bitmap(1, 1);
                    Graphics g = Graphics.FromImage(temp);
                    SizeF textSize = g.MeasureString(text, font);
                    g.Dispose();
                    if (textSize.Width > textArea.Width)
                    {
                        currentTextScrollRotation = text + "              ";
                        textScrollTimer.Enabled = value;
                    }
                }
                else
                {
                    textScrollTimer.Enabled = false;
                    currentTextScrollRotation = text;
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Force text to display in all caps
        /// </summary>
        /// <remarks>
        /// Does not change the value returned by the <see cref="Text">Text</see>
        /// property. Setting this property to its current value will not trigger a redraw.
        /// </remarks>
        public bool ForceCaps
        {
            get { return forceCaps; }
            set
            {
                if (value == forceCaps) return;
                forceCaps = value;
                Redraw();
            }
        }
        protected bool forceCaps = true;

        /// <summary>
        /// The color to display for the majority of the control
        /// </summary>
        /// <remarks>
        /// Setting this property to its current value will not trigger a redraw.
        /// </remarks>
        public LcarsColorFunction ColorFunction
        {
            get { return colorFunction; }
            set
            {
                if (colorFunction == value) return;
                colorFunction = value;
                Redraw();
            }
        }
        protected LcarsColorFunction colorFunction = LcarsColorFunction.MiscFunction;

        /// <summary>
        /// The font to use when displaying the control's text
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(EditorBrowsableState.Advanced)]
        public Font Font
        {
            get { return font; }
            set
            {
                font = value;
                Redraw();
            }
        }
        protected Font font = FontProvider.Lcars(14);

        /// <summary>
        /// Sets the lit state of the control
        /// </summary>
        /// <remarks>
        /// Updating this property to a new value will require a full redraw. Setting this
        /// property to its current value will not trigger a redraw.
        /// </remarks>
        public bool IsLit
        {
            get { return isLit; }
            set
            {
                if (value == isLit) return;
                isLit = value;
                Redraw();
            }
        }
        protected bool isLit = true;


        /// <summary>
        /// Sets the flashing state of the control
        /// </summary>
        /// <remarks>
        /// If flashing is enabled, it will occur on a separate thread.
        /// </remarks>
        public bool Flashing
        {
            get { return flashing; }
            set
            {
                if (value == flashing) return;
                flashing = value;
                flashTimer.Enabled = value;
                if (!value) Redraw();
            }
        }
        protected bool flashing = false;

        /// <summary>
        /// The interval on which to change the lit state of the button for flashing
        /// </summary>
        /// <remarks>
        /// A full flashing cycle is twice this interval.
        /// </remarks>
        public double FlashInterval
        {
            get { return flashTimer.Interval; }
            set { flashTimer.Interval = value; }
        }

        /// <summary>
        /// The beeping option for the control
        /// </summary>
        /// <remarks>
        /// Warning: high-pitched beeps can cause irritation in users. Use with caution!
        /// </remarks>
        public bool DoesSound
        {
            get { return doesBeep; }
            set { doesBeep = value; }
        }
        protected bool doesBeep = true;

        public virtual LcarsSoundAsset SoundAsset
        {
            get { return soundAsset; }
            set { soundAsset = value; }
        }
        private LcarsSoundAsset soundAsset = LcarsSoundAsset.Unset;

        /// <summary>
        /// Sets the clickable state of the control
        /// </summary>
        public bool Clickable
        {
            get { return canClick; }
            set { canClick = value; }
        }
        protected bool canClick = true;

        /// <summary>
        /// Sets the auto-ellipsis property of the control
        /// </summary>
        public bool AutoEllipsis
        {
            get { return autoEllipsis; }
            set
            {
                if (value == autoEllipsis) return;
                autoEllipsis = value;
                Redraw();
            }
        }
        protected bool autoEllipsis = true;

        /// <summary>
        /// Sets the ellipsis mode for text clipping
        /// </summary>
        /// <remarks>
        /// The default is nearest-character, but nearest-word can be specified if you wish.
        /// </remarks>
        public EllipsisModes EllipsisMode
        {
            get { return ellipsisMode; }
            set
            {
                if (value == ellipsisMode) return;
                ellipsisMode = value;
                Redraw();
            }
        }
        protected EllipsisModes ellipsisMode = EllipsisModes.Character;

        /// <summary>
        /// Height of the text for the control
        /// </summary>
        public float TextHeight
        {
            get { return font.Size; }
            set
            {
                if (value == font.Size) return;
                font = FontProvider.Lcars(value);
                Redraw();
            }
        }

        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null) colorManager.ColorsUpdated -= ReloadColors;
                colorManager = value;
                if (colorManager != null) colorManager.ColorsUpdated += ReloadColors;
                Redraw();
            }
        }
        private LcarsColorManager colorManager;
        #endregion

        #region " IDisposable Support "
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    textScrollTimer.Elapsed -= TextScrollTimer_Elapsed;
                    textScrollTimer.Enabled = false;
                    flashTimer.Elapsed -= FlashTimer_Elapsed;
                    flashTimer.Enabled = false;
                }

            }
            this.disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region " Conversion "
        public static implicit operator FlatButton(LCFlatButton o)
        {
            FlatButton newButton = new FlatButton();
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
            return newButton;
        }

        public static explicit operator LCFlatButton(FlatButton o)
        {
            LCFlatButton newButton = new LCFlatButton();
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
            return newButton;
        }

        public LCFlatButton()
        {
            ColorManager = new LcarsColorManager();
            textScrollTimer.Elapsed += TextScrollTimer_Elapsed;
            flashTimer.Elapsed += FlashTimer_Elapsed;
            Click += Beep;
            currentTextScrollRotation = text;
        }
        #endregion
    }
}
