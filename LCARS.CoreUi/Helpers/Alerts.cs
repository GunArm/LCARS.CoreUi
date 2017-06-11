using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LCARS.CoreUi.Helpers
{
    /// <summary>
    /// Contains methods for registering and using alerts.
    /// </summary>
    /// <remarks></remarks>
    public static class Alerts
    {
        private static SettingsStore settings = new SettingsStore("LCARS");
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cdData;
            public IntPtr lpData;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        const int WM_COPYDATA = 0x4a;
        /// <summary>
        /// Registers a new alert with LCARS, or returns the ID of the alert with the same name.
        /// </summary>
        /// <param name="Name">The name your code will reference the alert by</param>
        /// <param name="AlertColor">Color the alert will use. Ignored if alert by the same name already exists.</param>
        /// <param name="SoundPath">Sound that will be played for the alert. Ignored if alert by the same name already exists.</param>
        /// <returns>Integer representing the alert ID</returns>
        /// <remarks>
        /// Unless calling a red or yellow alert, use this to register the alert before activating it. The cost to do so is very slight,
        /// and it will avoid any problems with nonexistant alerts being activated.
        /// </remarks>
        public static int RegisterAlert(string Name, Color AlertColor, string SoundPath = "")
        {
            int result = -1;
            string[,] mysettings = settings.LoadAll("Alerts");

            for (int i = 0; i <= (mysettings.GetUpperBound(0)); i++)
            {
                if (Name == mysettings[i, 1].Substring(0, mysettings[i, 1].IndexOf("|")))
                {
                    result = Convert.ToInt32(mysettings[i, 0]);
                }
            }

            if (result == -1)
            {
                int id = GetNewID();
                settings.Save("Alerts", id.ToString(), Name + "|" + AlertColor.ToHex() + "|" + SoundPath);
                return id;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Registers a new alert with LCARS, or returns the ID of the alert with the same name.
        /// </summary>
        /// <param name="Name">The name your code will reference the alert by</param>
        /// <param name="AlertColor">Color the alert will use as HTML-style color code. Ignored if alert by the same name already exists.</param>
        /// <param name="SoundPath">Sound that will be played for the alert. Ignored if alert by the same name already exists.</param>
        /// <returns>Integer representing the alert ID</returns>
        /// <remarks>
        /// Unless calling a red or yellow alert, use this to register the alert before activating it. The cost to do so is very slight,
        /// and it will avoid any problems with nonexistant alerts being activated.<br />
        /// This variant allows the color to be specified as an HTML-style color code.
        /// </remarks>
        public static int RegisterAlert(string Name, string AlertColor, string SoundPath = "")
        {
            return RegisterAlert(Name, ColorTranslator.FromHtml(AlertColor), SoundPath);
        }

        /// <summary>
        /// Returns the alert ID of a preexisting alert
        /// </summary>
        /// <param name="Name">Name of the alert to look up</param>
        /// <returns>Alert ID as an integer</returns>
        /// <exception cref="Exception">
        /// Will throw an exception if the alert name does not exist.
        /// </exception>
        /// <remarks>
        /// This is a shortcut for checking that your alert's ID hasn't changed since you registered it. There can be some time
        /// savings over just using RegisterAlert, but RegisterAlert will register the alert if it does not already exist.
        /// </remarks>
        public static int GetAlertID(string Name)
        {
            int result = -1;
            string[,] mysettings = settings.LoadAll("Alerts");
            for (int i = 0; i <= (mysettings.GetUpperBound(0)); i++)
            {
                if (Name == mysettings[i, 1].Substring(0, mysettings[i, 1].IndexOf("|")))
                {
                    result = Convert.ToInt32(mysettings[i, 0]);
                }
            }
            if (result == -1)
            {
                throw new Exception("Given alert name does not exist");
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Sends a message to LCARS to activate the specified alert.
        /// </summary>
        /// <param name="ID">ID of alert to call</param>
        /// <param name="hwnd">Handle of calling object</param>
        /// <remarks>Use this if you already have the alert ID.</remarks>
        public static void ActivateAlert(int ID, IntPtr hwnd)
        {
            COPYDATASTRUCT myData = new COPYDATASTRUCT();
            myData.dwData = new IntPtr(11);
            myData.cdData = ID;
            IntPtr MyCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(COPYDATASTRUCT)));
            Marshal.StructureToPtr(myData, MyCopyData, true);

            SendMessage(
                new IntPtr(Convert.ToInt32(settings.Load("Application", "MainWindowHandle", "0"))),
                WM_COPYDATA,
                hwnd,
                MyCopyData);
            Marshal.FreeCoTaskMem(MyCopyData);
        }

        /// <summary>
        /// Sends a message to LCARS to activate the specified alert.
        /// </summary>
        /// <param name="Name">Name of the alert to call</param>
        /// <param name="hwnd">Handle of the calling object</param>
        /// <exception cref="Exception">Throws an exception if the alert does not already exist.</exception>
        /// <remarks>
        /// This sub is a wrapper for the overloaded sub that requires an ID. Use this for convenience, but use the other if you
        /// have the alert ID already. This is useful if you only need to call the Red or Yellow alerts.
        /// </remarks>
        public static void ActivateAlert(string Name, System.IntPtr hwnd)
        {
            ActivateAlert(GetAlertID(Name), hwnd);
        }

        /// <summary>
        /// Cancels the current alert.
        /// </summary>
        /// <param name="hwnd">Handle of calling object.</param>
        /// <remarks></remarks>
        public static void DeactivateAlert(System.IntPtr hwnd)
        {
            COPYDATASTRUCT myData = new COPYDATASTRUCT();
            myData.dwData = new IntPtr(7);
            IntPtr MyCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(COPYDATASTRUCT)));
            Marshal.StructureToPtr(myData, MyCopyData, true);

            SendMessage(
                new IntPtr(Convert.ToInt32(settings.Load("Application", "MainWindowHandle", "0"))),
                WM_COPYDATA,
                hwnd,
                MyCopyData);
            Marshal.FreeCoTaskMem(MyCopyData);
        }

        /// <summary>
        /// Gets a list of all valid alert names
        /// </summary>
        /// <returns>A list of all current valid alert names.</returns>
        /// <remarks>
        /// Use this if you need a list of all valid alert names. This is used internally by the settings program and LCARSmain.</remarks>
        public static List<string> GetAllAlertNames()
        {
            string[,] mySettings = settings.LoadAll("Alerts");
            List<string> myAlerts = new List<string>();
            for (int i = 0; i <= mySettings.GetUpperBound(0); i++)
            {
                myAlerts.Add(mySettings[i, 1].Substring(0, mySettings[i, 1].IndexOf("|")));
            }
            return myAlerts;
        }

        /// <summary>
        /// Gets the color of an alert by name
        /// </summary>
        /// <param name="alertName">Name of alert to look up</param>
        /// <returns>Color that will be displayed by the alert</returns>
        /// <remarks></remarks>
        public static Color GetAlertColor(string alertName)
        {
            int id = GetAlertID(alertName);
            return GetAlertColor(id);
        }

        /// <summary>
        /// Returns the color of the alert given.
        /// </summary>
        /// <param name="alertID">Alert ID to look up</param>
        /// <returns>Color of specified alert</returns>
        /// <remarks></remarks>
        public static Color GetAlertColor(int alertID)
        {
            string alertstring = settings.Load("Alerts", alertID.ToString(), "");
            int startIndex = alertstring.IndexOf("|");
            return ColorTranslator.FromHtml(alertstring.Substring(startIndex + 1, 7));
        }

        /// <summary>
        /// Returns an unoccupied alert ID.
        /// </summary>
        /// <returns>ID found</returns>
        /// <remarks>Do not make the assumption that alert IDs are consecutive. Use this function if you need an unoccupied one.</remarks>
        public static int GetNewID()
        {
            int i = 1;
            string alertString = "";
            while (!(alertString == "Blank"))
            {
                i += 1;
                alertString = settings.Load("Alerts", i.ToString(), "Blank");
            }
            return i;
        }

        /// <summary>
        /// Sends a message to x32 that there have been changes to the alerts.
        /// </summary>
        /// <param name="hwnd">Handle of caller</param>
        /// <remarks>Call this whenever you have made changes, or those changes will not be noticed until x32 has restarted.</remarks>
        public static void RefreshAlerts(IntPtr hwnd)
        {
            COPYDATASTRUCT myData = new COPYDATASTRUCT();
            myData.dwData = new IntPtr(12);
            IntPtr MyCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(COPYDATASTRUCT)));
            Marshal.StructureToPtr(myData, MyCopyData, true);

            SendMessage(
                new IntPtr(Convert.ToInt32(settings.Load("Application", "MainWindowHandle", "0"))),
                WM_COPYDATA,
                hwnd,
                MyCopyData);
            Marshal.FreeCoTaskMem(MyCopyData);
        }

        /// <summary>
        /// Deletes an alert
        /// </summary>
        /// <param name="id">ID of alert to delete</param>
        /// <exception cref="System.Exception">Will raise generic exception if alert is system-defined or if alert does not exist.</exception>
        /// <remarks>Use with caution, particularly if deleting someone else's alert</remarks>
        public static void DeleteAlert(int id)
        {
            if (id > 1)
            {
                settings.TryDelete("Alerts", id.ToString());
            }
            else
            {
                throw new Exception("Cannot delete a system-defined alert.");
            }
        }
    }
}
