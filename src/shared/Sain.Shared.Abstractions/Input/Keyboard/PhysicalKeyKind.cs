namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the different known physical locations of keyboard buttons.
/// </summary>
public enum PhysicalKeyKind : ushort
{
   /// <summary>The pysical location of the key is not known.</summary>
   Unknown,

   /// <summary>The physical location of the key is known, but it has not been defined.</summary>
   Other,

   #region Latin alphabet keys
   /// <summary>The A key on the keyboard.</summary>
   A,

   /// <summary>The B key on the keyboard.</summary>
   B,

   /// <summary>The C key on the keyboard.</summary>
   C,

   /// <summary>The D key on the keyboard.</summary>
   D,

   /// <summary>The E key on the keyboard.</summary>
   E,

   /// <summary>The F key on the keyboard.</summary>
   F,

   /// <summary>The G key on the keyboard.</summary>
   G,

   /// <summary>The H key on the keyboard.</summary>
   H,

   /// <summary>The I key on the keyboard.</summary>
   I,

   /// <summary>The J key on the keyboard.</summary>
   J,

   /// <summary>The K key on the keyboard.</summary>
   K,

   /// <summary>The L key on the keyboard.</summary>
   L,

   /// <summary>The M key on the keyboard.</summary>
   M,

   /// <summary>The N key on the keyboard.</summary>
   N,

   /// <summary>The O key on the keyboard.</summary>
   O,

   /// <summary>The P key on the keyboard.</summary>
   P,

   /// <summary>The Q key on the keyboard.</summary>
   Q,

   /// <summary>The R key on the keyboard.</summary>
   R,

   /// <summary>The S key on the keyboard.</summary>
   S,

   /// <summary>The T key on the keyboard.</summary>
   T,

   /// <summary>The U key on the keyboard.</summary>
   U,

   /// <summary>The V key on the keyboard.</summary>
   V,

   /// <summary>The W key on the keyboard.</summary>
   W,

   /// <summary>The X key on the keyboard.</summary>
   X,

   /// <summary>The Y key on the keyboard.</summary>
   Y,

   /// <summary>The Z key on the keyboard.</summary>
   Z,
   #endregion

   #region Number keys
   /// <summary>The number 1 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N1,

   /// <summary>The number 2 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N2,

   /// <summary>The number 3 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N3,

   /// <summary>The number 4 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N4,

   /// <summary>The number 5 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N5,

   /// <summary>The number 6 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N6,

   /// <summary>The number 7 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N7,

   /// <summary>The number 8 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N8,

   /// <summary>The number 9 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N9,

   /// <summary>The number 0 key on the keyboard.</summary>
   /// <remarks>This is typically the number on the top row of the keyboard.</remarks>
   N0,
   #endregion

   /// <summary>The return key on the keyboard.</summary>
   /// <remarks>This is the enter key.</remarks>
   Return,

   /// <summary>The escape key on the keyboard.</summary>
   Escape,

   /// <summary>The backspace key on the keyboard.</summary>
   Backspace,

   /// <summary>The tab key on the keyboard.</summary>
   Tab,

   /// <summary>The space key on the keyboard.</summary>
   /// <remarks>This is the spacebar.</remarks>
   Space,

   #region Programmer keys
   /// <summary>The minus key on the keyboard.</summary>
   /// <remarks>This is typically next to the numbers on the top row of the keyboard.</remarks>
   Minus,

   /// <summary>The equals key on the keyboard.</summary>
   /// <remarks>This is typically next to the numbers on the top row of the keyboard.</remarks>
   Equals,

   /// <summary>The left bracket key on the keyboard.</summary>
   LeftBracket,

   /// <summary>The right bracket key on the keyboard.</summary>
   RightBracket,

   /// <summary>The backslash key on the keyboard.</summary>
   Backslash,

   /// <summary>The (non US?) version of the backslash key on the keyboard.</summary>
   /// <remarks>Yeah I don't understand how this is different to the <see cref="Backslash"/>.</remarks>
   NonUsBackslash,

