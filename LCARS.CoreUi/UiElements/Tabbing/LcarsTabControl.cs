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
        Elbow topElbow = new Elbow();
        //Likewise, this is the heading bar at the very top of the control
        FlatButton myHeading = new FlatButton();
        //and the buttonPanel that holds the tab buttons along the right side
        public Panel buttonPanel = new Panel();
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
                if (selectedTab != null)
                {
                    //As long as the selected tab is something, show it.
                    ChangeTab(selectedTab);
                }
            }
        }
        private LcarsTabPage selectedTab;

        public LcarsTabControl()
        {
            tabPages = new LcarsTabPageCollection(this);
            Resize += LcarsTabControl_Resize;
            ParentChanged += LcarsTabControl_ParentChanged;
            InitializeComponent();
            //Create the controls that make up the tabcontrol.  
            
            //The heading above the tab area (very top),
            myHeading.ButtonText = "";
            myHeading.Width = Width - 126;
            myHeading.Height = 20;
            myHeading.Left = 0;
            myHeading.Top = 0;
            myHeading.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            myHeading.ColorFunction = LcarsColorFunction.LCARSDisplayOnly;
            myHeading.ButtonTextAlign = ContentAlignment.BottomLeft;
            Controls.Add(myHeading);

            //The elbow in the top right of the tabcontrol,
            topElbow.ElbowStyle = Elbow.LcarsElbowStyle.UpperRight;
            topElbow.ButtonHeight = 20;
            topElbow.ButtonWidth = 100;
            topElbow.Width = 120;
            topElbow.Height = 75;
            topElbow.Left = Width - topElbow.Width;
            topElbow.Top = 0;
            topElbow.ButtonText = "TABS";
            topElbow.ButtonTextAlign = ContentAlignment.BottomRight;
            topElbow.ColorFunction = LcarsColorFunction.StaticTan;
            topElbow.Clickable = false;
            topElbow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Controls.Add(topElbow);

            //And the panel that contains the buttons that act as the 'Tabs'.
            buttonPanel.Width = 100;
            buttonPanel.Height = Height - topElbow.Bottom;
            buttonPanel.Top = topElbow.Bottom;
            buttonPanel.Left = Width - buttonPanel.Width;
            buttonPanel.BackColor = Color.Black;
            buttonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Controls.Add(buttonPanel);

            TabPagesChanged();
            topElbow.SendToBack();
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
            Size = new System.Drawing.Size(400, 400);

            //Ok, now we can allow the control to draw again.
            ResumeLayout(false);
        }

        internal void TabPagesChanged()
        {
            //This is called whenever a tab has been added or removed from the TabPages collection.

            //Remove all of the tabs from the control
            foreach (Control mycontrol in Controls)
            {
                if (object.ReferenceEquals(mycontrol.GetType(), typeof(LcarsTabPage)))
                {
                    Controls.Remove(mycontrol);
                }
            }

            //Remove the tab's button from the buttonPanel.
            buttonPanel.Controls.Clear();

            //Add all of the tabs back to the control, so now the new ones are there, too 
            //(or old ones are gone...)
            foreach (LcarsTabPage mytab in TabPages)
            {
                Controls.Add(mytab);

                //Set the size of the tab equal to the area we want the tab to cover.  Basically,
                //everwhere our heading, elbow, and buttonpanel are not.
                mytab.Width = Width - 110;
                mytab.Height = Height - 26;
                mytab.Location = new Point(0, 26);

                //Set the anchor so the tab will resize with the tabcontrol
                mytab.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

                //Now that we have the tab added to the tab control, we need to create a button
                //that will bring it to the front when the user clicks it.
                FlatButton mybutton = new FlatButton();
                mybutton.Width = 100;
                mybutton.Height = 35;
                mybutton.ButtonText = mytab.Text;
                mybutton.ButtonTextAlign = ContentAlignment.BottomRight;
                mybutton.ColorFunction = mytab.ColorFunction;

                //Button beeping also needs to be handled.  I'm working on a way for new controls
                //and community made programs to easily interface with LCARSmain so they know when
                //they need to turn beeping on/off or when the colors have changed.
                mybutton.DoesBeep = false;

                //position the button based on how many buttons are already there.
                mybutton.Top = (buttonPanel.Controls.Count * 41) + 6;
                mybutton.Left = 0;

                //By setting the button's 'Tag' property to the Tab it is associated with,
                //we can easily tell what button goes with what tab.
                mybutton.Tag = mytab;

                //Make the button call "TabButton_Click" whenever someone clicks on it.
                mybutton.Click += TabButton_Click;
                buttonPanel.Controls.Add(mybutton);
            }

            //If there's any room left after all of tabs have been made, we need to fill the
            //empty space with another button that isn't clickable.
            if ((buttonPanel.Controls.Count * 41) + 6 < buttonPanel.Height)
            {
                FlatButton myButton = new FlatButton();

                myButton.Width = 100;

                //Start at the bottom of the last button
                myButton.Top = (buttonPanel.Controls.Count * 41) + 6;

                //and end at the bottom of the tab control.
                myButton.Height = buttonPanel.Height - myButton.Top;

                myButton.ColorFunction = LcarsColorFunction.StaticTan;
                myButton.Clickable = false;
                myButton.ButtonTextAlign = ContentAlignment.BottomRight;

                //have the button resize with the control.  We don't want it to get wider,
                //that 100px so we leave AnchorStyles.Right out of the code.
                myButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                buttonPanel.Controls.Add(myButton);
            }

            //If the "SelectedTab" property isn't set, then select the first tab added to the control.
            if (SelectedTab == null | TabPages.Contains(SelectedTab) == false)
            {
                if (TabPages.Count > 0)  SelectedTab = TabPages[0];
                else SelectedTab = null;
            }

            //There's a label placed on the form if no tabs have been added that explains how to 
            //add tabs to the control.  This message is only shown if there are no tabs and the 
            //control is in 'Design Time' (not executing, but rather in the designer).
            if (TabPages.Count > 0) designerMessage.Visible = false;
            else  designerMessage.Visible = true;

            //Bring the tab that is currently set as selected to the front.
            ChangeTab(selectedTab);
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
                myHeading.Text = tab.Text;

                //Set the selected tabs button's red alert to white and all others to normal
                foreach (FlatButton mybutton in buttonPanel.Controls)
                {
                    if (object.ReferenceEquals(mybutton.Tag, tab))
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

        public bool IsDesignerHosted
        {
            //Thanks to this (http://decav.com/blogs/andre/archive/2007/04/18/1078.aspx) 
            //website for explaining how to tell if you are in design time (editor) or run 
            //time (executing/running) whichi is what the below code does.

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

        private void LcarsTabControl_Resize(object sender, System.EventArgs e)
        {
            //Fires when the LcarsTabControl is resized.

            //For whatever reason, anchors alone weren't working, so I had to add code here to move
            //the main components of the LcarsTabControl whenever the control was resized.
            topElbow.Left = Width - topElbow.Width;
            topElbow.Top = 0;

            buttonPanel.Top = topElbow.Bottom;
            buttonPanel.Left = Width - buttonPanel.Width;
            buttonPanel.Height = Height - buttonPanel.Top;

            myHeading.Width = Width - 126;
            myHeading.Top = 0;
            myHeading.Left = 0;
        }
    }
}
