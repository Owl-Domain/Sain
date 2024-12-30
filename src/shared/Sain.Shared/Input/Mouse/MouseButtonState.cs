namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents state information about a single mouse button.
/// </summary>
/// <param name="button">The mouse button.</param>
/// <param name="name">The name of the mouse button.</param>
/// <param name="isDown">Whether the mouse button is currently pressed down.</param>
public sealed class MouseButtonState(MouseButton button, string name, bool isDown) : ObservableBase, IMouseButtonState
{
   #region Fields
   private string _name = name;
   private bool _isDown = isDown;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public MouseButton Button { get; } = button;

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
