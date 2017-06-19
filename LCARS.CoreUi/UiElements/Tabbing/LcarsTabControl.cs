using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    //The LcarsTabControl is made of two main parts:  The LcarsTabControl itself, and LcarsTabPages that get added
    //to it (just like the Windows Tab Control).

    //Set the defualt event to "SelectedTabChanged" (just makes it easier on the user).  At the same
    //time, apply the 'LcarsTabControlDesigner' which handles how LcarsTabControls interact with the 
    //Windows Form Designer (more info in the 'LcarsTabControlDesigner' class).
    [DefaultEvent("SelectedTabChanged"), Designer(typeof(LcarsTabControlDesigner))]
    public class LcarsTabControl : Control
    {
        //In case it's not obvious, this is the elbow in the top right of the LcarsTabControl.
        Elbow elbow = new Elbow();
        //Likewise, this is the heading bar at the very top of the control
        FlatButton horizantalBar = new FlatButton();
        //and the buttonPanel that holds the tab buttons along the right side
        public Panel tabButtonPanel = new Panel();
        //This label is used to display a message explaining how to add tabs when none
        //have been added and the control is in 'design time'.

        Label designerMessage = new Label();
        //An event that fires when a new tab has been selected.  Very useful for the user.
        public event SelectedTabChangedEventHandler SelectedTabChanged;
        public delegate void SelectedTabChangedEventHandler(LcarsTabPage Tab, int TabIndex);

        //Don't show it in the 'Properties' page, and remember it when the form is closed 
        //(Serialize it)
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LcarsTabPageCollection TabPages
        {
            //Returns a collection containing all of the tabs currently on the LcarsTabControl.
            get { return tabPages; }
        }
        private LcarsTabPageCollection tabPages;

        //Don't show it in the 'Properties' page
        [Browsable(false)]
        public LcarsTabPage SelectedTab
        {
            //Gets or sets the tab that is currently selected.  What more can I say?
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                if (selectedTab != null) ChangeTab(selectedTab);
            }
        }
        private LcarsTabPage selectedTab;

        public LcarsElbowStyle ElbowStyle
        {
            get { return elbowStyle; }
            set
            {
                if (elbowStyle == value) return;
                elbowStyle = value;
                RedoLayout();
            }
        }
        private LcarsElbowStyle elbowStyle = LcarsElbowStyle.UpperRight;

        public int VerticalBarWidth
        {
            get { return verticalBarWidth; }
            set
            {
                if (verticalBarWidth == value) return;
                verticalBarWidth = value;
                RedoLayout();
            }
        }
        private int verticalBarWidth = 100;

        public int HorizantalBarHeight
        {
            get { return horizantalBarHeight; }
            set
            {
                if (horizantalBarHeight == value) return;
                horizantalBarHeight = value;
                RedoLayout();
            }
        }
        private int horizantalBarHeight = 20;

        public int TabButtonHeight
        {
            get { return tabButtonHeight; }
            set
            {
                if (tabButtonHeight == value) return;
                tabButtonHeight = value;
                RedoLayout();
            }
        }
        private int tabButtonHeight = 35;


        public int ElbowButtonPadding
        {
            get { return elbowButtonPadding; }
            set
            {
                if (elbowButtonPadding == value) return;
                elbowButtonPadding = value;
                RedoLayout();
            }
        }
        private int elbowButtonPadding = 55;  // extra 55 for length into button bar

        public int Spacing
        {
            get { return spacing; }
            set
            {
                if (spacing == value) return;
                spacing = value;
                RedoLayout();
            }
        }
        private int spacing = 6;

        private void RedoLayout()
        {
            ElbowChanged();
            TabPagesChanged();
            LcarsTabControl_Resize(this, null);
        }

        public LcarsTabControl()
        {
            tabPages = new LcarsTabPageCollection(this);
            Resize += LcarsTabControl_Resize;
            ParentChanged += LcarsTabControl_ParentChanged;
            InitializeComponent();
            //Create the controls that make up the tabcontrol.  

            //The elbow in the top right of the tabcontrol,
            elbow.ButtonText = "TABS";
            elbow.ColorFunction = LcarsColorFunction.StaticTan;
            elbow.Clickable = false;
            Controls.Add(elbow);

            //The heading above the tab area (very top),
            horizantalBar.ButtonText = "";
            horizantalBar.ColorFunction = LcarsColorFunction.LCARSDisplayOnly;
            Controls.Add(horizantalBar);

            //And the panel that contains the buttons that act as the 'Tabs'.
            tabButtonPanel.BackColor = Color.Black;
            Controls.Add(tabButtonPanel);

            RedoLayout();
            elbow.SendToBack();
        }

        private void InitializeComponent()
        {
            //Sets the default values for the LcarsTabControl.

            //Allow this control to contain other controls
            SetStyle(ControlStyles.ContainerControl, true);
            UpdateStyles();

            //Don't redraw the control until we're done making changes.
            SuspendLayout();

            BackColor = Color.Black;
            Size = new Size(400, 400);

            //Ok, now we can allow the control to draw again.
            ResumeLayout(false);
        }

        private void ChangeTab(LcarsTabPage tab)
        {
            //Brings the provided tab to the front of all of the other tabs and sets it as 
            //the selected tab. 

            //NOTE: tab is used because if we used the property to set the selected tab, 
            //we would get into an 'endless loop' because the 'SelectedTab' property calls 
            //'ChangeTab' which in turn would call 'SelectedTab' again.  Not good!
            if (tab != null)
            {
                tab.BringToFront();
                selectedTab = tab;

                //Set the heading(the bar at the top)'s text to the tab's text
                horizantalBar.Text = tab.Text;

                //Set the selected tabs button's red alert to white and all others to normal
                foreach (FlatButton mybutton in tabButtonPanel.Controls)
                {
                    if (ReferenceEquals(mybutton.Tag, tab))
                    {
                        mybutton.AlertState = LcarsAlert.White;
                    }
                    else
                    {
                        mybutton.AlertState = LcarsAlert.Normal;
                    }
                }
            }

            //Let the user of the control know that a tab has been selected.
            SelectedTabChanged?.Invoke(tab, TabPages.IndexOf(tab));
        }

        private void TabButton_Click(object sender, EventArgs e)
        {
            //Handles the user clicking on one of the tab buttons.

            //"Sender" is the button that the user clicked.  We can use it's tag property (that
            //was set in the 'TabPagesChanged' Sub) to bring the tab associated with this button to 
            //the front.
            ChangeTab((LcarsTabPage)((Control)sender).Tag);
        }

        private void LcarsTabControl_ParentChanged(object sender, System.EventArgs e)
        {
            //This event fires when the control is added to the form or moved from one control to 
            //another.  The reason the following code is here rather than in the 'New' or 
            //InitializeComponents' Subs is because we have to wait until the control has been added
            //to the form or another control before we can check if any of those controls are running
            //in design time.

            //Here, we're checking if the control is running in "Design Time" or "Run Time".
            //Design Time is when you are editing the form in the designer.  Run Time is when you
            //execute the program.  We don't want to show a message explaining how to use the
            //LcarsTabControl unless they are a programmer!
            if (IsDesignerHosted & Controls.Contains(designerMessage) == false)
            {
                designerMessage.AutoSize = true;
                designerMessage.Text = "There are currently no tabs." + Environment.NewLine +
                    "Right click on the elbow and choose" + Environment.NewLine +
                    "'Add Tab' to add new tabs to the control.";

                //Center the message in the Tab area.
                designerMessage.Left = ((Width - 112) / 2) - (designerMessage.Width / 2);
                designerMessage.Top = (Height / 2) - (designerMessage.Height / 2);

                designerMessage.ForeColor = Color.White;

                //by removing all of it's anchors, the control will stay centered.
                designerMessage.Anchor = AnchorStyles.None;

                Controls.Add(designerMessage);

                //It's sent to the back just in case a tab is there.  It shouldn't be, but if
                //it is, this line ensures that the message is hidden behind it.
                SendToBack();
            }
        }

        private bool IsDesignerHosted
        {
            get
            {
                Control ctrl = this;
                while (ctrl != null)
                {
                    if (ctrl.Site == null)
                    {
                        //If a control does not have a site, then it's definately in 'Run Time'.
                        return false;
                    }
                    if (ctrl.Site.DesignMode == true)
                    {
                        return true;
                    }

                    //Check the control's parent to make sure it's not running in design time.
                    //if any of them are, then the whole program is.
                    ctrl = ctrl.Parent;
                }
                return false;
            }
        }
        
        internal void TabPagesChanged()
        {
            //This is called whenever a tab has been added or removed from the TabPages collection.

            //Remove all of the tabs from the control
            foreach (Control mycontrol in Controls)
            {
                if (ReferenceEquals(mycontrol.GetType(), typeof(LcarsTabPage)))
                {
                    Controls.Remove(mycontrol);
                }
            }

            //Remove the tab's button from the buttonPanel.
            tabButtonPanel.Controls.Clear();

            //Add all of the tabs back to the control, so now the new ones are there, too 
            //(or old ones are gone...)
            foreach (LcarsTabPage mytab in TabPages)
            {
                Controls.Add(mytab);

                //Set the size of the tab equal to the area we want the tab to cover.  Basically,
                //everwhere our heading, elbow, and buttonpanel are not.

                mytab.Width = Width - tabButtonPanel.Width - 10; // 10px spacing 
                mytab.Height = Height - (horizantalBar.Height + spacing);

                switch (elbowStyle)
                {
                    case LcarsElbowStyle.UpperRight:
                        mytab.Location = new Point(0, horizantalBar.Height + spacing);
                        break;
                    case LcarsElbowStyle.UpperLeft:
                        mytab.Location = new Point(Width - mytab.Width, horizantalBar.Height + spacing);
                        break;
                    case LcarsElbowStyle.LowerRight:
                        mytab.Location = new Point(0, 0);
                        break;
                    case LcarsElbowStyle.LowerLeft:
                        mytab.Location = new Point(Width - mytab.Width, 0);
                        break;
                }

                //Set the anchor so the tab will resize with the tabcontrol
                mytab.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

                //Now that we have the tab added to the tab control, we need to create a button
                //that will bring it to the front when the user clicks it.
                FlatButton mybutton = new FlatButton();
                mybutton.Width = verticalBarWidth;
                mybutton.Height = tabButtonHeight;
                mybutton.Left = 0;
                mybutton.ButtonText = mytab.Text;
                mybutton.ButtonTextAlign = ContentAlignment.BottomRight;
                mybutton.ColorFunction = mytab.ColorFunction;

                //Button beeping also needs to be handled.  I'm working on a way for new controls
                //and community made programs to easily interface with LCARSmain so they know when
                //they need to turn beeping on/off or when the colors have changed.
                mybutton.DoesBeep = false;

                //position the button based on how many buttons are already there.
                switch (elbowStyle)
                {
                    case LcarsElbowStyle.UpperRight:
                    case LcarsElbowStyle.UpperLeft:
                        mybutton.Top = tabButtonPanel.Controls.Count * (tabButtonHeight + spacing);
                        mybutton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        break;
                    case LcarsElbowStyle.LowerRight:
                    case LcarsElbowStyle.LowerLeft:
                        mybutton.Top = tabButtonPanel.Height - ((tabButtonPanel.Controls.Count + 1) * (tabButtonHeight + spacing)) + spacing;
                        mybutton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        break;
                }

                //By setting the button's 'Tag' property to the Tab it is associated with,
                //we can easily tell what button goes with what tab.
                mybutton.Tag = mytab;

                //Make the button call "TabButton_Click" whenever someone clicks on it.
                mybutton.Click += TabButton_Click;
                tabButtonPanel.Controls.Add(mybutton);
            }

            //If there's any room left after all of tabs have been made, we need to fill the
            //empty space with another button that isn't clickable.
            if ((tabButtonPanel.Controls.Count * (tabButtonHeight + spacing)) < tabButtonPanel.Height)
            {
                FlatButton myButton = new FlatButton();

                myButton.Width = verticalBarWidth;

                switch (elbowStyle)
                {
                    case LcarsElbowStyle.UpperRight:
                        myButton.Top = tabButtonPanel.Controls.Count * (tabButtonHeight + spacing);
                        myButton.Height = tabButtonPanel.Height - myButton.Top;
                        myButton.ButtonTextAlign = ContentAlignment.BottomRight;
                        myButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                        break;
                    case LcarsElbowStyle.UpperLeft:
                        myButton.Top = tabButtonPanel.Controls.Count * (tabButtonHeight + spacing);
                        myButton.Height = tabButtonPanel.Height - myButton.Top;
                        myButton.ButtonTextAlign = ContentAlignment.BottomLeft;
                        myButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                        break;
                    case LcarsElbowStyle.LowerRight:
                        myButton.Top = 0;
                        myButton.Height = tabButtonPanel.Height - (tabButtonPanel.Controls.Count * (tabButtonHeight + spacing));
                        myButton.ButtonTextAlign = ContentAlignment.TopRight;
                        myButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                        break;
                    case LcarsElbowStyle.LowerLeft:
                        myButton.Top = 0;
                        myButton.Height = tabButtonPanel.Height - (tabButtonPanel.Controls.Count * (tabButtonHeight + spacing));
                        myButton.ButtonTextAlign = ContentAlignment.TopLeft;
                        myButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                        break;
                }

                myButton.ColorFunction = LcarsColorFunction.StaticTan;
                myButton.Clickable = false;
                tabButtonPanel.Controls.Add(myButton);
            }

            //If the "SelectedTab" property isn't set, then select the first tab added to the control.
            if (SelectedTab == null | TabPages.Contains(SelectedTab) == false)
            {
                if (TabPages.Count > 0) SelectedTab = TabPages[0];
                else SelectedTab = null;
            }

            //There's a label placed on the form if no tabs have been added that explains how to 
            //add tabs to the control.  This message is only shown if there are no tabs and the 
            //control is in 'Design Time' (not executing, but rather in the designer).
            if (TabPages.Count > 0) designerMessage.Visible = false;
            else designerMessage.Visible = true;

            //Bring the tab that is currently set as selected to the front.
            ChangeTab(selectedTab);
        }

        private void ElbowChanged()
        {
            // Adjusts layout and alignment properties which are not affected by resize, but only when elbow proportions change
            // so these do not have to be called every time the control is resized but only on the more rare event of fundamental property change
            elbow.ElbowStyle = elbowStyle;
            elbow.HorizantalBarHeight = horizantalBarHeight;
            elbow.VerticalBarWidth = verticalBarWidth;
            elbow.Width = elbow.VerticalBarWidth + 20; // extra 20 for curve
            elbow.Height = elbow.HorizantalBarHeight + ElbowButtonPadding;

            tabButtonPanel.Height = Height - (elbow.Height + spacing);
            tabButtonPanel.Width = verticalBarWidth;
            horizantalBar.Height = horizantalBarHeight;
            horizantalBar.Width = Width - (elbow.Width + spacing);

            switch (elbowStyle)
            {
                case LcarsElbowStyle.UpperRight:
                    horizantalBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    horizantalBar.ButtonTextAlign = ContentAlignment.BottomLeft;
                    elbow.ButtonTextAlign = ContentAlignment.BottomRight;
                    elbow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    tabButtonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    break;
                case LcarsElbowStyle.UpperLeft:
                    horizantalBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    horizantalBar.ButtonTextAlign = ContentAlignment.BottomRight;
                    elbow.ButtonTextAlign = ContentAlignment.BottomLeft;
                    elbow.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    tabButtonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    break;
                case LcarsElbowStyle.LowerRight:
                    horizantalBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    horizantalBar.ButtonTextAlign = ContentAlignment.TopLeft;
                    elbow.ButtonTextAlign = ContentAlignment.TopRight;
                    elbow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    tabButtonPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    break;
                case LcarsElbowStyle.LowerLeft:
                    horizantalBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    horizantalBar.ButtonTextAlign = ContentAlignment.TopRight;
                    elbow.ButtonTextAlign = ContentAlignment.TopLeft;
                    elbow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                    tabButtonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    break;
            }
        }

        private void LcarsTabControl_Resize(object sender, System.EventArgs e)
        {
            // minimial layout changes for a resize when we don't have to worry about the elbow properties having changed

            tabButtonPanel.Height = Height - (elbow.Height + spacing);
            horizantalBar.Width = Width - (elbow.Width + spacing);

            switch (elbowStyle)
            {
                case LcarsElbowStyle.UpperRight:
                    horizantalBar.Left = 0;
                    horizantalBar.Top = 0;
                    elbow.Left = Width - elbow.Width;
                    elbow.Top = 0;
                    tabButtonPanel.Top = elbow.Bottom + spacing;
                    tabButtonPanel.Left = Width - tabButtonPanel.Width;
                    break;
                case LcarsElbowStyle.UpperLeft:
                    horizantalBar.Left = Width - horizantalBar.Width;
                    horizantalBar.Top = 0;
                    elbow.Left = 0;
                    elbow.Top = 0;
                    tabButtonPanel.Top = elbow.Bottom + spacing;
                    tabButtonPanel.Left = 0;
                    break;
                case LcarsElbowStyle.LowerRight:
                    horizantalBar.Left = 0;
                    horizantalBar.Top = Height - horizantalBar.Height;
                    elbow.Left = Width - elbow.Width;
                    elbow.Top = Height - elbow.Height;
                    tabButtonPanel.Top = 0;
                    tabButtonPanel.Left = Width - tabButtonPanel.Width;
                    break;
                case LcarsElbowStyle.LowerLeft:
                    horizantalBar.Left = Width - horizantalBar.Width;
                    horizantalBar.Top = Height - horizantalBar.Height;
                    elbow.Left = 0;
                    elbow.Top = Height - elbow.Height;
                    tabButtonPanel.Top = 0;
                    tabButtonPanel.Left = 0;
                    break;
            }
        }
    }
}
