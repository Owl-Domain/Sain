namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents the arguments used for mouse movement events.
/// </summary>
/// <param name="globalPosition">The new global position of the mouse.</param>
/// <param name="localPosition">The new local position of the mouse.</param>
public readonly struct MouseMoveEventArgs(Point globalPosition, Point localPosition)
{
   #region Properties
   /// <summary>The new global position of the mouse.</summary>
   public readonly Point GlobalPosition { get; } = globalPosition;

   /// <summary>The new loocal position of the mouse.</summary>
   public readonly Point LocalPosition { get; } = localPosition;
   #endregion
}

/// <summary>
///   Represents the arguments used for mouse button events.
/// </summary>
/// <param name="button">The mouse button that the event is for.</param>
/// <param name="name">The name of the mouse button.</param>
/// <param name="isDown">Whether the mouse button was pressed down.</param>
public readonly struct MouseButtonEventArgs(MouseButton button, string name, bool isDown)
{
   #region Properties
   /// <summary>The mouse button that the event is for.</summary>
   public readonly MouseButton Button { get; } = button;

   /// <summary>The name of the mouse button.</summary>
   public readonly string Name { get; } = name;

   /// <summary>Whether the mouse button was pressed down.</summary>
   public readonly bool IsDown { get; } = isDown;
   #endregion
}

/// <summary>
///   Represents the event handler delegate for mouse motion events.
/// </summary>
/// <param name="context">The mouse input context that raised the event.</param>
/// <param name="args">The arguments that contain information about the motion event.</param>
public delegate void MouseMoveEventHandler(IMouseInputContext context, MouseMoveEventArgs args);

/// <summary>
///   Represents the event handler delegate mouse button events.
/// </summary>
/// <param name="context">The mouse input context that raised the event.</param>
/// <param name="args">The arguments that contain information about the button event.</param>
public delegate void MouseButtonEventHandler(IMouseInputContext context, MouseButtonEventArgs args);
