namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the arguments for a window move event.
/// </summary>
/// <param name="oldPosition">The old position of the window.</param>
/// <param name="newPosition">The new position of the window.</param>
public readonly struct NativeWindowMovedEventArgs(Point oldPosition, Point newPosition)
{
   #region Properties
   /// <summary>The old position of the window.</summary>
   public readonly Point OldPosition { get; } = oldPosition;

   /// <summary>The new position of the window.</summary>
   public readonly Point NewPosition { get; } = newPosition;
   #endregion
}

/// <summary>
///   Represents the arguments for a window resize event.
/// </summary>
/// <param name="oldSize">The old size of the window.</param>
/// <param name="newSize">The new size of the window.</param>
public readonly struct NativeWindowResizedEventArgs(Size oldSize, Size newSize)
{
   #region Properties
   /// <summary>The old size of the window.</summary>
   public readonly Size OldSize { get; } = oldSize;

   /// <summary>The new size of the window.</summary>
   public readonly Size NewSize { get; } = newSize;
   #endregion
}

/// <summary>
///   Represents the arguments for a window state change event.
/// </summary>
/// <param name="oldState">The old state of the window.</param>
/// <param name="newState">The new state of the window.</param>
public readonly struct NativeWindowStateChangedEventArgs(NativeWindowState oldState, NativeWindowState newState)
{
   #region Properties
   /// <summary>The old state of the window.</summary>
   public readonly NativeWindowState OldState { get; } = oldState;

   /// <summary>The new state of the window.</summary>
   public readonly NativeWindowState NewState { get; } = newState;
   #endregion
}

/// <summary>
///   Represents the arguments for a window title change event.
/// </summary>
/// <param name="oldTitle">The old title of the window.</param>
/// <param name="newTitle">The new title of the window.</param>
public readonly struct NativeWindowTitleChangedEventArgs(string oldTitle, string newTitle)
{
   #region Properties
   /// <summary>The old title of the window.</summary>
   public readonly string OldTitle { get; } = oldTitle;

   /// <summary>The new title of the window.</summary>
   public readonly string NewTitle { get; } = newTitle;
   #endregion
}

/// <summary>
///   Represents the arguments for a window close request event.
/// </summary>
public class NativeWindowCloseRequestedEventArgs : EventArgs
{
   #region Properties
   /// <summary>Whether the window should be closed or not.</summary>
   public bool ShouldClose { get; set; } = true;
   #endregion
}

/// <summary>
///   Represents the arguments for a window mouse enter event.
/// </summary>
/// <param name="position">The first position of the mouse inside of the window when the mouse entered it.</param>
public readonly struct NativeWindowMouseEnteredEventArgs(Point position)
{
   #region Properties
   /// <summary>The first position of the mouse inside of the window when the mouse entered it.</summary>
   public readonly Point Position { get; } = position;
   #endregion
}

/// <summary>
///   Represents the arguments for a window mouse leave eevent.
/// </summary>
/// <param name="position">The last position of the mouse inside of the window when the mouse left it.</param>
public readonly struct NativeWindowMouseLeftEventArgs(Point position)
{
   #region Properties
   /// <summary>The last position of the mouse inside of the window when the mouse left it.</summary>
   public readonly Point Position { get; } = position;
   #endregion
}

/// <summary>
///   Represents the arguments for a window mouse move events.
/// </summary>
/// <param name="oldPosition">The old position of the mouse inside of the window.</param>
/// <param name="newPosition">The new position of the mouse inside of the window.</param>
public readonly struct NativeWindowMouseMovedEventArgs(Point oldPosition, Point newPosition)
{
   #region Properties
   /// <summary>The old position of the mouse inside of the window.</summary>
   public readonly Point OldPosition { get; } = oldPosition;

   /// <summary>The new position of the mouse inside of the window.</summary>
   public readonly Point NewPosition { get; } = newPosition;
   #endregion
}

