using LCARS.CoreUi.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements
{
    /// <summary>
    /// A base Form class to handle common LCARS functions
    /// </summary>
    /// <remarks>
    /// Any form that inherits from this class will have default handlers for LCARS events, and be bound
    /// to the working area when maximized. This eliminates the previous requirement that LCARS apps 
    /// register their main window with LCARS to get position and size updates.
    /// 
    /// Supported events:
    ///  - Alerts initiated/ended
    ///  - Colors changed
    ///  - Beeping updated
    ///  - LCARS closing
    /// 
    /// Default handlers are supplied for color changing, beep updating, and LCARS closing.
    /// </remarks>
    public partial class LcarsForm : Form
    {
        #region " Windows API "

        private uint X32_MSG;
        #endregion

        #region " Events "
        /// <summary>
        /// Raised when an alert is initiated.
        /// </summary>
        /// <param name="AlertID">ID of the alert initiated</param>
        public event AlertInitiatedEventHandler AlertInitiated;
        public delegate void AlertInitiatedEventHandler(int AlertID);
        /// <summary>
        /// Raised when the current alert has ended
        /// </summary>
        /// <remarks>
        /// If an alert ends because another alert has replaced it, this event will not be raised, only a
        /// new <see cref="AlertInitiated">AlertInitiated</see> event
        /// </remarks>
        public event EventHandler AlertEnded;
        #endregion
        /// <summary>
        /// Handles LCARS messages and maximized bounds
        /// </summary>
        /// <param name="m">Window message</param>
        /// <remarks>
        /// Any messages of type WM_MINMAXINFO or Lcars.CoreUi will be handled, and not passed to the
        /// default handler.
        /// </remarks>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == X32_MSG)
            {

                m.Result = (IntPtr)1;
                switch ((int)m.LParam)
                {
                    case 2:
                        OnColorsChange();
                        break;
                    case 3:
                        OnBeepingUpdate(bool.Parse(new SettingsStore("LCARS").Load("Application", "ButtonBeep", "TRUE")));
                        break;
                    case 11:
                        OnAlertInitiated((int)m.WParam);
                        break;
                    case 7:
                        OnAlertEnded();
                        break;
                    case 13:
                        OnLCARSClosing();
                        break;
                }

            }
            // Not sure what this was trying to do. Commented out to allow FullScreen functionality.
            //else if (m.Msg == WM_MINMAXINFO)
            //{
            //    MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
            //    IntPtr monitor = MonitorFromWindow(Handle, MonitorOptions.DefaultToNearest);
            //    PointStruct pt0 = new PointStruct
            //    {
            //        X = 0,
            //        Y = 0
            //    };
            //    IntPtr primary = MonitorFromPoint(pt0, MonitorOptions.DefaultToPrimary);
            //    if (monitor != IntPtr.Zero && primary != IntPtr.Zero)
            //    {
            //        MONITORINFO minfo = default(MONITORINFO);
            //        MONITORINFO pminfo = default(MONITORINFO);
            //        minfo.Size = Marshal.SizeOf(minfo);
            //        pminfo.Size = Marshal.SizeOf(pminfo);
            //        if (GetMonitorInfo(monitor, ref minfo) && GetMonitorInfo(primary, ref pminfo))
            //        {
            //            // This looks wrong, but Windows assumes the coordinates are for the primary
            //            // monitor, and then adjusts them. We need to undo the adjustments so that
            //            // the result is what we actually want.

            //            // First, we account for the position change between the primary and actual monitors
            //            mmi.MaxPosition.X = minfo.WorkArea.Left - minfo.Monitor.Left;
            //            mmi.MaxPosition.Y = minfo.WorkArea.Top - minfo.Monitor.Top;

            //            // For some reason setting the max track size bypasses the size adjustment.
            //            mmi.MaxTrackSize.X = minfo.WorkArea.Right - minfo.WorkArea.Left;
            //            mmi.MaxTrackSize.Y = minfo.WorkArea.Bottom - minfo.WorkArea.Top;

            //            Marshal.StructureToPtr(mmi, m.LParam, true);
            //            m.Result = (IntPtr)1;
            //            return;
            //        }
            //    }
            //    m.Result = (IntPtr)0;
            //}
            else
            {
                base.WndProc(ref m);
            }
        }

        //Register Lcars.CoreUi
        protected override void OnLoad(EventArgs e)
        {
            X32_MSG = RegisterWindowMessageA("Lcars.CoreUi");
            base.OnLoad(e);
        }

        /// <summary>
        /// Called when LCARS updates the color scheme
        /// </summary>
        protected virtual void OnColorsChange()
        {
            UpdateColors(this);
        }

        /// <summary>
        /// Called when beeping setting is updated
        /// </summary>
        /// <param name="beep">New beeping setting</param>
        protected virtual void OnBeepingUpdate(bool beep)
        {
            SetBeeping(this, beep);
        }

        /// <summary>
        /// Called when an alert is initiated.
        /// </summary>
        /// <param name="alertID">ID of current alert</param>
        protected virtual void OnAlertInitiated(int alertID)
        {
            AlertInitiated?.Invoke(alertID);
        }

        /// <summary>
        /// Called when all alerts have ended
        /// </summary>
        protected virtual void OnAlertEnded()
        {
            AlertEnded?.Invoke(this, null);
        }

        /// <summary>
        /// Called when LCARS is closing. By default, will close this window.
        /// </summary>
        protected virtual void OnLCARSClosing()
        {
            Close();
        }
    }
}
