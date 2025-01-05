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
