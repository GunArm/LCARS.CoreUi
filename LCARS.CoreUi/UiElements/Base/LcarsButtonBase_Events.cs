using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Base
{
    public partial class LcarsButtonBase
    {
        /// <summary>
        /// Raised when the button is clicked
        /// </summary>
        public new event EventHandler Click;

        /// <summary>
        /// Raised when the button is double-clicked
        /// </summary>
        public new event EventHandler DoubleClick;

        /// <summary>
        /// Raised when a mouse button is depressed while the mouse is over the button
        /// </summary>
        /// 
        public new event MouseEventHandler MouseDown;

        /// <summary>
        /// Raised when a mouse button is released while the mouse is over the button
        /// </summary>
        public new event MouseEventHandler MouseUp;

        /// <summary>
        /// Raised when the mouse moves over the button
        /// </summary>
        public new event MouseEventHandler MouseMove;

        private void InitEvents()
        {
            base.Resize += Button_Resize;  // set before InitializeComponent() or designer draw will be buggy

            // base event forwarding handlers
            base.Click += Base_Click;
            base.MouseDown += Base_MouseDown;
            base.MouseMove += Base_MouseMove;
            base.MouseUp += Base_MouseUp;
            base.DoubleClick += Base_DoubleClick;

            MouseDown += RedrawMouseDown;
            MouseUp += RedrawMouseUp;

            Click += DoButtonActions;
            DoubleClick += DoButtonActions;

            MouseEnter += StartTextScroll;
            textScrollTimer.Tick += TextScrollTimer_Tick;
            MouseLeave += StopTextScroll;
        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (textHeight == -1)
            {
                TextHeight = -1;
                //resize the text
            }
            DrawAllButtons();
        }

        private void RedrawMouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            DrawAllButtons();
        }

        private void RedrawMouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
            DrawAllButtons();
        }

        private void DoButtonActions(object sender, EventArgs e)
        {
            if (canClick) DoButtonDownActions();
        }

        private void StartTextScroll(object sender, EventArgs e)
        {
            if (this.CreateGraphics().MeasureString(buttonText, font).Width > TextSize.Width)
            {
                textScrollTimer.Interval = 200;
                oAlign = textAlign;
                if ((int)textAlign <= 16)
                {
                    //bottom aligned
                    textAlign = ContentAlignment.TopLeft;
                }
                else if ((int)textAlign <= 256)
                {
                    //middle aligned
                    textAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    //top aligned
                    textAlign = ContentAlignment.BottomLeft;
                }
                currentTextScrollRotation = buttonText + "              ";
                textScrollTimer.Enabled = true;
            }
        }

        private string currentTextScrollRotation = "";
        private void TextScrollTimer_Tick(object sender, EventArgs e)
        {
            currentTextScrollRotation = currentTextScrollRotation.Substring(1) + currentTextScrollRotation.Substring(0, 1);
            Invalidate();
        }

        private void StopTextScroll(object sener, EventArgs e)
        {
            if (textScrollTimer.Enabled)
            {
                textScrollTimer.Enabled = false;

                if (oAlign > 0)
                {
                    textAlign = oAlign;
                }

                currentTextScrollRotation = buttonText;
                Invalidate();
            }
        }

        private void ColorsUpdated(object sender, EventArgs e)
        {
            DrawAllButtons();
        }

        // base event forwarding handlers
        private void Base_Click(object sender, EventArgs e)
        {
            if (canClick) Click?.Invoke(this, e);
        }
        private void Base_DoubleClick(object sender, EventArgs e)
        {
            if (canClick) DoubleClick?.Invoke(this, e);
        }
        private void Base_MouseDown(object sender, MouseEventArgs e)
        {
            if (canClick) MouseDown?.Invoke(this, e);
        }
        private void Base_MouseMove(object sender, MouseEventArgs e)
        {
            if (canClick) MouseMove?.Invoke(this, e);
        }
        private void Base_MouseUp(object sender, MouseEventArgs e)
        {
            if (canClick) MouseUp?.Invoke(this, e);
        }
    }
}
