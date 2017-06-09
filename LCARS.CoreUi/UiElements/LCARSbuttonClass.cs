using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using LCARS.CoreUi.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LCARS.CoreUi.UiElements
{
    #region " GenericButton "
    /// <summary>
    /// Generic LCARS button that all others inherit from
    /// </summary>
    /// <remarks>
    /// This class can be placed directly on a form in the designer. If you do so, you will wind up with something resembling a Standard 
    /// Button, but with fewer features. This class should only be used directly when you don't know what kind of LCARS button you're
    /// dealing with. Almost everything in modBusiness is declared that way for exactly that reason. 
    /// </remarks>
    [DefaultEvent("Click"), Designer(typeof(GenericButtonDesigner))]
    public class LCARSbuttonClass : System.Windows.Forms.Control, IAlertable, IBeeping, IColorable
    {
        #region " Control Design Information "
        /// <summary>
        /// Creates a new instance of the LCARButtonClass object
        /// </summary>
        /// <remarks></remarks>
        public LCARSbuttonClass() : base()
        {
            Click += GenericButton_Click;
            MouseUp += GenericButton_MouseUp;
            MouseDown += GenericButton_MouseDown;
            MouseUp += lblText_MouseUp;
            MouseMove += lblText_MouseMove;
            MouseDown += lblText_MouseDown;
            Resize += Button_Resize;
            Click += doClick;
            MouseLeave += lblText_MouseLeave;
            MouseEnter += lblText_MouseEnter;
            DoubleClick += lblText_DoubleClick;
            Click += lblText_Click;

            _textSize = this.Size;
            _font = new Font("LCARS", textHeight, FontStyle.Regular, GraphicsUnit.Point);
            //This call is required by the Windows Form Designer.
            InitializeComponent();
            base.DoubleBuffered = true;
            Sound.Load();
            //Add any initialization after the InitializeComponent() call
        }

        /// <summary>
        /// Disposes of the current instance of LCARSButtonClass
        /// </summary>
        /// <param name="disposing">Boolean indicating whether to dispose of internal components</param>
        /// <remarks>Overriden to clean up the component list</remarks>
        protected override void Dispose(bool disposing)
        {
            //Needed to end the flashing thread and allow button to be disposed at all
            if (flashing)
            {
                flasher.Abort();
            }
            if (disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            //StandardButton
            //
            this.Name = "LCARSbutton";
            this.Size = new Size(147, 36);
            //Me.Controls.Add(Me.lblText)
            //lblText.BringToFront()
            this.ResumeLayout(false);
            this.Text = "LCARS";
        }
        #endregion

        #region " Global Variables "
        //Left button constants
        const int lButtonClick = 0x201;
        const int lButtonUp = 0x202;

        const int lButtonDouble = 0x203;
        //Right button constants
        const int rButtonClick = 0x204;
        const int rButtonUp = 0x205;

        const int rButtonDouble = 0x206;
        //Middle button constants
        const int mButtonClick = 0x207;
        const int mButtonUp = 0x208;

        const int mButtonDouble = 0x209;
        private LcarsColorManager colorManager = new LcarsColorManager();
        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null)
                {
                    colorManager.ColorsUpdated -= ColorsUpdated;
                }
                colorManager = value;
                if (colorManager != null)
                {
                    colorManager.ColorsUpdated += ColorsUpdated;
                }
            }

        }

        LcarsColorFunction colorFunction = LcarsColorFunction.MiscFunction;
        bool isLit = true;
        Bitmap NormalButton;
        Bitmap UnLitButton;
        protected LcarsAlert currentAlertState = LcarsAlert.Normal;
        bool RA = false;
        private static System.Media.SoundPlayer Sound = new System.Media.SoundPlayer(SoundProvider.PlainBeep);
        object buttonData;
        object buttonData2;
        bool noDraw = false;
        protected int textHeight = 14;
        bool doBeep = false;
        bool flashing = false;
        Thread flasher;
        int flashingInterval = 500;
        bool isFlashing;
        bool canClick = true;
        System.Drawing.ContentAlignment oAlign;
        private System.Windows.Forms.Timer withEventsField_tmrTextScroll = new System.Windows.Forms.Timer();
        public System.Windows.Forms.Timer tmrTextScroll
        {
            get { return withEventsField_tmrTextScroll; }
            set
            {
                if (withEventsField_tmrTextScroll != null)
                {
                    withEventsField_tmrTextScroll.Tick -= tmrTextScroll_Tick;
                }
                withEventsField_tmrTextScroll = value;
                if (withEventsField_tmrTextScroll != null)
                {
                    withEventsField_tmrTextScroll.Tick += tmrTextScroll_Tick;
                }
            }
        }
        protected string myText = "LCARS";
        protected bool forceCapital = true;
        private string tmpStr = "";
        protected Color _customAlertColor;
        protected ContentAlignment _textAlign = ContentAlignment.TopLeft;
        protected Font _font;
        protected EllipsisModes _ellipsisMode = EllipsisModes.Character;
        protected bool _autoEllipsis = true;
        protected bool _textVisible = true;
        protected Size _textSize;
        #endregion
        protected Point _textLocation = new Point(0, 0);

        #region " Events "

        /// <summary>
        /// Raised when the button is clicked
        /// </summary>
        public new event ClickEventHandler Click;
        public new delegate void ClickEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Raised when the button is double-clicked
        /// </summary>
        public new event DoubleClickEventHandler DoubleClick;
        public new delegate void DoubleClickEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Raised when a mouse button is depressed while the mouse is over the button
        /// </summary>
        public new event MouseDownEventHandler MouseDown;
        public new delegate void MouseDownEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
        /// <summary>
        /// Raised when a mouse button is released while the mouse is over the button
        /// </summary>
        public new event MouseUpEventHandler MouseUp;
        public new delegate void MouseUpEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
        /// <summary>
        /// Raised when the mouse moves over the button
        /// </summary>
        public new event MouseMoveEventHandler MouseMove;
        public new delegate void MouseMoveEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
        #endregion

        #region " Enum "
        /// <summary>
        /// Contains the types for different methods of AutoEllipsis functionality
        /// </summary>
        public enum EllipsisModes
        {
            /// <summary>
            /// Clips strings to the nearest character
            /// </summary>
            Character = 0,
            /// <summary>
            /// Clips strings to the nearest word
            /// </summary>
            Word = 1
        }

        #endregion

        #region " Properties "

        /// <summary>
        /// The background image of the control
        /// </summary>
        /// <value>New image to be set as background</value>
        /// <returns>Current background image</returns>
        /// <remarks>
        /// This property is hidden by default, and would be protected if it could be. It is used internally to draw the button, 
        /// so should not be modified. 
        /// </remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        /// <summary>
        /// The font used by the label for drawing text.
        /// </summary>
        /// <value>New font to be used</value>
        /// <returns>Current font in use</returns>
        /// <remarks>
        /// This property is hidden by default, and should only be used if you need to change the family of the font used.
        /// Size can be changed through other properties.
        /// </remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public override System.Drawing.Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// The text of the control
        /// </summary>
        /// <value>New text to set</value>
        /// <returns>Current text of the control.</returns>
        /// <remarks>This property duplicates the functionality of the ButtonText property.</remarks>
        public override string Text
        {
            get { return ButtonText; }
            set { ButtonText = value; }
        }

        /// <summary>
        /// Alignment for text on button
        /// </summary>
        /// <value>New alignment to use</value>
        /// <returns>Current alignment</returns>
        /// <remarks>Be careful if using this with an elbow; it can have interesting results...</remarks>
        [DefaultValue(ContentAlignment.BottomRight)]
        public virtual ContentAlignment ButtonTextAlign
        {

            get { return _textAlign; }
            set
            {
                _textAlign = value;
                this.Invalidate();
            }
        }

        protected override bool ScaleChildren
        {
            get { return false; }
        }

        /// <summary>
        /// Forces text to be displayed as all-capitals
        /// </summary>
        /// <value>New setting</value>
        /// <returns>Current setting</returns>
        /// <remarks>
        /// If this property is set to true, the text will be converted to all-capitals. If you are going to use that text for comparisons,
        /// you should be sure to use .ToUpper() to ensure they are the same case.
        /// </remarks>
        [DefaultValue(true)]
        public bool _ForceCaps
        {
            get { return forceCapital; }
            set { forceCapital = value; }
        }

        /// <summary>
        /// Cuts off text that is longer than the control with ...
        /// </summary>
        /// <value>New AutoEllipsis setting</value>
        /// <returns>Current AutoEllipsis setting</returns>
        /// <remarks>
        /// If the text is longer than the control, it will be shown in a marquee when the user mouses over it. 
        /// </remarks>
        [DefaultValue(true)]
        public bool AutoEllipsis
        {
            get { return _autoEllipsis; }
            set
            {
                _autoEllipsis = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Sets whether the control can be clicked
        /// </summary>
        /// <value>New Clickable setting</value>
        /// <returns>Current Clickable setting</returns>
        /// <remarks>
        /// If you are using a control as a static element, set this to false to prevent the user from trying to click on it.
        /// </remarks>
        public bool Clickable
        {
            get { return canClick; }
            set { canClick = value; }
        }

        /// <summary>
        /// Sets whether the control is flashing
        /// </summary>
        /// <value>New flashing setting</value>
        /// <returns>Current flashing setting</returns>
        /// <remarks>
        /// This works by setting the RedAlert property of the control. If you use it, it should be for good reason.
        /// </remarks>
        [DefaultValue(false)]
        public bool Flash
        {
            get { return flashing; }
            set
            {
                flashing = value;
                if (this.DesignMode == false)
                {
                    if (flashing)
                    {
                        flasher = new Thread(flashThread);
                        flasher.Start();
                    }
                    else
                    {
                        flasher.Abort();
                    }
                }

            }
        }

        /// <summary>
        /// Length of flashing interval
        /// </summary>
        /// <value>New flash length</value>
        /// <returns>Current flash length</returns>
        /// <remarks></remarks>
        public int FlashInterval
        {
            get { return flashingInterval; }
            set { flashingInterval = value; }
        }

        /// <summary>
        /// Stops control from redrawing if set to true
        /// </summary>
        /// <value>New draw state</value>
        /// <returns>Current draw state</returns>
        /// <remarks>
        /// This should be used to reduce CPU load when making many changes to a button's state that would require a redraw. Just
        /// remember to set it to True when you've finished.
        /// </remarks>
        public bool holdDraw
        {
            get { return noDraw; }
            set
            {
                noDraw = value;
                DrawAllButtons();
            }
        }

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <value>New value</value>
        /// <returns>Current value</returns>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data
        {
            get { return buttonData; }
            set { buttonData = value; }
        }

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <value>New value</value>
        /// <returns>Current value</returns>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data2
        {
            get { return buttonData2; }
            set { buttonData2 = value; }
        }

        /// <summary>
        /// Primary color of the control.
        /// </summary>
        /// <remarks>
        /// Do not attempt to use colors for visual effect. Color mappings may be changed by the end user, completely eliminating
        /// any color matchings that may exist. Use colors based on the control's function.
        /// </remarks>
        [DefaultValue(LcarsColorFunction.NavigationFunction)]
        public LcarsColorFunction ColorFunction
        {
            get { return colorFunction; }
            set
            {
                colorFunction = value;
                DrawAllButtons();
            }
        }

        /// <summary>
        /// The text displayed on the button.
        /// </summary>
        /// <remarks>
        /// The button's <see cref="Text">Text</see> property is an alias of this property.
        /// </remarks>
        [DefaultValue("LCARS Button")]
        public virtual string ButtonText
        {
            get { return myText; }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                if (forceCapital)
                {
                    myText = value.ToUpper();
                }
                else
                {
                    myText = value;
                }
                tmpStr = myText;
                if (textHeight == -1)
                {
                    ButtonTextHeight = -1;
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// Sets the visibility of the text
        /// </summary>
        protected bool lblTextVisible
        {
            get { return _textVisible; }
            set
            {
                _textVisible = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Location of label used to display text
        /// </summary>
        [Browsable(false), EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Point lblTextLoc
        {
            get { return _textLocation; }
            set
            {
                _textLocation = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Size of label used to display text
        /// </summary>
        /// <remarks>This label does not auto-size, and should be handled accordingly.</remarks>
        [Browsable(false), EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size lblTextSize
        {
            get { return _textSize; }
            set
            {
                _textSize = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Height of the control's text
        /// </summary>
        /// <remarks>
        /// For the <see cref="LCARS.Controls.TextButton">Text Button</see>, this directly sets the height of the control.
        /// </remarks>
        public virtual int ButtonTextHeight
        {
            get { return textHeight; }
            set
            {
                textHeight = value;
                if (textHeight == -1)
                {
                    if (!string.IsNullOrEmpty(myText))
                    {
                        SizeF mysize = new SizeF();
                        int i = 1;
                        Graphics g = Graphics.FromImage(new Bitmap(10, 10));
                        mysize = g.MeasureString(myText, new Font("LCARS", i, FontStyle.Regular, GraphicsUnit.Point));

                        while (!(mysize.Width >= this.Width - 8 | mysize.Height >= this.Height))
                        {
                            i += 1;
                            mysize = g.MeasureString(myText, new Font("LCARS", i, FontStyle.Regular, GraphicsUnit.Point));
                        }
                        if (i < 2)
                        {
                            i = 2;
                        }
                        _font = new Font("LCARS", i - 1, FontStyle.Regular, GraphicsUnit.Point);
                        textHeight = -1;
                    }

                }
                else
                {
                    _font = new Font("LCARS", textHeight, FontStyle.Regular, GraphicsUnit.Point);
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// Lit property of the control
        /// </summary>
        /// <remarks>
        /// A control that is not lit has been dimmed by applying an alpha layer. This can be used to show a function that is availiable,
        /// but turned off. An offline function should use the offline function color.
        /// </remarks>
        public virtual bool Lit
        {
            get { return isLit; }
            set
            {
                isLit = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// The alert status of the control.
        /// </summary>
        public LcarsAlert AlertState
        {
            get { return currentAlertState; }
            set
            {
                currentAlertState = value;
                if (currentAlertState == LcarsAlert.Normal)
                {
                    RA = false;
                }
                else
                {
                    RA = true;
                }
                DrawAllButtons();
            }
        }

        /// <summary>
        /// The color to display if RedAlert is set to Custom
        /// </summary>
        /// <remarks>
        /// Setting this property will not result in any change in the display unless
        /// the RedAlert property is set to Custom.
        /// </remarks>
        public Color CustomAlertColor
        {
            get { return _customAlertColor; }
            set
            {
                _customAlertColor = value;
                DrawAllButtons();
            }
        }

        /// <summary>
        /// Sets whether the control will beep when clicked.
        /// </summary>
        /// <remarks>
        /// This property should be set on application startup to match the global setting. This must be done manually.
        /// </remarks>
        public virtual bool Beeping
        {
            get { return doBeep; }
            set { doBeep = value; }
        }

        /// <summary>
        /// LCARSColorClass associated with this control.
        /// </summary>
        /// <remarks>
        /// If you need to change the color of a control to a non-predefined color, this has the tools to do it. For example:
        /// <code>
        /// Dim myButton as LCARSbuttonClass
        /// Dim myColors() as String = {"#3366CC", "#99CCFF", "#CC99CC", "#FFCC00", "#FFFF99", "#CC6666", "#FFFFFF", "#FF0000", "#FFCC66", "Orange", "#99CCFF"}
        /// myButton.ColorsAvailable.setColors(myColors)
        /// </code>
        /// That code will set all colors used by this control to the defaults. Naturally you can be more specific if needed.
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LcarsColorManager ColorsAvailable
        {
            get { return ColorManager; }
            set
            {
                ColorManager = value;
                DrawAllButtons();
            }
        }
        #endregion

        #region " Subs "
        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                if (!canClick)
                {
                    if (m.Msg == lButtonClick |
                        m.Msg == lButtonUp |
                        m.Msg == lButtonDouble |
                        m.Msg == rButtonClick |
                        m.Msg == rButtonUp |
                        m.Msg == rButtonDouble |
                        m.Msg == mButtonClick |
                        m.Msg == mButtonUp |
                        m.Msg == mButtonDouble)
                    {
                        return;
                    }
                }

                if (m.Msg == lButtonDouble) m.Msg = lButtonClick;

                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
            }
        }

        protected override void ScaleControl(System.Drawing.SizeF factor, System.Windows.Forms.BoundsSpecified specified)
        {
            this.ButtonTextHeight = textHeight * (int)factor.Height;
            base.ScaleControl(factor, specified);
        }

        private void lblText_Click(object sender, EventArgs e)
        {
            if (canClick) buttonDown();
        }

        private void lblText_DoubleClick(object sender, System.EventArgs e)
        {
            if (canClick) buttonDown();
        }

        private void lblText_MouseEnter(object sender, EventArgs e)
        {
            if (this.CreateGraphics().MeasureString(myText, _font).Width > lblTextSize.Width)
            {
                tmrTextScroll.Interval = 200;
                oAlign = _textAlign;
                if ((int)_textAlign <= 16)
                {
                    //bottom aligned
                    _textAlign = ContentAlignment.TopLeft;
                }
                else if ((int)_textAlign <= 256)
                {
                    //middle aligned
                    _textAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    //top aligned
                    _textAlign = ContentAlignment.BottomLeft;
                }
                tmpStr = myText + "              ";
                tmrTextScroll.Enabled = true;
            }
        }

        private void lblText_MouseLeave(object sener, EventArgs e)
        {
            if (tmrTextScroll.Enabled)
            {
                tmrTextScroll.Enabled = false;

                if (oAlign > 0)
                {
                    _textAlign = oAlign;
                }

                tmpStr = myText;
                Invalidate();
            }
        }

        private void tmrTextScroll_Tick(object sender, EventArgs e)
        {
            tmpStr = tmpStr.Substring(1) + tmpStr.Substring(0, 1);
            Invalidate();
        }

        /// <summary>
        /// Raises a click event from this control.
        /// </summary>
        public void doClick(object sender, EventArgs e)
        {
            if (canClick)
            {
                Click?.Invoke(this, e);
                //allow the user to create a click event (for voice commands)
            }
        }

        private void flashThread()
        {
            try
            {
                while (flashing)
                {
                    isFlashing = !isFlashing;
                    this.Invalidate();
                    Application.DoEvents();
                    Thread.Sleep(flashingInterval);
                }
            }
            catch (ThreadAbortException t)
            {
                isFlashing = false;
                this.Invalidate();
            }
        }

        private void playSound()
        {
            string soundPath = new SettingsStore("LCARS").Load("Application", "ButtonSound", "");
            if (Sound == null | Sound.SoundLocation != soundPath)
            {
                if (System.IO.File.Exists(soundPath))
                {
                    Sound = new System.Media.SoundPlayer(soundPath);
                }
                else
                {
                    Sound = new System.Media.SoundPlayer(SoundProvider.PlainBeep);
                }
            }
            Sound.Play();
        }

        /// <summary>
        /// Redraws the button and stores the bitmaps to memory.
        /// </summary>
        public void DrawAllButtons()
        {
            if (noDraw == false)
            {
                if (!(this.Width == 0 | this.Height == 0))
                {
                    if ((NormalButton != null))
                    {
                        //UnlitButton does not require null check
                        NormalButton.Dispose();
                        UnLitButton.Dispose();
                    }
                    //Draw and show the standard "normal" button.
                    //----------------------------------------------------------------------
                    NormalButton = DrawButton();

                    //Draw the button that is displayed when the function of the button is 
                    //unavailable or disabled.
                    //-----------------------------------------------------------------------
                    UnLitButton = drawUnlit(NormalButton);

                    Invalidate();
                }
            }
        }

        private void Button_Resize(object sender, System.EventArgs e)
        {
            if (textHeight == -1)
            {
                ButtonTextHeight = -1;
                //resize the text
            }
            DrawAllButtons();
        }

        private void lblText_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (canClick)
            {
                MouseDown?.Invoke(this, e);
                GenericButton_MouseDown(this, e);
            }
        }
        private void lblText_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (canClick)
            {
                MouseMove?.Invoke(this, e);
            }
        }

        private void lblText_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (canClick)
            {
                MouseUp?.Invoke(this, e);
                GenericButton_MouseUp(this, e);
            }
        }

        private void GenericButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // turn color white when pressed, unless already white, then turn it red
            LcarsColorManager lcarsColor = new LcarsColorManager();
            if (lcarsColor.GetColor(colorFunction).ToArgb() == System.Drawing.Color.White.ToArgb())
            {
                currentAlertState = LcarsAlert.Red;
            }
            else
            {
                currentAlertState = LcarsAlert.White;
            }
            DrawAllButtons();
        }

        private void GenericButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (RA == false)
            {
                AlertState = LcarsAlert.Normal;
            }
        }

        private void buttonDown()
        {
            if (doBeep)
            {
                playSound();
            }
        }
        private void GenericButton_Click(object sender, System.EventArgs e)
        {
            if (RA == false)
            {
                this.AlertState = LcarsAlert.Normal;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (isLit ^ isFlashing)
            {
                e.Graphics.DrawImage(NormalButton, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(UnLitButton, 0, 0);
            }
            if (lblTextVisible)
            {
                DrawText(e.Graphics);
            }
        }

        private void ColorsUpdated(object sender, System.EventArgs e)
        {
            DrawAllButtons();
        }
        #endregion

        #region " Draw Generic Button "
        public virtual Bitmap DrawButton()
        {
            Bitmap mybitmap = default(Bitmap);
            Graphics g = default(Graphics);
            SolidBrush myBrush = new SolidBrush(ColorsAvailable.GetColor(colorFunction));

            if (currentAlertState == LcarsAlert.Red)
            {
                myBrush = new SolidBrush(Color.Red);
            }
            else if (currentAlertState == LcarsAlert.White)
            {
                myBrush = new SolidBrush(Color.White);
            }
            else if (currentAlertState == LcarsAlert.Yellow)
            {
                myBrush = new SolidBrush(Color.Yellow);
            }
            else if (currentAlertState == LcarsAlert.Custom)
            {
                myBrush = new SolidBrush(_customAlertColor);
            }

            mybitmap = new Bitmap(this.Size.Width, this.Size.Height);
            g = Graphics.FromImage(mybitmap);

            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g.FillEllipse(myBrush, 0, 0, this.Size.Height, this.Size.Height);
            g.FillRectangle(myBrush, this.Size.Height / 2, 0, this.Size.Width - this.Size.Height, this.Size.Height);
            g.FillEllipse(myBrush, this.Size.Width - this.Size.Height, 0, this.Size.Height, this.Size.Height);
            //Draw text:
            this.lblTextLoc = new Point(0, 0);
            this.lblTextSize = (Size)new Point(this.Width - this.Height, this.Height);
            g.Dispose();
            return mybitmap;
        }

        /// <summary>
        /// Draws the text of the control in specified area.
        /// </summary>
        /// <param name="g">Graphics object being used to draw the control.</param>
        /// <remarks>This sub will handle all auto-ellipsis functionality and alignment.
        /// </remarks>
        protected void DrawText(Graphics g)
        {
            if (_textVisible)
            {
                Rectangle area = new Rectangle(_textLocation, _textSize);
                StringFormat format = new StringFormat();
                format.FormatFlags = StringFormatFlags.NoWrap;
                if (tmrTextScroll.Enabled | !_autoEllipsis)
                {
                    if (_ellipsisMode == EllipsisModes.Character)
                    {
                        format.Trimming = StringTrimming.Character;
                    }
                    else
                    {
                        format.Trimming = StringTrimming.Word;
                    }
                }
                else
                {
                    if (_ellipsisMode == EllipsisModes.Character)
                    {
                        format.Trimming = StringTrimming.EllipsisCharacter;
                    }
                    else
                    {
                        format.Trimming = StringTrimming.EllipsisWord;
                    }
                }
                if (tmrTextScroll.Enabled)
                {
                    format.Alignment = StringAlignment.Near;
                }
                else
                {
                    if (_textAlign == ContentAlignment.BottomCenter | _textAlign == ContentAlignment.MiddleCenter | _textAlign == ContentAlignment.TopCenter)
                    {
                        format.Alignment = StringAlignment.Center;
                    }
                    else if (_textAlign == ContentAlignment.BottomRight | _textAlign == ContentAlignment.MiddleRight | _textAlign == ContentAlignment.TopRight)
                    {
                        format.Alignment = StringAlignment.Far;
                    }
                    else if (_textAlign == ContentAlignment.BottomLeft | _textAlign == ContentAlignment.MiddleLeft | _textAlign == ContentAlignment.TopLeft)
                    {
                        format.Alignment = StringAlignment.Near;
                    }
                }
                if (_textAlign == ContentAlignment.TopCenter | _textAlign == ContentAlignment.TopLeft | _textAlign == ContentAlignment.TopRight)
                {
                    format.LineAlignment = StringAlignment.Near;
                }
                else if (_textAlign == ContentAlignment.MiddleCenter | _textAlign == ContentAlignment.MiddleLeft | _textAlign == ContentAlignment.MiddleRight)
                {
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (_textAlign == ContentAlignment.BottomCenter | _textAlign == ContentAlignment.BottomLeft | _textAlign == ContentAlignment.BottomRight)
                {
                    format.LineAlignment = StringAlignment.Far;
                }
                if (_ForceCaps)
                {
                    g.DrawString(tmpStr.ToUpper(), _font, Brushes.Black, area, format);
                }
                else
                {
                    g.DrawString(tmpStr, _font, Brushes.Black, area, format);
                }
            }
        }
        #endregion

        #region " Draw Unlit Button "
        private Bitmap drawUnlit(Bitmap normal)
        {
            Color fadeColor = System.Drawing.Color.FromArgb(128, 0, 0, 0);
            Bitmap mybitmap = new Bitmap(normal);
            //make a copy of the normal button.
            Graphics g = Graphics.FromImage(mybitmap);
            SolidBrush mybrush = new SolidBrush(fadeColor);

            g.FillRectangle(mybrush, 0, 0, mybitmap.Width, mybitmap.Height);
            return mybitmap;
        }
        #endregion
    }
    #endregion

    #region " GenericButton Designer "
    //We had to import some extra classes to be able to use this, so scroll all the way up to
    //see which ones (they are commented).
    public class GenericButtonDesigner : ControlDesigner
    {
        protected override void PostFilterProperties(System.Collections.IDictionary Properties)
        {
            Properties.Remove("AccessibleName");
            Properties.Remove("AccessibleRole");
            Properties.Remove("AccessibleDescription");
            Properties.Remove("AllowDrop");
            Properties.Remove("BackColor");
            Properties.Remove("BackgroundImageLayout");
            Properties.Remove("CausesValidation");
            Properties.Remove("ContextMenuStrip");
            Properties.Remove("Enabled");
            Properties.Remove("Font");
            Properties.Remove("ForeColor");
            Properties.Remove("GenerateMember");
            Properties.Remove("ImeMode");
            Properties.Remove("Locked");
            Properties.Remove("Margin");
            Properties.Remove("MaximumSize");
            Properties.Remove("MinimumSize");
            Properties.Remove("Modifiers");
            Properties.Remove("Padding");
            Properties.Remove("RightToLeft");
        }
    }
    #endregion
}
