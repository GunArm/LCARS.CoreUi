using LCARS.CoreUi.Enums;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    //Associate the LcarsPageDesigner with the LcarsPage class, Associate it also with the
    //LcarsPageConverter (used in the serialization process), don't show it in the toolbox, and
    //allow LcarsPages to have other controls drug-and-dropped onto them.
    [Designer(typeof(LcarsTabPageDesigner)), TypeConverter(typeof(LcarsTabPageConverter)), DesignTimeVisible(false), ToolboxItem(false), Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design", typeof(IDesigner))]
    public class LcarsTabPage : Control
    {
        //The text of the LcarsPage (what shows on the button and the heading).
        string text;

        LcarsColorFunction colorFunction = LcarsColorFunction.MiscFunction;
        public LcarsTabPage()
        {
            text = "NEW TAB";
            //This is LCARS after all.  We can't settle for a gray background.
            BackColor = System.Drawing.Color.Black;
        }

        public LcarsTabPage(string name)
        {
            text = name;
            BackColor = System.Drawing.Color.Black;
        }

        public override string Text
        {
            //Gets or sets the tabs button and heading text.
            get { return text; }
            set
            {
                //It's LCARS.  Text is UPPER CASE! unless it isn't...
                text = value.ToUpper();

                if (Parent != null)
                {
                    //Update the tabs
                    ((LcarsTabControl)Parent).TabPagesChanged();
                }
            }
        }

        [DefaultValue(LcarsColorFunction.MiscFunction)]
        public LcarsColorFunction ColorFunction
        {
            get { return colorFunction; }
            set
            {
                colorFunction = value;
                if (Parent != null)
                {
                    ((LcarsTabControl)Parent).TabPagesChanged();
                }
            }
        }
    }
}
