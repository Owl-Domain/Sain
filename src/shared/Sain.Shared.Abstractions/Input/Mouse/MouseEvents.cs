namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents the arguments used for mouse motion events.
/// </summary>
/// <param name="globalPosition">The new global position of the mouse device.</param>
/// <param name="localPosition">The new local position of the mouse device.</param>
public readonly struct MouseDeviceMotionEventArgs(Point globalPosition, Point localPosition)
{
   #region Fields
   /// <summary>The new global position of the mouse device.</summary>
   public readonly Point GlobalPosition = globalPosition;

   /// <summary>The new loocal position of the mouse device.</summary>
   public readonly Point LocalPosition = localPosition;
   #endregion
}

/// <summary>
///   Represents the arguments used for mouse button events.
/// </summary>
/// <param name="button">The mouse button that the event is for.</param>
/// <param name="state">The new state of the mouse <paramref name="button"/>.</param>
public readonly struct MouseDeviceButtonEventArgs(MouseButton button, IMouseButtonState state)
{
   #region Fields
   /// <summary>The mouse button that the event is for.</summary>
   public readonly MouseButton Button = button;

   /// <summary>The new state of the mouse button.</summary>
   public readonly IMouseButtonState State = state;
   #endregion
}

/// <summary>
///   Represents the event handler delegate for mouse motion events.
/// </summary>
/// <param name="mouse">The mouse device that raised the event.</param>
/// <param name="args">The arguments that contain information about the motion event.</param>
public delegate void MouseDeviceMotionEventHandler(IMouseDevice mouse, MouseDeviceMotionEventArgs args);


/// <summary>
///   Represents the event handler delegate mouse button events.
/// </summary>
/// <param name="mouse">The mouse device that raised the event.</param>
/// <param name="args">The arguments that contain information about the button event.</param>
public delegate void MouseDeviceButtonEventHandler(IMouseDevice mouse, MouseDeviceButtonEventArgs args);
