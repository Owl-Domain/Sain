namespace Sain.Shared.Audio;

/// <summary>
///   Represents information about an audio device.
/// </summary>
public interface IAudioDevice : IDevice
{
   #region Properties
   /// <summary>The type of the audio device.</summary>
   AudioDeviceType DeviceType { get; }

   /// <summary>The amount of channels that the audio device supports.</summary>
   int Channels { get; }

   /// <summary>The amount of samples that the audio device can play per second.</summary>
   int Frequency { get; }
   #endregion
}
