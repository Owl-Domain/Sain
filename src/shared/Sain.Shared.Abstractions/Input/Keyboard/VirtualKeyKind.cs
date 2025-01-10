namespace Sain.Shared.Input.Keyboard;

// Todo(Nightowl): Lock enum values upon release;

/// <summary>
///   Represents the different known virtual keys.
/// </summary>
public enum VirtualKeyKind : ushort
{
   /// <summary>The virtual key is not known.</summary>
   Unknown,

   /// <summary>The virtual key is known, but it has not been defined.</summary>
   /// <remarks>This represents a category and not just a single key.</remarks>
   Other,

   /// <summary>The virtual key is reserved.</summary>
   /// <remarks>This represents a category and not just a single key.</remarks>
   Reserved,

   #region Latin alphabet keys
   /// <summary>The <c>A</c> key.</summary>
   A,

   /// <summary>The <c>B</c> key.</summary>
   B,

   /// <summary>The <c>C</c> key.</summary>
   C,

   /// <summary>The <c>D</c> key.</summary>
   D,

   /// <summary>The <c>E</c> key.</summary>
   E,

   /// <summary>The <c>F</c> key.</summary>
   F,

   /// <summary>The <c>G</c> key.</summary>
   G,

   /// <summary>The <c>H</c> key.</summary>
   H,

   /// <summary>The <c>I</c> key.</summary>
   I,

   /// <summary>The <c>J</c> key.</summary>
   J,

   /// <summary>The <c>K</c> key.</summary>
   K,

   /// <summary>The <c>L</c> key.</summary>
   L,

   /// <summary>The <c>M</c> key.</summary>
   M,

   /// <summary>The <c>N</c> key.</summary>
   N,

   /// <summary>The <c>O</c> key.</summary>
   O,

   /// <summary>The <c>P</c> key.</summary>
   P,

   /// <summary>The <c>Q</c> key.</summary>
   Q,

   /// <summary>The <c>R</c> key.</summary>
   R,

   /// <summary>The <c>S</c> key.</summary>
   S,

   /// <summary>The <c>T</c> key.</summary>
   T,

   /// <summary>The <c>U</c> key.</summary>
   U,

   /// <summary>The <c>V</c> key.</summary>
   V,

   /// <summary>The <c>W</c> key.</summary>
   W,

   /// <summary>The <c>X</c> key.</summary>
   X,

   /// <summary>The <c>Y</c> key.</summary>
   Y,

   /// <summary>The <c>Z</c> key.</summary>
   Z,
   #endregion

   #region Number keys
   /// <summary>The number <c>1</c> key.</summary>
   N1,

   /// <summary>The number <c>2</c> key.</summary>
   N2,

   /// <summary>The number <c>3</c> key.</summary>
   N3,

   /// <summary>The number <c>4</c> key.</summary>
   N4,

   /// <summary>The number <c>5</c> key.</summary>
   N5,

   /// <summary>The number <c>6</c> key.</summary>
   N6,

   /// <summary>The number <c>7</c> key.</summary>
   N7,

   /// <summary>The number <c>8</c> key.</summary>
   N8,

   /// <summary>The number <c>9</c> key.</summary>
   N9,

   /// <summary>The number <c>0</c> key.</summary>
   N0,
   #endregion

   #region Programmer keys
   /// <summary>The minus <c>-</c> key.</summary>
   /// <remarks>This is typically next to the numbers on the top row of the keyboard.</remarks>
   Minus,

   /// <summary>The equals <c>=</c> key.</summary>
   /// <remarks>This is typically next to the numbers on the top row of the keyboard.</remarks>
   Equals,

   /// <summary>The left bracket <c>{</c> key.</summary>
   LeftBrace,

   /// <summary>The right bracket <c>}</c> key.</summary>
   RightBrace,

   /// <summary>The backslash <c>\</c> key.</summary>
   Backslash,

   /// <summary>The semicolon <c>;</c> key.</summary>
   Semicolon,

   /// <summary>The apostrophe <c>'</c> key.</summary>
   Apostrophe,

   /// <summary>The grave <c>`</c> key.</summary>
   Grave,

   /// <summary>The comma <c>,</c> key.</summary>
   Comma,

   /// <summary>The period <c>.</c> key.</summary>
   /// <remarks>Also known as a full stop.</remarks>
   Period,

   /// <summary>The slash <c>/</c> key.</summary>
   /// <remarks>Also known as a forward slash.</remarks>
   Slash,
   #endregion

