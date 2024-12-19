namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents the application's context for capturing audio.
/// </summary>
public interface IAudioCaptureContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio capture devices.</summary>
   IDeviceCollection<IAudioCaptureDevice> Devices { get; }
   #endregion
}
