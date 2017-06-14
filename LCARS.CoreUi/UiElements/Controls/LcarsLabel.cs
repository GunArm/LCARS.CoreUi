using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Interfaces;
using System;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.Designer(typeof(LCARSLabelDesigner))]
    public class LcarsLabel : System.Windows.Forms.Label, IColorable
    {
        public LcarsLabel()
        {
            this.BackColor = System.Drawing.Color.Black;
            this.Font = new System.Drawing.Font("LCARS", _textHeight, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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

        public int TextHeight
        {
            get { return _textHeight; }
            set
            {
                _textHeight = value;
                this.Font = new System.Drawing.Font("LCARS", _textHeight, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            }
        }
        private int _textHeight = 18;

        private void OnColorsUpdated(object sender, EventArgs e)
        {
            ForeColor = ColorManager.GetColor(colorFunction);
            this.Refresh();
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
