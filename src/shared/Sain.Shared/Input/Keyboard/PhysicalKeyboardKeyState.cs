namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents state information about a single physical keyboard key.
/// </summary>
/// <param name="physicalKey">The physical location of the key.</param>
/// <param name="name">The name of the key.</param>
/// <param name="isDown">Whether the key is currently pressed down.</param>
public class PhysicalKeyboardKeyState(PhysicalKey physicalKey, string name, bool isDown) : ObservableBase, IPhysicalKeyboardKeyState
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string _name = name;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isDown = isDown;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public PhysicalKey PhysicalKey { get; } = physicalKey;

   /// <inheritdoc/>
   public string Name
   {
      get => _name;
      set => Set(ref _name, value);
   }

   /// <inheritdoc/>
   public bool IsDown => _isDown;
   #endregion

   #region Methods
   /// <summary>Sets the new value for the <see cref="IsDown"/> property.</summary>
   /// <param name="value">The new value to set the <see cref="IsDown"/> property to.</param>
   /// <returns><see langword="true"/> if the value of the <see cref="IsDown"/> property changed, <see langword="false"/> otherwise.</returns>
   public bool SetIsDown(bool value) => Set(ref _isDown, value, nameof(value));
   #endregion
}
