using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Dialogs
{
    public class LcarsInputBox : Form
    {
        public static string Show(string prompt, string title = "LCARS", string defaultResponse = "", int posX = -1, int posY = -1)
        {
            LcarsInputBox box = new LcarsInputBox(prompt, title, defaultResponse, posX, posY);
            box.ShowDialog();
            string result = box.Result;
            box.Dispose();
            return result;
        }

        public string Result { get { return inputBox.Text; } }
        private TextBox inputBox = new TextBox();


        private Point oloc;
        public LcarsInputBox(string prompt, string title, string defaultResponse, int posX, int posY)
        {
            BackColor = Color.Black;
            ForeColor = Color.Orange;
            Size = new Size(400, 200);
            FormBorderStyle = FormBorderStyle.None;
            if (posX == -1)
            {
                posX = Screen.AllScreens[0].Bounds.Width / 2 - 200;
            }
            if (posY == -1)
            {
                posY = Screen.AllScreens[0].Bounds.Height / 2 - 100;
            }

            StartPosition = FormStartPosition.Manual;
            Location = new Point(posX, posY);

            TextButton titleBar = new TextButton();
            titleBar.Width = 390;
            titleBar.Height = 30;
            titleBar.Location = new Point(5, 5);
            titleBar.Text = title;
            titleBar.Clickable = true;
            Controls.Add(titleBar);

            TextButton bottomBar = new TextButton();
            bottomBar.Width = 390;
            bottomBar.Top = 195 - bottomBar.Height;
            bottomBar.Left = 5;
            bottomBar.Text = "";
            bottomBar.Clickable = false;
            Controls.Add(bottomBar);

            RichTextBox txtPrompt = new RichTextBox();
            txtPrompt.Width = 390;
            txtPrompt.Height = 65;
            txtPrompt.Top = titleBar.Bottom + 7;
            txtPrompt.Left = 5;
            txtPrompt.Text = prompt;
            txtPrompt.BackColor = Color.Black;
            txtPrompt.ForeColor = Color.Orange;
            txtPrompt.BorderStyle = BorderStyle.None;
            txtPrompt.Font = new Font("LCARS", 14, FontStyle.Regular);
            txtPrompt.ReadOnly = true;
            txtPrompt.WordWrap = true;
            Controls.Add(txtPrompt);

            inputBox.Left = 10;
            inputBox.Width = 380;
            inputBox.Top = txtPrompt.Bottom + 7;
            inputBox.BackColor = Color.Black;
            inputBox.ForeColor = Color.Orange;
            inputBox.Font = new Font("LCARS", 14, FontStyle.Regular);
            inputBox.Text = defaultResponse;
            inputBox.TabIndex = 0;
            inputBox.Focus();
            Controls.Add(inputBox);

            StandardButton okButton = new StandardButton();
            okButton.Height = bottomBar.Height;
            okButton.Top = bottomBar.Top - 7 - okButton.Height;
            okButton.Width = 70;
            okButton.Left = 325;
            okButton.ColorFunction = LcarsColorFunction.PrimaryFunction;
            okButton.Text = "OK";
            okButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(okButton);
            okButton.Click += Ok_Click;

            StandardButton cancelButton = new StandardButton();
            cancelButton.Height = bottomBar.Height;
            cancelButton.Top = bottomBar.Top - 7 - cancelButton.Height;
            cancelButton.Width = 70;
            cancelButton.Left = 248;
            cancelButton.ColorFunction = LcarsColorFunction.CriticalFunction;
            cancelButton.Text = "Cancel";
            cancelButton.ButtonTextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(cancelButton);
            cancelButton.Click += Cancel_Click;
            KeyPreview = true;
            KeyDown += Me_KeyDown;

            titleBar.MouseDown += Title_MouseDown;
            titleBar.MouseMove += Title_MouseMove;

            inputBox.Select();
        }

        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Ok_Click(sender, e);
                    break;
                case Keys.Escape:
                    Cancel_Click(sender, e);
                    break;
            }
        }

        private void Ok_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            inputBox.Text = "";
            Close();
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