   #region Function keys
   /// <summary>The F1 key.</summary>
   /// <remarks>Also known as the function 1 key, not the Formula 1 key, and it will not turn your computer into an ultra fast race car.</remarks>
   F1,

   /// <summary>The F2 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F2,

   /// <summary>The F3 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F3,

   /// <summary>The F4 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F4,

   /// <summary>The F5 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F5,

   /// <summary>The F6 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F6,

   /// <summary>The F7 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F7,

   /// <summary>The F8 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F8,

   /// <summary>The F9 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F9,

   /// <summary>The F10 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F10,

   /// <summary>The F11 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F11,

   /// <summary>The F12 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F12,

   /// <summary>The F13 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F13,

   /// <summary>The F14 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F14,

   /// <summary>The F15 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F15,

   /// <summary>The F16 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F16,

   /// <summary>The F17 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F17,

   /// <summary>The F18 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F18,

   /// <summary>The F19 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F19,

   /// <summary>The F20 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F20,

   /// <summary>The F21 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F21,

   /// <summary>The F22 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F22,

   /// <summary>The F23 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F23,

   /// <summary>The F24 key.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F24,
   #endregion

   #region Programmer movement keys
   /// <summary>The home key.</summary>
   /// <remarks>Take me homeeeeee, down country roaaaaaaadsssss.</remarks>
   Home,

   /// <summary>The nd key.</summary>
   /// <remarks>Does not end the universe, I hope.</remarks>
   End,

   /// <summary>The page up key.</summary>
   /// <remarks>Does anything actually use this to mean a <i>full</i> page up?</remarks>
   PageUp,

   /// <summary>The page down key.</summary>
   /// <remarks>Does anything actually use this to mean a <i>full</i> page down?</remarks>
   PageDown,
   #endregion

   #region Casual gamer movement keys
   /// <summary>The left arrow key.</summary>
   Left,

   /// <summary>The right arrow key.</summary>
   Right,

   /// <summary>The up arrow key.</summary>
   Up,

   /// <summary>The down arrow key.</summary>
   Down,
   #endregion

   #region Keypad keys
   /// <summary>The num lock (or clear) key.</summary>
   /// <remarks>Ewww why am I actually taking into account an apple product... Damn you SDL for making me aware of this difference.</remarks>
   NumLockOrClear,

   /// <summary>The divide key  on the keypad.</summary>
   KeypadDivide,

   /// <summary>The multiply key on the keypad.</summary>
   KeypadMultiply,

   /// <summary>The minus key on the keypad.</summary>
   KeypadMinus,

   /// <summary>The plus key on the keypad.</summary>
   KeypadPlus,

   /// <summary>The enter key on the keypad.</summary>
   KeypadEnter,

   /// <summary>The period key on the keypad.</summary>
   /// <remarks>Also known as a full stop.</remarks>
   KeypadPeriod,

   /// <summary>The equals key on the keypad.</summary>
   /// <remarks>I had no idea this key ever existed.</remarks>
   KeypadEquals,

   /// <summary>The (weird?) equals key on the keypad.</summary>
   /// <remarks>Seriously, what is this for.</remarks>
   KeypadEqualsAs400,

   /// <summary>The command key on the keypad.</summary>
   /// <remarks>Again, I had no idea that this key ever existed.</remarks>
   KeypadComma,

   /// <summary>The number 1 key on the keypad.</summary>
   Keypad1,

   /// <summary>The number 2 key on the keypad.</summary>
   Keypad2,

   /// <summary>The number 3 key on the keypad.</summary>
   Keypad3,

   /// <summary>The number 4 key on the keypad.</summary>
   Keypad4,

   /// <summary>The number 5 key on the keypad.</summary>
   Keypad5,

   /// <summary>The number 6 key on the keypad.</summary>
   Keypad6,

   /// <summary>The number 7 key on the keypad.</summary>
   Keypad7,

   /// <summary>The number 8 key on the keypad.</summary>
   Keypad8,

   /// <summary>The number 9 key on the keypad.</summary>
   Keypad9,

   /// <summary>The number 0 key on the keypad.</summary>
   Keypad0,

   /// <summary>The 00 key on the keypad.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   KeypadDouble0,

   /// <summary>The 000 key on the keypad.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   KeypadTriple0,

   /// <summary>The left bracket '(' key on the keypad.</summary>
   KeypadLeftBracket,

   /// <summary>The right bracket ')' key on the keypad.</summary>
   KeypadRightBracket,

