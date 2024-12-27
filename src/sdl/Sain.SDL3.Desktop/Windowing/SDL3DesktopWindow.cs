namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents an SDL3 specific desktop window.
/// </summary>
public unsafe sealed class SDL3DesktopWindow : ObservableBase, IDesktopWindow, ISDL3EventHandler<SDL3_WindowEvent>
{
   #region Fields
   private readonly IApplication _application;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isOpen = true;
   #endregion

   #region Properties
   private IDispatcherContext Dispatcher => _application.Context.Dispatcher;

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
   internal SDL3DesktopWindow(IApplication application, DesktopWindowKind kind, IDesktopWindow? parent, SDL3_WindowId* windowId)
   {
      _application = application;

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

      if (Dispatcher.NeedsDispatching)
         Dispatcher.Dispatch(CloseNative);
      else
         CloseNative();
   }
   private void CloseNative() => Native.DestroyWindow(WindowId);
   unsafe void ISDL3EventHandler<SDL3_WindowEvent>.OnEvent(in SDL3_WindowEvent ev)
   {
      if (ev.Type is SDL3_EventType.WindowCloseRequested)
      {
         WindowCloseRequestedEventArgs args = new();
         CloseRequested?.Invoke(this, args);

         if (args.ShouldCloseWindow)
            Close();
      }
      else if (ev.Type is SDL3_EventType.WindowDestroyed)
      {
         try
         {
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
