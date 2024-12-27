namespace Sain.SDL3;

internal readonly struct SDL3_WindowId(uint id)
{
   #region Fields
   public readonly uint Id = id;
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
   public static partial SDL3_WindowId* CreateWindow(string title, int width, int height, SDL3_WindowFlags flags);

   [LibraryImport(LibName, EntryPoint = "SDL_DestroyWindow")]
   public static partial void DestroyWindow(SDL3_WindowId* window);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowParent")]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowParent(SDL3_WindowId* window, SDL3_WindowId* parent);

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowPosition")]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowPosition(SDL3_WindowId* window, int x, int y);

   [LibraryImport(LibName, EntryPoint = "SDL_GetWindowTitle")]
   private static partial byte* _GetWindowTitle(SDL3_WindowId* window);
   public static string? GetWindowTitle(SDL3_WindowId* window)
   {
      byte* native = _GetWindowTitle(window);
      return Utf8StringMarshaller.ConvertToManaged(native);
   }

   [LibraryImport(LibName, EntryPoint = "SDL_SetWindowTitle", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool SetWindowTitle(SDL3_WindowId* window, string title);
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
