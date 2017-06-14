using System.Drawing;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Controls
{
    public class LcarsList : ListBox
    {
        public LcarsList() : base()
        {
            ResetFont();
            ResetBackColor();
            ResetForeColor();
            ResetBorderStyle();
            base.DrawMode = DrawMode.OwnerDrawFixed;
        }

        #region " Font "
        protected static Font defFont = new Font("LCARS", 16, FontStyle.Regular, GraphicsUnit.Point);
        public override void ResetFont()
        {
            Font = defFont;
        }

        public bool ShouldSerializeFont()
        {
            return !defFont.Equals(Font);
        }

        public override System.Drawing.Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                base.ItemHeight = value.Height + DefaultPadding.Vertical;
            }
        }
        #endregion

        #region " Back Color "
        protected static Color defBackColor = Color.Black;
        public override void ResetBackColor()
        {
            base.BackColor = defBackColor;
        }

        public bool ShouldSerializeBackColor()
        {
            return !(defBackColor == base.BackColor);
        }
        #endregion

        #region " Fore Color "
        protected static Color defForeColor = Color.Orange;
        public override void ResetForeColor()
        {
            base.ForeColor = defForeColor;
        }

        public bool ShouldSerializeForeColor()
        {
            return !(base.ForeColor == defForeColor);
        }
        #endregion

        #region " Border "
        protected static BorderStyle defBorder = BorderStyle.FixedSingle;
        public void ResetBorderStyle()
        {
            base.BorderStyle = defBorder;
        }

        public bool ShouldSerializeBorderStyle()
        {
            return base.BorderStyle != defBorder;
        }
        #endregion

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, Color.White, Color.Black);
            }

            e.DrawBackground();
            Brush b = new SolidBrush(e.ForeColor);
            if (e.Index >= this.Items.Count)
            {
                //Draw the name only for design-time
                if (this.DesignMode) e.Graphics.DrawString(this.Name, e.Font, b, e.Bounds, StringFormat.GenericDefault);
            }
            else
            {
                StringFormat format = new StringFormat(StringFormat.GenericDefault);
                format.SetTabStops(0, TabStops);
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, b, e.Bounds, format);
            }
        }

        public virtual float[] TabStops
        {
            get { return tabStops; }
            set
            {
                tabStops = value;
                this.Invalidate();
            }
        }
        private float[] tabStops = { };

        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }
    }
}
