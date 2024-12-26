namespace Sain.Shared.Audio.Capture;

/// <summary>
///   Represents the application's context for capturing audio.
/// </summary>
public interface IAudioCaptureContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IAudioCaptureDevice> Devices { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes all of the audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of the audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the names of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshName();

   /// <summary>Refreshes the channel counts of the audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshChannels();

   /// <summary>Refreshes the frequencies of the audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshFrequencies();

   /// <summary>Refreshes the kinds of the audio capture devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshKinds();
   #endregion
}
