using LCARS.CoreUi.Enums;

namespace LCARS.CoreUi.Interfaces
{
    public interface IAlertable
    {
        /// <summary>
        /// The alert status of the object
        /// </summary>
        /// <remarks>
        /// Sets the actual alert status of the object
        /// </remarks>
        LcarsAlert AlertState { get; set; }
        /// <summary>
        /// The color to display if RedAlert is set to Custom
        /// </summary>
        /// <remarks>
        /// Setting this property will not result in any change in the display unless
        /// the RedAlert property is set to Custom
        /// </remarks>
        System.Drawing.Color CustomAlertColor { get; set; }
    }
}
