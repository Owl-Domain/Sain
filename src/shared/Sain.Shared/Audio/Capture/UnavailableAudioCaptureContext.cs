namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents an <see cref="IAudioCaptureContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableAudioCaptureContext : BaseUnavailableContext, IAudioCaptureContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.AudioCapture;

   /// <inheritdoc/>
   public IDeviceCollection<IAudioCaptureDevice> Devices
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