   /// <summary>The left brace '}' key on the keypad.</summary>
   KeypadLeftBrace,

   /// <summary>The right '}' brace key on the keypad.</summary>
   KeypadRightBrace,

   /// <summary>The tab key on the keypad.</summary>
   KeypadTab,

   /// <summary>The backspace key on the keypad.</summary>
   /// <remarks>Seriously, why is the keypad just turning into a full keyboard...?</remarks>
   KeypadBackspace,

   /// <summary>The A key on the keypad.</summary>
   KeypadA,

   /// <summary>The B key on the keypad.</summary>
   KeypadB,

   /// <summary>The C key on the keypad.</summary>
   KeypadC,

   /// <summary>The D key on the keypad.</summary>
   KeypadD,

   /// <summary>The E key on the keypad.</summary>
   KeypadE,

   /// <summary>The F key on the keypad.</summary>
   KeypadF,

   /// <summary>The XOR key on the keypad.</summary>
   KeypadXor,

   /// <summary>The power key on the keypad.</summary>
   KeypadPower,

   /// <summary>The percent key on the keypad.</summary>
   KeypadPercent,

   /// <summary>The less than key on the keypad.</summary>
   KeypadLessThan,

   /// <summary>The greater than key on the keypad.</summary>
   KeypadGreaterThan,

   /// <summary>The ampersand (&amp;) key on the keypad.</summary>
   KeypadAmpersand,

   /// <summary>The double ampersand (&amp;&amp;) key on the keypad.</summary>
   KeypadDoubleAmpersand,

   /// <summary>The vertical bar (|) key on the keypad.</summary>
   /// <remarks>Also known as the pipe.</remarks>
   KeypadVerticalBar,

   /// <summary>The double vertical bar (||) key on the keypad.</summary>
   /// <remarks>Also known as the double pipe.</remarks>
   KeypadDoubleVerticalBar,

   /// <summary>The colon key on the keypad.</summary>
   KeypadColon,

   /// <summary>The hash key on the keypad.</summary>
   KeypadHash,

   /// <summary>The space key on the keypad.</summary>
   KeypadSpace,

   /// <summary>The at (@) key on the keypad.</summary>
   KeypadAt,

   /// <summary>The exclamation (!) key on the keypad.</summary>
   KeypadExclamation,

   /// <summary>The memory store key on the keypad.</summary>
   KeypadMemoryStore,

   /// <summary>The memory clear key on the keypad.</summary>
   KeypadMemoryClear,

   /// <summary>The memory add key on the keypad.</summary>
   KeypadMemoryAdd,

   /// <summary>The memory subtract key on the keypad.</summary>
   KeypadMemorySubtract,

   /// <summary>The memory multiply key on the keypad.</summary>
   KeypadMemoryMultiply,

   /// <summary>The memory divide key on the keypad.</summary>
   KeypadMemoryDivide,

   /// <summary>The memory plus minus key on the keypad.</summary>
   KeypadPlusMinus,

   /// <summary>The clear key on the keypad.</summary>
   KeypadClear,

   /// <summary>The clear entry key on the keypad.</summary>
   KeypadClearEntry,

   /// <summary>The binary key on the keypad.</summary>
   KeypadBinary,

   /// <summary>The octal key on the keypad.</summary>
   KeypadOctal,

   /// <summary>The decimal key on the keypad.</summary>
   KeypadDecimal,

   /// <summary>The hexadecimal key on the keypad.</summary>
   KeypadHexadecimal,
   #endregion

   #region Application keys
   /// <summary>The application help key.</summary>
   /// <remarks>Damn, didn't know there was a button dedicated to it.</remarks>
   ApplicationHelp,

   /// <summary>The application menu key.</summary>
   /// <remarks>This sounds like <see cref="Context"/>,
   /// but apparently it's different?</remarks>
   ApplicationMenu,

   /// <summary>The application select key.</summary>
   ApplicationSelect,

   /// <summary>The application stop key.</summary>
   ApplicationStop,

   /// <summary>The application again key.</summary>
   ApplicationAgain,

   /// <summary>The application undo key.</summary>
   ApplicationUndo,

   /// <summary>The application cut key.</summary>
   ApplicationCut,

   /// <summary>The application copy key.</summary>
   ApplicationCopy,

   /// <summary>The application paste key.</summary>
   ApplicationPaste,

   /// <summary>The application find key.</summary>
   ApplicationFind,

   /// <summary>The application new key.</summary>
   ApplicationNew,

