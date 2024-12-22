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
}
