namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents an SDL3 specific desktop window.
/// </summary>
public unsafe sealed class SDL3DesktopWindow : ObservableBase, IDesktopWindow, ISDL3EventHandler<SDL3_WindowEvent>
{
   #region Fields
   private static readonly Color BackgroundColor = new(23, 23, 23);
   private readonly IApplicationContext _context;
   private SDL3_Renderer* _renderer;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isOpen = true;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public DesktopWindowKind Kind { get; }

   /// <inheritdoc/>
   public IDesktopWindow? Parent { get; }
   internal SDL3_WindowId WindowId { get; private set; }
   internal SDL3_Window* Window { get; private set; }

   /// <inheritdoc/>
   public bool IsOpen
   {
      get => _isOpen;
      private set => Set(ref _isOpen, value);
   }

   /// <inheritdoc/>
   public string Title
   {
      get
      {
         throw new NotImplementedException();
      }
      set
      {
         throw new NotImplementedException();
      }
   }

   /// <inheritdoc/>
   public Point Position { get; set; }

   /// <inheritdoc/>
   public Size Size { get; set; }

   /// <inheritdoc/>
   public bool CanResize { get; set; }

   /// <inheritdoc/>
   public bool IsBorderless { get; set; }

   /// <inheritdoc/>
   public bool IsVisible { get; set; }

   /// <inheritdoc/>
   public bool AllowsTransparency { get; set; }

   /// <inheritdoc/>
   public DesktopWindowState State { get; set; }

   /// <inheritdoc/>
   public bool IsAlwaysOnTop { get; set; }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event DesktopWindowEventHandler<WindowCloseRequestedEventArgs>? CloseRequested;

   /// <inheritdoc/>
   public event DesktopWindowEventHandler? Closed;
   #endregion

   #region Constructors
   internal SDL3DesktopWindow(IApplicationContext context, DesktopWindowKind kind, IDesktopWindow? parent, SDL3_Window* window, SDL3_Renderer* renderer, SDL3_WindowId windowId)
   {
      _context = context;

      Kind = kind;
      Parent = parent;
      Window = window;
      _renderer = renderer;
      WindowId = windowId;

      DrawBackground();
      DisplayRender();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Close()
   {
      if (IsOpen is false)
         throw new InvalidOperationException("The desktop window is already closed.");

      if (_context.Dispatcher.NeedsDispatching)
         _context.Dispatcher.Dispatch(CloseNative);
      else
         CloseNative();
   }
   private void CloseNative()
   {
      if (_context.Logging.IsAvailable)
         _context.Logging.Debug<SDL3DesktopWindow>($"Window destroyed internally, id = ({WindowId}).");

      try
      {
         Native.DestroyRenderer(_renderer);
      }
      finally
      {
         _renderer = null;
      }
      Native.DestroyWindow(Window);
   }
   unsafe void ISDL3EventHandler<SDL3_WindowEvent>.OnEvent(in SDL3_WindowEvent ev)
   {
      if (ev.Type is SDL3_EventType.WindowCloseRequested)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3DesktopWindow>($"Window received a close request, id = ({WindowId}).");

         WindowCloseRequestedEventArgs args = new();
         CloseRequested?.Invoke(this, args);

         if (args.ShouldCloseWindow)
            Close();
      }
      else if (ev.Type is SDL3_EventType.WindowDestroyed)
      {
         try
         {
            if (_context.Logging.IsAvailable)
               _context.Logging.Debug<SDL3DesktopWindow>($"Window destroyed externally, id = ({ev.WindowId}).");

            Closed?.Invoke(this);
         }
         finally
         {
            WindowId = default;
            Window = null;
            IsOpen = false;
         }
      }
      else if (ev.Type is SDL3_EventType.WindowExposed)
      {
         DrawBackground();
         DisplayRender(); // Note(Nightowl): Modify later when the window is actually drawn;
      }
   }
   #endregion

   #region Helpers
   private void DrawBackground()
   {
      if (Native.SetRenderDrawColor(_renderer, BackgroundColor) is false && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DesktopWindow>($"Failed to set the background color for the window ({WindowId}). ({Native.LastError})");

      if (Native.RenderClear(_renderer) is false && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DesktopWindow>($"Failed to clear the background for the window ({WindowId}). ({Native.LastError})");
   }
   private void DisplayRender()
   {
      if (Native.RenderPresent(_renderer) is false && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DesktopWindow>($"Failed to update the screen with the renderer for the window ({WindowId}). ({Native.LastError})");
   }
   #endregion
}