/// <summary>
///   Represents the arguments for a window mouse button event.
/// </summary>
/// <param name="position">The position of the mouse inside of the window when the event was raised.</param>
/// <param name="button">The mouse button that the event is for.</param>
/// <param name="name">The name of the mouse button.</param>
/// <param name="isDown">Whether the mouse button was pressed down.</param>
public readonly struct NativeWindowMouseButtonEventArgs(Point position, MouseButton button, string name, bool isDown)
{
   #region Properties
   /// <summary>The position of the mouse inside of the window when the event was raised.</summary>
   public readonly Point Position { get; } = position;

   /// <summary>The mouse button that the event is for.</summary>
   public readonly MouseButton Button { get; } = button;

   /// <summary>The name of the mouse button.</summary>
   public readonly string Name { get; } = name;

   /// <summary>Whether the mouse button was pressed down.</summary>
   public readonly bool IsDown { get; } = isDown;
   #endregion
}

/// <summary>
///   Represents the arguments for a window mouse wheel scroll event.
/// </summary>
/// <param name="position">The position of the mouse inside of the window when the event was raised.</param>
/// <param name="deltaX">The amount that the mouse wheel was scrolled horizontally.</param>
/// <param name="deltaY">The amount that the mouse wheel was scrolled vertically.</param>
public readonly struct NativeWindowMouseWheelScrolledEventArgs(Point position, double deltaX, double deltaY)
{
   #region Properties
   /// <summary>The position of the mouse inside of the window when the event was raised.</summary>
   public readonly Point Position { get; } = position;

   /// <summary>The amount that the mouse wheel was scrolled horizontally.</summary>
   public readonly double DeltaX { get; } = deltaX;

   /// <summary>The amount that the mouse wheel was scrolled vertically.</summary>
   public readonly double DeltaY { get; } = deltaY;
   #endregion
}

/// <summary>
///   Represents the arguments for a window keyboard key event.
/// </summary>
/// <param name="physicalKey">The physical representation of the key.</param>
/// <param name="physicalKeyName">The name of the <paramref name="physicalKey"/>.</param>
/// <param name="virtualKey">The virtual representation of the key.</param>
/// <param name="virtualKeyName">The name of the <paramref name="virtualKey"/>.</param>
/// <param name="modifiers">Any of the used key modifiers.</param>
/// <param name="isDown">Whether the key was pressed down.</param>
/// <param name="isRepeat">Whether the key event is a repeat event.</param>
public readonly struct NativeWindowKeyboardKeyEventArgs(
   PhysicalKey physicalKey,
   string physicalKeyName,
   VirtualKey virtualKey,
   string virtualKeyName,
   KeyModifiers modifiers,
   bool isDown,
   bool isRepeat)
{
   #region Properties
   /// <summary>The physical representation of the key.</summary>
   public readonly PhysicalKey PhysicalKey { get; } = physicalKey;

   /// <summary>The name of the physical key.</summary>
   public readonly string PhysicalKeyName { get; } = physicalKeyName;

   /// <summary>The virtual representation of the key.</summary>
   public readonly VirtualKey VirtualKey { get; } = virtualKey;

   /// <summary>Any of the used key modifiers.</summary>
   public readonly KeyModifiers Modifiers { get; } = modifiers;

   /// <summary>The name of the virtual key.</summary>
   public readonly string VirtualKeyName { get; } = virtualKeyName;

   /// <summary>Whether the key was pressed down.</summary>
   public readonly bool IsDown { get; } = isDown;

   /// <summary>Whether the key event is a repeat event.</summary>
   /// <remarks>A repeat event is typically caused by holding down a key.</remarks>
   public readonly bool IsRepeat { get; } = isRepeat;
   #endregion
}

/// <summary>Represents the event handler for a window move event.</summary>
/// <param name="window">The window that has moved.</param>
/// <param name="args">The arguments for the move event.</param>
public delegate void NativeWindowMovedEventHandler(INativeWindow window, NativeWindowMovedEventArgs args);

/// <summary>Represents the event handler for a window resize event.</summary>
/// <param name="window">The window that has resized.</param>
/// <param name="args">The arguments for the resize event.</param>
public delegate void NativeWindowResizedEventHandler(INativeWindow window, NativeWindowResizedEventArgs args);

