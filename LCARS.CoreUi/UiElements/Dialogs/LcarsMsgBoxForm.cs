using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using LCARS.CoreUi.UiElements.Controls;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Dialogs
{
    internal class LcarsMessageBoxForm : Form
    {
        public MsgBoxResult buttonclick;
        private MsgBoxStyle msgtype;
        private Point oloc;

        public LcarsMessageBoxForm(object prompt, MsgBoxStyle buttons, string title)
        {
            //ByVal buttons As LcarsColorFunction,
            //Dim buttonclick As String = "OK"
            LcarsColorFunction colorFunction = default(LcarsColorFunction);
            bool cancleEnabled = false;
            bool abortVisible = false;
            bool retryVisible = false;
            bool ignoreVisible = false;
            bool YesVis = false;
            bool NoVis = false;
            int okx = 183;
            int cx = 50;

            // enumeration splitter
            //buttons
            int[] buttonStyle = new int[5];
            for (MsgBoxStyle ii = 0; (int)ii <= 5; ii++)
            {
                if ((buttons & ii) != 0)
                {
                    buttonStyle[0] = (int)(buttons & ii);
                }
            }
            //style
            for (MsgBoxStyle ii = (MsgBoxStyle)16; (int)ii <= 64; ii += 16)
            {
                if ((buttons & ii) != 0)
                {
                    buttonStyle[1] = (int)(buttons & ii);
                }
            }
            //default button
            for (MsgBoxStyle ii = (MsgBoxStyle)256; (int)ii <= 768; ii += 256)
            {
                if ((buttons & ii) != 0)
                {
                    buttonStyle[2] = (int)(buttons & ii);
                }
            }
            //system modal
            if ((buttons & MsgBoxStyle.SystemModal) != 0)
            {
                buttonStyle[3] = (int)(buttons & MsgBoxStyle.SystemModal);
            }
            //Special args
            for (MsgBoxStyle ii = MsgBoxStyle.MsgBoxHelp; ii <= MsgBoxStyle.MsgBoxRtlReading; ii += (int)MsgBoxStyle.MsgBoxHelp)
            {
                if ((buttons & ii) != 0)
                {
                    buttonStyle[4] = (int)(buttons & ii);
                }
            }
            // end unumeration splitter

            switch (buttonStyle[0])
            {
                case 0:
                    //vbOkOnly
                    msgtype = MsgBoxStyle.OkOnly;
                    break;
                case 1:
                    //vbOkCancle
                    cancleEnabled = true;
                    msgtype = MsgBoxStyle.OkCancel;
                    break;
                case 2:
                    //vbAbortRetryIgnore
                    abortVisible = true;
                    retryVisible = true;
                    ignoreVisible = true;
                    cancleEnabled = true;
                    cx = 210;
                    okx = 115;
                    msgtype = MsgBoxStyle.AbortRetryIgnore;
                    break;
                case 3:
                    //vbYesNoCancel
                    cancleEnabled = true;
                    YesVis = true;
                    NoVis = true;
                    okx = 210;
                    cx = 115;
                    msgtype = MsgBoxStyle.YesNoCancel;
                    break;
                case 4:
                    //vbYesNo
                    YesVis = true;
                    NoVis = true;
                    okx = 210;
                    cx = 115;
                    msgtype = MsgBoxStyle.YesNo;
                    break;
                case 5:
                    //vbRetryCancle
                    retryVisible = true;
                    cancleEnabled = true;
                    msgtype = MsgBoxStyle.RetryCancel;
                    break;
            }
            switch (buttonStyle[1])
            {
                case 16:
                    //vbCritical
                    colorFunction = LcarsColorFunction.FunctionOffline;
                    // pass redAlert() to LCARSmain if exists.

                    try
                    {

                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case 32:
                    //vbQuestion
                    colorFunction = LcarsColorFunction.StaticBlue;
                    break;
                case 48:
                    //vbExclamation
                    colorFunction = LcarsColorFunction.StaticTan;
                    break;
                case 64:
                    //vbInformation
                    colorFunction = LcarsColorFunction.LCARSDisplayOnly;
                    break;
                default:
                    colorFunction = LcarsColorFunction.StaticTan;
                    break;
            }
            switch (buttonStyle[2])
            {
                case 256:
                    break;
                //vbDefaultButton2
                case 512:
                    break;
                //vbDefaultButton3
                case 768:
                    break;
                    //vbDefaultbutton4
            }
            switch (buttonStyle[4])
            {
                case 2:
                    break;
                //vbMsgBoxHelpButton
                case 3:
                    break;
                //vbMsgBoxSetForground
                case 4:
                    break;
                //vbMsgBoxRight
                case 5:
                    break;
                    //vbMsgBoxRtlReading
            }

            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(305, 200);
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;


            Elbow elbow1 = new Elbow();
            this.Controls.Add(elbow1);
            elbow1.Location = new Point(0, 0);
            elbow1.Size = new Size(80, 60);
            elbow1.ButtonHeight = 30;
            elbow1.ButtonWidth = 10;
            elbow1.ElbowStyle = Elbow.LcarsElbowStyle.UpperLeft;
            elbow1.ColorFunction = colorFunction;
            elbow1.ButtonText = "";

            Elbow elbow2 = new Elbow();
            this.Controls.Add(elbow2);
            elbow2.Location = new Point(0, 140);
            elbow2.Size = new Size(80, 60);
            elbow2.ButtonHeight = 15;
            elbow2.ButtonWidth = 10;
            elbow2.Clickable = false;
            elbow2.ElbowStyle = Elbow.LcarsElbowStyle.LowerLeft;
            elbow2.ColorFunction = colorFunction;
            elbow2.ButtonText = "";

            TextButton titleBar = new TextButton();
            this.Controls.Add(titleBar);
            titleBar.Location = new Point(50, 0);
            titleBar.Size = new Size(255, 30);
            titleBar.TextHeight = 31;
            titleBar.ColorFunction = colorFunction;
            titleBar.ButtonText = title;

            FlatButton vertBar = new FlatButton();
            this.Controls.Add(vertBar);
            vertBar.Location = new Point(0, 66);
            vertBar.Size = new Size(10, 68);
            vertBar.Clickable = false;
            vertBar.ColorFunction = colorFunction;
            vertBar.ButtonText = "";

            HalfPillButton bottomBar = new HalfPillButton();
            this.Controls.Add(bottomBar);
            bottomBar.Location = new Point(85, 185);
            bottomBar.Size = new Size(217, 15);
            bottomBar.Clickable = false;
            bottomBar.ColorFunction = colorFunction;
            bottomBar.ButtonText = "";

            //draw text
            RichTextBox errorTextBox = new RichTextBox();
            this.Controls.Add(errorTextBox);
            errorTextBox.Location = new Point(30, 36);
            errorTextBox.Size = new Size(243, 107);
            errorTextBox.BackColor = Color.Black;
            errorTextBox.BorderStyle = BorderStyle.None;
            errorTextBox.WordWrap = true;
            errorTextBox.Font = new Font("LCARS", 14, FontStyle.Regular);
            errorTextBox.ForeColor = Color.Orange;
            errorTextBox.BringToFront();
            errorTextBox.Text = prompt.ToString();
            errorTextBox.ReadOnly = true;

            // BUTTONS

            //draw OK/Yes/retry button
            StandardButton yesOkRetryButton = new StandardButton();
            this.Controls.Add(yesOkRetryButton);

            yesOkRetryButton.Location = new Point(okx, 149);
            yesOkRetryButton.Size = new Size(90, 30);
            yesOkRetryButton.ColorFunction = LcarsColorFunction.PrimaryFunction;
            yesOkRetryButton.Name = "yesOkRetryButton";
            if (YesVis)
            {
                yesOkRetryButton.ButtonText = "YES";
                yesOkRetryButton.Click += OnsbYerrClick;
            }
            else if (retryVisible)
            {
                yesOkRetryButton.ButtonText = "RETRY";
                yesOkRetryButton.Click += OnsbRerrClick;
            }
            else
            {
                yesOkRetryButton.ButtonText = "OK";
                yesOkRetryButton.Click += OnsbokerrClick;
            }
            yesOkRetryButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            yesOkRetryButton.BringToFront();

            StandardButton cancelIgnoreButton = new StandardButton();
            this.Controls.Add(cancelIgnoreButton);
            cancelIgnoreButton.Location = new Point(cx, 149);
            cancelIgnoreButton.Size = new Size(90, 30);
            cancelIgnoreButton.Name = "cancelIgnoreButton";
            if (cancleEnabled)
                cancelIgnoreButton.ColorFunction = LcarsColorFunction.CriticalFunction;
            else
                cancelIgnoreButton.ColorFunction = LcarsColorFunction.FunctionOffline;
            if (ignoreVisible)
            {
                cancelIgnoreButton.ButtonText = "IGNORE";
                cancelIgnoreButton.Click += OnsbIerrClick;
            }
            else
            {
                cancelIgnoreButton.ButtonText = "CANCEL";
                cancelIgnoreButton.Click += OnsbcerrClick;
            }
            cancelIgnoreButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            cancelIgnoreButton.Flash = !cancleEnabled;
            cancelIgnoreButton.Clickable = cancleEnabled;
            cancelIgnoreButton.BringToFront();


            //draw abort/no button
            StandardButton noAbortButton = new StandardButton();
            this.Controls.Add(noAbortButton);
            noAbortButton.Location = new Point(20, 149);
            noAbortButton.Size = new Size(90, 30);
            noAbortButton.ColorFunction = LcarsColorFunction.CriticalFunction;
            if (abortVisible)
            {
                noAbortButton.ButtonText = "ABORT";
                noAbortButton.Click += OnsbAerrClick;
            }
            else if (NoVis)
            {
                noAbortButton.ButtonText = "NO";
                noAbortButton.Click += OnsbNerrClick;
            }
            else
            {
                noAbortButton.Visible = false;
            }
            noAbortButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            noAbortButton.BringToFront();
            this.KeyPreview = true;
            this.KeyDown += Me_KeyDown;
            elbow1.MouseDown += tbTitle_MouseDown;
            elbow1.MouseMove += tbTitle_MouseMove;
            titleBar.MouseDown += tbTitle_MouseDown;
            titleBar.MouseMove += tbTitle_MouseMove;
            //Application.DoEvents()
        }

        private void OnsbokerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Ok;
            this.Close();
        }
        private void OnsbcerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Cancel;
            this.Close();
        }
        private void OnsbAerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Abort;
            this.Close();
        }
        private void OnsbRerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Retry;
            this.Close();
        }
        private void OnsbIerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Ignore;
            this.Close();
        }
        private void OnsbYerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.Yes;
            this.Close();
        }
        private void OnsbNerrClick(object sender, EventArgs e)
        {
            buttonclick = MsgBoxResult.No;
            this.Close();
        }
        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if ((msgtype == MsgBoxStyle.OkCancel | msgtype == MsgBoxStyle.AbortRetryIgnore | msgtype == MsgBoxStyle.YesNoCancel))
                    {
                        ((LcarsButtonBase)this.Controls.Find("cancelIgnoreButton", true)[0]).DoClick();
                    }
                    break;
                case Keys.Enter:
                    ((LcarsButtonBase)this.Controls.Find("yesOkRetryButton", true)[0]).DoClick();
                    break;
            }
        }
        private void tbTitle_MouseDown(object sender, MouseEventArgs e)
        {
            oloc = new Point(MousePosition.X, MousePosition.Y);
        }
        private void tbTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                this.Left += MousePosition.X - oloc.X;
                this.Top += MousePosition.Y - oloc.Y;
                oloc = new Point(MousePosition.X, MousePosition.Y);
                Application.DoEvents();
            }
        }

    }

}
