namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents an SDL3 specific native window.
/// </summary>
public sealed unsafe class SDL3NativeWindow : INativeWindow, ISDL3EventHandler<SDL3_WindowEvent>
{
   #region Fields
   private readonly IApplicationContext _context;
   private readonly SDL3NativeWindowDrawContext _drawContext;

   private Point _lastPosition;
   private Size _lastActualSize;
   private NativeWindowState _lastState;
   private bool _lastIsMouseInside;
   private bool _lastHasFocus;
   private Point _lastMousePosition;
   private bool _isDrawing;
   private string _lastTitle;
   #endregion

   #region Properties
   internal SDL3_WindowId WindowId { get; private set; }
   internal SDL3_Window* Window { get; private set; }
   internal SDL3_Renderer* Renderer { get; private set; }

   /// <inheritdoc/>
   public Guid Id { get; }

   /// <inheritdoc/>
   public bool IsOpen { get; private set; } = true;

   /// <inheritdoc/>
   public Point Position
   {
      get => _context.Dispatcher.TryDispatch(GetPosition).Result;
      set => _context.Dispatcher.TryDispatch(SetPosition, value);
   }

   /// <inheritdoc/>
   public Size ActualSize
   {
      get => _context.Dispatcher.TryDispatch(GetSize).Result;
      set => _context.Dispatcher.TryDispatch(SetSize, value);
   }

   /// <inheritdoc/>
   public string Title
   {
      get => _context.Dispatcher.TryDispatch(GetTitle).Result;
      set => _context.Dispatcher.TryDispatch(SetTitle, value);
   }

   /// <inheritdoc/>
   public bool NeedsRedraw { get; private set; } = true;

   /// <inheritdoc/>
   public NativeWindowState State => _context.Dispatcher.TryDispatch(GetState).Result;

   /// <inheritdoc/>
   public bool IsMouseInside => _context.Dispatcher.TryDispatch(GetIsMouseInside).Result;

   /// <inheritdoc/>
   public bool HasFocus => _context.Dispatcher.TryDispatch(GetHasFocus).Result;

   /// <inheritdoc/>
   public Point MousePosition => _context.Dispatcher.TryDispatch(GetMousePosition).Result;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event NativeWindowMovedEventHandler? Moved;

   /// <inheritdoc/>
   public event NativeWindowResizedEventHandler? Resized;

   /// <inheritdoc/>
   public event NativeWindowRedrawRequestedEventHandler? RedrawRequested;

   /// <inheritdoc/>
   public event NativeWindowStateChangedEventHandler? StateChanged;

   /// <inheritdoc/>
   public event NativeWindowTitleChangedEventHandler? TitleChanged;

   /// <inheritdoc/>
   public event NativeWindowCloseRequestedEventHandler? CloseRequested;

   /// <inheritdoc/>
   public event NativeWindowClosingEventHandler? Closing;

   /// <inheritdoc/>
   public event NativeWindowClosedEventHandler? Closed;

   /// <inheritdoc/>
   public event NativeWindowMouseEnteredEventHandler? MouseEntered;

   /// <inheritdoc/>
   public event NativeWindowMouseLeftEventHandler? MouseLeft;

   /// <inheritdoc/>
   public event NativeWindowMouseMovedEventHandler? MouseMoved;

   /// <inheritdoc/>
   public event NativeWindowMouseButtonEventHandler? MouseButtonUp;

   /// <inheritdoc/>
   public event NativeWindowMouseButtonEventHandler? MouseButtonDown;

   /// <inheritdoc/>
   public event NativeWindowMouseWheelScrolledEventHandler? MouseWheelScrolled;

   /// <inheritdoc/>
   public event NativeWindowGotFocusEventHandler? GotFocus;

   /// <inheritdoc/>
   public event NativeWindowLostFocusEventHandler? LostFocus;

   /// <inheritdoc/>
   public event NativeWindowKeyboardKeyEventHandler? KeyboardKeyUp;

   /// <inheritdoc/>
   public event NativeWindowKeyboardKeyEventHandler? KeyboardKeyDown;
   #endregion

