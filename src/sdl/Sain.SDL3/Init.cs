namespace Sain.SDL3;

[Flags]
internal enum SDL3_InitFlags : uint
{
   None = 0,
   Audio = 0x10u,
   Video = 0x20u,
   Joystick = 0x200u,
   Haptic = 0x1000u,
   Gamepad = 0x2000u,
   Events = 0x4000u,
   Sensor = 0x8000u,
   Camera = 0x10000u
}

static partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_InitSubSystem")]
   [return: MarshalAs(Bool)]
   public static partial bool InitSubSystem(SDL3_InitFlags flags);

   [LibraryImport(LibName, EntryPoint = "SDL_QuitSubSystem")]
   public static partial void QuitSubSystem(SDL3_InitFlags flags);

   [LibraryImport(LibName, EntryPoint = "SDL_Quit")]
   public static partial void Quit();
   #endregion
}
