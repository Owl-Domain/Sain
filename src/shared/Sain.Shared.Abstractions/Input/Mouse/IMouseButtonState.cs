namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents state information about a single mouse button.
/// </summary>
public interface IMouseButtonState : INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Properties
   /// <summary>The mouse button.</summary>
   MouseButton Button { get; }

   /// <summary>The name of the mouse button.</summary>
   string Name { get; }

   /// <summary>Whether the mouse button is currently pressed down.</summary>
   bool IsDown { get; }
   #endregion
}