   #region Constructors
   internal SDL3NativeWindow(IApplicationContext context, SDL3_Window* window, SDL3_WindowId windowId, SDL3_Renderer* renderer)
   {
      _context = context;
      Id = Guid.NewGuid();

      Window = window;
      WindowId = windowId;
      Renderer = renderer;

      _drawContext = new(context, this);

      _lastPosition = GetPosition();
      _lastActualSize = GetSize();
      _lastState = GetState();
      _lastIsMouseInside = GetIsMouseInside();
      _lastHasFocus = GetHasFocus();
      _lastMousePosition = GetMousePosition();
      _lastTitle = GetTitle();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public INativeWindowDrawContext StartDraw()
   {
      if (_isDrawing)
         throw new InvalidOperationException($"The window is already being redrawn.");

      NeedsRedraw = false;
      _isDrawing = true;

      return _drawContext;
   }

   /// <inheritdoc/>
   public void EndDraw()
   {
      if (_isDrawing is false)
         throw new InvalidOperationException($"The window hasn't started being drawn yet.");

      try
      {
         if (Native.RenderPresent(Renderer) is false && _context.Logging.IsAvailable)
            _context.Logging.Error<SDL3NativeWindow>($"Failed to present the render for the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
      }
      finally
      {
         _isDrawing = false;
      }
   }

   /// <inheritdoc/>
   public bool RequestClose()
   {
      if (_context.Logging.IsAvailable)
         _context.Logging.Debug<SDL3NativeWindow>($"Native window close requested, Id = ({Id}), WindowId = ({WindowId}).");

      NativeWindowCloseRequestedEventArgs args = new();
      CloseRequested?.Invoke(this, args);

      if (args.ShouldClose)
      {
         Close();
         return true;
      }

      return false;
   }

   /// <inheritdoc/>
   public void Close()
   {
      if (IsOpen is false)
         return;

      try
      {

         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3NativeWindow>($"Native window closing, Id = ({Id}), WindowId = ({WindowId}).");

         Closing?.Invoke(this);

         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3NativeWindow>($"Native window close started, Id = ({Id}), WindowId = ({WindowId}).");

         if (Renderer is not null)
         {
            Native.DestroyRenderer(Renderer);
            Renderer = null;
         }

         SDL3_WindowId windowId = WindowId;
         if (Window is not null)
         {
            Native.DestroyWindow(Window);
            Window = null;
            WindowId = default;
         }

         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3NativeWindow>($"Native window fully closed, Id = ({Id}), WindowId = ({windowId}).");

         Closed?.Invoke(this);
      }
      finally
      {
         Renderer = null;
         Window = null;
         WindowId = default;

         IsOpen = false;
      }
   }

   /// <inheritdoc/>
   public void AskForRedraw()
   {
      if (NeedsRedraw)
         return;

      NeedsRedraw = true;

      // Note(Nightowl): Should already be on the Visual dispatch priority;
      RedrawRequested?.Invoke(this);
   }
   #endregion

   #region Helpers
   private Point GetPosition()
   {
      if (Native.GetWindowPosition(Window, out int x, out int y))
         return new(x, y);

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to get the position of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");

      return default;
   }
   private Size GetSize()
   {
      if (Native.GetWindowSize(Window, out int width, out int height))
         return new(width, height);

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to get the size of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");

      return default;
   }
   private string GetTitle()
   {
      string? title = Native.GetWindowTitle(Window);
      if (title is null && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to get the title of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");

      return title ?? string.Empty;
   }
   private void SetPosition(Point position)
   {
      int x = (int)position.X;
      int y = (int)position.Y;

      if (Native.SetWindowPosition(Window, x, y))
         _lastPosition = position;
      else if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to set the position ({position}) of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }
   private void SetTitle(string title)
   {
      string last = _lastTitle;

      if (Native.SetWindowTitle(Window, title) is false && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to set the title ({title}) of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");

      _lastTitle = title;
      TitleChanged?.Invoke(this, new(last, title));
   }
   private void SetSize(Size size)
   {
      int width = (int)size.Width;
      int height = (int)size.Height;

      if (Native.SetWindowSize(Window, width, height))
         _lastActualSize = size;
      else if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3NativeWindow>($"Failed to set the size ({size}) of the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }
   private NativeWindowState GetState()
   {
      SDL3_WindowFlags flags = Native.GetWindowFlags(Window);

      if (flags.HasFlag(SDL3_WindowFlags.Minimized))
         return NativeWindowState.Minimized;

      if (flags.HasFlag(SDL3_WindowFlags.Maximized))
         return NativeWindowState.Maximized;

      return NativeWindowState.Normal;
   }
   private bool GetHasFocus()
   {
      SDL3_WindowFlags flags = Native.GetWindowFlags(Window);

      if (flags.HasFlag(SDL3_WindowFlags.InputFocus))
         return true;

      return false;
   }
   private bool GetIsMouseInside()
   {
      SDL3_WindowFlags flags = Native.GetWindowFlags(Window);

      if (flags.HasFlag(SDL3_WindowFlags.MouseFocus))
         return true;

      return false;
   }
   private Point GetMousePosition()
   {
      if (IsMouseInside && _context.Input.Mouse.IsAvailable)
         return _context.Input.Mouse.LocalPosition;

      return _lastMousePosition;
   }
   private void CheckStateChange()
   {
      NativeWindowState last = _lastState;
      NativeWindowState current = GetState();

      if (last != current)
      {
         _lastState = current;

         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3NativeWindow>($"Native window has changed stated ({last}) -> ({current}), Id = ({Id}), WindowId = ({WindowId}).");

         StateChanged?.Invoke(this, new(last, current));
      }
   }
   private void CheckHasFocusChanged()
   {
      bool last = _lastHasFocus;
      bool current = GetHasFocus();

      if (last != current)
      {
         _lastHasFocus = current;
         if (current)
         {
            if (_context.Logging.IsAvailable)
               _context.Logging.Debug<SDL3NativeWindow>($"Native window has obtained focus, Id = ({Id}), WindowId = ({WindowId}).");

            if (_context.Input.Keyboard.IsAvailable)
            {
               _context.Input.Keyboard.KeyboardKeyUp += OnKeyboardKey;
               _context.Input.Keyboard.KeyboardKeyDown += OnKeyboardKey;
            }

            GotFocus?.Invoke(this);
         }
         else
         {
            if (_context.Logging.IsAvailable)
               _context.Logging.Debug<SDL3NativeWindow>($"Native window has lost focus, Id = ({Id}), WindowId = ({WindowId}).");

            if (_context.Input.Keyboard.IsAvailable)
            {
               _context.Input.Keyboard.KeyboardKeyUp -= OnKeyboardKey;
               _context.Input.Keyboard.KeyboardKeyDown -= OnKeyboardKey;
            }

            LostFocus?.Invoke(this);
         }
      }
   }
   private void CheckIsMouseInsideChanged()
   {
      bool last = _lastIsMouseInside;
      bool current = GetIsMouseInside();

      if (last != current)
      {
         _lastIsMouseInside = current;
         if (current)
         {
            if (_context.Logging.IsAvailable)
               _context.Logging.Debug<SDL3NativeWindow>($"The mouse has entered the native window, Id = ({Id}), WindowId = ({WindowId}).");

            if (_context.Input.Mouse.IsAvailable)
            {
               _context.Input.Mouse.MouseMoved += OnMouseMoved;
               _context.Input.Mouse.MouseButtonUp += OnMouseButton;
               _context.Input.Mouse.MouseButtonDown += OnMouseButton;
               _context.Input.Mouse.MouseWheelScrolled += OnMouseWheelScrolled;

               _lastPosition = _context.Input.Mouse.LocalPosition;
            }

            MouseEntered?.Invoke(this, new(MousePosition));
         }
         else
         {
            if (_context.Logging.IsAvailable)
               _context.Logging.Debug<SDL3NativeWindow>($"The mouse has left the native window, Id = ({Id}), WindowId = ({WindowId}).");

            if (_context.Input.Mouse.IsAvailable)
            {
               _context.Input.Mouse.MouseMoved -= OnMouseMoved;
               _context.Input.Mouse.MouseButtonUp -= OnMouseButton;
               _context.Input.Mouse.MouseButtonDown -= OnMouseButton;
               _context.Input.Mouse.MouseWheelScrolled -= OnMouseWheelScrolled;
            }

            MouseLeft?.Invoke(this, new(MousePosition));
         }
      }
   }
   private void CheckMousePositionChanged()
   {
      Point last = _lastMousePosition;
      Point current = GetMousePosition();

      // Note(Nightowl): No need to check if mouse is inside as GetMousePosition will return the _lastMousePosition if the mouse is outside the window;
      if (last != current)
      {
         _lastMousePosition = current;
         MouseMoved?.Invoke(this, new(last, current));
      }
   }
   #endregion

   #region Event handlers
   private void OnMouseMoved(IMouseInputContext context, MouseMoveEventArgs args) => CheckMousePositionChanged();
   private void OnMouseButton(IMouseInputContext context, MouseButtonEventArgs args)
   {
      if (IsMouseInside)
      {
         // Note(Nightowl): Should already be on the Input dispatch priority;
         NativeWindowMouseButtonEventHandler? handler = args.IsDown ? MouseButtonDown : MouseButtonUp;
         handler?.Invoke(this, new(MousePosition, args.Button, args.Name, args.IsDown));
      }
   }
   private void OnMouseWheelScrolled(IMouseInputContext context, MouseWheelScrollEventArgs args)
   {
      if (IsMouseInside)
      {
         // Note(Nightowl): Should already be on the Input dispatch priority;
         MouseWheelScrolled?.Invoke(this, new(MousePosition, args.DeltaX, args.DeltaY));
      }
   }
   private void OnKeyboardKey(IKeyboardInputContext context, KeyboardKeyEventArgs args)
   {
      if (HasFocus)
      {
         NativeWindowKeyboardKeyEventHandler? handler = args.IsDown ? KeyboardKeyDown : KeyboardKeyUp;
         handler?.Invoke(
            this,
            new(
               args.PhysicalKey,
               args.PhysicalKeyName,
               args.VirtualKey,
               args.VirtualKeyName,
               args.Modifiers,
               args.IsDown,
               args.IsRepeat));
      }
   }
   void ISDL3EventHandler<SDL3_WindowEvent>.OnEvent(in SDL3_WindowEvent ev)
   {
      if (ev.Type is SDL3_EventType.WindowMoved)
         OnWindowMoved(ev);
      else if (ev.Type is SDL3_EventType.WindowResized)
         OnWindowResized(ev);
      else if (ev.Type is SDL3_EventType.WindowCloseRequested)
         RequestClose();
      else if (ev.Type is SDL3_EventType.WindowDestroyed)
         Close();
      else if (ev.Type is SDL3_EventType.WindowExposed)
         AskForRedraw();
      else if (ev.Type is SDL3_EventType.WindowMinimized or SDL3_EventType.WindowMaximized or SDL3_EventType.WindowRestored)
         CheckStateChange();
      else if (ev.Type is SDL3_EventType.WindowMouseEnter or SDL3_EventType.WindowMouseLeave)
         CheckIsMouseInsideChanged();
      else if (ev.Type is SDL3_EventType.WindowFocusLost or SDL3_EventType.WindowFocusGained)
         CheckHasFocusChanged();
      else if (ev.Type is SDL3_EventType.WindowPixelSizeChanged or SDL3_EventType.WindowSafeAreaChanged)
      {
         // Note(Nightowl): Safe to ignore I think;
      }
      else
         Console.WriteLine($"Other window event {ev.Type}.");
   }
   private void OnWindowMoved(in SDL3_WindowEvent window)
   {
      Point oldPosition = _lastPosition;
      Point newPosition = new(window.Data1, window.Data2);
      _lastPosition = newPosition;

      Moved?.Invoke(this, new(oldPosition, newPosition));
   }
   private void OnWindowResized(in SDL3_WindowEvent window)
   {
      Size oldSize = _lastActualSize;
      Size newSize = new(window.Data1, window.Data2);
      _lastActualSize = newSize;

      Resized?.Invoke(this, new(oldSize, newSize));
   }
   #endregion
}
