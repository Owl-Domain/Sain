namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a native window created by the <see cref="IDesktopWindowingContext"/>.
/// </summary>
public interface INativeWindow
{
   #region Properties
   /// <summary>The unique id of the window.</summary>
   Guid Id { get; }

   /// <summary>Whether the window is currently open.</summary>
   bool IsOpen { get; }

   /// <summary>Whether the window needs to be redrawn.</summary>
   bool NeedsRedraw { get; }

   /// <summary>The position of the window in the virtual screen space.</summary>
   Point Position { get; set; }

   /// <summary>The actual size of the window.</summary>
   Size ActualSize { get; set; }

   /// <summary>The title of the window.</summary>
   string Title { get; set; }

   /// <summary>The current state of the window.</summary>
   NativeWindowState State { get; }

   /// <summary>Whether the mouse is currently inside the window.</summary>
   bool IsMouseInside { get; }

   /// <summary>Whether the window is currently focused.</summary>
   bool HasFocus { get; }

   /// <summary>Gets the last known mouse position inside of the window.</summary>
   Point MousePosition { get; }

   /// <summary>The current visibility of the window.</summary>
   NativeWindowVisibility Visibility { get; }
   #endregion

   #region Events
   /// <summary>Raised when the window is moved.</summary>
   event NativeWindowMovedEventHandler? Moved;

   /// <summary>Raised when the window is resized.</summary>
   event NativeWindowResizedEventHandler? Resized;

   /// <summary>Raised when the window is requesting to be redrawn.</summary>
   event NativeWindowRedrawRequestedEventHandler? RedrawRequested;

   /// <summary>Raised when the window state is changed.</summary>
   event NativeWindowStateChangedEventHandler? StateChanged;

   /// <summary>Raised when the window title is changed.</summary>
   event NativeWindowTitleChangedEventHandler? TitleChanged;

   /// <summary>Raised when the window has received a request to be closed.</summary>
   event NativeWindowCloseRequestedEventHandler? CloseRequested;

   /// <summary>Raised when the window is closing.</summary>
   /// <remarks>At this point, window properties should still be available.</remarks>
   event NativeWindowClosingEventHandler? Closing;

   /// <summary>Raised when the window is closed.</summary>
   /// <remarks>At this point, window properties are no longer available.</remarks>
   event NativeWindowClosedEventHandler? Closed;

   /// <summary>Raised when the mouse enters the window.</summary>
   event NativeWindowMouseEnteredEventHandler? MouseEntered;

   /// <summary>Raised when the mouse leaves the window.</summary>
   event NativeWindowMouseLeftEventHandler? MouseLeft;

   /// <summary>Raised when the mouse is moved over the window.</summary>
   event NativeWindowMouseMovedEventHandler? MouseMoved;

   /// <summary>Raised when a mouse button is released while the mouse is inside of the window.</summary>
   event NativeWindowMouseButtonEventHandler? MouseButtonUp;

   /// <summary>Raised when a mouse button is pressed down while the mouse is inside of the window.</summary>
   event NativeWindowMouseButtonEventHandler? MouseButtonDown;

   /// <summary>Raised when the mouse wheel is scrolled whlie the mouse is inside of the window.</summary>
   event NativeWindowMouseWheelScrolledEventHandler? MouseWheelScrolled;

   /// <summary>Raised when the window obtains focus.</summary>
   event NativeWindowGotFocusEventHandler? GotFocus;

   /// <summary>Raised when the window loses focus.</summary>
   event NativeWindowLostFocusEventHandler? LostFocus;

   /// <summary>Raised when a keyboard key is released while the window has focus.</summary>
   event NativeWindowKeyboardKeyEventHandler? KeyboardKeyUp;

   /// <summary>Raised when a keyboard key is pressed down while the window has focus.</summary>
   event NativeWindowKeyboardKeyEventHandler? KeyboardKeyDown;

   /// <summary>Raised when the window is hidden.</summary>
   event NativeWindowHiddenEventHandler? Hidden;

   /// <summary>Raised when the window is shown.</summary>
   event NativeWindowShownEventHandler? Shown;

   /// <summary>Raised when the visibility of the window has changed.</summary>
   event NativeWindowVisibilityChangedEventHandler? VisibilityChanged;
   #endregion

   #region Methods
   /// <summary>Requests for the window to be closed.</summary>
   /// <returns><see langword="true"/> if the window was closed, <see langword="false"/> otherwise.</returns>
   bool RequestClose();

   /// <summary>Closes the window.</summary>
   void Close();

   /// <summary>Starts redrawing the window.</summary>
   INativeWindowDrawContext StartDraw();

   /// <summary>Stops redrawing the window.</summary>
   void EndDraw();

   /// <summary>Marks the window as requesting a redraw.</summary>
   void AskForRedraw();

   /// <summary>Hides the window.</summary>
   void Hide();

   /// <summary>Shows the window.</summary>
   void Show();

   /// <summary>Minimizes the window.</summary>
   void Minimize();

   /// <summary>Maximizes the window.</summary>
   void Maximize();

   /// <summary>Restores the window to the state it was before it was either minimized or maximized.</summary>
   void Restore();
   #endregion
}

/// <summary>
///   Represents a scope during which a native window can be drawn to.
/// </summary>
/// <param name="window">The window that can be drawn to.</param>
public readonly struct NativeWindowDrawScope(INativeWindow window) : IDisposable
{
   #region Properties
   /// <summary>The window that can be drawn to.</summary>
   public readonly INativeWindow Window { get; } = window;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Dispose() => Window.EndDraw();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="INativeWindow"/>.
/// </summary>
public static class INativeWindowExtensions
{
   #region Methods
   /// <summary>Starts drawing to the given <paramref name="window"/>.</summary>
   /// <param name="window">The window to start drawing to.</param>
   /// <param name="drawContext">The drawing context that can be used to draw to the given <paramref name="window"/>.</param>
   /// <returns>The drawing scope which will end the drawing operation once disposed.</returns>
   public static NativeWindowDrawScope Draw(this INativeWindow window, out INativeWindowDrawContext drawContext)
   {
      drawContext = window.StartDraw();
      return new(window);
   }
   #endregion
}
