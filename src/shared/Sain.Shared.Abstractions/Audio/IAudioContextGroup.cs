namespace Sain.Shared.Audio;

/// <summary>
///   Represents a context group for the application's audio contexts.
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
