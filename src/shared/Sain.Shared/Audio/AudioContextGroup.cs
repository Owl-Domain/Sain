namespace Sain.Shared.Audio;

/// <summary>
///   Represents the application's context for audio.
/// </summary>
/// <param name="playback">The application's context for playing audio.</param>
/// <param name="capture">The application's context for capturing audio.</param>
public sealed class AudioContextGroup(IAudioPlaybackContext playback, IAudioCaptureContext capture) : IAudioContextGroup
{
   #region Properties
   /// <inheritdoc/>
   public IAudioPlaybackContext Playback { get; } = playback;
   /// <inheritdoc/>
   public IAudioCaptureContext Capture { get; } = capture;
   #endregion
}
