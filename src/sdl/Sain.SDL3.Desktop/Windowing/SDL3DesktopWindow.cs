namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents an SDL3 specific desktop window.
/// </summary>
public unsafe sealed class SDL3DesktopWindow : ObservableBase, IDesktopWindow, ISDL3EventHandler<SDL3_WindowEvent>
{
   #region Fields
   private readonly IApplicationContext _context;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isOpen = true;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public DesktopWindowKind Kind { get; }

   /// <inheritdoc/>
   public IDesktopWindow? Parent { get; }
   internal SDL3_WindowId* WindowId { get; private set; }

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
   internal SDL3DesktopWindow(IApplicationContext context, DesktopWindowKind kind, IDesktopWindow? parent, SDL3_WindowId* windowId)
   {
      _context = context;

      Kind = kind;
      Parent = parent;
      WindowId = windowId;
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
         _context.Logging.Debug<SDL3DesktopWindow>($"Window destroyed internally, id = ({WindowId->Id}).");

      Native.DestroyWindow(WindowId);
   }
   unsafe void ISDL3EventHandler<SDL3_WindowEvent>.OnEvent(in SDL3_WindowEvent ev)
   {
      if (ev.Type is SDL3_EventType.WindowCloseRequested)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3DesktopWindow>($"Window received a close request, id = ({WindowId->Id}).");

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
            WindowId = null;
            IsOpen = false;
         }
      }
   }
   #endregion
}
