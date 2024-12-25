namespace Sain.SDL3.Desktop.Windowing;

public unsafe sealed class SDL3DesktopWindow : ObservableBase, IDesktopWindow
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
         if (Dispatcher.IsOnMainThread)
            return Native.GetWindowTitle(WindowId);

         return Dispatcher.Dispatch(() => Native.GetWindowTitle(WindowId)).Result;
      }
      set
      {
         if (Dispatcher.IsOnMainThread)
            Native.SetWindowTitle(WindowId, value);
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
   internal unsafe void OnEvent(SDL3_Event* ev)
   {
      if (ev->Type is SDL3_EventType.WindowCloseRequested)
      {
         WindowCloseRequestedEventArgs args = new();
         CloseRequested?.Invoke(this, args);

         if (args.ShouldCloseWindow)
            Close();
      }
      else if (ev->Type is SDL3_EventType.WindowDestroyed)
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
