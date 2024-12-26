namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents information about an audio playback device.
/// </summary>
public interface IAudioPlaybackDevice : IAudioDevice
{
   #region Properties
   /// <summary>The kind of the audio playback device.</summary>
   AudioPlaybackDeviceKind Kind { get; }
   AudioDeviceType IAudioDevice.DeviceType => AudioDeviceType.Playback;
   #endregion

   #region Methods
   /// <summary>Refreshes the kind of the audio playback device.</summary>
   void RefreshKind();
   #endregion
}
