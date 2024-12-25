namespace Sain.SDL3;

internal enum SDL3_EventType : uint
{
   Unknown = 0,

   #region Application events
   Quit = 256,
   Terminating,
   LowMemory,
   WillEnterBackground,
   DidEnterBackground,
   WillEnterForeground,
   DidEnterForeground,
   LocaleChanged,
   SystemThemeChanged,
   #endregion

   #region Display events
   DisplayOrientationChanged = 337,
   DisplayAdded,
   DisplayRemoved,
   DisplayMoved,
   DisplayDesktopModeChanged,
   DisplayCurrentModeChanged,
   DisplayContentScaleChanged,
   #endregion

   #region Window events
   WindowShown = 514,
   WindowHidden,
   WindowExposed,
   WindowMoved,
   WindowResized,
   WindowPixelSizeChanged,
   WindowMetalViewResized,
   WindowMinimized,
   WindowMaximized,
   WindowRestored,
   WindowMouseEnter,
   WindowMouseLeave,
   WindowFocusGained,
   WindowFocusLost,
   WindowCloseRequested,
   WindowHitTest,
   WindowIccProfileChanged,
   WindowDisplayChanged,
   WindowDisplayScaleChanged,
   WindowSafeAreaChanged,
   WindowOccluded,
   WindowEnterFullscreen,
   WindowLeaveFullscreen,
   WindowDestroyed,
   WindowHdrStateChanged,
   #endregion

   #region Keyboard events
   KeyDown = 768,
   KeyUp,
   TextEditing,
   TextInput,
   KeymapChanged,
   KeyboardAdded,
   KeyboardRemoved,
   TextEditingCandidates,
   #endregion

   #region Mouse events
   MouseMotion = 1024,
   MouseButtonDown,
   MouseButtonUp,
   MouseWheel,
   MouseAdded,
   MouseRemoved,
   #endregion

   #region Joystick events
   JostickAxisMotion = 1536,
   JoystickBallMotion,
   JoystickHatMotion,
   JoystickButtonDown,
   JoystickButtonUp,
   JoystickAdded,
   JoystickRemoved,
   JoystickBatteryUpdated,
   JoystickUpdateComplete,
   #endregion

   #region Gamepad events
   GamepadAxisMotion = 1616,
   GamepadButtonDown,
   GamepadButtonUp,
   GamepadAdded,
   GamepadRemoved,
   GamepadRemapped,
   GamepadTouchpadDown,
   GamepadTouchpadUp,
   GamepadSensorUpdate,
   GamepadUpdateComplete,
   GamepadSteamHandleUpdated,
   #endregion

   #region Touch events
   FingerDown = 1792,
   FingerUp,
   FingerMotion,
   #endregion

   #region Clipboard events
   ClipboardUpdate = 2304,
   #endregion

   #region Drag & Drop events
   DropFile = 4096,
   DropText,
   DropBegin,
   DropComplete,
   DropPosition,
   #endregion

   #region Audio events
   AudioDeviceAdded = 4352,
   AudioDeviceRemoved,
   AudioDeviceFormatChanged,
   #endregion

   #region Sensor events
   SensorUpdate = 4608,
   #endregion

   #region Pen events
   PenProximityIn = 4864,
   PenProximityOut,
   PenDown,
   PenUp,
   PenButtonDown,
   PenButtonUp,
   PenMotion,
   PenAxis,
   #endregion

   #region Camera events
   CameraDeviceAdded = 5120,
   CameraDeviceRemoved,
   CameraDeviceApproved,
   CameraDeviceDenied,
   #endregion

   #region Render events
   RenderTargetsReset = 8192,
   RenderDeviceReset,
   RenderDevicelost,
   #endregion

   #region Reserved for private platform events
   Private0 = 16_384,
   Private1,
   Private2,
   Private3,
   #endregion

   #region Internal events
   PollSentinel = 32_512,
   EventUser = 32_768,
   #endregion
}

internal readonly struct SDL3_CommonEvent
{
   #region Fields
   public readonly SDL3_EventType Type;
   private readonly uint _reserved;
   private readonly ulong _timestamp;
   #endregion

   #region Properties
   public readonly TimeSpan Timestamp => TimeSpan.FromMicroseconds((double)_timestamp / 1000);
   #endregion
}

[StructLayout(LayoutKind.Explicit, Size = 128)]
internal readonly struct SDL3_Event
{
   #region Fields
   [FieldOffset(0)] public readonly SDL3_EventType Type;
   [FieldOffset(0)] private readonly SDL3_CommonEvent _common;
   [FieldOffset(0)] private readonly SDL3_WindowEvent _window;
   #endregion

   #region Window event methods
   public readonly bool IsWindowEvent(out SDL3_WindowEvent window)
   {
      switch (Type)
      {
         case SDL3_EventType.WindowShown:
         case SDL3_EventType.WindowHidden:
         case SDL3_EventType.WindowExposed:
         case SDL3_EventType.WindowMoved:
         case SDL3_EventType.WindowResized:
         case SDL3_EventType.WindowPixelSizeChanged:
         case SDL3_EventType.WindowMetalViewResized:
         case SDL3_EventType.WindowMinimized:
         case SDL3_EventType.WindowMaximized:
         case SDL3_EventType.WindowRestored:
         case SDL3_EventType.WindowMouseEnter:
         case SDL3_EventType.WindowMouseLeave:
         case SDL3_EventType.WindowFocusGained:
         case SDL3_EventType.WindowFocusLost:
         case SDL3_EventType.WindowCloseRequested:
         case SDL3_EventType.WindowHitTest:
         case SDL3_EventType.WindowIccProfileChanged:
         case SDL3_EventType.WindowDisplayChanged:
         case SDL3_EventType.WindowDisplayScaleChanged:
         case SDL3_EventType.WindowSafeAreaChanged:
         case SDL3_EventType.WindowOccluded:
         case SDL3_EventType.WindowEnterFullscreen:
         case SDL3_EventType.WindowLeaveFullscreen:
         case SDL3_EventType.WindowDestroyed:
         case SDL3_EventType.WindowHdrStateChanged:
            window = _window;
            return true;
      }

      window = default;
      return false;
   }
   #endregion
}

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_PollEvent")]
   [return: MarshalAs(Bool)]
   public static partial bool PollEvent(out SDL3_Event ev);

   [LibraryImport(LibName, EntryPoint = "SDL_WaitEventTimeout")]
   [return: MarshalAs(Bool)]
   public static partial bool WaitEvent(out SDL3_Event ev, int timeoutMs);

   [LibraryImport(LibName, EntryPoint = "SDL_AddEventWatch")]
   [return: MarshalAs(Bool)]
   public static partial bool AddEventWatch(delegate*<void*, SDL3_Event*, byte> filter, void* userData);

   [LibraryImport(LibName, EntryPoint = "SDL_RemoveEventWatch")]
   [return: MarshalAs(Bool)]
   public static partial bool RemoveEventWatch(delegate*<void*, SDL3_Event*, byte> filter, void* userData);
   #endregion
}