   /// <summary>The hash key (#) on the keyboard.</summary>
   /// <remarks>This key is not used in the US for some reason.</remarks>
   NonUsHash,

   /// <summary>The semicolon key on the keyboard.</summary>
   Semicolon,

   /// <summary>The apostrophe key on the keyboard.</summary>
   Apostrophe,

   /// <summary>The grave key on the keyboard.</summary>
   Grave,

   /// <summary>The comma key on the keyboard.</summary>
   Comma,

   /// <summary>The period key on the keyboard.</summary>
   /// <remarks>Also known as a full stop.</remarks>
   Period,

   /// <summary>The slash key on the keyboard.</summary>
   /// <remarks>Also known as a forward slash.</remarks>
   Slash,
   #endregion

   /// <summary>The Caps Lock key on the keyboard.</summary>
   /// <remarks>
   ///   Aka the annoying button I never use seriously and just like to spam when I'm thinking.
   ///   Almost like clicking a physical pen...
   /// </remarks>
   CapsLock,

   #region Function keys
   /// <summary>The F1 key on the keyboard.</summary>
   /// <remarks>Also known as the function 1 key, not the Formula 1 key, and it will not turn your computer into an ultra fast race car.</remarks>
   F1,

   /// <summary>The F2 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F2,

   /// <summary>The F3 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F3,

   /// <summary>The F4 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F4,

   /// <summary>The F5 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F5,

   /// <summary>The F6 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F6,

   /// <summary>The F7 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F7,

   /// <summary>The F8 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F8,

   /// <summary>The F9 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F9,

   /// <summary>The F10 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F10,

   /// <summary>The F11 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F11,

   /// <summary>The F12 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F12,

   /// <summary>The F13 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F13,

   /// <summary>The F14 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F14,

   /// <summary>The F15 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F15,

   /// <summary>The F16 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F16,

   /// <summary>The F17 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F17,

   /// <summary>The F18 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F18,

   /// <summary>The F19 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F19,

   /// <summary>The F20 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F20,

   /// <summary>The F21 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F21,

   /// <summary>The F22 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F22,

   /// <summary>The F23 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F23,

   /// <summary>The F24 key on the keyboard.</summary>
   /// <remarks>Also known as a function key.</remarks>
   F24,
   #endregion

   /// <summary>The Print Screen key on the keyboard.</summary>
   /// <remarks>The key you press when you want to take a screenshot of your screen.</remarks>
   PrintScreen,

   /// <summary>The Scroll Lock key on the keyboard.</summary>
   /// <remarks>I still haven't figured out what this key is actually used for.</remarks>
   ScrollLock,

   /// <summary>The pause / break key on the keyboard.</summary>
   /// <remarks>I also haven't figured out what this key is actually used for, maybe it pauses or breaks the universe.</remarks>
   PauseBreak,

   /// <summary>The <i>insert</i> key on the keyboard.</summary>
   /// <remarks>Must. Resist. Joke.</remarks>
   Insert,

   /// <summary>The delete key on the keyboard.</summary>
   /// <remarks>Same as <see cref="Backspace"/>, but in the other direction.</remarks>
   Delete,

   #region Programmer movement keys
   /// <summary>The home key on the keyboard.</summary>
   /// <remarks>Take me homeeeeee, down country roaaaaaaadsssss.</remarks>
   Home,

   /// <summary>The nd key on the keyboard.</summary>
   /// <remarks>Does not end the universe, I hope.</remarks>
   End,

   /// <summary>The page up key on the keyboard.</summary>
   /// <remarks>Does anything actually use this to mean a <i>full</i> page up?</remarks>
   PageUp,

   /// <summary>The page down key on the keyboard.</summary>
   /// <remarks>Does anything actually use this to mean a <i>full</i> page down?</remarks>
   PageDown,
   #endregion

   #region Casual gamer movement keys
   /// <summary>The left arrow key on the keyboard.</summary>
   Left,

   /// <summary>The right arrow key on the keyboard.</summary>
   Right,

