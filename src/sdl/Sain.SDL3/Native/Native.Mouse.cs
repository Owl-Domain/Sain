using System.Security.Cryptography.X509Certificates;

namespace Sain.SDL3;

internal enum SDL3_SystemCursor : byte
{
   Default,
   Text,
   Wait,
   Crosshair,
   Progress,
   NWSE_Resize,
   NESW_Resize,
   EW_Resize,
   NS_Resize,
   Move,
   NotAllowed,
   Pointer,
   NW_Resize,
   N_Resize,
   NE_Resize,
   E_Resize,
   SE_Resize,
   S_Resize,
   SW_Resize,
   W_Resize,
}

internal enum SDL3_MouseWheelDirection : byte
{
   Normal,
   Flipped
}

internal enum SDL3_MouseButton : byte
{
   Left = 1,
   Middle = 2,
   Right = 3,
   X1 = 4,
   X2 = 5,
}

[Flags]
internal enum SDL3_MouseButtonFlags : uint
{
   Left = 1u << 0,
   Middle = 1u << 1,
   Right = 1u << 2,
   X1 = 1u << 3,
   X2 = 1u << 4
}

internal readonly struct SDL3_MouseId(uint id)
{
   #region Fields
   public readonly uint Id = id;
   #endregion

   #region Methods
   public override string ToString() => Id.ToString();
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_MouseDeviceEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_MouseId MouseId;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_MouseMotionEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_WindowId WindowId;
   public readonly SDL3_MouseId MouseId;
   public readonly SDL3_MouseButtonFlags Buttons;
   public readonly float X;
   public readonly float Y;
   public readonly float RelativeX;
   public readonly float RelativeY;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_MouseButtonEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_WindowId WindowId;
   public readonly SDL3_MouseId MouseId;
   public readonly SDL3_MouseButton Button;
   private readonly byte _isDown;
   public readonly byte Clicks;
   private readonly byte _padding;
   public readonly float X;
   public readonly float Y;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   public readonly bool IsDown => _isDown is not 0;
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_MouseWheelEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_WindowId WindowId;
   public readonly SDL3_MouseId MouseId;
   public readonly float ScrollX;
   public readonly float ScrollY;
   public readonly SDL3_MouseWheelDirection Direction;
   public readonly float MouseX;
   public readonly float MouseY;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   #endregion
}

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_GetMice")]
   public static partial SDL3_MouseId* GetMice(out int count);

   [LibraryImport(LibName, EntryPoint = "SDL_GetMouseNameForID")]
   private static partial byte* _GetMouseNameForId(SDL3_MouseId mouseId);
   public static string? GetMouseNameForId(SDL3_MouseId mouseId)
   {
      byte* native = _GetMouseNameForId(mouseId);
      return Utf8StringMarshaller.ConvertToManaged(native);
   }
   [LibraryImport(LibName, EntryPoint = "SDL_HideCursor")]
   [return: MarshalAs(Bool)]
   public static partial bool HideCursor();

   [LibraryImport(LibName, EntryPoint = "SDL_ShowCursor")]
   [return: MarshalAs(Bool)]
   public static partial bool ShowCursor();

   [LibraryImport(LibName, EntryPoint = "SDL_CursorVisible")]
   [return: MarshalAs(Bool)]
   public static partial bool IsCursorVisible();

   [LibraryImport(LibName, EntryPoint = "SDL_GetMouseState")]
   public static partial SDL3_MouseButtonFlags GetMouseState(out float x, out float y);

   [LibraryImport(LibName, EntryPoint = "SDL_GetGlobalMouseState")]
   public static partial SDL3_MouseButtonFlags GetGlobalMouseState(out float x, out float y);

   [LibraryImport(LibName, EntryPoint = "SDL_WarpMouseGlobal")]
   [return: MarshalAs(Bool)]
   public static partial bool WarpMouseGlobal(float x, float y);

   [LibraryImport(LibName, EntryPoint = "SDL_WarpMouseInWindow")]
   [return: MarshalAs(Bool)]
   public static partial bool WarpMouseInWindow(in SDL3_WindowId window, float x, float y);

   [LibraryImport(LibName, EntryPoint = "SDL_CaptureMouse")]
   [return: MarshalAs(Bool)]
   public static partial bool CaptureMouse([MarshalAs(Bool)] bool enabled);
   #endregion
}
