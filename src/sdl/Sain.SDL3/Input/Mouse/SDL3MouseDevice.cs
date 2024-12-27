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
   private Point _position;
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
   public Point Position
   {
      get
      {
         RefreshPosition();
         return _position;
      }
      set
      {
         if (TrySetPosition(value) is false)
            throw new NotSupportedException($"Setting the mouse position is not supported.");
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
   public bool TrySetPosition(Point position)
   {
      if (Native.WarpMouseGlobal((float)position.X, (float)position.Y) is false)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Warning<SDL3MouseDevice>($"Failed to set the mouse ({MouseId}) position ({position}). ({Native.LastError})");

         return false;
      }

      RaisePropertyChanging(nameof(Position));
      _position = position;
      RaisePropertyChanged(nameof(Position));

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
      RefreshState();
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
   public void RefreshPosition() => RefreshState();

   /// <inheritdoc/>
   public void RefreshButtons() => RefreshButtons();

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
   private void RefreshState()
   {
      SDL3_MouseButtonFlags buttons = Native.GetGlobalMouseState(out float x, out float y);

      _buttons[0].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Left);
      _buttons[1].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Middle);
      _buttons[2].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.Right);
      _buttons[3].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X1);
      _buttons[4].IsDown = buttons.HasFlag(SDL3_MouseButtonFlags.X2);

      RaisePropertyChanging(nameof(Position));
      _position = new(x, y);
      RaisePropertyChanged(nameof(Position));
   }
   void ISDL3EventHandler<SDL3_MouseMotionEvent>.OnEvent(in SDL3_MouseMotionEvent ev)
   {
      RefreshState();
      UpdateLastUsed();
   }
   void ISDL3EventHandler<SDL3_MouseButtonEvent>.OnEvent(in SDL3_MouseButtonEvent ev)
   {
      RefreshState();
      UpdateLastUsed();
   }
   void ISDL3EventHandler<SDL3_MouseWheelEvent>.OnEvent(in SDL3_MouseWheelEvent ev) => UpdateLastUsed();
   #endregion
}
