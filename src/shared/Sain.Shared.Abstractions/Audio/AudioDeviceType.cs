namespace Sain.Shared.Audio;

/// <summary>
///   Represents the different types of audio devices.
/// </summary>
public enum AudioDeviceType : byte
{
   /// <summary>The device is of an unknown type.</summary>
   Unknown,

   /// <summary>The type of the audio device is known, but it has not been defined.</summary>
   Other,

   /// <summary>The device is an audio playback device.</summary>
   Playback,

   /// <summary>The device is an audio capture device.</summary>
   Capture,
}
