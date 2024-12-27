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
   public bool IsDown
   {
      get => _isDown;
      set => Set(ref _isDown, value);
   }
   #endregion
}
