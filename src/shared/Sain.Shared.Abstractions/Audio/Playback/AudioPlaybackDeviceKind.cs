namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents the different kinds of audio playback devices.
/// </summary>
public enum AudioPlaybackDeviceKind : byte
{
   /// <summary>The audio device is of an unknown kind.</summary>
   Unknown,

   /// <summary>The kind of the audio device is known, but it has not been defined.</summary>
   Other,

   /// <summary>The audio device is a pair of headphones.</summary>
   /// <remarks>Might not actually be a <i>pair</i> of headphones.</remarks>
   Headphones,

   /// <summary>The audio device is a pair of speakers.</summary>
   /// <remarks>Might not actually be a <i>pair</i> of speakers.</remarks>
   Speakers,
}
