namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents the SDL3 specific context for managing the desktop application's windows.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public unsafe sealed class SDL3DesktopWindowingContext(IContextProvider? provider) : BaseContext(provider), IDesktopWindowingContext, ISDL3Context
{
   #region Fields
   private readonly NativeWindowCollection<SDL3NativeWindow> _windows = [];
   private readonly Dictionary<SDL3_WindowId, SDL3NativeWindow> _idLookup = [];
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Video | SDL3_InitFlags.Events;

   /// <inheritdoc/>
   public override string Kind => DesktopContextKinds.Windowing;

   /// <inheritdoc/>
   public INativeWindowCollection<INativeWindow> Windows => _windows;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      Debug.Assert(_windows.Count is 0);
      Debug.Assert(_idLookup.Count is 0);
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      _idLookup.Clear();
      _windows.Clear();
   }

   /// <inheritdoc/>
   public INativeWindow CreateWindow(Point position, Size size, string title)
   {
      int width = (int)size.Width;
      int height = (int)size.Height;

      if (Native.CreateWindowAndRenderer(title, width, height, SDL3_WindowFlags.Resizable, out SDL3_Window* window, out SDL3_Renderer* renderer) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3DesktopWindowingContext>($"Failed to create a new native window. ({Native.LastError})");

         throw new InvalidOperationException($"Failed to create a new window.");
      }

      SDL3_WindowId windowId = Native.GetWindowId(window);
      SDL3NativeWindow nativeWindow = new(Context, window, windowId, renderer);

      nativeWindow.Closing += WindowClosing;
      nativeWindow.Closed += WindowClosed;

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3DesktopWindowingContext>($"Created a new native window, Id = ({nativeWindow.Id}), WindowId = ({windowId}).");

      _idLookup.Add(windowId, nativeWindow);
      _windows.Add(nativeWindow);

      return nativeWindow;
   }
   private void WindowClosing(INativeWindow window) => _idLookup.Remove(((SDL3NativeWindow)window).WindowId);
   private void WindowClosed(INativeWindow window) => _windows.Remove((SDL3NativeWindow)window);
   #endregion

   #region Helpers
   void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsWindowEvent(out SDL3_WindowEvent window))
         OnWindowEvent(window);
   }
   private void OnWindowEvent(SDL3_WindowEvent ev)
   {
      if (_idLookup.TryGetValue(ev.WindowId, out SDL3NativeWindow? window) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DesktopWindowingContext>($"Received an event for an unknown window id ({ev.WindowId}).");

         return;
      }

      window.RouteEvent(ev);
   }
   #endregion
}
