using ScanCode = Sain.SDL3.SDL3_ScanCode;
using KeyCode = Sain.SDL3.SDL3_KeyCode;

namespace Sain.SDL3.Input.Keyboard;

/// <summary>
///   Represents the SDL3 specific context for keyboard input.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class SDL3KeyboardInputContext(IContextProvider? provider) : BaseKeyboardInputContext(provider), ISDL3Context
{
   #region Fields
   private readonly DeviceCollection<SDL3KeyboardDevice> _devices = [];
   private readonly Dictionary<SDL3_KeyboardId, SDL3KeyboardDevice> _deviceLookup = [];
   private readonly Dictionary<PhysicalKey, PhysicalKeyboardKeyState> _keys = [];
   private SDL3_WindowId _lastActiveWindow;
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Events;

   /// <inheritdoc/>
   public override IDeviceCollection<IKeyboardDevice> Devices => _devices;

   /// <inheritdoc/>
   public override IReadOnlyCollection<IPhysicalKeyboardKeyState> Keys => _keys.Values;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      SetupKeys();
      RefreshDevices();
   }
   private void SetupKeys()
   {
      Debug.Assert(_keys.Count is 0);

      ReadOnlySpan<byte> keys = Native.GetKeyboardState();
      for (int i = 0; i < keys.Length; i++)
      {
         ScanCode scanCode = (ScanCode)i;
         bool isDown = keys[i] is not 0;

         PhysicalKey physicalKey = Translate(scanCode);
         string name = Native.GetScanCodeName(scanCode);

         _keys.Add(physicalKey, new(physicalKey, name, isDown));
      }
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      _devices.Clear();
      _deviceLookup.Clear();
      _keys.Clear();
   }

   /// <inheritdoc/>
   public override bool IsKeyUp(PhysicalKey key)
   {
      if (_keys.TryGetValue(key, out PhysicalKeyboardKeyState? state))
         return state.IsDown is false;

      throw new ArgumentException($"The given keyboard key ({key}) is not supported.", nameof(key));
   }

   /// <inheritdoc/>
   public override bool IsKeyDown(PhysicalKey key)
   {
      if (_keys.TryGetValue(key, out PhysicalKeyboardKeyState? state))
         return state.IsDown;

      throw new ArgumentException($"The given keyboard key ({key}) is not supported.", nameof(key));
   }
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   public override void RefreshDevices()
   {
      HashSet<SDL3_KeyboardId> seen = [];

      foreach (SDL3_KeyboardId id in EnumerateKeyboardIds())
      {
         if (_deviceLookup.TryGetValue(id, out SDL3KeyboardDevice? device))
            device.Refresh();
         else
            AddDevice(id);

         seen.Add(id);
      }

      HashSet<SDL3_KeyboardId> toRemove = [];
      foreach (SDL3_KeyboardId id in _deviceLookup.Keys)
      {
         if (seen.Contains(id) is false)
            toRemove.Add(id);
      }

      foreach (SDL3_KeyboardId id in toRemove)
         RemoveDevice(id);
   }

   /// <inheritdoc/>
   public override void RefreshKeys()
   {
      ReadOnlySpan<byte> keys = Native.GetKeyboardState();
      Debug.Assert(keys.Length == _keys.Count);

      if (keys.Length != _keys.Count && Context.Logging.IsAvailable)
         Context.Logging.Warning<SDL3KeyboardInputContext>($"The amount of keys returned from the keyboard state ({keys.Length}), doesn't match the length of the internal keyboard state ({_keys.Count}).");

      for (int i = 0; i < keys.Length; i++)
      {
         ScanCode scanCode = (ScanCode)i;
         bool isDown = keys[i] is not 0;

         PhysicalKey physicalKey = Translate(scanCode);

         if (_keys.TryGetValue(physicalKey, out PhysicalKeyboardKeyState? state) is false)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Warning<SDL3KeyboardInputContext>($"Tried to update the state of an unknown keyboard key ({physicalKey}), native scan code = ({scanCode}).");

            continue;
         }

         KeyCode keyCode = Native.GetKeyFromScanCode(scanCode, SDL3_KeyModifiers.None, false);
         VirtualKey virtualKey = Translate(keyCode);
         string virtualKeyName = Native.GetKeyName(keyCode);

         if (state.SetIsDown(isDown))
         {
            if (state.IsDown)
               RaiseKeyboardKeyDown(physicalKey, state.Name, virtualKey, virtualKeyName, KeyModifiers.None, false);
            else
               RaiseKeyboardKeyUp(physicalKey, state.Name, virtualKey, virtualKeyName, KeyModifiers.None, false);
         }
      }
   }
   #endregion

   #region Helpers
   private IEnumerable<SDL3_KeyboardId> EnumerateKeyboardIds()
   {
      nint ptr;
      SDL3_KeyboardId[] ids;

      // Todo(Nightowl): Optimise ids allocation using ArrayPool?

      unsafe
      {
         SDL3_KeyboardId* native = Native.GetKeyboards(out int count);
         if (native is null)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Error<SDL3DisplayContext>($"Couldn't get the keyboard devices. ({Native.LastError}).");

            yield break;
         }

         Debug.Assert(count >= 0);

         ptr = new(native);
         ReadOnlySpan<SDL3_KeyboardId> span = new(native, count);
         ids = [.. span];
      }

      foreach (SDL3_KeyboardId id in ids)
         yield return id;

      unsafe { Native.Free(ptr.ToPointer()); }
   }
   private void RemoveDevice(SDL3_KeyboardId id)
   {
      if (_deviceLookup.TryGetValue(id, out SDL3KeyboardDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3KeyboardInputContext>($"A keyboard device with an unknown keyboard id was removed ({id}).");

         return;
      }

      _deviceLookup.Remove(device.KeyboardId);
      _devices.Remove(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3KeyboardInputContext>($"Keyboard device removed, id = ({device.Id}), keyboard id = ({device.KeyboardId}).");
   }
   private void AddDevice(SDL3_KeyboardId id)
   {
      SDL3KeyboardDevice device = new(Context, id);
      if (_deviceLookup.TryAdd(id, device) is false)
      {
         SDL3KeyboardDevice old = _deviceLookup[id];

         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3KeyboardInputContext>($"A device with a duplicate keyboard id ({id}) was detected, old id = ({old.Id}), new id = ({device.Id}).");

         _devices.Remove(old);
         _deviceLookup[id] = device; // Override
      }

      _devices.Add(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3KeyboardInputContext>($"Keyboard device added, id = ({device.Id}), keyboard id = ({device.KeyboardId}).");
   }
   private static PhysicalKey Translate(ScanCode code)
   {
      return code switch
      {
         ScanCode.UNKNOWN => PhysicalKey.Unknown,

         #region Latin alphabet keys
         ScanCode.A => new(PhysicalKeyKind.A),
         ScanCode.B => new(PhysicalKeyKind.B),
         ScanCode.C => new(PhysicalKeyKind.C),
         ScanCode.D => new(PhysicalKeyKind.D),
         ScanCode.E => new(PhysicalKeyKind.E),
         ScanCode.F => new(PhysicalKeyKind.F),
         ScanCode.G => new(PhysicalKeyKind.G),
         ScanCode.H => new(PhysicalKeyKind.H),
         ScanCode.I => new(PhysicalKeyKind.I),
         ScanCode.J => new(PhysicalKeyKind.J),
         ScanCode.K => new(PhysicalKeyKind.K),
         ScanCode.L => new(PhysicalKeyKind.L),
         ScanCode.M => new(PhysicalKeyKind.M),
         ScanCode.N => new(PhysicalKeyKind.N),
         ScanCode.O => new(PhysicalKeyKind.O),
         ScanCode.P => new(PhysicalKeyKind.P),
         ScanCode.Q => new(PhysicalKeyKind.Q),
         ScanCode.R => new(PhysicalKeyKind.R),
         ScanCode.S => new(PhysicalKeyKind.S),
         ScanCode.T => new(PhysicalKeyKind.T),
         ScanCode.U => new(PhysicalKeyKind.U),
         ScanCode.V => new(PhysicalKeyKind.V),
         ScanCode.W => new(PhysicalKeyKind.W),
         ScanCode.X => new(PhysicalKeyKind.X),
         ScanCode.Y => new(PhysicalKeyKind.Y),
         ScanCode.Z => new(PhysicalKeyKind.Z),
         #endregion
         #region Number keys
         ScanCode.Num1 => new(PhysicalKeyKind.Num1),
         ScanCode.Num2 => new(PhysicalKeyKind.Num2),
         ScanCode.Num3 => new(PhysicalKeyKind.Num3),
         ScanCode.Num4 => new(PhysicalKeyKind.Num4),
         ScanCode.Num5 => new(PhysicalKeyKind.Num5),
         ScanCode.Num6 => new(PhysicalKeyKind.Num6),
         ScanCode.Num7 => new(PhysicalKeyKind.Num7),
         ScanCode.Num8 => new(PhysicalKeyKind.Num8),
         ScanCode.Num9 => new(PhysicalKeyKind.Num9),
         ScanCode.Num0 => new(PhysicalKeyKind.Num0),
         #endregion
         #region Programmer keys
         ScanCode.MINUS => new(PhysicalKeyKind.Minus),
         ScanCode.EQUALS => new(PhysicalKeyKind.Equals),
         ScanCode.LEFTBRACKET => new(PhysicalKeyKind.LeftBracket),
         ScanCode.RIGHTBRACKET => new(PhysicalKeyKind.RightBracket),
         ScanCode.BACKSLASH => new(PhysicalKeyKind.Backslash),
         ScanCode.NONUSBACKSLASH => new(PhysicalKeyKind.NonUsBackslash),
         ScanCode.NONUSHASH => new(PhysicalKeyKind.Hash),
         ScanCode.SEMICOLON => new(PhysicalKeyKind.Semicolon),
         ScanCode.APOSTROPHE => new(PhysicalKeyKind.Apostrophe),
         ScanCode.GRAVE => new(PhysicalKeyKind.Grave),
         ScanCode.COMMA => new(PhysicalKeyKind.Comma),
         ScanCode.PERIOD => new(PhysicalKeyKind.Period),
         ScanCode.SLASH => new(PhysicalKeyKind.Slash),
         #endregion
         #region Function keys
         ScanCode.F1 => new(PhysicalKeyKind.F1),
         ScanCode.F2 => new(PhysicalKeyKind.F2),
         ScanCode.F3 => new(PhysicalKeyKind.F3),
         ScanCode.F4 => new(PhysicalKeyKind.F4),
         ScanCode.F5 => new(PhysicalKeyKind.F5),
         ScanCode.F6 => new(PhysicalKeyKind.F6),
         ScanCode.F7 => new(PhysicalKeyKind.F7),
         ScanCode.F8 => new(PhysicalKeyKind.F8),
         ScanCode.F9 => new(PhysicalKeyKind.F9),
         ScanCode.F10 => new(PhysicalKeyKind.F10),
         ScanCode.F11 => new(PhysicalKeyKind.F11),
         ScanCode.F12 => new(PhysicalKeyKind.F12),
         ScanCode.F13 => new(PhysicalKeyKind.F13),
         ScanCode.F14 => new(PhysicalKeyKind.F14),
         ScanCode.F15 => new(PhysicalKeyKind.F15),
         ScanCode.F16 => new(PhysicalKeyKind.F16),
         ScanCode.F17 => new(PhysicalKeyKind.F17),
         ScanCode.F18 => new(PhysicalKeyKind.F18),
         ScanCode.F19 => new(PhysicalKeyKind.F19),
         ScanCode.F20 => new(PhysicalKeyKind.F20),
         ScanCode.F21 => new(PhysicalKeyKind.F21),
         ScanCode.F22 => new(PhysicalKeyKind.F22),
         ScanCode.F23 => new(PhysicalKeyKind.F23),
         ScanCode.F24 => new(PhysicalKeyKind.F24),
         #endregion
         #region Programmer movement keys
         ScanCode.HOME => new(PhysicalKeyKind.Home),
         ScanCode.END => new(PhysicalKeyKind.End),
         ScanCode.PAGEUP => new(PhysicalKeyKind.PageUp),
         ScanCode.PAGEDOWN => new(PhysicalKeyKind.PageDown),
         #endregion
         #region Casual gamer movement keys
         ScanCode.LEFT => new(PhysicalKeyKind.Left),
         ScanCode.RIGHT => new(PhysicalKeyKind.Right),
         ScanCode.UP => new(PhysicalKeyKind.Up),
         ScanCode.DOWN => new(PhysicalKeyKind.Down),
         #endregion
         #region Keypad keys
         ScanCode.NUMLOCKCLEAR => new(PhysicalKeyKind.NumLockOrClear),
         ScanCode.Keypad_DIVIDE => new(PhysicalKeyKind.KeypadDivide),
         ScanCode.Keypad_MULTIPLY => new(PhysicalKeyKind.KeypadMultiply),
         ScanCode.Keypad_MINUS => new(PhysicalKeyKind.KeypadMinus),
         ScanCode.Keypad_PLUS => new(PhysicalKeyKind.KeypadPlus),
         ScanCode.Keypad_ENTER => new(PhysicalKeyKind.KeypadEnter),
         ScanCode.Keypad_PERIOD => new(PhysicalKeyKind.KeypadPeriod),
         ScanCode.Keypad_EQUALS => new(PhysicalKeyKind.KeypadEquals),
         ScanCode.Keypad_EQUALSAS400 => new(PhysicalKeyKind.KeypadEqualsAs400),
         ScanCode.Keypad_COMMA => new(PhysicalKeyKind.KeypadComma),
         ScanCode.Keypad_1 => new(PhysicalKeyKind.Keypad1),
         ScanCode.Keypad_2 => new(PhysicalKeyKind.Keypad2),
         ScanCode.Keypad_3 => new(PhysicalKeyKind.Keypad3),
         ScanCode.Keypad_4 => new(PhysicalKeyKind.Keypad4),
         ScanCode.Keypad_5 => new(PhysicalKeyKind.Keypad5),
         ScanCode.Keypad_6 => new(PhysicalKeyKind.Keypad6),
         ScanCode.Keypad_7 => new(PhysicalKeyKind.Keypad7),
         ScanCode.Keypad_8 => new(PhysicalKeyKind.Keypad8),
         ScanCode.Keypad_9 => new(PhysicalKeyKind.Keypad9),
         ScanCode.Keypad_0 => new(PhysicalKeyKind.Keypad0),
         ScanCode.Keypad_00 => new(PhysicalKeyKind.KeypadDouble0),
         ScanCode.Keypad_000 => new(PhysicalKeyKind.KeypadTriple0),
         ScanCode.Keypad_LEFTPAREN => new(PhysicalKeyKind.KeypadLeftBracket),
         ScanCode.Keypad_RIGHTPAREN => new(PhysicalKeyKind.KeypadRightBracket),
         ScanCode.Keypad_LEFTBRACE => new(PhysicalKeyKind.KeypadLeftBrace),
         ScanCode.Keypad_RIGHTBRACE => new(PhysicalKeyKind.KeypadRightBrace),
         ScanCode.Keypad_TAB => new(PhysicalKeyKind.KeypadTab),
         ScanCode.Keypad_BACKSPACE => new(PhysicalKeyKind.KeypadBackspace),
         ScanCode.Keypad_A => new(PhysicalKeyKind.KeypadA),
         ScanCode.Keypad_B => new(PhysicalKeyKind.KeypadB),
         ScanCode.Keypad_C => new(PhysicalKeyKind.KeypadC),
         ScanCode.Keypad_D => new(PhysicalKeyKind.KeypadD),
         ScanCode.Keypad_E => new(PhysicalKeyKind.KeypadE),
         ScanCode.Keypad_F => new(PhysicalKeyKind.KeypadF),
         ScanCode.Keypad_XOR => new(PhysicalKeyKind.KeypadXor),
         ScanCode.Keypad_POWER => new(PhysicalKeyKind.KeypadPower),
         ScanCode.Keypad_PERCENT => new(PhysicalKeyKind.KeypadPercent),
         ScanCode.Keypad_LESS => new(PhysicalKeyKind.KeypadLessThan),
         ScanCode.Keypad_GREATER => new(PhysicalKeyKind.KeypadGreaterThan),
         ScanCode.Keypad_AMPERSAND => new(PhysicalKeyKind.KeypadAmpersand),
         ScanCode.Keypad_DBLAMPERSAND => new(PhysicalKeyKind.KeypadDoubleAmpersand),
         ScanCode.Keypad_VERTICALBAR => new(PhysicalKeyKind.KeypadVerticalBar),
         ScanCode.Keypad_DBLVERTICALBAR => new(PhysicalKeyKind.KeypadDoubleVerticalBar),
         ScanCode.Keypad_COLON => new(PhysicalKeyKind.KeypadColon),
         ScanCode.Keypad_HASH => new(PhysicalKeyKind.KeypadHash),
         ScanCode.Keypad_SPACE => new(PhysicalKeyKind.KeypadSpace),
         ScanCode.Keypad_AT => new(PhysicalKeyKind.KeypadAt),
         ScanCode.Keypad_EXCLAM => new(PhysicalKeyKind.KeypadExclamation),
         ScanCode.Keypad_MEMSTORE => new(PhysicalKeyKind.KeypadMemoryStore),
         ScanCode.Keypad_MEMCLEAR => new(PhysicalKeyKind.KeypadMemoryClear),
         ScanCode.Keypad_MEMADD => new(PhysicalKeyKind.KeypadMemoryAdd),
         ScanCode.Keypad_MEMSUBTRACT => new(PhysicalKeyKind.KeypadMemorySubtract),
         ScanCode.Keypad_MEMMULTIPLY => new(PhysicalKeyKind.KeypadMemoryMultiply),
         ScanCode.Keypad_MEMDIVIDE => new(PhysicalKeyKind.KeypadMemoryDivide),
         ScanCode.Keypad_PLUSMINUS => new(PhysicalKeyKind.KeypadPlusMinus),
         ScanCode.Keypad_CLEAR => new(PhysicalKeyKind.KeypadClear),
         ScanCode.Keypad_CLEARENTRY => new(PhysicalKeyKind.KeypadClearEntry),
         ScanCode.Keypad_BINARY => new(PhysicalKeyKind.KeypadBinary),
         ScanCode.Keypad_OCTAL => new(PhysicalKeyKind.KeypadOctal),
         ScanCode.Keypad_DECIMAL => new(PhysicalKeyKind.KeypadDecimal),
         ScanCode.Keypad_HEXADECIMAL => new(PhysicalKeyKind.KeypadHexadecimal),
         #endregion
         #region Office keys
         ScanCode.HELP => new(PhysicalKeyKind.ApplicationHelp),
         ScanCode.MENU => new(PhysicalKeyKind.ApplicationMenu),
         ScanCode.SELECT => new(PhysicalKeyKind.ApplicationSelect),
         ScanCode.STOP => new(PhysicalKeyKind.ApplicationStop),
         ScanCode.AGAIN => new(PhysicalKeyKind.ApplicationAgain),
         ScanCode.UNDO => new(PhysicalKeyKind.ApplicationUndo),
         ScanCode.CUT => new(PhysicalKeyKind.ApplicationCut),
         ScanCode.COPY => new(PhysicalKeyKind.ApplicationCopy),
         ScanCode.PASTE => new(PhysicalKeyKind.ApplicationPaste),
         ScanCode.FIND => new(PhysicalKeyKind.ApplicationFind),
         ScanCode.AC_NEW => new(PhysicalKeyKind.ApplicationNew),
         ScanCode.AC_OPEN => new(PhysicalKeyKind.ApplicationOpen),
         ScanCode.AC_EXIT => new(PhysicalKeyKind.ApplicationExit),
         ScanCode.AC_SAVE => new(PhysicalKeyKind.ApplicationSave),
         ScanCode.AC_PRINT => new(PhysicalKeyKind.ApplicationPrint),
         ScanCode.AC_PROPERTIES => new(PhysicalKeyKind.ApplicationProperties),
         ScanCode.AC_SEARCH => new(PhysicalKeyKind.ApplicationSearch),
         ScanCode.AC_HOME => new(PhysicalKeyKind.ApplicationHome),
         ScanCode.AC_BACK => new(PhysicalKeyKind.ApplicationBack),
         ScanCode.AC_FORWARD => new(PhysicalKeyKind.ApplicationForward),
         ScanCode.AC_STOP => new(PhysicalKeyKind.ApplicationStop2),
         ScanCode.AC_REFRESH => new(PhysicalKeyKind.ApplicationRefresh),
         ScanCode.AC_BOOKMARKS => new(PhysicalKeyKind.ApplicationBookmarks),
         #endregion
         #region Media keys
         ScanCode.MUTE => new(PhysicalKeyKind.MediaMute),
         ScanCode.VOLUMEUP => new(PhysicalKeyKind.MediaVolumeUp),
         ScanCode.VOLUMEDOWN => new(PhysicalKeyKind.MediaVolumeDown),
         ScanCode.MEDIA_PLAY => new(PhysicalKeyKind.MediaPlay),
         ScanCode.MEDIA_PAUSE => new(PhysicalKeyKind.MediaPause),
         ScanCode.MEDIA_RECORD => new(PhysicalKeyKind.MediaRecord),
         ScanCode.MEDIA_FAST_FORWARD => new(PhysicalKeyKind.MediaFastForward),
         ScanCode.MEDIA_REWIND => new(PhysicalKeyKind.MediaRewind),
         ScanCode.MEDIA_NEXT_TRACK => new(PhysicalKeyKind.MediaNextTrack),
         ScanCode.MEDIA_PREVIOUS_TRACK => new(PhysicalKeyKind.MediaPreviousTrack),
         ScanCode.MEDIA_STOP => new(PhysicalKeyKind.MediaStop),
         ScanCode.MEDIA_EJECT => new(PhysicalKeyKind.MediaEject),
         ScanCode.MEDIA_PLAY_PAUSE => new(PhysicalKeyKind.MediaPlayPause),
         ScanCode.MEDIA_SELECT => new(PhysicalKeyKind.MediaSelect),
         #endregion
         #region International keys
         ScanCode.INTERNATIONAL1 => new(PhysicalKeyKind.International1),
         ScanCode.INTERNATIONAL2 => new(PhysicalKeyKind.International2),
         ScanCode.INTERNATIONAL3 => new(PhysicalKeyKind.International3),
         ScanCode.INTERNATIONAL4 => new(PhysicalKeyKind.International4),
         ScanCode.INTERNATIONAL5 => new(PhysicalKeyKind.International5),
         ScanCode.INTERNATIONAL6 => new(PhysicalKeyKind.International6),
         ScanCode.INTERNATIONAL7 => new(PhysicalKeyKind.International7),
         ScanCode.INTERNATIONAL8 => new(PhysicalKeyKind.International8),
         ScanCode.INTERNATIONAL9 => new(PhysicalKeyKind.International9),
         #endregion
         #region Language keys
         ScanCode.LANG1 => new(PhysicalKeyKind.Language1),
         ScanCode.LANG2 => new(PhysicalKeyKind.Language2),
         ScanCode.LANG3 => new(PhysicalKeyKind.Language3),
         ScanCode.LANG4 => new(PhysicalKeyKind.Language4),
         ScanCode.LANG5 => new(PhysicalKeyKind.Language5),
         ScanCode.LANG6 => new(PhysicalKeyKind.Language6),
         ScanCode.LANG7 => new(PhysicalKeyKind.Language7),
         ScanCode.LANG8 => new(PhysicalKeyKind.Language8),
         ScanCode.LANG9 => new(PhysicalKeyKind.Language9),
         #endregion
         #region I don't understand keys (seriously, what keyboards have these).
         ScanCode.ALTERASE => new(PhysicalKeyKind.Alterase),
         ScanCode.SYSREQ => new(PhysicalKeyKind.SysReq),
         ScanCode.CANCEL => new(PhysicalKeyKind.Cancel),
         ScanCode.CLEAR => new(PhysicalKeyKind.Clear),
         ScanCode.PRIOR => new(PhysicalKeyKind.Prior),
         ScanCode.RETURN2 => new(PhysicalKeyKind.Return2),
         ScanCode.SEPARATOR => new(PhysicalKeyKind.Separator),
         ScanCode.OPER => new(PhysicalKeyKind.Oper),
         ScanCode.CLEARAGAIN => new(PhysicalKeyKind.ClearAgain),
         ScanCode.CRSEL => new(PhysicalKeyKind.Crsel),
         ScanCode.EXSEL => new(PhysicalKeyKind.Exsel),
         ScanCode.THOUSANDSSEPARATOR => new(PhysicalKeyKind.ThousandSeparator),
         ScanCode.DECIMALSEPARATOR => new(PhysicalKeyKind.DecimalSeparator),
         ScanCode.CURRENCYUNIT => new(PhysicalKeyKind.CurrencyUnit),
         ScanCode.CURRENCYSUBUNIT => new(PhysicalKeyKind.CurrencySubUnit),
         #endregion
         #region Modifier keys
         ScanCode.LCTRL => new(PhysicalKeyKind.LeftControl),
         ScanCode.RCTRL => new(PhysicalKeyKind.RightControl),
         ScanCode.LSHIFT => new(PhysicalKeyKind.LeftShift),
         ScanCode.RSHIFT => new(PhysicalKeyKind.RightShift),
         ScanCode.LALT => new(PhysicalKeyKind.LeftAlt),
         ScanCode.RALT => new(PhysicalKeyKind.RightAlt),
         ScanCode.LGUI => new(PhysicalKeyKind.LeftMeta),
         ScanCode.RGUI => new(PhysicalKeyKind.RightMeta),
         ScanCode.MODE => new(PhysicalKeyKind.AltGr),
         ScanCode.CAPSLOCK => new(PhysicalKeyKind.CapsLock),
         ScanCode.SCROLLLOCK => new(PhysicalKeyKind.ScrollLock),
         #endregion
         #region Mobile keys
         ScanCode.SOFTLEFT => new(PhysicalKeyKind.MobileLeft),
         ScanCode.SOFTRIGHT => new(PhysicalKeyKind.MobileRight),
         ScanCode.CALL => new(PhysicalKeyKind.AcceptCall),
         ScanCode.ENDCALL => new(PhysicalKeyKind.EndCall),
         #endregion
         #region Special keys
         ScanCode.RETURN => new(PhysicalKeyKind.Return),
         ScanCode.ESCAPE => new(PhysicalKeyKind.Escape),
         ScanCode.BACKSPACE => new(PhysicalKeyKind.Backspace),
         ScanCode.TAB => new(PhysicalKeyKind.Tab),
         ScanCode.SPACE => new(PhysicalKeyKind.Space),
         ScanCode.PRINTSCREEN => new(PhysicalKeyKind.PrintScreen),
         ScanCode.PAUSE => new(PhysicalKeyKind.PauseBreak),
         ScanCode.INSERT => new(PhysicalKeyKind.Insert),
         ScanCode.DELETE => new(PhysicalKeyKind.Delete),
         ScanCode.APPLICATION => new(PhysicalKeyKind.Context),
         ScanCode.POWER => new(PhysicalKeyKind.Power),
         ScanCode.SLEEP => new(PhysicalKeyKind.Sleep),
         ScanCode.WAKE => new(PhysicalKeyKind.Wake),
         ScanCode.CHANNEL_INCREMENT => new(PhysicalKeyKind.ChannelIncrement),
         ScanCode.CHANNEL_DECREMENT => new(PhysicalKeyKind.ChannelDecrement),
         ScanCode.EXECUTE => new(PhysicalKeyKind.Execute),
         #endregion

         _ => new(PhysicalKeyKind.Other, (uint)code)
      };
   }
   private static VirtualKey Translate(KeyCode code)
   {
      return code switch
      {
         KeyCode.UNKNOWN => VirtualKey.Unknown,

         #region Latin alphabet keys
         KeyCode.A => new(VirtualKeyKind.A),
         KeyCode.B => new(VirtualKeyKind.B),
         KeyCode.C => new(VirtualKeyKind.C),
         KeyCode.D => new(VirtualKeyKind.D),
         KeyCode.E => new(VirtualKeyKind.E),
         KeyCode.F => new(VirtualKeyKind.F),
         KeyCode.G => new(VirtualKeyKind.G),
         KeyCode.H => new(VirtualKeyKind.H),
         KeyCode.I => new(VirtualKeyKind.I),
         KeyCode.J => new(VirtualKeyKind.J),
         KeyCode.K => new(VirtualKeyKind.K),
         KeyCode.L => new(VirtualKeyKind.L),
         KeyCode.M => new(VirtualKeyKind.M),
         KeyCode.N => new(VirtualKeyKind.N),
         KeyCode.O => new(VirtualKeyKind.O),
         KeyCode.P => new(VirtualKeyKind.P),
         KeyCode.Q => new(VirtualKeyKind.Q),
         KeyCode.R => new(VirtualKeyKind.R),
         KeyCode.S => new(VirtualKeyKind.S),
         KeyCode.T => new(VirtualKeyKind.T),
         KeyCode.U => new(VirtualKeyKind.U),
         KeyCode.V => new(VirtualKeyKind.V),
         KeyCode.W => new(VirtualKeyKind.W),
         KeyCode.X => new(VirtualKeyKind.X),
         KeyCode.Y => new(VirtualKeyKind.Y),
         KeyCode.Z => new(VirtualKeyKind.Z),
         #endregion
         #region Number keys
         KeyCode.Num_1 => new(VirtualKeyKind.N1),
         KeyCode.Num_2 => new(VirtualKeyKind.N2),
         KeyCode.Num_3 => new(VirtualKeyKind.N3),
         KeyCode.Num_4 => new(VirtualKeyKind.N4),
         KeyCode.Num_5 => new(VirtualKeyKind.N5),
         KeyCode.Num_6 => new(VirtualKeyKind.N6),
         KeyCode.Num_7 => new(VirtualKeyKind.N7),
         KeyCode.Num_8 => new(VirtualKeyKind.N8),
         KeyCode.Num_9 => new(VirtualKeyKind.N9),
         KeyCode.Num_0 => new(VirtualKeyKind.N0),
         #endregion
         #region Programmer keys
         KeyCode.MINUS => new(VirtualKeyKind.Minus),
         KeyCode.EQUALS => new(VirtualKeyKind.Equals),
         KeyCode.LEFTBRACKET => new(VirtualKeyKind.LeftBrace),
         KeyCode.RIGHTBRACKET => new(VirtualKeyKind.RightBrace),
         KeyCode.BACKSLASH => new(VirtualKeyKind.Backslash),
         KeyCode.SEMICOLON => new(VirtualKeyKind.Semicolon),
         KeyCode.APOSTROPHE => new(VirtualKeyKind.Apostrophe),
         KeyCode.GRAVE => new(VirtualKeyKind.Grave),
         KeyCode.COMMA => new(VirtualKeyKind.Comma),
         KeyCode.PERIOD => new(VirtualKeyKind.Period),
         KeyCode.SLASH => new(VirtualKeyKind.Slash),
         #endregion
         #region Function keys
         KeyCode.F1 => new(VirtualKeyKind.F1),
         KeyCode.F2 => new(VirtualKeyKind.F2),
         KeyCode.F3 => new(VirtualKeyKind.F3),
         KeyCode.F4 => new(VirtualKeyKind.F4),
         KeyCode.F5 => new(VirtualKeyKind.F5),
         KeyCode.F6 => new(VirtualKeyKind.F6),
         KeyCode.F7 => new(VirtualKeyKind.F7),
         KeyCode.F8 => new(VirtualKeyKind.F8),
         KeyCode.F9 => new(VirtualKeyKind.F9),
         KeyCode.F10 => new(VirtualKeyKind.F10),
         KeyCode.F11 => new(VirtualKeyKind.F11),
         KeyCode.F12 => new(VirtualKeyKind.F12),
         KeyCode.F13 => new(VirtualKeyKind.F13),
         KeyCode.F14 => new(VirtualKeyKind.F14),
         KeyCode.F15 => new(VirtualKeyKind.F15),
         KeyCode.F16 => new(VirtualKeyKind.F16),
         KeyCode.F17 => new(VirtualKeyKind.F17),
         KeyCode.F18 => new(VirtualKeyKind.F18),
         KeyCode.F19 => new(VirtualKeyKind.F19),
         KeyCode.F20 => new(VirtualKeyKind.F20),
         KeyCode.F21 => new(VirtualKeyKind.F21),
         KeyCode.F22 => new(VirtualKeyKind.F22),
         KeyCode.F23 => new(VirtualKeyKind.F23),
         KeyCode.F24 => new(VirtualKeyKind.F24),
         #endregion
         #region Programmer movement keys
         KeyCode.HOME => new(VirtualKeyKind.Home),
         KeyCode.END => new(VirtualKeyKind.End),
         KeyCode.PAGEUP => new(VirtualKeyKind.PageUp),
         KeyCode.PAGEDOWN => new(VirtualKeyKind.PageDown),
         #endregion
         #region Casual gamer movement keys
         KeyCode.LEFT => new(VirtualKeyKind.Left),
         KeyCode.RIGHT => new(VirtualKeyKind.Right),
         KeyCode.UP => new(VirtualKeyKind.Up),
         KeyCode.DOWN => new(VirtualKeyKind.Down),
         #endregion
         #region Keypad keys
         KeyCode.NUMLOCKCLEAR => new(VirtualKeyKind.NumLockOrClear),
         KeyCode.Keypad_DIVIDE => new(VirtualKeyKind.KeypadDivide),
         KeyCode.Keypad_MULTIPLY => new(VirtualKeyKind.KeypadMultiply),
         KeyCode.Keypad_MINUS => new(VirtualKeyKind.KeypadMinus),
         KeyCode.Keypad_PLUS => new(VirtualKeyKind.KeypadPlus),
         KeyCode.Keypad_ENTER => new(VirtualKeyKind.KeypadEnter),
         KeyCode.Keypad_PERIOD => new(VirtualKeyKind.KeypadPeriod),
         KeyCode.Keypad_EQUALS => new(VirtualKeyKind.KeypadEquals),
         KeyCode.Keypad_EQUALSAS400 => new(VirtualKeyKind.KeypadEqualsAs400),
         KeyCode.Keypad_COMMA => new(VirtualKeyKind.KeypadComma),
         KeyCode.Keypad_1 => new(VirtualKeyKind.Keypad1),
         KeyCode.Keypad_2 => new(VirtualKeyKind.Keypad2),
         KeyCode.Keypad_3 => new(VirtualKeyKind.Keypad3),
         KeyCode.Keypad_4 => new(VirtualKeyKind.Keypad4),
         KeyCode.Keypad_5 => new(VirtualKeyKind.Keypad5),
         KeyCode.Keypad_6 => new(VirtualKeyKind.Keypad6),
         KeyCode.Keypad_7 => new(VirtualKeyKind.Keypad7),
         KeyCode.Keypad_8 => new(VirtualKeyKind.Keypad8),
         KeyCode.Keypad_9 => new(VirtualKeyKind.Keypad9),
         KeyCode.Keypad_0 => new(VirtualKeyKind.Keypad0),
         KeyCode.Keypad_00 => new(VirtualKeyKind.KeypadDouble0),
         KeyCode.Keypad_000 => new(VirtualKeyKind.KeypadTriple0),
         KeyCode.Keypad_LEFTPAREN => new(VirtualKeyKind.KeypadLeftBracket),
         KeyCode.Keypad_RIGHTPAREN => new(VirtualKeyKind.KeypadRightBracket),
         KeyCode.Keypad_LEFTBRACE => new(VirtualKeyKind.KeypadLeftBrace),
         KeyCode.Keypad_RIGHTBRACE => new(VirtualKeyKind.KeypadRightBrace),
         KeyCode.Keypad_TAB => new(VirtualKeyKind.KeypadTab),
         KeyCode.Keypad_BACKSPACE => new(VirtualKeyKind.KeypadBackspace),
         KeyCode.Keypad_A => new(VirtualKeyKind.KeypadA),
         KeyCode.Keypad_B => new(VirtualKeyKind.KeypadB),
         KeyCode.Keypad_C => new(VirtualKeyKind.KeypadC),
         KeyCode.Keypad_D => new(VirtualKeyKind.KeypadD),
         KeyCode.Keypad_E => new(VirtualKeyKind.KeypadE),
         KeyCode.Keypad_F => new(VirtualKeyKind.KeypadF),
         KeyCode.Keypad_XOR => new(VirtualKeyKind.KeypadXor),
         KeyCode.Keypad_POWER => new(VirtualKeyKind.KeypadPower),
         KeyCode.Keypad_PERCENT => new(VirtualKeyKind.KeypadPercent),
         KeyCode.Keypad_LESS => new(VirtualKeyKind.KeypadLessThan),
         KeyCode.Keypad_GREATER => new(VirtualKeyKind.KeypadGreaterThan),
         KeyCode.Keypad_AMPERSAND => new(VirtualKeyKind.KeypadAmpersand),
         KeyCode.Keypad_DBLAMPERSAND => new(VirtualKeyKind.KeypadDoubleAmpersand),
         KeyCode.Keypad_VERTICALBAR => new(VirtualKeyKind.KeypadVerticalBar),
         KeyCode.Keypad_DBLVERTICALBAR => new(VirtualKeyKind.KeypadDoubleVerticalBar),
         KeyCode.Keypad_COLON => new(VirtualKeyKind.KeypadColon),
         KeyCode.Keypad_HASH => new(VirtualKeyKind.KeypadHash),
         KeyCode.Keypad_SPACE => new(VirtualKeyKind.KeypadSpace),
         KeyCode.Keypad_AT => new(VirtualKeyKind.KeypadAt),
         KeyCode.Keypad_EXCLAM => new(VirtualKeyKind.KeypadExclamation),
         KeyCode.Keypad_MEMSTORE => new(VirtualKeyKind.KeypadMemoryStore),
         KeyCode.Keypad_MEMCLEAR => new(VirtualKeyKind.KeypadMemoryClear),
         KeyCode.Keypad_MEMADD => new(VirtualKeyKind.KeypadMemoryAdd),
         KeyCode.Keypad_MEMSUBTRACT => new(VirtualKeyKind.KeypadMemorySubtract),
         KeyCode.Keypad_MEMMULTIPLY => new(VirtualKeyKind.KeypadMemoryMultiply),
         KeyCode.Keypad_MEMDIVIDE => new(VirtualKeyKind.KeypadMemoryDivide),
         KeyCode.Keypad_PLUSMINUS => new(VirtualKeyKind.KeypadPlusMinus),
         KeyCode.Keypad_CLEAR => new(VirtualKeyKind.KeypadClear),
         KeyCode.Keypad_CLEARENTRY => new(VirtualKeyKind.KeypadClearEntry),
         KeyCode.Keypad_BINARY => new(VirtualKeyKind.KeypadBinary),
         KeyCode.Keypad_OCTAL => new(VirtualKeyKind.KeypadOctal),
         KeyCode.Keypad_DECIMAL => new(VirtualKeyKind.KeypadDecimal),
         KeyCode.Keypad_HEXADECIMAL => new(VirtualKeyKind.KeypadHexadecimal),
         #endregion
         #region Application keys
         KeyCode.HELP => new(VirtualKeyKind.ApplicationHelp),
         KeyCode.MENU => new(VirtualKeyKind.ApplicationMenu),
         KeyCode.SELECT => new(VirtualKeyKind.ApplicationSelect),
         KeyCode.STOP => new(VirtualKeyKind.ApplicationStop),
         KeyCode.AGAIN => new(VirtualKeyKind.ApplicationAgain),
         KeyCode.UNDO => new(VirtualKeyKind.ApplicationUndo),
         KeyCode.CUT => new(VirtualKeyKind.ApplicationCut),
         KeyCode.COPY => new(VirtualKeyKind.ApplicationCopy),
         KeyCode.PASTE => new(VirtualKeyKind.ApplicationPaste),
         KeyCode.FIND => new(VirtualKeyKind.ApplicationFind),
         KeyCode.AC_NEW => new(VirtualKeyKind.ApplicationNew),
         KeyCode.AC_OPEN => new(VirtualKeyKind.ApplicationOpen),
         KeyCode.AC_CLOSE => new(VirtualKeyKind.ApplicationClose),
         KeyCode.AC_EXIT => new(VirtualKeyKind.ApplicationExit),
         KeyCode.AC_SAVE => new(VirtualKeyKind.ApplicationSave),
         KeyCode.AC_PRINT => new(VirtualKeyKind.ApplicationPrint),
         KeyCode.AC_PROPERTIES => new(VirtualKeyKind.ApplicationProperties),
         KeyCode.AC_SEARCH => new(VirtualKeyKind.ApplicationSearch),
         KeyCode.AC_HOME => new(VirtualKeyKind.ApplicationHome),
         KeyCode.AC_BACK => new(VirtualKeyKind.ApplicationBack),
         KeyCode.AC_FORWARD => new(VirtualKeyKind.ApplicationForward),
         KeyCode.AC_STOP => new(VirtualKeyKind.ApplicationStop2),
         KeyCode.AC_REFRESH => new(VirtualKeyKind.ApplicationRefresh),
         KeyCode.AC_BOOKMARKS => new(VirtualKeyKind.ApplicationBookmarks),
         #endregion
         #region Media keys
         KeyCode.MUTE => new(VirtualKeyKind.MediaMute),
         KeyCode.VOLUMEUP => new(VirtualKeyKind.MediaVolumeUp),
         KeyCode.VOLUMEDOWN => new(VirtualKeyKind.MediaVolumeDown),
         KeyCode.MEDIA_PLAY => new(VirtualKeyKind.MediaPlay),
         KeyCode.MEDIA_PAUSE => new(VirtualKeyKind.MediaPause),
         KeyCode.MEDIA_RECORD => new(VirtualKeyKind.MediaRecord),
         KeyCode.MEDIA_FAST_FORWARD => new(VirtualKeyKind.MediaFastForward),
         KeyCode.MEDIA_REWIND => new(VirtualKeyKind.MediaRewind),
         KeyCode.MEDIA_NEXT_TRACK => new(VirtualKeyKind.MediaNextTrack),
         KeyCode.MEDIA_PREVIOUS_TRACK => new(VirtualKeyKind.MediaPreviousTrack),
         KeyCode.MEDIA_STOP => new(VirtualKeyKind.MediaStop),
         KeyCode.MEDIA_EJECT => new(VirtualKeyKind.MediaEject),
         KeyCode.MEDIA_PLAY_PAUSE => new(VirtualKeyKind.MediaPlayPause),
         KeyCode.MEDIA_SELECT => new(VirtualKeyKind.MediaSelect),
         #endregion
         #region I don't understand keys (KeyCode.seriously => new(VirtualKeyKind.seriously), what keyboards have these).
         KeyCode.ALTERASE => new(VirtualKeyKind.Alterase),
         KeyCode.SYSREQ => new(VirtualKeyKind.SysReq),
         KeyCode.CANCEL => new(VirtualKeyKind.Cancel),
         KeyCode.CLEAR => new(VirtualKeyKind.Clear),
         KeyCode.PRIOR => new(VirtualKeyKind.Prior),
         KeyCode.RETURN2 => new(VirtualKeyKind.Return2),
         KeyCode.SEPARATOR => new(VirtualKeyKind.Separator),
         KeyCode.OPER => new(VirtualKeyKind.Oper),
         KeyCode.CLEARAGAIN => new(VirtualKeyKind.ClearAgain),
         KeyCode.CRSEL => new(VirtualKeyKind.Crsel),
         KeyCode.EXSEL => new(VirtualKeyKind.Exsel),
         KeyCode.THOUSANDSSEPARATOR => new(VirtualKeyKind.ThousandSeparator),
         KeyCode.DECIMALSEPARATOR => new(VirtualKeyKind.DecimalSeparator),
         KeyCode.CURRENCYUNIT => new(VirtualKeyKind.CurrencyUnit),
         KeyCode.CURRENCYSUBUNIT => new(VirtualKeyKind.CurrencySubUnit),
         #endregion
         #region Modifier keys
         KeyCode.LCTRL => new(VirtualKeyKind.LeftControl),
         KeyCode.RCTRL => new(VirtualKeyKind.RightControl),
         KeyCode.LSHIFT => new(VirtualKeyKind.LeftShift),
         KeyCode.RSHIFT => new(VirtualKeyKind.RightShift),
         KeyCode.LALT => new(VirtualKeyKind.LeftAlt),
         KeyCode.RALT => new(VirtualKeyKind.RightAlt),
         KeyCode.LGUI => new(VirtualKeyKind.LeftMeta),
         KeyCode.RGUI => new(VirtualKeyKind.RightMeta),
         KeyCode.MODE => new(VirtualKeyKind.AltGr),
         KeyCode.CAPSLOCK => new(VirtualKeyKind.CapsLock),
         KeyCode.SCROLLLOCK => new(VirtualKeyKind.ScrollLock),
         #endregion
         #region Mobile keys
         KeyCode.SOFTLEFT => new(VirtualKeyKind.SoftwareLeft),
         KeyCode.SOFTRIGHT => new(VirtualKeyKind.SoftwareRight),
         KeyCode.CALL => new(VirtualKeyKind.AcceptCall),
         KeyCode.ENDCALL => new(VirtualKeyKind.EndCall),
         #endregion
         #region Special keys
         KeyCode.RETURN => new(VirtualKeyKind.Return),
         KeyCode.ESCAPE => new(VirtualKeyKind.Escape),
         KeyCode.BACKSPACE => new(VirtualKeyKind.Backspace),
         KeyCode.TAB => new(VirtualKeyKind.Tab),
         KeyCode.SPACE => new(VirtualKeyKind.Space),
         KeyCode.PRINTSCREEN => new(VirtualKeyKind.PrintScreen),
         KeyCode.PAUSE => new(VirtualKeyKind.PauseBreak),
         KeyCode.INSERT => new(VirtualKeyKind.Insert),
         KeyCode.DELETE => new(VirtualKeyKind.Delete),
         KeyCode.APPLICATION => new(VirtualKeyKind.Context),
         KeyCode.POWER => new(VirtualKeyKind.Power),
         KeyCode.SLEEP => new(VirtualKeyKind.Sleep),
         KeyCode.WAKE => new(VirtualKeyKind.Wake),
         KeyCode.CHANNEL_INCREMENT => new(VirtualKeyKind.ChannelIncrement),
         KeyCode.CHANNEL_DECREMENT => new(VirtualKeyKind.ChannelDecrement),
         KeyCode.EXECUTE => new(VirtualKeyKind.Execute),
         #endregion

         _ => new(VirtualKeyKind.Other, (uint)code)
      };
   }
   private static KeyModifiers Translate(SDL3_KeyModifiers sdl)
   {
      KeyModifiers modifiers = KeyModifiers.None;

      if (sdl.HasFlag(SDL3_KeyModifiers.LeftControl)) modifiers |= KeyModifiers.LeftControl;
      if (sdl.HasFlag(SDL3_KeyModifiers.RightControl)) modifiers |= KeyModifiers.RightControl;
      if (sdl.HasFlag(SDL3_KeyModifiers.LeftShift)) modifiers |= KeyModifiers.LeftShift;
      if (sdl.HasFlag(SDL3_KeyModifiers.RightShift)) modifiers |= KeyModifiers.RightShift;
      if (sdl.HasFlag(SDL3_KeyModifiers.LeftAlt)) modifiers |= KeyModifiers.LeftAlt;
      if (sdl.HasFlag(SDL3_KeyModifiers.RightAlt)) modifiers |= KeyModifiers.RightAlt;
      if (sdl.HasFlag(SDL3_KeyModifiers.LeftMeta)) modifiers |= KeyModifiers.LeftMeta;
      if (sdl.HasFlag(SDL3_KeyModifiers.RightMeta)) modifiers |= KeyModifiers.RightMeta;
      if (sdl.HasFlag(SDL3_KeyModifiers.NumLock)) modifiers |= KeyModifiers.NumLock;
      if (sdl.HasFlag(SDL3_KeyModifiers.CapsLock)) modifiers |= KeyModifiers.CapsLock;
      if (sdl.HasFlag(SDL3_KeyModifiers.Mode)) modifiers |= KeyModifiers.AltGr;
      if (sdl.HasFlag(SDL3_KeyModifiers.ScrollLock)) modifiers |= KeyModifiers.ScrollLock;

      return modifiers;
   }
   void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsKeyboardDeviceEvent(out SDL3_KeyboardDeviceEvent device))
      {
         if (ev.Type is SDL3_EventType.KeyboardAdded)
            AddDevice(device.KeyboardId);
         else if (device.Type is SDL3_EventType.KeyboardRemoved)
            RemoveDevice(device.KeyboardId);
         else if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3KeyboardInputContext>($"Unknown keyboard device event ({device.Type}), this should've been known but wasn't handled properly here.");
      }
      else if (ev.IsKeyboardEvent(out SDL3_KeyboardEvent keyboard))
         OnKeyboardEvent(keyboard);
   }
   private void OnKeyboardEvent(SDL3_KeyboardEvent keyboard)
   {
      _lastActiveWindow = keyboard.WindowId;

      PhysicalKey physicalKey = Translate(keyboard.ScanCode);

      if (_keys.TryGetValue(physicalKey, out PhysicalKeyboardKeyState? state) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3KeyboardInputContext>($"Tried to update the state of an unknown keyboard key ({physicalKey}) during a keyboard key event, native scan code = ({keyboard.ScanCode}).");

         return;
      }

      VirtualKey virtualKey = Translate(keyboard.KeyCode);
      string virtualKeyName = Native.GetKeyName(keyboard.KeyCode);

      KeyModifiers modifiers = Translate(keyboard.Modifiers);

      if (state.SetIsDown(keyboard.IsDown) || keyboard.IsRepeat)
      {
         if (keyboard.IsDown)
            RaiseKeyboardKeyDown(physicalKey, state.Name, virtualKey, virtualKeyName, modifiers, keyboard.IsRepeat);
         else
            RaiseKeyboardKeyUp(physicalKey, state.Name, virtualKey, virtualKeyName, modifiers, keyboard.IsRepeat);
      }
   }
   #endregion
}

