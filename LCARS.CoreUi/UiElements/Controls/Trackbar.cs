using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Scroll")]
    public class TrackBar : Control
    {
        public event EventHandler Scroll;

        private int tempX = 0;
        private bool scrolling = false;
        public LcarsColorManager ColorsAvailable = new LcarsColorManager();

        FlatButton movingButton = new FlatButton();

        #region " Properties "
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (currentPage == value) return;
                if (!(value < pages && value >= 0)) return;
                currentPage = value;
                if (pages > 1)
                {
                    movingButton.Left = (int)(((double)(Width - 5) / (pages - 1)) * (currentPage) - 2.5);
                }
                Scroll?.Invoke(this, new EventArgs());
            }
        }
        private int currentPage = 1;

        public int Pages
        {
            get { return pages; }
            set
            {
                //Check that the value actually changes
                if (value == pages) return;
                if (value <= 0) throw new IndexOutOfRangeException();
                pages = value;

                if (CurrentPage >= pages) CurrentPage = 1;

                //If there is only one page
                if (value == 1) movingButton.Visible = false;
                else //If there is more than one page
                {
                    movingButton.Visible = true;
                    movingButton.Left = (int)-2.5;
                }
                Invalidate();
            }
        }
        private int pages = 1;

        public LcarsColorFunction TickColor
        {
            get { return tickColor; }
            set
            {
                tickColor = value;
                Invalidate();
            }
        }
        private LcarsColorFunction tickColor = LcarsColorFunction.StaticTan;

        public bool DoesBeep
        {
            get { return doesBeep; }
            set { doesBeep = value; }
        }
        private bool doesBeep = true;
        #endregion

        #region " Event Handlers "
        private void Me_Resize(object sender, EventArgs e)
        {
            movingButton.Height = Height;
            Invalidate();
        }
        private void Button_Mouse_Down(object sender, EventArgs e)
        {
            tempX = MousePosition.X - movingButton.Left;
            scrolling = true;
        }
        private void Button_Mouse_Move(object sender, EventArgs e)
        {
            if (scrolling)
            {
                movingButton.Left = MousePosition.X - tempX;
            }
        }
        private void Button_Mouse_Up(object sender, EventArgs e)
        {
            scrolling = false;
            int page = (movingButton.Left + (Width - 5) / (2 * pages - 1)) / ((Width - 5) / (pages - 1));
            if (page < 0)
            {
                page = 0;
            }
            else if (page >= pages)
            {
                page = pages - 1;
            }
            movingButton.Left = (int)(((double)(Width - 5) / (pages - 1)) * (page) - 2.5);
            CurrentPage = page;
        }
        private void Me_Click(object sender, EventArgs e)
        {
            if (Pages > 1)
            {
                Point localPosition = PointToClient(Cursor.Position);
                decimal pagewidth = Width / (Pages - 1);
                int page = (int)Math.Round((localPosition.X / pagewidth));
                movingButton.Left = (int)(((double)(Width - 5) / (pages - 1)) * (page) - 2.5);
                CurrentPage = page;
            }
        }
        #endregion

        #region " Subs "

        public void InitializeComponent()
        {
            SetStyle(ControlStyles.ContainerControl, true);
            UpdateStyles();
            SuspendLayout();
            BackColor = Color.Black;
            Size = new Size(250, 15);
            ResumeLayout();
        }
        public TrackBar()
        {
            Click += Me_Click;
            Resize += Me_Resize;
            InitializeComponent();

            movingButton.Width = 15;
            movingButton.Height = Height;
            movingButton.Left = -5;
            movingButton.Top = 0;
            movingButton.Text = "";
            movingButton.Visible = false;
            movingButton.DoesSound = false;
            movingButton.MouseDown += Button_Mouse_Down;
            movingButton.MouseUp += Button_Mouse_Up;
            movingButton.MouseMove += Button_Mouse_Move;
            Controls.Add(movingButton);
            movingButton.BringToFront();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics myG = CreateGraphics();
            Bitmap myBitmap = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(myBitmap);
            SolidBrush myBrush = new SolidBrush(ColorsAvailable.GetColor(TickColor));
            //Draw base bar
            g.Clear(Color.Black);
            g.FillRectangle(myBrush, new Rectangle(0, Height * 2 / 3, Width, Height / 3));
            if (Pages > 1)
            {
                int i = 0;
                decimal x = (Width - 5) / (pages - 1);
                for (i = 0; i <= Pages; i++)
                {
                    //Code to draw all tick marks
                    g.FillRectangle(myBrush, new Rectangle((int)(i * x), 0, 5, Height));
                }
            }
            myG.DrawImage(myBitmap, new Point(0, 0));
        }
        #endregion
    }
}
