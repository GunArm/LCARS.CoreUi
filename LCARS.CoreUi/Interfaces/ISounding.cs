using LCARS.CoreUi.Assets.Access;

namespace LCARS.CoreUi.Interfaces
{
    /// <summary>
    /// Marks a class as having a beeping property
    /// </summary>
    /// <remarks>
    /// This is most often used to allow for a control to match the global beeping setting. Implementation
    /// will vary based on whether there are subcontrols.
    /// </remarks>
    public interface ISounding
    {
        /// <summary>
        /// Sets the beeping property of the control.
        /// </summary>
        bool DoesSound { get; set; }
        LcarsSoundAsset SoundAsset { get; set; }
    }
}
