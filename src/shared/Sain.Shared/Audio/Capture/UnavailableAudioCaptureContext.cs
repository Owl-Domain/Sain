namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents an <see cref="IAudioCaptureContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableAudioCaptureContext : BaseUnavailableContext, IAudioCaptureContext
{
   #region Properties
   /// <inheritdoc/>
   public IDeviceCollection<IAudioCaptureDevice> Devices
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
