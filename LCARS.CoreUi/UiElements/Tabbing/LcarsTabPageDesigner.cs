using System.ComponentModel;
using System.Windows.Forms.Design;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    public class LcarsTabPageDesigner : ParentControlDesigner
    {
        //Specifies how the LcarsPage interacts with the user in the Windows Form Designer.
        //Basically, all this class does is locks the LcarsControl so it can't be moved or
        //resized.  

        //Our parent control
        LcarsTabPage myControl;
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            myControl = (LcarsTabPage)component;
        }

        public override SelectionRules SelectionRules
        {
            //SelectionRules.Locked prevents the user from being able to move or resize the 
            //LcarsPage in the designer.
            get
            {
                return SelectionRules.Locked;
            }
        }
    }
}
