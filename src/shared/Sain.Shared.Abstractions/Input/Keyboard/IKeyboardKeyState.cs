namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents state information about a single keyboard key.
/// </summary>
public interface IKeyboardKeyState
{
   #region Properties
   /// <summary>The physical location of the key.</summary>
   PhysicalKey PhysicalKey { get; }

   /// <summary>The name of the key.</summary>
   string Name { get; }

   /// <summary>Whether the key is currently pressed down.</summary>
   bool IsDown { get; }
   #endregion
}
