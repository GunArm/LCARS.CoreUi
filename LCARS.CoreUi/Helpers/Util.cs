using LCARS.CoreUi.Interfaces;
namespace LCARS.CoreUi.Helpers
{
    /// <summary>
    /// Contains utility functions for general use.
    /// </summary>
    /// <remarks>
    /// This is the module for various things that don't fit in anywhere else, but are used in multiple places.
    /// </remarks>
    public static class Util
    {
        /// <summary>
        /// Sets the beeping of a control and all subcontrols
        /// </summary>
        /// <param name="Container">The control to set the beeping for</param>
        /// <param name="Beeping">The beeping value to use</param>
        /// <remarks>
        /// If you are using windowless controls, the <see cref="Controls.WindowlessContainer">Windowless 
        /// Container</see> will set the beeping for all of its windowless controls if its own beeping is set.
        /// </remarks>
        public static void SetBeeping(System.Windows.Forms.Control Container, bool Beeping)
        {
            IBeeping temp = Container as IBeeping;
            if (temp != null) temp.Beeping = Beeping;

            foreach (System.Windows.Forms.Control myControl in Container.Controls)
            {
                SetBeeping(myControl, Beeping);
            }
        }

        /// <summary>
        /// Reloads all colors from the registry for the specified container
        /// </summary>
        /// <param name="Container">The control to reload the colors for.</param>
        public static void UpdateColors(System.Windows.Forms.Control Container)
        {
            IColorable temp = default(IColorable);
            temp = Container as IColorable;
            if (temp != null)
            {
                temp.ColorManager.ReloadColors();
            }
            foreach (System.Windows.Forms.Control myControl in Container.Controls)
            {
                UpdateColors(myControl);
            }
        }
    }
}
