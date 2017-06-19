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
            topLeft.VerticalBarWidth = verticalBarHeight;
            topLeft.HorizantalBarHeight = horizantalBarWidth;
            topLeft.Height = Spacing + horizantalBarWidth;
            topLeft.Width = Spacing + verticalBarHeight;
            Controls.Add(topLeft);

            topRight.ColorFunction = colorFunction1;
            topRight.Clickable = false;
            topRight.ElbowStyle = LcarsElbowStyle.UpperRight;
            topRight.Text = "";
            topRight.Height = Spacing + horizantalBarWidth;
            topRight.Width = Spacing + verticalBarHeight;
            topRight.Top = 0;
            topRight.Left = Width - topRight.Width;
            topRight.VerticalBarWidth = verticalBarHeight;
            topRight.HorizantalBarHeight = horizantalBarWidth;
            Controls.Add(topRight);

            bottomRight.ColorFunction = colorFunction1;
            bottomRight.Clickable = false;
            bottomRight.ElbowStyle = LcarsElbowStyle.LowerRight;
            bottomRight.Text = "";
            bottomRight.Height = Spacing + horizantalBarWidth;
            bottomRight.Width = Spacing + verticalBarHeight;
            bottomRight.Top = Height - bottomRight.Height;
            bottomRight.Left = Width - bottomRight.Width;
            bottomRight.VerticalBarWidth = verticalBarHeight;
            bottomRight.HorizantalBarHeight = horizantalBarWidth;
            Controls.Add(bottomRight);

            bottomLeft.ColorFunction = colorFunction1;
            bottomLeft.Clickable = false;
            bottomLeft.ElbowStyle = LcarsElbowStyle.LowerLeft;
            bottomLeft.Text = "";
            bottomLeft.Height = Spacing + horizantalBarWidth;
            bottomLeft.Width = Spacing + verticalBarHeight;
            bottomLeft.Top = Height - bottomLeft.Height;
            bottomLeft.Left = 0;
            bottomLeft.VerticalBarWidth = verticalBarHeight;
            bottomLeft.HorizantalBarHeight = horizantalBarWidth;
            Controls.Add(bottomLeft);

            //Bars
            topBar.ColorFunction = colorFunction1;
            topBar.Clickable = false;
            topBar.Height = horizantalBarWidth;
            topBar.Width = Width - (topLeft.Width * 2);
            topBar.Top = 0;
            topBar.Left = topLeft.Width;
            Controls.Add(topBar);

            bottomBar.ColorFunction = colorFunction1;
            bottomBar.Clickable = false;
            bottomBar.ButtonTextAlign = ContentAlignment.BottomRight;
            bottomBar.Height = horizantalBarWidth;
            bottomBar.Width = Width - (topLeft.Width * 2);
            bottomBar.Top = Height - bottomBar.Height;
            bottomBar.Left = topLeft.Width;
            Controls.Add(bottomBar);

            leftBar.ColorFunction = colorFunction1;
            leftBar.Clickable = false;
            leftBar.Text = "";
            leftBar.Height = Height - (topLeft.Height * 2);
            leftBar.Width = verticalBarHeight;
            leftBar.Top = topLeft.Height;
            leftBar.Left = 0;
            Controls.Add(leftBar);

            rightBar.ColorFunction = colorFunction1;
            rightBar.Clickable = false;
            rightBar.Text = "";
            rightBar.Height = Height - (topLeft.Height * 2);
            rightBar.Width = verticalBarHeight;
            rightBar.Top = topLeft.Height;
            rightBar.Left = Width - rightBar.Width;
            Controls.Add(rightBar);

            //Display
            display.ColorFunction = colorFunction2;
            display.Clickable = false;
            display.Text = "";
            if (orientation == ProgressBarStyle.Horizontal)
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

        public int VerticalBarHeight
        {
            get { return verticalBarHeight; }
            set
            {
                if (verticalBarHeight == value) return;
                verticalBarHeight = value;
                Redraw();
            }
        }
        private int verticalBarHeight = 10;

        public int HorizantalBarWidth
        {
            get { return horizantalBarWidth; }
            set
            {
                if (horizantalBarWidth == value) return;
                horizantalBarWidth = value;
                Redraw();
            }
        }
        private int horizantalBarWidth = 20;

        public int Spacing
        {
            get { return spacing; }
            set
            {
                if (spacing == value) return;
                spacing = value;
                Redraw();
            }
        }
        private int spacing = 5;

        public ProgressBarStyle Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation == value) return;
                orientation = value;
                Redraw();
            }
        }
        private ProgressBarStyle orientation = ProgressBarStyle.Horizontal;
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
            topLeft.VerticalBarWidth = verticalBarHeight;
            topLeft.HorizantalBarHeight = horizantalBarWidth;
            topLeft.Height = Spacing + horizantalBarWidth;
            topLeft.Width = Spacing + verticalBarHeight;

            topRight.Height = Spacing + horizantalBarWidth;
            topRight.Width = Spacing + verticalBarHeight;
            topRight.Top = 0;
            topRight.Left = Width - topRight.Width;
            topRight.VerticalBarWidth = verticalBarHeight;
            topRight.HorizantalBarHeight = horizantalBarWidth;

            bottomRight.Height = Spacing + horizantalBarWidth;
            bottomRight.Width = Spacing + verticalBarHeight;
            bottomRight.Top = Height - bottomRight.Height;
            bottomRight.Left = Width - bottomRight.Width;
            bottomRight.VerticalBarWidth = verticalBarHeight;
            bottomRight.HorizantalBarHeight = horizantalBarWidth;

            bottomLeft.Height = Spacing + horizantalBarWidth;
            bottomLeft.Width = Spacing + verticalBarHeight;
            bottomLeft.Top = Height - bottomLeft.Height;
            bottomLeft.Left = 0;
            bottomLeft.VerticalBarWidth = verticalBarHeight;
            bottomLeft.HorizantalBarHeight = horizantalBarWidth;

            //Bars
            topBar.Height = horizantalBarWidth;
            topBar.Width = Width - (this.topLeft.Width * 2);
            topBar.Top = 0;
            topBar.Left = this.topLeft.Width;

            bottomBar.Height = horizantalBarWidth;
            bottomBar.Width = Width - (this.topLeft.Width * 2);
            bottomBar.Top = Height - bottomBar.Height;
            bottomBar.Left = this.topLeft.Width;

            leftBar.Height = Height - (this.topLeft.Height * 2);
            leftBar.Width = verticalBarHeight;
            leftBar.Top = this.topLeft.Height;
            leftBar.Left = 0;

            rightBar.Height = Height - (this.topLeft.Height * 2);
            rightBar.Width = verticalBarHeight;
            rightBar.Top = this.topLeft.Height;
            rightBar.Left = Width - rightBar.Width;

            //Display
            if (orientation == ProgressBarStyle.Horizontal)
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