   /// <summary>The up arrow key on the keyboard.</summary>
   Up,

   /// <summary>The down arrow key on the keyboard.</summary>
   Down,
   #endregion

   #region Keypad keys
   /// <summary>The num lock (or clear) key on the keyboard.</summary>
   /// <remarks>Ewww why am I actually taking into account an apple product... Damn you SDL for making me aware of this difference.</remarks>
   NumLockOrClear,

   /// <summary>The divide key on the keypad.</summary>
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

   /// <summary>The left (opening?) parenthesis key on the keypad.</summary>
   KeypadLeftParenthesis,

   /// <summary>The right (closing?) parenthesis key on the keypad.</summary>
   KeypadRightParenthesis,

   /// <summary>The left (opening?) brace key on the keypad.</summary>
   KeypadLeftBrace,

   /// <summary>The right (closing?) brace key on the keypad.</summary>
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

   /// <summary>The contextual key on the keyboard.</summary>
   /// <remarks>On keyboards with a windows key, this is usually the key right next to the windows key on the right, this is the key that also sometimes behaves like a right click.</remarks>
   Context,

   /// <summary>The power key on the keyboard.</summary>
   Power,

   /// <summary>The execute key on the keyboard.</summary>
   /// <remarks>What does it execute...?</remarks>
   Execute,

   #region Office keys
   /// <summary>The (office) help key on the keyboard.</summary>
   /// <remarks>Damn, didn't know there was a button dedicated to it.</remarks>
   OfficeHelp,

   /// <summary>The (office) menu key on the keyboard.</summary>
   /// <remarks>This sounds like <see cref="Context"/>,
   /// but apparently it's different?</remarks>
   OfficeMenu,

   /// <summary>The (office) select key on the keyboard.</summary>
   OfficeSelect,

   /// <summary>The (office) stop key on the keyboard.</summary>
   OfficeStop,

   /// <summary>The (office) again key on the keyboard.</summary>
   OfficeAgain,

   /// <summary>The (office) undo key on the keyboard.</summary>
   OfficeUndo,

   /// <summary>The (office) cut key on the keyboard.</summary>
   OfficeCut,

   /// <summary>The (office) copy key on the keyboard.</summary>
   OfficeCopy,

   /// <summary>The (office) paste key on the keyboard.</summary>
   OfficePaste,

   /// <summary>The (office) find key on the keyboard.</summary>
   OfficeFind,

   /// <summary>The (office) new key on the keyboard.</summary>
   OfficeNew,

   /// <summary>The (office) open key on the keyboard.</summary>
   OfficeOpen,

   /// <summary>The (office) close key on the keyboard.</summary>
   OfficeClose,

   /// <summary>The (office) exit key on the keyboard.</summary>
   OfficeExit,

   /// <summary>The (office) save key on the keyboard.</summary>
   OfficeSave,

   /// <summary>The (office) print key on the keyboard.</summary>
   OfficePrint,

   /// <summary>The (office) properties key on the keyboard.</summary>
   OfficeProperties,

   /// <summary>The (office) search key on the keyboard.</summary>
   OfficeSearch,

   /// <summary>The (office) home key on the keyboard.</summary>
   OfficeHome,

   /// <summary>The (office) back key on the keyboard.</summary>
   OfficeBack,

   /// <summary>The (office) forward key on the keyboard.</summary>
   OfficeForward,

   /// <summary>The (office) stop key on the keyboard.</summary>
   /// <remarks>Why are there 2?</remarks>
   OfficeStop2,

   /// <summary>The (office) refresh key on the keyboard.</summary>
   OfficeRefresh,

   /// <summary>The (office) bookmarks key on the keyboard.</summary>
   OfficeBookmarks,
   #endregion

   #region Media keys
   /// <summary>The mute key on the keyboard.</summary>
   /// <remarks>If this sets the volume to 0 instead of actually muting and then unmuting, then I feel sorry for you.</remarks>
   MediaMute,

