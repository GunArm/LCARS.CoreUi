using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Controls
{
    /// <summary>
    /// Aligns a set of lightweight controls to a grid, and passes events to them.
    /// </summary>
    /// <remarks>
    /// The ButtonGrid control is designed to allow the display of large quantities of data without using a separate control for
    /// each button. The upper limit for using windowed controls is about 500 before the program runs out of handles, but with 
    /// windowless controls, the upper limit is defined by the computer's memory capacity. For obvious reasons, don't try to add
    /// more controls than the maximum value of an integer.
    /// <br />
    /// Controls are stored internally in a list(of T) object initialized to store controls as an ILightweightControl interface.
    /// This provides bare-minimum access to the control for redrawing and passing of events. In a future version, a generic ButtonGrid
    /// may be provided to allow for more specific typing, but such a control would not be able to be added to a form in the designer.
    /// </remarks>
    public class ButtonGrid : WindowlessContainer
    {
        /// <summary>
        /// Direction to add controls in.
        /// </summary>
        public enum ControlDirection
        {
            /// <summary>
            /// Add controls vertically in columns
            /// </summary>
            Vertical = 0,
            /// <summary>
            /// Add controls horizontally in rows.
            /// </summary>
            Horizontal = 1
        }

        int componentWidth = 150;
        int componentHeight = 70;
        TrackBar myScroll;

        int curPage = 0;
        int pageSize = 1;

        Size oldSize;
        Point oldMousePoint;

        /// <summary>
        /// Returns a new instance of the ButtonGrid control
        /// </summary>
        /// <remarks></remarks>
        public ButtonGrid() : base()
        {
            Resize += Me_Resize;
            LightweightControlAdded += Me_ControlAdded;
            myScroll = new TrackBar();
            MinimumSize = new Size(50, 50);
            myScroll.Width = Width;
            myScroll.Height = 30;
            myScroll.Left = 0;
            myScroll.Top = Height - myScroll.Height;
            myScroll.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            myScroll.Scroll += MyScroll_Scroll;
            Controls.Add(myScroll);
        }

        private void Me_ControlAdded(object sender, System.EventArgs e)
        {
            RearrangeButtons();
        }

        private void Me_Resize(object sender, EventArgs e)
        {
            Invalidate();
            //Ensures that the control is repainted
        }


        //Handles this control's paint event, and resizes controls to match.
        protected override void OnPaint(PaintEventArgs e)
        {
            //Needed to rearrange everything
            if (oldSize != Size)
            {
                //If the control is resized, this fires before the Resize event does.
                RearrangeButtons();
                oldSize = Size;
            }
            base.OnPaint(e);
        }

        //Rearranges all buttons to align to the grid and redraws them. Does not trigger a paint event.
        private void RearrangeButtons()
        {
            if (myList.Count > 0)
            {
                int columnNumber = Width / (componentWidth + myPadding);
                if (columnNumber == 0)
                {
                    columnNumber = 1;
                }
                int mywidth = Convert.ToInt32(Width / columnNumber - myPadding);
                int rowNumber = (Height - myScroll.Height) / (componentHeight + myPadding);
                if (rowNumber == 0)
                {
                    rowNumber = 1;
                }

                pageSize = columnNumber * rowNumber;
                int x = 0;
                int y = 0;
                int i = 0;
                Rectangle newBounds;
                int pages = 0;
                curPage = 0;
                myScroll.CurrentPage = 0;
                if (direction == ControlDirection.Vertical)
                {
                    while (i < myList.Count)
                    {
                        while (y < rowNumber & i < myList.Count)
                        {
                            newBounds = new Rectangle((x % columnNumber) * (mywidth + myPadding), y * (componentHeight + myPadding), mywidth, componentHeight);
                            if (x > columnNumber - 1)
                            {
                                myList[i].HoldDraw = true;
                            }
                            myList[i].Bounds = newBounds;
                            y += 1;
                            i += 1;
                        }
                        y = 0;
                        x += 1;
                    }
                    pages = x / columnNumber;
                    if (x % columnNumber > 0)
                    {
                        pages += 1;
                    }
                }
                else
                {
                    while (i < myList.Count)
                    {
                        while (x < columnNumber & i < myList.Count)
                        {
                            newBounds = new Rectangle((x) * (mywidth + myPadding), (y % rowNumber) * (componentHeight + myPadding), mywidth, componentHeight);
                            if (y > rowNumber - 1)
                            {
                                myList[i].HoldDraw = true;
                            }
                            myList[i].Bounds = newBounds;
                            x += 1;
                            i += 1;
                        }
                        x = 0;
                        y += 1;
                    }
                    pages = y / rowNumber;
                    if (y % rowNumber > 0)
                    {
                        pages += 1;
                    }
                }
                myScroll.Pages = pages;
            }
        }

        //Handles the scroll events of the trackbar on this control
        private void MyScroll_Scroll(object sender, EventArgs e)
        {
            CreateGraphics().Clear(BackColor);
            curPage = myScroll.CurrentPage;

            for (int i = 0; i <= myList.Count - 1; i++)
            {
                if (i >= pageSize * (curPage) & i < pageSize * (curPage + 1))
                {
                    myList[i].HoldDraw = false;
                }
                else
                {
                    myList[i].HoldDraw = true;
                }
            }
        }

        #region " Properties "
        /// <summary>
        /// Sets the display size of the controls
        /// </summary>
        /// <value>New size for the controls</value>
        /// <returns>Current size of the controls</returns>
        /// <remarks>
        /// If this is made user-configurable, be sure to test for absurd values. The control drawing may fail
        /// at very small sizes. Also, beware of text clipping.<br />
        /// Important note: The width set here is a minimum. The controls will by dynamically sized to take up the full width of the
        /// grid. The height, however, will be used directly.
        /// </remarks>
        public Size ControlSize
        {
            get { return new Size(componentWidth, componentHeight); }
            set
            {
                componentWidth = value.Width;
                componentHeight = value.Height;
                MinimumSize = new Size(value.Width + myPadding, value.Height + myPadding);
                RearrangeButtons();
                Invalidate();
            }
        }

        /// <summary>
        /// The amount of padding between controls.
        /// </summary>
        /// <value>New padding value</value>
        /// <returns>Current padding value</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// An IndexOutOfRangeException will be thrown if an attempt is made to access a control index that has not been assigned.
        /// </exception>
        /// <remarks>
        /// This padding value is approximate, but should be exact except under very rare conditions. On occasion, the rounding used
        /// to position controls may vary it by one pixel.<br />
        /// Note that setting this to its current value will not trigger a redraw.
        /// </remarks>
        public int ControlPadding
        {
            get { return myPadding; }
            set
            {
                if (value == myPadding) return;
                myPadding = value;
                RearrangeButtons();
                Invalidate();
            }
        }
        int myPadding = 5;

        /// <summary>
        /// Direction to add controls in
        /// </summary>
        /// <value>New direction</value>
        /// <returns>Old direction</returns>
        /// <remarks>
        /// This cannot be set for individual controls; all controls will be rearranged to follow the new direction.
        /// <br />
        /// Setting this property to its current value will not trigger a redraw.
        /// </remarks>
        public ControlDirection ControlAddingDirection
        {
            get { return direction; }
            set
            {
                if (value == direction) return;
                direction = value;
                RearrangeButtons();
                Invalidate();
            }
        }
        ControlDirection direction = ControlDirection.Vertical;

        /// <summary>
        /// Returns current page displayed
        /// </summary>
        public int CurrentPage
        {
            get { return myScroll.CurrentPage; }
            set { myScroll.CurrentPage = value; }
        }

        /// <summary>
        /// Returns a count of pages that can be displayed
        /// </summary>
        /// <remarks>
        /// The PageCount will always be at least one.
        /// </remarks>
        public int PageCount
        {
            get { return myScroll.Pages; }
        }
        #endregion
    }

}
