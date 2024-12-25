namespace Sain.Desktop.Windowing;

/// <summary>Represents the event handler for window events.</summary>
/// <param name="window">The window that the event was raised for.</param>
public delegate void DesktopWindowEventHandler(IDesktopWindow window);

/// <summary>Represents the event handler for window events.</summary>
/// <param name="window">The window that the event was raised for.</param>
/// <param name="args">The event arguments.</param>
public delegate void DesktopWindowEventHandler<in T>(IDesktopWindow window, T args);

/// <summary>
///   Represents a desktop's application window.
/// </summary>
public interface IDesktopWindow : INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Properties
   /// <summary>The kind of the desktop window.</summary>
   DesktopWindowKind Kind { get; }

   /// <summary>The parent of the window.</summary>
   IDesktopWindow? Parent { get; }

   /// <summary>Whether the window is currently open.</summary>
   bool IsOpen { get; }

   /// <summary>The title of the window.</summary>
   string Title { get; set; }

   /// <summary>The position of the window.</summary>
   Point Position { get; set; }

   /// <summary>The size of the window.</summary>
   Size Size { get; set; }

   /// <summary>Whether the window can be resized.</summary>
   bool CanResize { get; set; }

   /// <summary>Whether the window is borderless.</summary>
   bool IsBorderless { get; set; }

   /// <summary>Whether the window is currently visible.</summary>
   bool IsVisible { get; set; }

   /// <summary>Whether the window allows transparency.</summary>
   bool AllowsTransparency { get; set; }

   /// <summary>The current state of the window.</summary>
   DesktopWindowState State { get; set; }

   /// <summary>Whether the window is always on top.</summary>
   bool IsAlwaysOnTop { get; set; }
   #endregion

   #region Events
   /// <summary>Raised when the user requests for the window to close.</summary>
   event DesktopWindowEventHandler<WindowCloseRequestedEventArgs>? CloseRequested;

   /// <summary>Raised when the window is closed.</summary>
   event DesktopWindowEventHandler? Closed;
   #endregion

   #region Methods
   /// <summary>Closes the window.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the window is already closed.</exception>
   void Close();
   #endregion
}
