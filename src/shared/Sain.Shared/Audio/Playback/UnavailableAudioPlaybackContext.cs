namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents an <see cref="IAudioPlaybackContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableAudioPlaybackContext : BaseUnavailableContext, IAudioPlaybackContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.AudioPlayback;

   /// <inheritdoc/>
   public IDeviceCollection<IAudioPlaybackDevice> Devices
   {
      get
      {
         ThrowForUnavailable();
         Debug.Fail("Should never be reached.");
         return null; // Never reached but needed for the compiler.
      }
   }
   #endregion
}
