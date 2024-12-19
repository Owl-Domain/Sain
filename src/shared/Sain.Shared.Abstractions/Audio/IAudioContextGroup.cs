namespace Sain.Shared.Audio;

/// <summary>
///   Represents the application's context for audio.
/// </summary>
public interface IAudioContextGroup
{
   #region Properties
   /// <summary>The application's context for playing audio.</summary>
   IAudioPlaybackContext Playback { get; }

   /// <summary>The application's context for capturing audio.</summary>
   IAudioCaptureContext Capture { get; }
   #endregion
}
