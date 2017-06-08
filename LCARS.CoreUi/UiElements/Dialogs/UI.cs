using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Dialogs
{
    /// <summary>
    /// Contains methods and functions that replicate various dialogs.
    /// </summary>
    /// <remarks>
    /// To use these with minimal effor add this line to the top of your file:
    /// <c>Imports LCARS.UI</c>
    /// To switch back to standard Windows style dialogs, comment it out.
    /// </remarks>
    public class UI
    {

        /// <summary>
        /// Displays an LCARS-style message box
        /// </summary>
        /// <param name="prompt">Prompt text to display</param>
        /// <param name="buttons">Style of message box</param>
        /// <param name="title">Title to display</param>
        /// <returns>Button clicked</returns>
        /// <remarks>
        /// This is designed to be a full replacement for the standard message box. To convert
        /// all message boxes produced by a file, add the line: <c>import LCARS.UI</c> to the top
        /// of the file
        /// </remarks>
        public static MsgBoxResult MsgBox(object prompt, MsgBoxStyle buttons = MsgBoxStyle.OkOnly, string title = "LCARS")
        {
            LcarsMessageBoxForm myform = new LcarsMessageBoxForm(prompt, buttons, title);
            myform.ShowDialog();
            MsgBoxResult result = myform.buttonclick;
            myform.Dispose();
            return result;
        }

        /// <summary>
        /// Displays an LCARS-style input box
        /// </summary>
        /// <param name="prompt">Prompt to display</param>
        /// <param name="title">Title to display</param>
        /// <param name="defaultResponse">Default response to fill in</param>
        /// <param name="posX">X-coordinate. Defaults to center screen</param>
        /// <param name="posY">Y-coordinate. Defaults to center screen</param>
        /// <returns>Text entered into input box</returns>
        /// <remarks>
        /// This is designed to be a full replacement for the standard input box. To convert
        /// all input boxes produced by a file, add the line: <c>import LCARS.UI</c> to the top
        /// of the file
        ///</remarks>
        public static string InputBox(string prompt, string title = "LCARS", string defaultResponse = "", int posX = -1, int posY = -1)
        {
            LCARSInputBoxForm myform = new LCARSInputBoxForm(prompt, title, defaultResponse, posX, posY);
            myform.ShowDialog();
            string result = myform.txtInput.Text;
            myform.Dispose();
            return result;
        }

    }
}