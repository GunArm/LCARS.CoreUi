using LCARS.CoreUi.Colors;

namespace LCARS.CoreUi.Interfaces
{
    /// <summary>
    /// Marks a class as using LCARS colors and allows those colors to be accessed.
    /// </summary>
    /// <remarks>
    /// This interface exposes the LcarsColorManager class, but that class is rarely directly set.
    /// </remarks>
    public interface IColorable
    {
        /// <summary>
        /// Exposes LcarsColorManager class used by the control for color management
        /// </summary>
        LcarsColorManager ColorsManager { get; set; }
    }
}
