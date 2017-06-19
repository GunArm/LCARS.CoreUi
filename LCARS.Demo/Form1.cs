using LCARS.CoreUi;
using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using LCARS.CoreUi.UiElements;
using LCARS.CoreUi.UiElements.Base;
using LCARS.CoreUi.UiElements.Dialogs;
using LCARS.CoreUi.UiElements.Lightweight;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.Demo
{
    public partial class LcarsDemo : LcarsForm
    {
        public LcarsDemo()
        {
            InitializeComponent();
            Alerts.RestoreDefaults();
            InitButtonGrid();
            InitAlienFontList();
            elbowCounter = (int)lcarsTabControl2.ElbowStyle;
        }

        private void InitAlienFontList()
        {
            lcarsList1.Items.AddRange(FontProvider.AlienList.ToArray());
        }

        private void InitButtonGrid()
        {
            for (int x = 0; x < 200; x++)
            {
                int i = Randomizer.NextInt(0, 5);
                switch (i)
                {
                    case 0:
                        LCArrowButton control0 = new LCArrowButton();
                        control0.ArrowDirection = Enum<LcarsArrowDirection>.Random();
                        control0.ColorFunction = Enum<LcarsColorFunction>.Random();
                        control0.IsLit = Randomizer.NextBool();
                        control0.DoesBeep = true;
                        buttonGrid1.Add(control0);
                        break;
                    case 1:
                        LCComplexButton control1 = new LCComplexButton();
                        control1.ColorFunction = Enum<LcarsColorFunction>.Random();
                        control1.IsLit = Randomizer.NextBool();
                        control1.DoesBeep = true;
                        control1.Text = Randomizer.NextInt(int.MaxValue).ToString();
                        control1.SideText = Randomizer.NextInt(1000).ToString();
                        control1.SideBlockColor = Enum<LcarsColorFunction>.Random();
                        buttonGrid1.Add(control1);
                        break;
                    case 2:
                        LCFlatButton control2 = new LCFlatButton();
                        control2.ColorFunction = Enum<LcarsColorFunction>.Random();
                        control2.IsLit = Randomizer.NextBool();
                        control2.DoesBeep = true;
                        control2.Text = Randomizer.NextInt(int.MaxValue).ToString();
                        buttonGrid1.Add(control2);
                        break;
                    case 3:
                        LCHalfPillButton control3 = new LCHalfPillButton();
                        control3.ColorFunction = Enum<LcarsColorFunction>.Random();
                        control3.IsLit = Randomizer.NextBool();
                        control3.DoesBeep = true;
                        control3.Text = Randomizer.NextInt(int.MaxValue).ToString();
                        control3.ButtonStyle = Enum<LcarsButtonStyles>.Random();
                        buttonGrid1.Add(control3);
                        break;
                    case 4:
                        LCStandardButton control4 = new LCStandardButton();
                        control4.ColorFunction = Enum<LcarsColorFunction>.Random();
                        control4.IsLit = Randomizer.NextBool();
                        control4.DoesBeep = true;
                        control4.Text = Randomizer.NextInt(int.MaxValue).ToString();
                        buttonGrid1.Add(control4);
                        break;
                }
            }
            buttonGrid1.ControlSize = new Size(200, 30);
        }

        int elbowCounter;
        private void elbow5_Click(object sender, System.EventArgs e)
        {
            LcarsElbowStyle style = (LcarsElbowStyle)(++elbowCounter % Enum<LcarsElbowStyle>.Count);
            lcarsTabControl2.ElbowStyle = style;
            elbow5.ElbowStyle = style;
        }

        private void slider1_ValueChanged(object sender, System.EventArgs e)
        {
            lcarsTabControl2.VerticalBarWidth = slider1.Value;
        }

        private void slider2_ValueChanged(object sender, System.EventArgs e)
        {
            lcarsTabControl2.HorizantalBarHeight = slider2.Value;
        }

        private void slider3_ValueChanged(object sender, System.EventArgs e)
        {
            lcarsTabControl2.Spacing = slider3.Value;
        }

        private void ToggleFlash(object sender, System.EventArgs e)
        {
            var button = ((LcarsButtonBase)sender);
            button.Flash = !button.Flash;
        }

        private void slider4_ValueChanged(object sender, System.EventArgs e)
        {
            levelIndicator1.Value = slider4.Value;
            levelIndicator2.Value = slider4.Value;
            levelIndicator3.Value = slider4.Value;
            int offset = slider4.Min - 0;
            int val = slider4.Value - offset;
            int max = slider4.Max - offset;
            double ratio = (double)val / max;
            progressBar1.Value = ratio;
            progressBar2.Value = ratio;
        }

        private void standardButton17_Click(object sender, EventArgs e)
        {
            var redAlertId = Alerts.GetAlertID("Red");
            //Alerts.ActivateAlert(redAlertId, this.Handle);
            OnAlertInitiated(redAlertId);
        }

        private void lcarsList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lcarsLabel2.Font = FontProvider.Alien(lcarsList1.SelectedItem.ToString(), 12);
        }

        private void standardButton18_Click(object sender, EventArgs e)
        {
            LcarsMessageBox.Show("MessageBox prompt?", "MessageBox Title", MessageBoxButtons.YesNoCancel);
        }

        private void standardButton19_Click(object sender, EventArgs e)
        {
            LcarsInputBox.Show("InputBox prompt?", "InputBox Title", "default text");
        }

        private void FullScreenButton_Click(object sender, EventArgs e)
        {
            FullScreenButton.IsLit = !FullScreenButton.IsLit;
            FullScreen = FullScreenButton.IsLit;
        }
    }
}
