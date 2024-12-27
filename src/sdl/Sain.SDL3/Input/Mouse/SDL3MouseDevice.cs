namespace Sain.SDL3.Input.Mouse;

/// <summary>
///   Represents SDL3 specific information about a mouse device.
/// </summary>
public class SDL3MouseDevice :
   ObservableBase,
   IMouseDevice,
   ISDL3EventHandler<SDL3_MouseMotionEvent>,
   ISDL3EventHandler<SDL3_MouseButtonEvent>,
   ISDL3EventHandler<SDL3_MouseWheelEvent>
{
   #region Fields
   private readonly IApplicationContext _context;
   private readonly MouseButtonState[] _buttons;
   private readonly SDL3MouseInputContext _sdlContext;
   private readonly Stopwatch _useStopwatch = Stopwatch.StartNew();

   private TimeSpan _lastUseTimestamp = TimeSpan.MaxValue;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IDeviceId _deviceId;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string _name;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Point _globalPosition;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Point _localPosition;

   private SDL3_WindowId _lastActiveWindow;
   #endregion

   #region Properties
   internal SDL3_MouseId MouseId { get; }

   /// <inheritdoc/>
   public Guid Id { get; }

   /// <inheritdoc/>
   public IDeviceId DeviceId
   {
      get => _deviceId;

      [MemberNotNull(nameof(_deviceId))]
      private set => Set(ref _deviceId!, value);
   }

   /// <inheritdoc/>
   public string Name
   {
      get => _name;

      [MemberNotNull(nameof(_name))]
      private set
      {
         if (Set(ref _name!, value))
            RefreshDeviceId();
      }
   }

   /// <inheritdoc/>
   public IReadOnlyCollection<IMouseButtonState> Buttons => _buttons;

   /// <inheritdoc/>
   public Point GlobalPosition
   {
      get
      {
         RefreshGlobalPosition();
         return _globalPosition;
      }
      set
      {
         if (TrySetGlobalPosition(value) is false)
            throw new NotSupportedException($"Setting the global mouse position is not supported.");
      }
   }

   /// <inheritdoc/>
   public Point LocalPosition
   {
      get
      {
         RefreshLocalPosition();
         return _localPosition;
      }
      set
      {
         if (_lastActiveWindow.Id is 0)
            throw new InvalidOperationException($"Setting the local mouse position is not possible if there is no active window.");

         if (TrySetLocalPosition(value) is false)
            throw new NotSupportedException($"Setting the local mouse position is not supported.");
      }
   }

   /// <inheritdoc/>
   public bool IsCaptured
   {
      get => _sdlContext.IsMouseCaptured;
      private set
      {
         _sdlContext.IsMouseCaptured = value;
         _sdlContext.RefreshIsCaptured();
      }
   }

   /// <inheritdoc/>
   public bool IsCursorVisible => Native.IsCursorVisible();

   /// <inheritdoc/>
   public TimeSpan LastUsed
   {
      get
      {
         if (_lastUseTimestamp == TimeSpan.MaxValue)
            return TimeSpan.MaxValue;

         Debug.Assert(_lastUseTimestamp < _useStopwatch.Elapsed);
         return _useStopwatch.Elapsed - _lastUseTimestamp;
      }
   }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event MouseDeviceMotionEventHandler? MouseMoved;

   /// <inheritdoc/>
   public event MouseDeviceButtonEventHandler? ButtonUp;

   /// <inheritdoc/>
   public event MouseDeviceButtonEventHandler? ButtonDown;
   #endregion

   #region Constructors
   internal SDL3MouseDevice(IApplicationContext context, SDL3MouseInputContext sdlContext, SDL3_MouseId id)
   {
      _context = context;
      _sdlContext = sdlContext;

      MouseId = id;
      Id = Guid.NewGuid();

      _buttons =
      [
         new(new(MouseButtonKind.Left, (uint)SDL3_MouseButton.Left), "left", false),
         new(new(MouseButtonKind.Middle, (uint)SDL3_MouseButton.Middle), "middle", false),
         new(new(MouseButtonKind.Right, (uint)SDL3_MouseButton.Right), "right", false),
         new(new(MouseButtonKind.Other, (uint)SDL3_MouseButton.X1), "x1", false),
         new(new(MouseButtonKind.Other, (uint)SDL3_MouseButton.X2), "x2", false),
      ];

      Refresh();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool IsMatch(IDeviceId id, out int score) => DeviceId.IsBasicPartialMatch(id, out score);

   /// <inheritdoc/>
   public bool TrySetGlobalPosition(Point position)
   {
      if (Native.WarpMouseGlobal((float)position.X, (float)position.Y) is false)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Warning<SDL3MouseDevice>($"Failed to set the global mouse ({MouseId}) position ({position}). ({Native.LastError})");

         return false;
      }

      RaisePropertyChanging(nameof(GlobalPosition));
      _globalPosition = position;
      RaisePropertyChanged(nameof(GlobalPosition));

      return true;
   }

   /// <inheritdoc/>
   public bool TrySetLocalPosition(Point position)
   {
      if (_lastActiveWindow.Id is 0) // No active window / invalid window
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3MouseDevice>($"Setting the local mouse ({MouseId}) position ({position}) failed because no window was last active.");

         return false;
      }

      if (Native.WarpMouseInWindow(_lastActiveWindow, (float)position.X, (float)position.Y) is false)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Warning<SDL3MouseDevice>($"Failed to set the local mouse ({MouseId}) position ({position}) in the window ({_lastActiveWindow}). ({Native.LastError})");

         return false;
      }

      RaisePropertyChanging(nameof(GlobalPosition));
      _globalPosition = position;
      RaisePropertyChanged(nameof(GlobalPosition));

      return true;
   }

   /// <inheritdoc/>
   public bool StartCapture()
   {
      if (IsCaptured)
         return true;

      if (Native.CaptureMouse(true))
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3MouseDevice>($"Successfully started capturing the mouse events.");

         IsCaptured = true;
         return true;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to start capturing the mouse events. ({Native.LastError})");

      return false;
   }

   /// <inheritdoc/>
   public bool StopCapture()
   {
      if (IsCaptured is false)
         return true;

      if (Native.CaptureMouse(false))
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Debug<SDL3MouseDevice>($"Successfully stopped capturing the mouse events.");

         IsCaptured = false;
         return true;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to stop capturing the mouse events. ({Native.LastError})");

      return false;
   }

   /// <inheritdoc/>
   public bool ShowCursor()
   {
      if (Native.ShowCursor())
      {
         _sdlContext.RefreshCursorVisibility();
         return true;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to show the mouse cursor.");

      return false;
   }

   /// <inheritdoc/>
   public bool HideCursor()
   {
      if (Native.HideCursor())
      {
         _sdlContext.RefreshCursorVisibility();
         return true;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to hide the mouse cursor.");

      return false;
   }

   /// <inheritdoc/>
   public bool IsButtonUp(MouseButton button)
   {
      foreach (IMouseButtonState state in Buttons)
      {
         if (state.Button == button)
            return state.IsDown is false;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Warning<SDL3MouseDevice>($"The checked mouse button ({button}) is not one of the available mouse buttons.");

      return false;
   }

   /// <inheritdoc/>
   public bool IsButtonDown(MouseButton button)
   {
      foreach (IMouseButtonState state in Buttons)
      {
         if (state.Button == button)
            return state.IsDown;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Warning<SDL3MouseDevice>($"The checked mouse button ({button}) is not one of the available mouse buttons.");

      return false;
   }
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   [MemberNotNull(nameof(_deviceId), nameof(_name))]
   public void Refresh()
   {
      RefreshName();
      RefreshDeviceId();
      RefreshGlobalState();
      RefreshLocalState();
      RefreshIsCursorVisible();
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_deviceId))]
   public void RefreshDeviceId()
   {
      DeviceId = new DeviceId($"{MouseId}", Name);
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_name))]
   public void RefreshName()
   {
      string? name = Native.GetMouseNameForId(MouseId);
      if (name is null && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to get the name for the mouse ({MouseId}). ({Native.LastError})");

      Name = name ?? string.Empty;
   }

   /// <inheritdoc/>
   public void RefreshGlobalPosition() => RefreshGlobalState();

   /// <inheritdoc/>
   public void RefreshLocalPosition() => RefreshLocalState();

   /// <inheritdoc/>
   public void RefreshPosition()
   {
      RefreshGlobalPosition();
      RefreshLocalPosition();
   }

   /// <inheritdoc/>
   public void RefreshButtons() => RefreshLocalState();

   /// <inheritdoc/>
   public void RefreshIsCaptured()
   {
      RaisePropertyChanging(nameof(IsCaptured));
      RaisePropertyChanged(nameof(IsCaptured));
   }

   /// <inheritdoc/>
   public void RefreshIsCursorVisible()
   {
      RaisePropertyChanging(nameof(IsCursorVisible));
      RaisePropertyChanged(nameof(IsCursorVisible));
   }
   #endregion

   #region Helpers
   private void UpdateLastUsed()
   {
      RaisePropertyChanging(nameof(LastUsed));
      _lastUseTimestamp = _useStopwatch.Elapsed;
      RaisePropertyChanged(nameof(LastUsed));
   }
   private void RefreshGlobalState()
   {
      SDL3_MouseButtonFlags buttons = Native.GetGlobalMouseState(out float x, out float y);

      _buttons[0].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Left);
      _buttons[1].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Middle);
      _buttons[2].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Right);
      _buttons[3].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X1);
      _buttons[4].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X2);

      RaisePropertyChanging(nameof(GlobalPosition));
      _globalPosition = new(x, y);
      RaisePropertyChanged(nameof(GlobalPosition));
   }
   private void RefreshLocalState()
   {
      SDL3_MouseButtonFlags buttons = Native.GetMouseState(out float x, out float y);

      _buttons[0].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Left);
      _buttons[1].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Middle);
      _buttons[2].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Right);
      _buttons[3].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X1);
      _buttons[4].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X2);

      RaisePropertyChanging(nameof(LocalPosition));
      _localPosition = new(x, y);
      RaisePropertyChanged(nameof(LocalPosition));
   }
   void ISDL3EventHandler<SDL3_MouseMotionEvent>.OnEvent(in SDL3_MouseMotionEvent ev)
   {
      _lastActiveWindow = ev.WindowId;
      RefreshGlobalState();
      RefreshLocalState();
      UpdateLastUsed();

      if (MouseMoved is not null)
      {
         MouseDeviceMotionEventArgs args = new(GlobalPosition, LocalPosition);
         MouseMoved?.Invoke(this, args);
      }
   }
   void ISDL3EventHandler<SDL3_MouseButtonEvent>.OnEvent(in SDL3_MouseButtonEvent ev)
   {
      _lastActiveWindow = ev.WindowId;
      RefreshGlobalState();
      RefreshLocalState();
      UpdateLastUsed();

      MouseButtonState? state = ev.Button switch
      {
         SDL3_MouseButton.Left => _buttons[0],
         SDL3_MouseButton.Middle => _buttons[1],
         SDL3_MouseButton.Right => _buttons[2],
         SDL3_MouseButton.X1 => _buttons[3],
         SDL3_MouseButton.X2 => _buttons[4],

         _ => null,
      };

      if (state is null)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Warning<SDL3MouseDevice>($"Failed to raise the mouse button event for the mouse ({MouseId}) because the mouse button was unknown ({ev.Button}).");

         return;
      }

      MouseDeviceButtonEventArgs args = new(state.Button, state);

      if (ev.IsDown)
         ButtonDown?.Invoke(this, args);

      if (ev.IsDown is false)
         ButtonUp?.Invoke(this, args);
   }
   void ISDL3EventHandler<SDL3_MouseWheelEvent>.OnEvent(in SDL3_MouseWheelEvent ev)
   {
      _lastActiveWindow = ev.WindowId;
      UpdateLastUsed();
   }
   #endregion
}
