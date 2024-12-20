namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents the application's context for playing audio.
/// </summary>
public interface IAudioPlaybackContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IAudioPlaybackDevice> Devices { get; }
   #endregion
}
