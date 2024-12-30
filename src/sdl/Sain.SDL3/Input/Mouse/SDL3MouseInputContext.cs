namespace Sain.SDL3.Input.Mouse;

/// <summary>
///   Represents the SDL3 specific context for mouse input.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class SDL3MouseInputContext(IContextProvider? provider) : BaseMouseInputContext(provider), ISDL3Context
{
   #region Fields
   private static readonly MouseButton LeftButton = new(MouseButtonKind.Left);
   private static readonly MouseButton MiddleButton = new(MouseButtonKind.Middle);
   private static readonly MouseButton RightButton = new(MouseButtonKind.Right);
   private static readonly MouseButton X1Button = new(MouseButtonKind.Other, (uint)SDL3_MouseButton.X1);
   private static readonly MouseButton X2Button = new(MouseButtonKind.Other, (uint)SDL3_MouseButton.X2);

   private readonly DeviceCollection<SDL3MouseDevice> _devices = [];
   private readonly Dictionary<SDL3_MouseId, SDL3MouseDevice> _deviceLookup = [];
   private readonly Dictionary<MouseButton, MouseButtonState> _buttons = [];

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Point _globalPosition;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Point _localPosition;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isCaptured;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isCursorVisible;

   private SDL3_WindowId _lastActiveWindow;
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Events;

   /// <inheritdoc/>
   public override IDeviceCollection<IMouseDevice> Devices => _devices;

   /// <inheritdoc/>
   public override IReadOnlyCollection<IMouseButtonState> Buttons => _buttons.Values;

   /// <inheritdoc/>
   public override Point GlobalPosition
   {
      get => _globalPosition;
      set
      {
         if (TrySetGlobalPosition(value) is false)
            throw new NotSupportedException($"Setting the global mouse position is not supported.");
      }
   }

   /// <inheritdoc/>
   public override Point LocalPosition
   {
      get => _localPosition;
      set
      {
         if (TrySetLocalPosition(value) is false)
         {
            if (_lastActiveWindow.Id is 0)
               throw new NotSupportedException($"Setting the local mouse position is not possible until it has interacted with some window.");

            throw new NotSupportedException($"Setting the local mouse position is not supported.");
         }
      }
   }

   /// <inheritdoc/>
   public override bool IsCaptured => _isCaptured;

   /// <inheritdoc/>
   public override bool IsCursorVisible => _isCursorVisible;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      Debug.Assert(_buttons.Count is 0);

      _buttons[LeftButton] = new(LeftButton, "left", false);
      _buttons[MiddleButton] = new(MiddleButton, "middle", false);
      _buttons[RightButton] = new(RightButton, "right", false);
      _buttons[X1Button] = new(X1Button, "x1", false);
      _buttons[X2Button] = new(X2Button, "x2", false);

      Refresh();
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      _devices.Clear();
      _deviceLookup.Clear();
      _buttons.Clear();
   }

   /// <inheritdoc/>
   public override bool TrySetGlobalPosition(Point position)
   {
      if (Native.WarpMouseGlobal((float)position.X, (float)position.Y) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"Failed to set the global mouse position ({position}). ({Native.LastError})");

         return false;
      }

      if (Set(ref _globalPosition, position, nameof(GlobalPosition)))
      {
         RaisePropertyChanging(nameof(LocalPosition));
         RaisePropertyChanged(nameof(LocalPosition));
      }

      return true;
   }

   /// <inheritdoc/>
   public override bool TrySetLocalPosition(Point position)
   {
      if (_lastActiveWindow.Id is 0) // No active window / invalid window
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Debug<SDL3MouseInputContext>($"Setting the local mouse position ({position}) failed because no window was last active.");

         return false;
      }

      if (Native.WarpMouseInWindow(_lastActiveWindow, (float)position.X, (float)position.Y) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"Failed to set the local mouse position ({position}) in the window ({_lastActiveWindow}). ({Native.LastError})");

         return false;
      }

      if (Set(ref _localPosition, position, nameof(LocalPosition)))
      {
         RaisePropertyChanging(nameof(GlobalPosition));
         RaisePropertyChanged(nameof(GlobalPosition));
      }

      return true;
   }

   /// <inheritdoc/>
   public override bool StartCapture()
   {
      if (IsCaptured)
         return true;

      if (Native.CaptureMouse(true))
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Debug<SDL3MouseInputContext>($"Successfully started capturing the mouse events.");

         Set(ref _isCaptured, true, nameof(IsCaptured));
         return true;
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Error<SDL3MouseInputContext>($"Failed to start capturing the mouse events. ({Native.LastError})");

      return false;
   }

   /// <inheritdoc/>
   public override bool StopCapture()
   {
      if (IsCaptured is false)
         return true;

      if (Native.CaptureMouse(false))
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Debug<SDL3MouseInputContext>($"Successfully stopped capturing the mouse events.");

         Set(ref _isCaptured, false, nameof(IsCaptured));
         return true;
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Error<SDL3MouseInputContext>($"Failed to stop capturing the mouse events. ({Native.LastError})");

      return false;
   }

   /// <inheritdoc/>
   public override bool ShowCursor()
   {
      if (Native.ShowCursor())
      {
         Set(ref _isCursorVisible, true, nameof(IsCursorVisible));
         return true;
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Error<SDL3MouseInputContext>($"Failed to show the mouse cursor.");

      return false;
   }

   /// <inheritdoc/>
   public override bool HideCursor()
   {
      if (Native.HideCursor())
      {
         Set(ref _isCursorVisible, false, nameof(IsCursorVisible));
         return true;
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Error<SDL3MouseInputContext>($"Failed to hide the mouse cursor.");

      return false;
   }

   /// <inheritdoc/>
   public override bool IsSupported(MouseButton button) => _buttons.ContainsKey(button);

   /// <inheritdoc/>
   public override bool IsButtonUp(MouseButton button)
   {
      if (_buttons.TryGetValue(button, out MouseButtonState? state))
         return state.IsDown is false;

      throw new ArgumentException($"The given mouse button ({button}) is not supported.", nameof(button));
   }

   /// <inheritdoc/>
   public override bool IsButtonDown(MouseButton button)
   {
      if (_buttons.TryGetValue(button, out MouseButtonState? state))
         return state.IsDown;

      throw new ArgumentException($"The given mouse button ({button}) is not supported.", nameof(button));
   }
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   public override void Refresh()
   {
      RefreshDevices();
      RefreshGlobalState(false);
      RefreshLocalState(false);
      // RefreshIsCaptured(); // Note(Nightowl): No point in calling since it won't do anything;
      RefreshIsCursorVisible();
   }

   /// <inheritdoc/>
   public override void RefreshDevices()
   {
      HashSet<SDL3_MouseId> seen = [];

      foreach (SDL3_MouseId id in EnumerateMouseIds())
      {
         if (_deviceLookup.TryGetValue(id, out SDL3MouseDevice? device))
            device.Refresh();
         else
            AddDevice(id);

         seen.Add(id);
      }

      HashSet<SDL3_MouseId> toRemove = [];
      foreach (SDL3_MouseId id in _deviceLookup.Keys)
      {
         if (seen.Contains(id) is false)
            toRemove.Add(id);
      }

      foreach (SDL3_MouseId id in toRemove)
         RemoveDevice(id);
   }

   /// <inheritdoc/>
   public override void RefreshGlobalPosition() => RefreshGlobalState(true);
   /// <inheritdoc/>
   public override void RefreshLocalPosition() => RefreshLocalState(true);

   /// <inheritdoc/>
   public override void RefreshButtons() => RefreshLocalState(true);

   /// <inheritdoc/>
   public override void RefreshIsCaptured()
   {
      // Note(Nightowl): The current state cannot be retrieved from SDL;
   }

   /// <inheritdoc/>
   public override void RefreshIsCursorVisible() => Set(ref _isCursorVisible, Native.IsCursorVisible(), nameof(IsCursorVisible));
   #endregion

   #region Helpers
   private IEnumerable<SDL3_MouseId> EnumerateMouseIds()
   {
      nint ptr;
      SDL3_MouseId[] ids;

      unsafe
      {
         SDL3_MouseId* native = Native.GetMice(out int count);
         if (native is null)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Error<SDL3DisplayContext>($"Couldn't get the mouse devices. ({Native.LastError})");

            yield break;
         }

         Debug.Assert(count >= 0);

         ptr = new(native);
         ReadOnlySpan<SDL3_MouseId> span = new(native, count);
         ids = [.. span];
      }

      foreach (SDL3_MouseId id in ids)
         yield return id;

      unsafe { Native.Free(ptr.ToPointer()); }
   }
   private void RemoveDevice(SDL3_MouseId id)
   {
      if (_deviceLookup.TryGetValue(id, out SDL3MouseDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"A display mouse with an unknown mouse id was removed ({id}).");

         return;
      }

      _deviceLookup.Remove(device.MouseId);
      _devices.Remove(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3MouseInputContext>($"Mouse device removed, id = ({device.Id}), mouse id = ({device.MouseId})");
   }
   private void AddDevice(SDL3_MouseId id)
   {
      SDL3MouseDevice device = new(Context, id);
      if (_deviceLookup.TryAdd(id, device) is false)
      {
         SDL3MouseDevice old = _deviceLookup[id];

         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"A device with a duplicate mouse id ({id}) was detected, old id = ({old.Id}), new id = ({device.Id}).");

         _devices.Remove(old);
         _deviceLookup[id] = device; // Override
      }

      _devices.Add(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3MouseInputContext>($"Mouse device added, id = ({device.Id}), mouse id = ({device.MouseId})");
   }
   private void RefreshGlobalState(bool notifyLocal)
   {
      _ = Native.GetGlobalMouseState(out float x, out float y);

      Point position = GetPosition(x, y);
      if (Set(ref _globalPosition, position, nameof(GlobalPosition)) && notifyLocal)
      {
         RaisePropertyChanging(nameof(LocalPosition));
         RaisePropertyChanged(nameof(LocalPosition));
      }
   }
   private void RefreshLocalState(bool notifyGlobal)
   {
      SDL3_MouseButtonFlags buttons = Native.GetMouseState(out float x, out float y);

      UpdateButtonState(LeftButton, buttons.HasFlag(SDL3_MouseButtonFlags.Left));
      UpdateButtonState(MiddleButton, buttons.HasFlag(SDL3_MouseButtonFlags.Middle));
      UpdateButtonState(RightButton, buttons.HasFlag(SDL3_MouseButtonFlags.Right));
      UpdateButtonState(X1Button, buttons.HasFlag(SDL3_MouseButtonFlags.X1));
      UpdateButtonState(X2Button, buttons.HasFlag(SDL3_MouseButtonFlags.X2));

      Point position = GetPosition(x, y);
      if (Set(ref _localPosition, position, nameof(LocalPosition)) && notifyGlobal)
      {
         RaisePropertyChanging(nameof(GlobalPosition));
         RaisePropertyChanged(nameof(GlobalPosition));
      }
   }
   private void UpdateButtonState(MouseButton button, bool isDown)
   {
      Debug.Assert(_buttons.ContainsKey(button));

      MouseButtonState state = _buttons[button];
      if (state.SetIsDown(isDown))
      {
         if (state.IsDown)
            RaiseMouseButtonDown(button, state.Name);
         else
            RaiseMouseButtonUp(button, state.Name);
      }
   }
   void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsMouseDeviceEvent(out SDL3_MouseDeviceEvent device))
      {
         if (device.Type is SDL3_EventType.MouseAdded)
            AddDevice(device.MouseId);
         else if (device.Type is SDL3_EventType.MouseRemoved)
            RemoveDevice(device.MouseId);
         else if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"Unknown mouse device event ({device.Type}), this should've been known but wasn't handled properly here.");
      }
      else if (ev.IsMouseMotionEvent(out SDL3_MouseMotionEvent motion))
      {
         _lastActiveWindow = motion.WindowId;

         Point position = GetPosition(motion.X, motion.Y);
         if (Set(ref _localPosition, position, nameof(LocalPosition)))
         {
            RefreshGlobalState(false);
            RaiseMouseMoved(position, GlobalPosition);
         }
      }
      else if (ev.IsMouseButtonEvent(out SDL3_MouseButtonEvent button))
      {
         _lastActiveWindow = button.WindowId;

         MouseButton mouseButton = button.Button switch
         {
            SDL3_MouseButton.Left => LeftButton,
            SDL3_MouseButton.Middle => MiddleButton,
            SDL3_MouseButton.Right => RightButton,
            SDL3_MouseButton.X1 => X1Button,
            SDL3_MouseButton.X2 => X2Button,

            _ => default,
         };

         if (mouseButton == default)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Warning<SDL3MouseInputContext>($"Failed to raise the mouse button event for the mouse because the mouse button was unknown ({button.Button}).");

            return;
         }

         Point position = GetPosition(button.X, button.Y);
         if (Set(ref _localPosition, position, nameof(LocalPosition)))
         {
            RefreshGlobalState(false);
            RaiseMouseMoved(position, GlobalPosition);
         }

         UpdateButtonState(mouseButton, button.IsDown);
      }
      else if (ev.IsMouseWheelEvent(out SDL3_MouseWheelEvent wheel))
      {
         _lastActiveWindow = wheel.WindowId;
      }
   }
   private static Point GetPosition(float x, float y) => new((long)x, (long)y);
   #endregion
}
