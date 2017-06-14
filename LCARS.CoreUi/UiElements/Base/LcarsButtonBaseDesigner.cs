using System.Windows.Forms.Design;

namespace LCARS.CoreUi.UiElements.Base
{
    public class LcarsButtonBaseDesigner : ControlDesigner
    {
        protected override void PostFilterProperties(System.Collections.IDictionary Properties)
        {
            Properties.Remove("AccessibleName");
            Properties.Remove("AccessibleRole");
            Properties.Remove("AccessibleDescription");
            Properties.Remove("AllowDrop");
            Properties.Remove("BackColor");
            Properties.Remove("BackgroundImageLayout");
            Properties.Remove("CausesValidation");
            Properties.Remove("ContextMenuStrip");
            Properties.Remove("Enabled");
            Properties.Remove("Font");
            Properties.Remove("ForeColor");
            Properties.Remove("GenerateMember");
            Properties.Remove("ImeMode");
            Properties.Remove("Locked");
            Properties.Remove("Margin");
            Properties.Remove("MaximumSize");
            Properties.Remove("MinimumSize");
            Properties.Remove("Modifiers");
            Properties.Remove("Padding");
            Properties.Remove("RightToLeft");
        }
    }
}
