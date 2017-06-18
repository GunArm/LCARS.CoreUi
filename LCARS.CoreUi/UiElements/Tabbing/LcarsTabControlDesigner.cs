using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    //The LcarsTabControlDesigner allows us to control how the LcarsTabControl behaves when in "Design 
    //Time". It's the way we allow the user to right click on the elbow and choose 'Add Tab', and 
    //to allow them to click on the tab buttons. 
    //We had to import some extra classes to be able to use this, so scroll all the way up to
    //see which ones (they are commented).
    public class LcarsTabControlDesigner : ControlDesigner
    {
        //This holds a reference to the LcarsTabControl currently being modified.
        LcarsTabControl myControl;
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            //"Component" is passed to the designer by the LcarsTabControl when it is added to a form.
            //We have to convert it from an IComponent to an LcarsTabControl.  Since it's actually 
            //already just an LcarsTabControl, we cast it's type (just to make it easy on ourselves later).
            myControl = (LcarsTabControl)component;

            //Here we're adding an event handler that will fire when something is added to our
            //control--in our case, LcarsTabPages.
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            c.ComponentRemoving += OnComponentRemoving;
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs e)
        {
            //This sub is fired when a control is removed from our LcarsTabControl.

            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            LcarsTabPage mytabPage = null;
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            int i = 0;

            //If the user is removing a TabPage
            if (e.Component is LcarsTabPage)
            {
                mytabPage = (LcarsTabPage)e.Component;
                if (myControl.TabPages.Contains(mytabPage))
                {
                    c.OnComponentChanging(myControl, null);
                    myControl.TabPages.Remove(mytabPage);
                    c.OnComponentChanged(myControl, null, null, null);
                    return;
                }
            }

            //If the user is removing the control itself, remove all of the TabPages first.
            if (object.ReferenceEquals(e.Component, myControl))
            {
                for (i = myControl.TabPages.Count - 1; i >= 0; i += -1)
                {
                    mytabPage = myControl.TabPages[i];
                    c.OnComponentChanging(myControl, null);
                    myControl.TabPages.Remove(mytabPage);
                    h.DestroyComponent(mytabPage);
                    c.OnComponentChanged(myControl, null, null, null);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            //Unhook events
            c.ComponentRemoving -= OnComponentRemoving;

            base.Dispose(disposing);
        }

        public override ICollection AssociatedComponents
        {
            //This and the 'OnComponentRemoving' sub are used to keep the "TabPages" Collection in
            //tact when the form is reloaded (called 'serialization').
            get { return myControl.TabPages; }
        }

        public override DesignerVerbCollection Verbs
        {
            //Adds the 'Add Tab' option to the right click (context) menu of the LcarsTabControl and
            //associates the menu item with the 'AddTab' sub.
            get
            {
                DesignerVerbCollection myVerbs = new DesignerVerbCollection();

                //Create the verb to add a tab
                myVerbs.Add(new DesignerVerb("&Add Tab", AddTab));

                //Create the verb to move a tab down
                myVerbs.Add(new DesignerVerb("Move Tab &Down", MoveTabDown));

                //Create the verb to move a tab up
                myVerbs.Add(new DesignerVerb("Move Tab &Up", MoveTabUp));

                return myVerbs;
            }
        }

        private void AddTab(object sender, EventArgs e)
        {
            //I don't understand everthing that goes on here.  I know that the end result is a new
            //tab being added to the LcarsTabPageControl.

            //Our new tab
            LcarsTabPage mytabpage = null;

            //An instance of the IDesignerHost interface which allows us to create transactions
            //in the designer.
            IDesignerHost myHost = (IDesignerHost)GetService(typeof(IDesignerHost));

            //A transaction.  From what I read, I believe that this just allows our "Add Tab" to 
            //be undone using the "Undo" button.
            DesignerTransaction myTransaction = default(DesignerTransaction);

            //This is used to call events that are also used in the Undo and Redo functions.
            IComponentChangeService myChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            //Set the name of the transaction.  I believe this also sets the starting point.
            myTransaction = myHost.CreateTransaction("Add Tab");
            //Create a new LcarsTabPage using the DesignerHost.
            mytabpage = (LcarsTabPage)myHost.CreateComponent(typeof(LcarsTabPage));
            //Let Visual Studio know we changed a component (our LcarsTabControl)
            myChangeService.OnComponentChanging(myControl, null);
            //Add the tabpage to the tabcontrol.
            myControl.TabPages.Add(mytabpage);
            //Visual Studio... we changed it again.
            myChangeService.OnComponentChanged(myControl, null, null, null);
            //Finish the transaction so it knows when the "undo" should stop.
            myTransaction.Commit();
        }

        private void MoveTabDown(object sender, EventArgs e)
        {
            int current = myControl.TabPages.IndexOf(myControl.SelectedTab);
            if (myControl.TabPages.Count > 1 & current < myControl.TabPages.Count - 1)
            {
                // Setup transaction
                IDesignerHost myHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                DesignerTransaction myTransaction = default(DesignerTransaction);
                IComponentChangeService myChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                myTransaction = myHost.CreateTransaction("Move Tab Down");
                myChangeService.OnComponentChanging(myControl, null);

                //Move tab
                myControl.TabPages.MoveDown(myControl.SelectedTab);

                // Complete transaction
                myChangeService.OnComponentChanged(myControl, null, null, null);
                myTransaction.Commit();
            }
        }

        private void MoveTabUp(object sender, EventArgs e)
        {
            int current = myControl.TabPages.IndexOf(myControl.SelectedTab);
            if (myControl.TabPages.Count > 1 & current > 0)
            {
                // Setup transaction
                IDesignerHost myHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                DesignerTransaction myTransaction = default(DesignerTransaction);
                IComponentChangeService myChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                myTransaction = myHost.CreateTransaction("Move Tab Up");
                myChangeService.OnComponentChanging(myControl, null);

                //Move tab
                myControl.TabPages.MoveUp(myControl.SelectedTab);

                // Complete transaction
                myChangeService.OnComponentChanged(myControl, null, null, null);
                myTransaction.Commit();
            }
        }

        protected override bool GetHitTest(Point point)
        {
            //GetHitTest is allows us to make controls clickable during design time.  For example,
            //on the LcarsTabControl, you'll notice that even in the designer, you can switch tabs
            //by clicking on the relevant tab button.  Take this sub out and that won't work any
            //more.

            //Make sure that we have been added to a control. Otherwise, there's no point.

            if (Control == null) return false;
            //Convert the point from screen coordinates to coordinates based on our LcarsTabControl
            Point mypoint = ((LcarsTabControl)Control).tabButtonPanel.PointToClient(point);
            //Find out what control is under the mouse at that point.
            Control child = ((LcarsTabControl)Control).tabButtonPanel.GetChildAtPoint(mypoint);

            //If there's nothing under the mouse, don't bother.
            if (child == null) return false;
            if (child is FlatButton)
            {
                //If the control under the mouse is a flatbutton, let us click on it.
                return true;
            }
            else
            {
                //Otherwise... no clicky.
                return base.GetHitTest(point);
            }
        }
    }
}
