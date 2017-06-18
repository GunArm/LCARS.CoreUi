using LCARS.CoreUi.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Controls
{
    public class ProgressBar : Control
    {
        #region " Control Design Information "
        public ProgressBar()
        {
            Resize += OnResize;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            SetStyle(ControlStyles.ContainerControl, true);
            UpdateStyles();
            SuspendLayout();
            BackColor = Color.Black;
            Size = new Size(250, 100);
            ResumeLayout();

            //Elbows
            topLeft.ColorFunction = colorFunction1;
            topLeft.Clickable = false;
            topLeft.ElbowStyle = LcarsElbowStyle.UpperLeft;
            topLeft.Text = "";
            topLeft.Top = 0;
            topLeft.Left = 0;
            topLeft.VerticalBarWidth = horizontal;
            topLeft.HorizantalBarHeight = vertical;
            topLeft.Height = Spacing + vertical;
            topLeft.Width = Spacing + horizontal;
            Controls.Add(topLeft);

            topRight.ColorFunction = colorFunction1;
            topRight.Clickable = false;
            topRight.ElbowStyle = LcarsElbowStyle.UpperRight;
            topRight.Text = "";
            topRight.Height = Spacing + vertical;
            topRight.Width = Spacing + horizontal;
            topRight.Top = 0;
            topRight.Left = Width - topRight.Width;
            topRight.VerticalBarWidth = horizontal;
            topRight.HorizantalBarHeight = vertical;
            Controls.Add(topRight);

            bottomRight.ColorFunction = colorFunction1;
            bottomRight.Clickable = false;
            bottomRight.ElbowStyle = LcarsElbowStyle.LowerRight;
            bottomRight.Text = "";
            bottomRight.Height = Spacing + vertical;
            bottomRight.Width = Spacing + horizontal;
            bottomRight.Top = Height - bottomRight.Height;
            bottomRight.Left = Width - bottomRight.Width;
            bottomRight.VerticalBarWidth = horizontal;
            bottomRight.HorizantalBarHeight = vertical;
            Controls.Add(bottomRight);

            bottomLeft.ColorFunction = colorFunction1;
            bottomLeft.Clickable = false;
            bottomLeft.ElbowStyle = LcarsElbowStyle.LowerLeft;
            bottomLeft.Text = "";
            bottomLeft.Height = Spacing + vertical;
            bottomLeft.Width = Spacing + horizontal;
            bottomLeft.Top = Height - bottomLeft.Height;
            bottomLeft.Left = 0;
            bottomLeft.VerticalBarWidth = horizontal;
            bottomLeft.HorizantalBarHeight = vertical;
            Controls.Add(bottomLeft);

            //Bars
            topBar.ColorFunction = colorFunction1;
            topBar.Clickable = false;
            topBar.Height = vertical;
            topBar.Width = Width - (topLeft.Width * 2);
            topBar.Top = 0;
            topBar.Left = topLeft.Width;
            Controls.Add(topBar);

            bottomBar.ColorFunction = colorFunction1;
            bottomBar.Clickable = false;
            bottomBar.ButtonTextAlign = ContentAlignment.BottomRight;
            bottomBar.Height = vertical;
            bottomBar.Width = Width - (topLeft.Width * 2);
            bottomBar.Top = Height - bottomBar.Height;
            bottomBar.Left = topLeft.Width;
            Controls.Add(bottomBar);

            leftBar.ColorFunction = colorFunction1;
            leftBar.Clickable = false;
            leftBar.Text = "";
            leftBar.Height = Height - (topLeft.Height * 2);
            leftBar.Width = horizontal;
            leftBar.Top = topLeft.Height;
            leftBar.Left = 0;
            Controls.Add(leftBar);

            rightBar.ColorFunction = colorFunction1;
            rightBar.Clickable = false;
            rightBar.Text = "";
            rightBar.Height = Height - (topLeft.Height * 2);
            rightBar.Width = horizontal;
            rightBar.Top = topLeft.Height;
            rightBar.Left = Width - rightBar.Width;
            Controls.Add(rightBar);

            //Display
            display.ColorFunction = colorFunction2;
            display.Clickable = false;
            display.Text = "";
            if (_style == ProgressBarStyle.Horizontal)
            {
                display.Height = rightBar.Height;
                display.Width = (int)(topBar.Width * Value);
                display.Top = topRight.Height;
                display.Left = topRight.Width;
            }
            else
            {
                display.Height = (int)(rightBar.Height * value);
                display.Width = topBar.Width;
                display.Top = Height - (display.Height + topRight.Height);
                display.Left = topRight.Width;
            }
            Controls.Add(display);
        }
        #endregion

        #region " Global Variables "
        //Permanent Controls
        private Elbow topLeft = new Elbow();
        private Elbow topRight = new Elbow();
        private Elbow bottomLeft = new Elbow();
        private Elbow bottomRight = new Elbow();
        private FlatButton topBar = new FlatButton();
        private FlatButton bottomBar = new FlatButton();
        private FlatButton leftBar = new FlatButton();
        private FlatButton rightBar = new FlatButton();
        private FlatButton display = new FlatButton();
        #endregion

        #region " Enum "
        public enum ProgressBarStyle
        {
            Horizontal = 0,
            Vertical = 1
        }
        #endregion

        #region " Properties "
        public double Value
        {
            get { return value; }
            set
            {
                if (value >= 0 & value <= 1)
                {
                    this.value = value;
                    Redraw();
                }
            }
        }
        private double value = 0.5;

        public LcarsColorFunction ColorFunction1
        {
            get { return colorFunction1; }
            set
            {
                colorFunction1 = value;
                topLeft.ColorFunction = value;
                topRight.ColorFunction = value;
                bottomLeft.ColorFunction = value;
                bottomRight.ColorFunction = value;
                topBar.ColorFunction = value;
                bottomBar.ColorFunction = value;
                rightBar.ColorFunction = value;
                leftBar.ColorFunction = value;
            }
        }
        private LcarsColorFunction colorFunction1 = LcarsColorFunction.StaticTan;

        public LcarsColorFunction ColorFunction2
        {
            get { return colorFunction2; }
            set
            {
                colorFunction2 = value;
                display.ColorFunction = value;
            }
        }
        private LcarsColorFunction colorFunction2 = LcarsColorFunction.PrimaryFunction;

        public string TopText
        {
            get { return topBar.Text; }
            set { topBar.Text = value; }
        }

        public ContentAlignment TopTextAlign
        {
            get { return topBar.ButtonTextAlign; }
            set { topBar.ButtonTextAlign = value; }
        }

        public string BottomText
        {
            get { return bottomBar.Text; }
            set { bottomBar.Text = value; }
        }

        public ContentAlignment BottomTextAlign
        {
            get { return bottomBar.ButtonTextAlign; }
            set { bottomBar.ButtonTextAlign = value; }
        }

        public int HorizontalBarThickness
        {
            get { return horizontal; }
            set
            {
                horizontal = value;
                Redraw();
            }
        }
        private int horizontal = 10;

        public int VerticalBarThickness
        {
            get { return vertical; }
            set
            {
                vertical = value;
                Redraw();
            }
        }
        private int vertical = 20;

        public int Spacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                Redraw();
            }
        }
        private int spacing = 5;

        public ProgressBarStyle ProgressBarOrientation
        {
            get { return _style; }
            set
            {
                _style = value;
                Redraw();
            }
        }
        private ProgressBarStyle _style = ProgressBarStyle.Horizontal;
        #endregion

        #region " Event Handlers "
        public void OnResize(object sender, EventArgs e)
        {
            Redraw();
        }
        #endregion

        #region " Draw "
        public void Redraw()
        {
            topLeft.Top = 0;
            topLeft.Left = 0;
            topLeft.VerticalBarWidth = horizontal;
            topLeft.HorizantalBarHeight = vertical;
            topLeft.Height = Spacing + vertical;
            topLeft.Width = Spacing + horizontal;

            topRight.Height = Spacing + vertical;
            topRight.Width = Spacing + horizontal;
            topRight.Top = 0;
            topRight.Left = Width - topRight.Width;
            topRight.VerticalBarWidth = horizontal;
            topRight.HorizantalBarHeight = vertical;

            bottomRight.Height = Spacing + vertical;
            bottomRight.Width = Spacing + horizontal;
            bottomRight.Top = Height - bottomRight.Height;
            bottomRight.Left = Width - bottomRight.Width;
            bottomRight.VerticalBarWidth = horizontal;
            bottomRight.HorizantalBarHeight = vertical;

            bottomLeft.Height = Spacing + vertical;
            bottomLeft.Width = Spacing + horizontal;
            bottomLeft.Top = Height - bottomLeft.Height;
            bottomLeft.Left = 0;
            bottomLeft.VerticalBarWidth = horizontal;
            bottomLeft.HorizantalBarHeight = vertical;

            //Bars
            topBar.Height = vertical;
            topBar.Width = Width - (this.topLeft.Width * 2);
            topBar.Top = 0;
            topBar.Left = this.topLeft.Width;

            bottomBar.Height = vertical;
            bottomBar.Width = Width - (this.topLeft.Width * 2);
            bottomBar.Top = Height - bottomBar.Height;
            bottomBar.Left = this.topLeft.Width;

            leftBar.Height = Height - (this.topLeft.Height * 2);
            leftBar.Width = horizontal;
            leftBar.Top = this.topLeft.Height;
            leftBar.Left = 0;

            rightBar.Height = Height - (this.topLeft.Height * 2);
            rightBar.Width = horizontal;
            rightBar.Top = this.topLeft.Height;
            rightBar.Left = Width - rightBar.Width;

            //Display
            if (_style == ProgressBarStyle.Horizontal)
            {
                display.Height = this.rightBar.Height;
                display.Width = (int)(this.topBar.Width * Value);
                display.Top = this.topRight.Height;
                display.Left = this.topRight.Width;
            }
            else
            {
                display.Height = (int)(this.rightBar.Height * value);
                display.Width = this.topBar.Width;
                display.Top = Height - (display.Height + this.topRight.Height);
                display.Left = this.topRight.Width;
            }

        }
        #endregion
    }
}
