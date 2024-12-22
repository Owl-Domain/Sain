namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents information about a keyboard device.
/// </summary>
public interface IKeyboardDevice : IDevice
{
   #region Methods
   /// <summary>Refreshes the internal state of the keyboard.</summary>
   /// <remarks>This should also happen automatically whenever keyboard events are processed, so you shouldn't have to manually call it.</remarks>
   void Refresh();
   #endregion
}
