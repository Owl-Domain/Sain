namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents information about an audio capture device.
/// </summary>
public interface IAudioCaptureDevice : IAudioDevice
{
   #region Properties
   /// <summary>The kind of the audio capture device.</summary>
   AudioCaptureDeviceKind Kind { get; }
   AudioDeviceType IAudioDevice.DeviceType => AudioDeviceType.Capture;
   #endregion
}
