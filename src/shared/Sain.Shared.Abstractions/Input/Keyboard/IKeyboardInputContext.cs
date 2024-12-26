namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the application's context for keyboard input.
/// </summary>
public interface IKeyboardInputContext : IContext
{
   #region Properties
   /// <summary>A collection of the available keyboard devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IKeyboardDevice> Devices { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes all of the keyboard devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();
   #endregion
}
