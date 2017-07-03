using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Dialogs
{
    public class LcarsMessageBox : Form
    {
        public static DialogResult Show(string prompt, string title = "LCARS", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            LcarsMessageBox box = new LcarsMessageBox(prompt, title, buttons, icon);
            box.ShowDialog();
            DialogResult result = box.Result;
            box.Dispose();
            return result;
        }

        public DialogResult Result { get; private set; }
        private MessageBoxButtons buttonStyle;
        private Point oloc;

        public LcarsMessageBox(object prompt, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            buttonStyle = buttons;
            bool cancelVisible = false;
            bool abortVisible = false;
            bool retryVisible = false;
            bool ignoreVisible = false;
            bool yesVisible = false;
            bool noVisible = false;
            int okx = 183;
            int cx = 50;

            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    cancelVisible = true;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    abortVisible = true;
                    retryVisible = true;
                    ignoreVisible = true;
                    // cancelEnabled = true;
                    cx = 210;
                    okx = 115;
                    break;
                case MessageBoxButtons.RetryCancel:
                    retryVisible = true;
                    cancelVisible = true;
                    break;
                case MessageBoxButtons.YesNo:
                    yesVisible = true;
                    noVisible = true;
                    okx = 210;
                    cx = 115;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    cancelVisible = true;
                    yesVisible = true;
                    noVisible = true;
                    okx = 210;
                    cx = 115;
                    break;
            }

            LcarsColorFunction colorFunction;
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    //vbCritical
                    colorFunction = LcarsColorFunction.FunctionOffline;
                    // pass redAlert() to LCARSmain if exists.
                    break;
                case MessageBoxIcon.Question:
                    //vbQuestion
                    colorFunction = LcarsColorFunction.StaticBlue;
                    break;
                case MessageBoxIcon.Exclamation:
                    //vbExclamation
                    colorFunction = LcarsColorFunction.StaticTan;
                    break;
                case MessageBoxIcon.Information:
                    //vbInformation
                    colorFunction = LcarsColorFunction.LCARSDisplayOnly;
                    break;
                default:
                    colorFunction = LcarsColorFunction.StaticTan;
                    break;
            }

            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(305, 200);
            BackColor = Color.Black;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;

            Elbow elbow1 = new Elbow();
            Controls.Add(elbow1);
            elbow1.Location = new Point(0, 0);
            elbow1.Size = new Size(80, 60);
            elbow1.HorizantalBarHeight = 30;
            elbow1.VerticalBarWidth = 10;
            elbow1.ElbowStyle = LcarsElbowStyle.UpperLeft;
            elbow1.ColorFunction = colorFunction;
            elbow1.Text = "";

            Elbow elbow2 = new Elbow();
            Controls.Add(elbow2);
            elbow2.Location = new Point(0, 140);
            elbow2.Size = new Size(80, 60);
            elbow2.HorizantalBarHeight = 15;
            elbow2.VerticalBarWidth = 10;
            elbow2.Clickable = false;
            elbow2.ElbowStyle = LcarsElbowStyle.LowerLeft;
            elbow2.ColorFunction = colorFunction;
            elbow2.Text = "";

            TextButton titleBar = new TextButton();
            Controls.Add(titleBar);
            titleBar.Location = new Point(50, 0);
            titleBar.Size = new Size(255, 30);
            titleBar.TextHeight = 31;
            titleBar.ColorFunction = colorFunction;
            titleBar.Text = title;

            FlatButton vertBar = new FlatButton();
            Controls.Add(vertBar);
            vertBar.Location = new Point(0, 66);
            vertBar.Size = new Size(10, 68);
            vertBar.Clickable = false;
            vertBar.ColorFunction = colorFunction;
            vertBar.Text = "";

            HalfPillButton bottomBar = new HalfPillButton();
            Controls.Add(bottomBar);
            bottomBar.Location = new Point(85, 185);
            bottomBar.Size = new Size(217, 15);
            bottomBar.Clickable = false;
            bottomBar.ColorFunction = colorFunction;
            bottomBar.Text = "";

            //draw text
            RichTextBox errorTextBox = new RichTextBox();
            Controls.Add(errorTextBox);
            errorTextBox.Location = new Point(30, 36);
            errorTextBox.Size = new Size(243, 107);
            errorTextBox.BackColor = Color.Black;
            errorTextBox.BorderStyle = BorderStyle.None;
            errorTextBox.WordWrap = true;
            errorTextBox.Font = FontProvider.Lcars(14);
            errorTextBox.ForeColor = Color.Orange;
            errorTextBox.BringToFront();
            errorTextBox.Text = prompt.ToString();
            errorTextBox.ReadOnly = true;

            // BUTTONS

            //draw OK/Yes/retry button
            StandardButton yesOkRetryButton = new StandardButton();
            Controls.Add(yesOkRetryButton);

            yesOkRetryButton.Location = new Point(okx, 149);
            yesOkRetryButton.Size = new Size(90, 30);
            yesOkRetryButton.ColorFunction = LcarsColorFunction.PrimaryFunction;
            yesOkRetryButton.Name = "yesOkRetryButton";
            if (yesVisible)
            {
                yesOkRetryButton.Text = "YES";
                yesOkRetryButton.Click += OnYesClick;
            }
            else if (retryVisible)
            {
                yesOkRetryButton.Text = "RETRY";
                yesOkRetryButton.Click += OnRetryClick;
            }
            else
            {
                yesOkRetryButton.Text = "OK";
                yesOkRetryButton.Click += OnOkClick;
            }
            yesOkRetryButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            yesOkRetryButton.BringToFront();

            if (cancelVisible || ignoreVisible)
            {
                StandardButton cancelIgnoreButton = new StandardButton();
                Controls.Add(cancelIgnoreButton);
                cancelIgnoreButton.Location = new Point(cx, 149);
                cancelIgnoreButton.Size = new Size(90, 30);
                cancelIgnoreButton.Name = "cancelIgnoreButton";
                cancelIgnoreButton.ColorFunction = LcarsColorFunction.CriticalFunction;

                if (ignoreVisible)
                {
                    cancelIgnoreButton.Text = "IGNORE";
                    cancelIgnoreButton.Click += OnIgnoreClick;
                }
                else if (cancelVisible)
                {
                    cancelIgnoreButton.Text = "CANCEL";
                    cancelIgnoreButton.Click += OnCancelClick;
                }
                cancelIgnoreButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
                cancelIgnoreButton.BringToFront();
            }

            if (abortVisible || noVisible)
            {
                //draw abort/no button
                StandardButton noAbortButton = new StandardButton();
                Controls.Add(noAbortButton);
                noAbortButton.Location = new Point(20, 149);
                noAbortButton.Size = new Size(90, 30);
                noAbortButton.ColorFunction = LcarsColorFunction.CriticalFunction;
                if (abortVisible)
                {
                    noAbortButton.Text = "ABORT";
                    noAbortButton.Click += OnAbortClick;
                }
                else if (noVisible)
                {
                    noAbortButton.Text = "NO";
                    noAbortButton.Click += OnNoClick;
                }
                noAbortButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
                noAbortButton.BringToFront();
            }

            KeyPreview = true;
            KeyDown += Me_KeyDown;
            elbow1.MouseDown += Title_MouseDown;
            elbow1.MouseMove += Title_MouseMove;
            titleBar.MouseDown += Title_MouseDown;
            titleBar.MouseMove += Title_MouseMove;
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            Result = DialogResult.OK;
            Close();
        }
        private void OnCancelClick(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            Close();
        }
        private void OnAbortClick(object sender, EventArgs e)
        {
            Result = DialogResult.Abort;
            Close();
        }
        private void OnRetryClick(object sender, EventArgs e)
        {
            Result = DialogResult.Retry;
            Close();
        }
        private void OnIgnoreClick(object sender, EventArgs e)
        {
            Result = DialogResult.Ignore;
            Close();
        }
        private void OnYesClick(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            Close();
        }
        private void OnNoClick(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            Close();
        }
        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if ((buttonStyle == MessageBoxButtons.OKCancel ||
                        buttonStyle == MessageBoxButtons.AbortRetryIgnore ||
                        buttonStyle == MessageBoxButtons.YesNoCancel ||
                        buttonStyle == MessageBoxButtons.RetryCancel))
                    {
                        ((LcarsButtonBase)Controls.Find("cancelIgnoreButton", true)[0]).DoClick();
                    }
                    break;
                case Keys.Enter:
                    ((LcarsButtonBase)Controls.Find("yesOkRetryButton", true)[0]).DoClick();
                    break;
            }
        }
        private void Title_MouseDown(object sender, MouseEventArgs e)
        {
            oloc = new Point(MousePosition.X, MousePosition.Y);
        }
        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                Left += MousePosition.X - oloc.X;
                Top += MousePosition.Y - oloc.Y;
                oloc = new Point(MousePosition.X, MousePosition.Y);
                Application.DoEvents();
            }
        }
    }
}
