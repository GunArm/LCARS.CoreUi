using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using LCARS.CoreUi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LCARS.CoreUi.UiElements.Controls
{
    /// <summary>
    /// Draws windowless controls and passes events to them.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Designer(typeof(WindowlessDesigner))]
    public class WindowlessContainer : Control, IColorable, IBeeping
    {
        //Global Variables
        protected List<ILightweightControl> myList = new List<ILightweightControl>();
        Point oldMouseMovePoint;
        Point oldMouseDownPoint;

        //Events
        /// <summary>
        /// Raised when a windowless control has been added.
        /// </summary>
        public event EventHandler LightweightControlAdded;

        /// <summary>
        /// Adds a lightweight control to the current instance
        /// </summary>
        /// <param name="item">The lightweight control to add</param>
        /// <remarks>
        /// The lightweight control's parent property will be set to the current instance for the 
        /// purposes of easier multithreading.
        /// </remarks>
        public void Add(ILightweightControl item)
        {
            item.SetParent(this);
            myList.Add(item);
            item.Update += DrawButton;
            LightweightControlAdded?.Invoke(this, new EventArgs());
            Invalidate();
        }
        /// <summary>
        /// Paints the windowless container and all visible windowless controls
        /// </summary>
        /// <param name="e">Standard paint event args</param>
        /// <remarks>
        /// Controls with their <see cref="ILightweightControl.HoldDraw">HoldDraw
        /// </see> property set will not be drawn. Controls will be drawn in the order they were added,
        /// resulting in the last-added control having the highest z-order.
        /// </remarks>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = CreateGraphics();
            g.Clear(Color.Black);
            for (int i = 0; i <= myList.Count - 1; i++)
            {
                if (!myList[i].HoldDraw)
                    g.DrawImage(myList[i].GetBitmap(), myList[i].Bounds);
            }
        }

        private void DrawButton(object sender, EventArgs e)
        {
            try
            {
                var ilwcSender = (ILightweightControl)sender;
                Graphics g = CreateGraphics();
                g.DrawImage(ilwcSender.GetBitmap(), new Point(ilwcSender.Bounds.Left, ilwcSender.Bounds.Top));
                g.Dispose();
            }
            catch (InvalidOperationException ex)
            {
                //The control tried to update itself from a timer at the same time.
            }
        }
        /// <summary>
        /// Clears all current controls from the control
        /// </summary>
        public void Clear()
        {
            foreach (ILightweightControl mybutton in myList)
            {
                mybutton.Update -= DrawButton;
            }
            myList.Clear();
            CreateGraphics().Clear(Color.Black);
        }
        //Passes MouseDown events to the child controls
        private void Me_MouseDown(object sender, EventArgs e)
        {
            Point localPoint = PointToClient(Cursor.Position);
            oldMouseDownPoint = localPoint;
            for (int i = myList.Count - 1; i >= 0; i += -1)
            {
                if (myList[i].Bounds.Contains(localPoint) & myList[i].HoldDraw == false)
                {
                    myList[i].DoEvent(LightweightEvents.MouseDown);
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }
        //Passes MouseUp and click events to the child controls if applicable
        private void Me_MouseUp(object sender, EventArgs e)
        {
            Point localPoint = PointToClient(Cursor.Position);
            for (int i = myList.Count - 1; i >= 0; i += -1)
            {
                if (myList[i].Bounds.Contains(localPoint) & myList[i].HoldDraw == false)
                {
                    //If the mouse is still where it was, do a click event too.
                    if (localPoint == oldMouseDownPoint)
                    {
                        myList[i].DoClick();
                    }
                    try
                    {
                        myList[i].DoEvent(LightweightEvents.MouseUp);
                    }
                    catch (Exception ex)
                    {
                        //Click modified the collection
                    }
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }
        //Passes MouseOver events to the child controls as MouseMove, MouseEnter, and MouseLeave events
        private void Me_MouseOver(object sender, EventArgs e)
        {
            Point localPoint = PointToClient(Cursor.Position);
            bool foundTop = false;
            //Top level control found
            for (int i = myList.Count - 1; i >= 0; i += -1)
            {
                if (!myList[i].HoldDraw)
                {
                    if (myList[i].Bounds.Contains(localPoint) & !foundTop)
                    {
                        if (myList[i].Bounds.Contains(oldMouseMovePoint))
                        {
                            myList[i].DoEvent(LightweightEvents.MouseMove);
                        }
                        else
                        {
                            myList[i].DoEvent(LightweightEvents.MouseEnter);
                        }
                        foundTop = true;
                    }
                    else
                    {
                        if (myList[i].Bounds.Contains(oldMouseMovePoint))
                        {
                            myList[i].DoEvent(LightweightEvents.MouseLeave);
                        }
                    }
                }
            }
            oldMouseMovePoint = localPoint;
        }

        private void Me_MouseLeave(object sender, EventArgs e)
        {
            for (int i = myList.Count - 1; i >= 0; i += -1)
            {
                if (!myList[i].HoldDraw)
                {
                    if (myList[i].Bounds.Contains(oldMouseMovePoint))
                    {
                        myList[i].DoEvent(LightweightEvents.MouseLeave);
                    }
                }
            }
            oldMouseMovePoint = default(Point);
        }

        //Passes DoubleClick events to the child controls
        private void Me_DoubleClick(object sender, EventArgs e)
        {
            Point localPoint = PointToClient(Cursor.Position);
            for (int i = myList.Count - 1; i >= 0; i += -1)
            {
                if (!myList[i].HoldDraw & myList[i].Bounds.Contains(localPoint))
                {
                    myList[i].DoEvent(LightweightEvents.DoubleClick);
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }

        //Updates colors of child controls
        private void ReloadColors(object sender, EventArgs e)
        {
            IColorable temp = null;
            foreach (ILightweightControl mycontrol in myList)
            {
                temp = mycontrol as IColorable;
                if ((temp != null))
                {
                    temp.ColorManager.ReloadColors();
                }
            }
        }

        #region " Properties "
        /// <summary>
        /// Gives access to the windowless controls already added to the control.
        /// </summary>
        /// <param name="index">Zero-based index of the control to access</param>
        /// <value>The control to assign at that point</value>
        /// <returns>The current control at that point</returns>
        /// <remarks>
        /// This will allow direct access as in:
        /// <code>
        /// myGrid(6).Text = "The quick brown fox jumped over the lazy dogs."
        /// </code>
        /// However, this property can also be used to replace a control at a given point. Under no circumstances should this be
        /// used to delete a control.
        /// </remarks>
        public ILightweightControl this[int index]
        {
            get { return myList[index]; }
            set
            {
                myList[index] = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Returns the number of windowless controls in this instance
        /// </summary>
        public int Count
        {
            get { return myList.Count; }
        }

        /// <summary>
        /// Allows access to the LCARS color class used by this control
        /// </summary>
        /// <remarks>
        /// Setting this property will cause all subcontrols to reload their colors
        /// </remarks>
        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null)
                {
                    colorManager.ColorsUpdated -= ReloadColors;
                }
                colorManager = value;
                if (colorManager != null)
                {
                    colorManager.ColorsUpdated += ReloadColors;
                }
                ReloadColors(this, null);
            }
        }
        private LcarsColorManager colorManager;

        /// <summary>
        /// Allows beeping to be set for all current windowless controls
        /// </summary>
        public bool DoesBeep
        {
            get { return doesBeep; }
            set
            {
                doesBeep = value;
                IBeeping temp = null;
                foreach (ILightweightControl mycontrol in myList)
                {
                    temp = mycontrol as IBeeping;
                    if (temp != null) temp.DoesBeep = value;
                }
            }
        }
        bool doesBeep = bool.Parse(new SettingsStore("LCARS").Load("Application", "ButtonBeep", "TRUE"));

        public WindowlessContainer()
        {
            DoubleClick += Me_DoubleClick;
            MouseLeave += Me_MouseLeave;
            MouseMove += Me_MouseOver;
            MouseUp += Me_MouseUp;
            MouseDown += Me_MouseDown;
            ColorManager = new LcarsColorManager();
        }
        #endregion
    }

    /// <summary>
    /// Designer for <see cref="WindowlessContainer">Windowless Container</see>
    /// </summary>
    public class WindowlessDesigner : ControlDesigner
    {

        /// <summary>
        /// Paints a cyan border around the control to make it more visible.
        /// </summary>
        /// <param name="pe">PaintEventArgs used for drawing</param>
        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            pe.Graphics.DrawRectangle(Pens.Cyan, 0, 0, Control.Size.Width - 1, Control.Size.Height - 1);
            base.OnPaintAdornments(pe);
        }
    }
}
