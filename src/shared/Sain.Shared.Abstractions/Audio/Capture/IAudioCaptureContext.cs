namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents the application's context for capturing audio.
/// </summary>
public interface IAudioCaptureContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IAudioCaptureDevice> Devices { get; }
   #endregion
}
