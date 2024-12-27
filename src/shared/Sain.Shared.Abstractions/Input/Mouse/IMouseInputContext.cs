namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents the application's context for mouse input.
/// </summary>
public interface IMouseInputContext : IContext
{
   #region Properties
   /// <summary>A collection of the available mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IMouseDevice> Devices { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes all of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the name of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshNames();

   /// <summary>Refreshes the position of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshPositions();

   /// <summary>Refreshes the button states of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshButtons();

   /// <summary>Refreshes the capture state of the mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshIsCaptured();

   /// <summary>Refreshes the visibility of the mouse cursors.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshCursorVisibility();
   #endregion
}