/// <summary>Represents the event handler for a window redraw request event.</summary>
/// <param name="window">The window that is requesting to be redrawn.</param>
public delegate void NativeWindowRedrawRequestedEventHandler(INativeWindow window);

/// <summary>Represents the event handler for a window state change event.</summary>
/// <param name="window">The window that has changed state.</param>
/// <param name="args">The arguments for the state change event.</param>
public delegate void NativeWindowStateChangedEventHandler(INativeWindow window, NativeWindowStateChangedEventArgs args);

/// <summary>Represents the event handler for a window title change event.</summary>
/// <param name="window">The window of which the title has changed.</param>
/// <param name="args">The arguments for the title change event.</param>
public delegate void NativeWindowTitleChangedEventHandler(INativeWindow window, NativeWindowTitleChangedEventArgs args);

/// <summary>Represents the event handler for a window close request event.</summary>
/// <param name="window">The window that has been requested to close.</param>
/// <param name="args">The arguments for the close request event.</param>
public delegate void NativeWindowCloseRequestedEventHandler(INativeWindow window, NativeWindowCloseRequestedEventArgs args);

/// <summary>Represents the event handler for a window closing event.</summary>
/// <param name="window">The window that is closing.</param>
public delegate void NativeWindowClosingEventHandler(INativeWindow window);

/// <summary>Represents the event handler for a window closed event.</summary>
/// <param name="window">The window that has closed.</param>
public delegate void NativeWindowClosedEventHandler(INativeWindow window);

/// <summary>Represents the event handler for a window got-focus event.</summary>
/// <param name="window">The window that has obtained focus.</param>
public delegate void NativeWindowGotFocusEventHandler(INativeWindow window);

/// <summary>Represents the event handler for a window lost-focus event.</summary>
/// <param name="window">The window that has lost focus.</param>
public delegate void NativeWindowLostFocusEventHandler(INativeWindow window);

/// <summary>Represents the event handler for a window mouse enter event.</summary>
/// <param name="window">The window that the mouse has entered.</param>
/// <param name="args">The arguments for the window mouse enter event.</param>
public delegate void NativeWindowMouseEnteredEventHandler(INativeWindow window, NativeWindowMouseEnteredEventArgs args);

/// <summary>Represents the event handler for a window mouse leave event.</summary>
/// <param name="window">The window that the mouse has left.</param>
/// <param name="args">The arguments for the window mouse leave event.</param>
public delegate void NativeWindowMouseLeftEventHandler(INativeWindow window, NativeWindowMouseLeftEventArgs args);

/// <summary>Represents the event handler for a window mouse move event.</summary>
/// <param name="window">The window that the mouse has moved in.</param>
/// <param name="args">The arguments for the window mouse move event.</param>
public delegate void NativeWindowMouseMovedEventHandler(INativeWindow window, NativeWindowMouseMovedEventArgs args);

/// <summary>Represents the event handler for a window mouse button event.</summary>
/// <param name="window">The window that the mouse button was used in.</param>
/// <param name="args">The arguments for the window mouse button event.</param>
public delegate void NativeWindowMouseButtonEventHandler(INativeWindow window, NativeWindowMouseButtonEventArgs args);

/// <summary>Represents the event handler for a window mouse wheel scroll event.</summary>
/// <param name="window">The window that the mouse wheel was scrolled in.</param>
/// <param name="args">The arguments for the window mouse wheel scroll event.</param>
public delegate void NativeWindowMouseWheelScrolledEventHandler(INativeWindow window, NativeWindowMouseWheelScrolledEventArgs args);

/// <summary>Represents the event handler for a window keyboard key event.</summary>
/// <param name="window">The window that received the keyboard key event.</param>
/// <param name="args">The arguments for the window keyboard key event.</param>
public delegate void NativeWindowKeyboardKeyEventHandler(INativeWindow window, NativeWindowKeyboardKeyEventArgs args);
