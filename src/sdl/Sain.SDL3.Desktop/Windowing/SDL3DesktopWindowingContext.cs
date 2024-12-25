namespace Sain.SDL3.Desktop.Windowing;

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

      SDL3_WindowId* id = Native.CreateWindow(title, width, height, flags);

      if (configuration.Parent is SDL3DesktopWindow parent && (Native.SetWindowParent(id, parent.WindowId) is false))
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3DesktopWindowingContext>($"Couldn't set the window ({(nint)parent.WindowId}) to be the parent of the window ({(nint)id}). ({Native.LastError})");
      }

      int x = (int)location.X;
      int y = (int)location.Y;

      if (Native.SetWindowPosition(id, x, y) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3DesktopWindowingContext>($"Couldn't set the position of the window ({(nint)id}) to X = ({x:n0}), Y = ({y:n0}). ({Native.LastError})");
      }

      SDL3DesktopWindow window = new(Application, configuration.Kind, configuration.Parent, id);
      window.Closed += WindowClosed;

      _windows.Add(window);
      return window;
   }

   unsafe void ISDL3Context.OnEvent(SDL3_Event* ev)
   {
      foreach (SDL3DesktopWindow current in _windows)
      {
         if (current.WindowId == current.WindowId)
         {
            current.OnEvent(ev);
            break;
         }
      }
   }
   private void WindowClosed(IDesktopWindow window)
   {
      window.Closed -= WindowClosed;
      _windows.Remove((SDL3DesktopWindow)window);
   }
   #endregion
}