   /// <summary>The application open key.</summary>
   ApplicationOpen,

   /// <summary>The application close key.</summary>
   ApplicationClose,

   /// <summary>The application exit key.</summary>
   ApplicationExit,

   /// <summary>The application save key.</summary>
   ApplicationSave,

   /// <summary>The application print key.</summary>
   ApplicationPrint,

   /// <summary>The application properties key.</summary>
   ApplicationProperties,

   /// <summary>The application search key.</summary>
   ApplicationSearch,

   /// <summary>The application home key.</summary>
   ApplicationHome,

   /// <summary>The application back key.</summary>
   ApplicationBack,

   /// <summary>The application forward key.</summary>
   ApplicationForward,

   /// <summary>The application stop key.</summary>
   /// <remarks>Why are there 2?</remarks>
   ApplicationStop2,

   /// <summary>The application refresh key.</summary>
   ApplicationRefresh,

   /// <summary>The application bookmarks key.</summary>
   ApplicationBookmarks,
   #endregion

   #region Media keys
   /// <summary>The mute key.</summary>
   /// <remarks>If this sets the volume to 0 instead of actually muting and then unmuting, then I feel sorry for you.</remarks>
   MediaMute,

   /// <summary>The volume up key.</summary>
   /// <remarks>Makes sounds louder.</remarks>
   MediaVolumeUp,

   /// <summary>The volume down key.</summary>
   /// <remarks>Makes sounds quieter.</remarks>
   MediaVolumeDown,

   /// <summary>The play media key.</summary>
   /// <remarks>The key that is notorious for not being implemented properly in applications.</remarks>
   MediaPlay,

   /// <summary>The pause media key.</summary>
   MediaPause,

   /// <summary>The record media key.</summary>
   MediaRecord,

   /// <summary>The fast forward media key.</summary>
   MediaFastForward,

   /// <summary>The rewind media key.</summary>
   MediaRewind,

   /// <summary>The next track media key.</summary>
   MediaNextTrack,

   /// <summary>The previous track media key.</summary>
   MediaPreviousTrack,

   /// <summary>The stop media key.</summary>
   MediaStop,

   /// <summary>The eject media key.</summary>
   MediaEject,

   /// <summary>The play/pause media toggle key.</summary>
   MediaPlayPause,

   /// <summary>The select media key.</summary>
   MediaSelect,
   #endregion

   #region I don't understand keys (seriously, what keyboards have these).
   /// <summary>The alterase key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Alterase,

   /// <summary>The sysreq key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   SysReq,

   /// <summary>The cancel key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Cancel,

   /// <summary>The clear key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Clear,

   /// <summary>The prior key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Prior,

   /// <summary>The return2 key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Return2,

   /// <summary>The separator key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Separator,

   /// <summary>The oper (operation?) key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Oper,

   /// <summary>The alterase key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   ClearAgain,

   /// <summary>The clear again key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Crsel,

   /// <summary>The exsel key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Exsel,

   /// <summary>The thousand separator key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   ThousandSeparator,

   /// <summary>The decimal key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   DecimalSeparator,

   /// <summary>The currency unit key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   CurrencyUnit,

   /// <summary>The currency sub-unit key.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   CurrencySubUnit,
   #endregion

   #region Modifier keys
   /// <summary>The left control (CTRL) key.</summary>
   LeftControl,

   /// <summary>The right control (CTRL) key.</summary>
   RightControl,

   /// <summary>The left shift key.</summary>
   LeftShift,

   /// <summary>The right shift key.</summary>
   RightShift,

   /// <summary>The left alt key.</summary>
   LeftAlt,

   /// <summary>The right alt key.</summary>
   RightAlt,

   /// <summary>The left meta key.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   LeftMeta,

   /// <summary>The right meta key.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   RightMeta,

   /// <summary>The Alt Gr modifier key.</summary>
   /// <remarks><i>I think...</i></remarks>
   AltGr,

   /// <summary>The Caps Lock key.</summary>
   /// <remarks>
   ///   Aka the annoying button I never use seriously and just like to spam when I'm thinking.
   ///   Almost like clicking a physical pen...
   /// </remarks>
   CapsLock,

   /// <summary>The Scroll Lock key.</summary>
   /// <remarks>I still haven't figured out what this key is actually used for.</remarks>
   ScrollLock,
   #endregion

   #region Mobile keys
   /// <summary>The software specific mobile phone key.</summary>
   /// <remarks>This key is usually on the left side of the mobile phone.</remarks>
   SoftwareLeft,

