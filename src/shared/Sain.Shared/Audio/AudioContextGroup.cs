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

   #region Functions
   /// <summary>Creates a new <see cref="AudioContextGroup"/> using the given application <paramref name="context"/>.</summary>
   /// <param name="context">The application context to use when creating the audio context group.</param>
   /// <returns>The created audio context group.</returns>
   public static IAudioContextGroup Create(IApplicationContext context)
   {
      IAudioPlaybackContext playback = context.GetContext<IAudioPlaybackContext>();
      IAudioCaptureContext capture = context.GetContext<IAudioCaptureContext>();

      return new AudioContextGroup(playback, capture);
   }
   #endregion
}
