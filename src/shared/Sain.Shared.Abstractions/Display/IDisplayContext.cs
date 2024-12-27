namespace Sain.Shared.Display;

/// <summary>
///   Represents the application's context for display information.
/// </summary>
public interface IDisplayContext : IContext
{
   #region Properties
   /// <summary>A collection of the available display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IDisplayDevice> Devices { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes all of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the name of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshNames();

   /// <summary>Refreshes the resolution of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshResolutions();

   /// <summary>Refreshes the area of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshAreas();

   /// <summary>Refreshes the bounds of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshBounds();

   /// <summary>Refreshes the refresh rate of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshRefreshRates();

   /// <summary>Refreshes the display scale of the display devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDisplayScales();

   /// <summary>Refreshes the information about which display is the primary one.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshIsPrimary();
   #endregion
}