   /// <summary>The volume up key on the keyboard.</summary>
   /// <remarks>Makes sounds louder.</remarks>
   MediaVolumeUp,

   /// <summary>The volume down key on the keyboard.</summary>
   /// <remarks>Makes sounds quieter.</remarks>
   MediaVolumeDown,

   /// <summary>The play media key on the keyboard.</summary>
   /// <remarks>The key that is notorious for not being implemented properly in applications.</remarks>
   MediaPlay,

   /// <summary>The pause media key on the keyboard.</summary>
   MediaPause,

   /// <summary>The record media key on the keyboard.</summary>
   MediaRecord,

   /// <summary>The fast forward media key on the keyboard.</summary>
   MediaFastForward,

   /// <summary>The rewind media key on the keyboard.</summary>
   MediaRewind,

   /// <summary>The next track media key on the keyboard.</summary>
   MediaNextTrack,

   /// <summary>The previous track media key on the keyboard.</summary>
   MediaPreviousTrack,

   /// <summary>The stop media key on the keyboard.</summary>
   MediaStop,

   /// <summary>The eject media key on the keyboard.</summary>
   MediaEject,

   /// <summary>The play/pause media toggle key on the keyboard.</summary>
   MediaPlayPause,

   /// <summary>The select media key on the keyboard.</summary>
   MediaSelect,
   #endregion

   #region International keys
   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International1,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International2,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International3,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International4,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International5,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International6,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International7,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International8,

   /// <summary>Some international key on the keyboard.</summary>
   /// <remarks>I apologise for not knowing/understanding more.</remarks>
   International9,
   #endregion

   #region Language keys
   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>This typically toggles between Hangul/English.</remarks>
   Language1,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>This is typically used for Hanja conversion.</remarks>
   Language2,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>This is typically used for Katakana.</remarks>
   Language3,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>This is typically used for Hiragana.</remarks>
   Language4,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>This typically toggles between Zenkaku/Hankaku.</remarks>
   Language5,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>Reserved for future use.</remarks>
   Language6,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>Reserved for future use.</remarks>
   Language7,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>Reserved for future use.</remarks>
   Language8,

   /// <summary>The language toggle key on the keyboard.</summary>
   /// <remarks>Reserved for future use.</remarks>
   Language9,
   #endregion

   #region I don't understand keys (seriously, what keyboards have these).
   /// <summary>The alterase key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Alterase,

   /// <summary>The sysreq key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   SysReq,

   /// <summary>The cancel key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Cancel,

   /// <summary>The clear key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Clear,

   /// <summary>The prior key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Prior,

   /// <summary>The return2 key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Return2,

   /// <summary>The separator key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Separator,

   /// <summary>The oper (operation?) key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Oper,

   /// <summary>The alterase key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   ClearAgain,

   /// <summary>The clear again key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Crsel,

   /// <summary>The exsel key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   Exsel,

   /// <summary>The thousand separator key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   ThousandSeparator,

   /// <summary>The decimal key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   DecimalSeparator,

   /// <summary>The currency unit key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   CurrencyUnit,

   /// <summary>The currency sub-unit key on the keyboard.</summary>
   /// <remarks>I no longer understand what I am writing.</remarks>
   CurrencySubUnit,
   #endregion

   #region Modifier keys
   /// <summary>The left control (CTRL) key on the keyboard.</summary>
   LeftControl,

   /// <summary>The right control (CTRL) key on the keyboard.</summary>
   RightControl,

   /// <summary>The left shift key on the keyboard.</summary>
   LeftShift,

   /// <summary>The right shift key on the keyboard.</summary>
   RightShift,

   /// <summary>The left alt key on the keyboard.</summary>
   LeftAlt,

   /// <summary>The right alt key on the keyboard.</summary>
   RightAlt,

   /// <summary>The left META key on the keyboard.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   LeftMeta,

   /// <summary>The right META key on the keyboard.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   RightMeta,
   #endregion

   /// <summary>I genuinely do not have a clue.</summary>
   Mode,

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
}
