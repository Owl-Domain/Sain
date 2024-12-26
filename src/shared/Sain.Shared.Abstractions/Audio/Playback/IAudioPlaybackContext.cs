namespace Sain.Shared.Audio.Playback;

/// <summary>
///   Represents the application's context for playing audio.
/// </summary>
public interface IAudioPlaybackContext : IContext
{
   #region Properties
   /// <summary>A collection of the available audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IAudioPlaybackDevice> Devices { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes all of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the names of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshName();

   /// <summary>Refreshes the channel counts of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshChannels();

   /// <summary>Refreshes the frequencies of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshFrequencies();

   /// <summary>Refreshes the kinds of the audio playback devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshKinds();
   #endregion
}