   /// <summary>The software specific mobile phone key.</summary>
   /// <remarks>This key is usually on the right side of the mobile phone.</remarks>
   SoftwareRight,

   /// <summary>The mobile phone key for accepting calls.</summary>
   AcceptCall,

   /// <summary>The mobile phone key for ending calls.</summary>
   EndCall,
   #endregion

   #region Special keys
   /// <summary>The return key.</summary>
   /// <remarks>This is the enter key.</remarks>
   Return,

   /// <summary>The escape key.</summary>
   Escape,

   /// <summary>The backspace key.</summary>
   Backspace,

   /// <summary>The tab key.</summary>
   Tab,

   /// <summary>The space key.</summary>
   /// <remarks>This is the spacebar.</remarks>
   Space,

   /// <summary>The Print Screen key.</summary>
   /// <remarks>The key you press when you want to take a screenshot of your screen.</remarks>
   PrintScreen,

   /// <summary>The pause / break key.</summary>
   /// <remarks>I also haven't figured out what this key is actually used for, maybe it pauses or breaks the universe.</remarks>
   PauseBreak,

   /// <summary>The <i>insert</i> key.</summary>
   /// <remarks>Must. Resist. Joke.</remarks>
   Insert,

   /// <summary>The delete key.</summary>
   /// <remarks>Same as <see cref="Backspace"/>, but in the other direction.</remarks>
   Delete,

   /// <summary>The contextual key.</summary>
   /// <remarks>On keyboards with a windows key, this is usually the key right next to the windows key on the right, this is the key that also sometimes behaves like a right click.</remarks>
   Context,

   /// <summary>The power key.</summary>
   Power,

   /// <summary>The sleep key.</summary>
   Sleep,

   /// <summary>The wake key.</summary>
   Wake,

   /// <summary>The channel increment key.</summary>
   ChannelIncrement,

   /// <summary>The channel decrement key.</summary>
   ChannelDecrement,

   /// <summary>The execute key.</summary>
   /// <remarks>What does it execute...?</remarks>
   Execute,
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="VirtualKeyKind"/>.
/// </summary>
public static class VirtualKeyKindExtensions
{
   #region Methods
   /// <summary>Checks whether the given virtual key <paramref name="kind"/> represents a category.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given virtual key <paramref name="kind"/> represents a category, <see langword="false"/> otherwise.</returns>
   public static bool IsCategory(this VirtualKeyKind kind)
   {
      return kind switch
      {
         VirtualKeyKind.Other => true,
         VirtualKeyKind.Reserved => true,

         _ => false,
      };
   }

   /// <summary>Checks whether the given key <paramref name="kind"/> is either one of the left or right control keys.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="kind"/> is either one of the left or right control keys, <see langword="false"/> otherwise.</returns>
   public static bool IsControl(this VirtualKeyKind kind) => kind is VirtualKeyKind.LeftControl or VirtualKeyKind.RightControl;

   /// <summary>Checks whether the given key <paramref name="kind"/> is either one of the left or right shift keys.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="kind"/> is either one of the left or right shift keys, <see langword="false"/> otherwise.</returns>
   public static bool IsShift(this VirtualKeyKind kind) => kind is VirtualKeyKind.LeftShift or VirtualKeyKind.RightShift;

   /// <summary>Checks whether the given key <paramref name="kind"/> is either one of the left or right alt keys.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="kind"/> is either one of the left or right alt keys, <see langword="false"/> otherwise.</returns>
   public static bool IsAlt(this VirtualKeyKind kind) => kind is VirtualKeyKind.LeftAlt or VirtualKeyKind.RightAlt;

   /// <summary>Checks whether the given key <paramref name="kind"/> is either one of the left or right meta keys.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="kind"/> is either one of the left or right meta keys, <see langword="false"/> otherwise.</returns>
   public static bool IsMeta(this VirtualKeyKind kind) => kind is VirtualKeyKind.LeftMeta or VirtualKeyKind.RightMeta;

   /// <summary>Checks whether the given key <paramref name="kind"/> is represents the enter key.</summary>
   /// <param name="kind">The kind of the virtual key to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="kind"/> represents the enter key, <see langword="false"/> otherwise.</returns>
   public static bool IsEnter(this VirtualKeyKind kind)
   {
      // Note(Nightowl): Maybe Return2 as well, but I have no idea how that is actually used, can fix later;
      return kind is VirtualKeyKind.Return or VirtualKeyKind.KeypadEnter;
   }
   #endregion
}
