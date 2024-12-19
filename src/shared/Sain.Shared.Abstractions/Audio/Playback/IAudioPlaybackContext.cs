namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents the application's context for playing audio.
/// </summary>
public interface IAudioPlaybackContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio playback devices.</summary>
   IDeviceCollection<IAudioPlaybackDevice> Devices { get; }
   #endregion
}
