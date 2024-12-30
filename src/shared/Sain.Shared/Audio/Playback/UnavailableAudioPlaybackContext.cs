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
         return default; // Never reached but needed for the compiler.
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void RefreshDevices() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshDeviceIds() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshName() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshChannels() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshFrequencies() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshKinds() => ThrowForUnavailable();
   #endregion
}
