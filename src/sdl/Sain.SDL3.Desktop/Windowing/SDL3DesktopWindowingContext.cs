namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents the SDL3 specific context for managing windows.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public unsafe sealed class SDL3DesktopWindowingContext(IContextProvider? provider) : BaseContext(provider), ISDL3Context, IDesktopWindowingContext
{
   #region Fields
   private readonly DesktopWindowCollection<SDL3DesktopWindow> _windows = [];
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Events;

   /// <inheritdoc/>
   public override string Kind => DesktopContextKinds.Windowing;

   /// <inheritdoc/>
   public IDesktopWindowCollection<IDesktopWindow> Windows => _windows;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public unsafe IDesktopWindow CreateWindow(DesktopWindowConfiguration configuration)
   {
      Debug.Assert(configuration.Kind is DesktopWindowKind.Normal);

      string title = configuration.Title ?? Application.Name;

      int width = (int)configuration.Size.Width;
      int height = (int)configuration.Size.Height;

      // Todo(Nightowl): Determine location;
      Point location = default;

      SDL3_WindowFlags flags = SDL3_WindowFlags.None;

      if (configuration.CanResize) flags |= SDL3_WindowFlags.Resizable;
      if (configuration.AllowsTransparency) flags |= SDL3_WindowFlags.Transparent;
      if (configuration.IsAlwaysOnTop) flags |= SDL3_WindowFlags.AlwaysOnTop;

      //SDL3_Window* window = Native.CreateWindow(title, width, height, flags);

      if (Native.CreateWindowAndRenderer(title, width, height, flags, out SDL3_Window* window, out SDL3_Renderer* renderer) is false)
      {
         string message = $"Failed to create a new window. ({Native.LastError})";

         if (Context.Logging.IsAvailable)
            Context.Logging.Fatal<SDL3DesktopWindowingContext>(message);

         throw new InvalidOperationException(message);
      }

      SDL3_WindowId windowId = Native.GetWindowId(window);
      if (windowId.Id is 0)
      {
         string message = $"Failed to get the id of the newly created window. ({Native.LastError})";

         if (Context.Logging.IsAvailable)
            Context.Logging.Fatal<SDL3DesktopWindowingContext>(message);

         throw new InvalidOperationException(message);
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3DesktopWindowingContext>($"Created a new window, id = ({windowId}).");

      if (configuration.Parent is SDL3DesktopWindow parent && (Native.SetWindowParent(window, parent.Window) is false))
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3DesktopWindowingContext>($"Couldn't set the window ({parent.WindowId}) to be the parent of the window ({windowId}). ({Native.LastError}).");
      }

      int x = (int)location.X;
      int y = (int)location.Y;

      if (Native.SetWindowPosition(window, x, y) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3DesktopWindowingContext>($"Couldn't set the position of the window ({windowId}) to X = ({x:n0}), Y = ({y:n0}). ({Native.LastError}).");
      }

      SDL3DesktopWindow desktopWwindow = new(Context, configuration.Kind, configuration.Parent, window, renderer, windowId);
      desktopWwindow.Closed += WindowClosed;

      _windows.Add(desktopWwindow);
      return desktopWwindow;
   }
   private void WindowClosed(IDesktopWindow untyped)
   {
      SDL3DesktopWindow window = (SDL3DesktopWindow)untyped;
      if (_windows.Remove(window))
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Debug<SDL3DesktopWindowingContext>($"Window ({window.WindowId}) is closed.");

         window.Closed -= WindowClosed;
      }
   }
   unsafe void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsWindowEvent(out SDL3_WindowEvent window) is false)
         return;

      foreach (SDL3DesktopWindow current in _windows)
      {
         if (current.WindowId.Id == current.WindowId.Id)
         {
            current.RouteEvent(window);
            break;
         }
      }
   }
   #endregion
}
