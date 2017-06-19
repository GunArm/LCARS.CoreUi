using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LCARS.CoreUi.UiElements.Base
{
    public partial class LcarsButtonBase
    {
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
        public override Image BackgroundImage
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
        public override Font Font
        {
            get { return font; }
            set
            {
                font = value;
                this.Invalidate();
            }
        }
        protected Font font;

        /// <summary>
        /// Alignment for text on button
        /// </summary>
        /// <value>New alignment to use</value>
        /// <returns>Current alignment</returns>
        /// <remarks>Be careful if using this with an elbow; it can have interesting results...</remarks>
        [DefaultValue(ContentAlignment.BottomRight)]
        public virtual ContentAlignment ButtonTextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                this.Invalidate();
            }
        }
        protected ContentAlignment textAlign = ContentAlignment.TopLeft;

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
        public bool ForceCaps
        {
            get { return forceCapital; }
            set { forceCapital = value; }
        }
        protected bool forceCapital = true;

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
            get { return autoEllipsis; }
            set
            {
                autoEllipsis = value;
                this.Invalidate();
            }
        }
        protected bool autoEllipsis = true;

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
        bool canClick = true;

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
                if (!DesignMode)
                {
                    if (flashing)
                    {
                        flasher = new Thread(FlasherThread);
                        flasher.Start();
                    }
                    else
                    {
                        if(flasher != null) flasher.Abort();
                    }
                }

            }
        }
        private bool flashing = false;

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
        private int flashingInterval = 500;

        /// <summary>
        /// Stops control from redrawing if set to true
        /// </summary>
        /// <value>New draw state</value>
        /// <returns>Current draw state</returns>
        /// <remarks>
        /// This should be used to reduce CPU load when making many changes to a button's state that would require a redraw. Just
        /// remember to set it to True when you've finished.
        /// </remarks>
        public bool HoldDraw
        {
            get { return holdDraw; }
            set
            {
                holdDraw = value;
                DrawAllButtons();
            }
        }
        private bool holdDraw = false;

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <value>New value</value>
        /// <returns>Current value</returns>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data { get; set; }

        /// <summary>
        /// Data field for the control
        /// </summary>
        /// <value>New value</value>
        /// <returns>Current value</returns>
        /// <remarks>
        /// This is for the programmer's convenience in associating information with a control. There is no built-in functionality, 
        /// so it is exactly what you make it.
        /// </remarks>
        public object Data2 { get; set; }

        /// <summary>
        /// The text displayed on the button.
        /// </summary>
        /// <remarks>
        /// The button's <see cref="Text">Text</see> property is an alias of this property.
        /// </remarks>
        [DefaultValue("LCARS Button")]
        public virtual string ButtonText
        {
            get { return buttonText; }
            set
            {
                if (value == null) value = string.Empty;

                if (forceCapital) buttonText = value.ToUpper();
                else buttonText = value;

                currentTextScrollRotation = buttonText;
                if (textHeight == -1) TextHeight = -1;

                this.Invalidate();
            }
        }
        protected string buttonText = "LCARS";

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
        /// Sets the visibility of the text
        /// </summary>
        public bool TextVisible
        {
            get { return textVisible; }
            set
            {
                textVisible = value;
                this.Invalidate();
            }
        }
        protected bool textVisible = true;

        /// <summary>
        /// Location of label used to display text
        /// </summary>
        [Browsable(false), EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Point TextLocation
        {
            get { return textLocation; }
            set
            {
                textLocation = value;
                this.Invalidate();
            }
        }
        protected Point textLocation = new Point(0, 0);

        /// <summary>
        /// Size of label used to display text
        /// </summary>
        /// <remarks>This label does not auto-size, and should be handled accordingly.</remarks>
        [Browsable(false), EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                this.Invalidate();
            }
        }
        protected Size textSize;

        /// <summary>
        /// Height of the control's text
        /// </summary>
        /// <remarks>
        /// For the <see cref="LCARS.Controls.TextButton">Text Button</see>, this directly sets the height of the control.
        /// </remarks>
        public virtual int TextHeight
        {
            get { return textHeight; }
            set
            {
                textHeight = value;
                if (textHeight == -1)
                {
                    if (!string.IsNullOrEmpty(buttonText))
                    {
                        SizeF mysize = new SizeF();
                        int i = 1;
                        Graphics g = Graphics.FromImage(new Bitmap(10, 10));
                        mysize = g.MeasureString(buttonText, new Font("LCARS", i, FontStyle.Regular, GraphicsUnit.Point));

                        while (!(mysize.Width >= this.Width - 8 | mysize.Height >= this.Height))
                        {
                            i += 1;
                            mysize = g.MeasureString(buttonText, new Font("LCARS", i, FontStyle.Regular, GraphicsUnit.Point));
                        }
                        if (i < 2)
                        {
                            i = 2;
                        }
                        font = new Font("LCARS", i - 1, FontStyle.Regular, GraphicsUnit.Point);
                        textHeight = -1;
                    }
                }
                else
                {
                    font = new Font("LCARS", textHeight, FontStyle.Regular, GraphicsUnit.Point);
                }
                this.Invalidate();
            }
        }
        protected int textHeight = 14;

        /// <summary>
        /// Lit property of the control
        /// </summary>
        /// <remarks>
        /// A control that is not lit has been dimmed by applying an alpha layer. This can be used to show a function that is availiable,
        /// but turned off. An offline function should use the offline function color.
        /// </remarks>
        public virtual bool IsLit
        {
            get { return isLit; }
            set
            {
                isLit = value;
                this.Invalidate();
            }
        }
        bool isLit = true;

        /// <summary>
        /// The alert status of the control.
        /// </summary>
        public LcarsAlert AlertState
        {
            get { return alertState; }
            set
            {
                alertState = value;
                DrawAllButtons();
            }
        }
        protected LcarsAlert alertState = LcarsAlert.Normal;

        /// <summary>
        /// The color to display if RedAlert is set to Custom
        /// </summary>
        /// <remarks>
        /// Setting this property will not result in any change in the display unless
        /// the RedAlert property is set to Custom.
        /// </remarks>
        public Color CustomAlertColor
        {
            get { return customAlertColor; }
            set
            {
                customAlertColor = value;
                DrawAllButtons();
            }
        }
        protected Color customAlertColor;

        /// <summary>
        /// Sets whether the control will beep when clicked.
        /// </summary>
        /// <remarks>
        /// This property should be set on application startup to match the global setting. This must be done manually.
        /// </remarks>
        public virtual bool DoesBeep
        {
            get { return doesBeep; }
            set { doesBeep = value; }
        }
        private bool doesBeep = false;

        /// <summary>
        /// LcarsColorManager associated with this control.
        /// </summary>
        /// <remarks>
        /// If you need to change the color of a control to a non-predefined color, this has the tools to do it.
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null) colorManager.ColorsUpdated -= ColorsUpdated;

                colorManager = value;
                if (colorManager != null) colorManager.ColorsUpdated += ColorsUpdated;
                DrawAllButtons();
            }
        }
        private LcarsColorManager colorManager = new LcarsColorManager();

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
        private LcarsColorFunction colorFunction = LcarsColorFunction.MiscFunction;
    }
}
