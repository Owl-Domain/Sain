namespace Sain.SDL3;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_Window;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_WindowId(uint id)
{
   #region Fields
   public readonly uint Id = id;
   #endregion

   #region Methods
   public override string ToString() => Id.ToString();
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_WindowEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_WindowId WindowId;
   public readonly int Data1;
   public readonly int Data2;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   #endregion
}

[Flags]
internal enum SDL3_WindowFlags : ulong
{
   None = 0,
   Fullscreen = 0x1,
   OpenGL = 0x2,
   Occluded = 0x4,
   Hidden = 0x8,
   Borderless = 0x10,
   Resizable = 0x20,
   Minimized = 0x40,
   Maximized = 0x80,
   MouseGrabbed = 0x100,
   InputFocus = 0x200,
   MouseFocus = 0x400,
   External = 0x800,
   Modal = 0x1000,
   HighPixelDensity = 0x2000,
   MouseRelativeMode = 0x8000,
   AlwaysOnTop = 0x10000,
   Utility = 0x20000,
   Tooltip = 0x40000,
   PopupMenu = 0x80000,
   KeyboardGrabbed = 0x100000,
   Vulkan = 0x10000000,
   Metal = 0x20000000,
   Transparent = 0x40000000,
   NotFocusable = 0x80000000,
}

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_CreateWindow", StringMarshalling = String)]
   public static partial SDL3_Window* CreateWindow(string title, int width, int height, SDL3_WindowFlags flags);

   [LibraryImport(LibName, EntryPoint = "SDL_DestroyWindow")]
   public static partial void DestroyWindow(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowParent")]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowParent(SDL3_Window* window, SDL3_Window* parent);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowPosition")]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowPosition(SDL3_Window* window, int x, int y);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowSize")]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowSize(SDL3_Window* window, int width, int height);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowTitle", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowTitle(SDL3_Window* window, string title);

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowTitle")]
   private static partial byte* _GetWindowTitle(SDL3_Window* window);
   public static string? GetWindowTitle(SDL3_Window* window)
   {
      byte* native = _GetWindowTitle(window);
      return Utf8StringMarshaller.ConvertToManaged(native);
   }

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowID")]
   public static partial SDL3_WindowId GetWindowId(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowPosition")]
   [return: MarshalAs(Bool)]
   public static partial bool GetWindowPosition(SDL3_Window* window, out int x, out int y);

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowSize")]
   [return: MarshalAs(Bool)]
   public static partial bool GetWindowSize(SDL3_Window* window, out int width, out int height);

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowFlags")]
   public static partial SDL3_WindowFlags GetWindowFlags(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_ShowWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool ShowWindow(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_HideWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool HideWindow(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_MinimizeWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool MinimizeWindow(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_MaximizeWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool MaximizeWindow(SDL3_Window* window);

   [LibraryImport(LibName, EntryPoint = "SDL_RestoreWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool RestoreWindow(SDL3_Window* window);
   #endregion

   #region Screen saver functions
   [LibraryImport(LibName, EntryPoint = "SDL_EnableScreenSaver")]
   [return: MarshalAs(Bool)]
   public static partial bool EnableScreenSaver();

   [LibraryImport(LibName, EntryPoint = "SDL_DisableScreenSaver")]
   [return: MarshalAs(Bool)]
   public static partial bool DisableScreenSaver();

   [LibraryImport(LibName, EntryPoint = "SDL_ScreenSaverEnabled")]
   [return: MarshalAs(Bool)]
   public static partial bool IsScreenSavedEnabled();
   #endregion
}
