using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Dialogs
{
    internal class LCARSInputBoxForm : System.Windows.Forms.Form
    {
        public TextBox txtInput = new TextBox();
        private TextButton withEventsField_tbTitle = new TextButton();
        internal TextButton tbTitle
        {
            get { return withEventsField_tbTitle; }
            set
            {
                if (withEventsField_tbTitle != null)
                {
                    withEventsField_tbTitle.MouseDown -= tbTitle_MouseDown;
                    withEventsField_tbTitle.MouseMove -= tbTitle_MouseMove;
                }
                withEventsField_tbTitle = value;
                if (withEventsField_tbTitle != null)
                {
                    withEventsField_tbTitle.MouseDown += tbTitle_MouseDown;
                    withEventsField_tbTitle.MouseMove += tbTitle_MouseMove;
                }
            }
        }
        private Point oloc;
        public LCARSInputBoxForm(string prompt, string title, string defaultResponse, int posX, int posY)
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.Orange;
            this.Size = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.None;
            if (posX == -1)
            {
                posX = Screen.AllScreens[0].Bounds.Width / 2 - 200;
            }
            if (posY == -1)
            {
                posY = Screen.AllScreens[0].Bounds.Height / 2 - 100;
            }

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);
            var _with4 = tbTitle;
            _with4.Width = 390;
            _with4.Height = 30;
            _with4.Location = new Point(5, 5);
            _with4.Text = title;
            _with4.Clickable = true;
            this.Controls.Add(tbTitle);

            TextButton tbBottom = new TextButton();
            var _with5 = tbBottom;
            _with5.Width = 390;
            _with5.Top = 195 - _with5.Height;
            _with5.Left = 5;
            _with5.Text = "";
            _with5.Clickable = false;
            this.Controls.Add(tbBottom);

            RichTextBox txtPrompt = new RichTextBox();
            var _with6 = txtPrompt;
            _with6.Width = 390;
            _with6.Height = 65;
            _with6.Top = tbTitle.Bottom + 7;
            _with6.Left = 5;
            _with6.Text = prompt;
            _with6.BackColor = Color.Black;
            _with6.ForeColor = Color.Orange;
            _with6.BorderStyle = BorderStyle.None;
            _with6.Font = new Font("LCARS", 14, FontStyle.Regular);
            _with6.ReadOnly = true;
            _with6.WordWrap = true;
            this.Controls.Add(txtPrompt);

            var _with7 = txtInput;
            _with7.Left = 10;
            _with7.Width = 380;
            _with7.Top = txtPrompt.Bottom + 7;
            _with7.BackColor = Color.Black;
            _with7.ForeColor = Color.Orange;
            _with7.Font = new System.Drawing.Font("LCARS", 14, FontStyle.Regular);
            _with7.Text = defaultResponse;
            _with7.TabIndex = 0;
            txtInput.Focus();
            this.Controls.Add(txtInput);

            StandardButton sbOK = new StandardButton();
            var _with8 = sbOK;
            _with8.Height = tbBottom.Height;
            _with8.Top = tbBottom.Top - 7 - _with8.Height;
            _with8.Width = 70;
            _with8.Left = 325;
            _with8.ColorFunction = LcarsColorFunction.PrimaryFunction;
            _with8.Text = "OK";
            _with8.ButtonTextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(sbOK);
            sbOK.Click += sbOK_Click;

            StandardButton sbCancel = new StandardButton();
            var _with9 = sbCancel;
            _with9.Height = tbBottom.Height;
            _with9.Top = tbBottom.Top - 7 - _with9.Height;
            _with9.Width = 70;
            _with9.Left = 248;
            _with9.ColorFunction = LcarsColorFunction.CriticalFunction;
            _with9.Text = "Cancel";
            _with9.ButtonTextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(sbCancel);
            sbCancel.Click += sbCancel_Click;
            this.KeyPreview = true;
            this.KeyDown += Me_KeyDown;
        }

        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    sbOK_Click(sender, e);
                    break;
                case Keys.Escape:
                    sbCancel_Click(sender, e);
                    break;
            }
        }

        private void sbOK_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void sbCancel_Click(object sender, System.EventArgs e)
        {
            txtInput.Text = "";
            this.Close();
        }

        private void tbTitle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            oloc = new Point(MousePosition.X, MousePosition.Y);
        }
        private void tbTitle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
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
