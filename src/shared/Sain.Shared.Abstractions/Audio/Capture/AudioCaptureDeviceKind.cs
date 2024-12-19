namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents the different kinds of audio aapture devices.
/// </summary>
public enum AudioCaptureDeviceKind : byte
{
   /// <summary>The audio device is of an unknown kind.</summary>
   Unknown,

   /// <summary>The kind of the audio device is known, but it has not been defined.</summary>
   Other,

   /// <summary>The audio device is a microphone.</summary>
   Microphone,
}
