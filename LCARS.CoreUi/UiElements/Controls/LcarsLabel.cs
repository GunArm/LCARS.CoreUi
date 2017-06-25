using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Interfaces;
using System;
using System.Drawing;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.Designer(typeof(LCARSLabelDesigner))]
    public class LcarsLabel : System.Windows.Forms.Label, IColorable
    {
        public LcarsLabel()
        {
            UseCompatibleTextRendering = true;
            BackColor = Color.Black;
            Font = FontProvider.Lcars(textHeight);
            ColorManager = new LcarsColorManager();
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null) colorManager.ColorsUpdated -= OnColorsUpdated;
                colorManager = value;
                colorManager.ColorsUpdated += OnColorsUpdated;
                OnColorsUpdated(this, null);
            }
        }
        private LcarsColorManager colorManager;

        public LcarsColorFunction ColorFunction
        {
            get { return colorFunction; }
            set
            {
                colorFunction = value;
                OnColorsUpdated(this, null);
            }
        }
        private LcarsColorFunction colorFunction = LcarsColorFunction.Orange;

        public new Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textHeight = (int)value.SizeInPoints;
            }
        }

        public int TextHeight
        {
            get { return textHeight; }
            set
            {
                textHeight = value;
                Font = new Font(Font.FontFamily, textHeight, FontStyle.Regular, GraphicsUnit.Point);
            }
        }
        private int textHeight = 18;

        private void OnColorsUpdated(object sender, EventArgs e)
        {
            ForeColor = ColorManager.GetColor(colorFunction);
            Refresh();
        }
    }

    #region " Designer "
    internal class LCARSLabelDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        protected override void PostFilterProperties(System.Collections.IDictionary properties)
        {
            base.PostFilterProperties(properties);
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("Font");
        }
    }
    #endregion
}
