namespace Sain.SDL3;

internal enum SDL3_DisplayOrientation : byte
{
   Unknown,
   Landscape,
   LandscapeFlipped,
   Portrait,
   PortraitFlipped
}

internal readonly struct SDL3_DisplayId(uint id)
{
   #region Fields
   public readonly uint Id = id;
   #endregion

   #region Methods
   public override string ToString() => Id.ToString();
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_DisplayModeData;

[StructLayout(LayoutKind.Sequential)]
internal unsafe readonly struct SDL3_DisplayMode
{
   #region Fields
   public readonly SDL3_DisplayId DisplayId;
   public readonly SDL3_PixelFormat PixelFormat;
   public readonly int Width;
   public readonly int Height;
   public readonly float PixelDensity;
   public readonly float RefreshRate;
   public readonly int RefreshRateNumerator;
   public readonly int RefreshRateDenominator;
   private readonly SDL3_DisplayModeData* _internal;
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_DisplayEvent
{
   #region Fields
   public readonly SDL3_CommonEvent Common;
   public readonly SDL3_DisplayId DisplayId;
   public readonly int Data1;
   public readonly int Data2;
   #endregion

   #region Properties
   public readonly SDL3_EventType Type => Common.Type;
   #endregion
}

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_GetDisplays")]
   public static partial SDL3_DisplayId* GetDisplays(out int count);

   [LibraryImport(LibName, EntryPoint = "SDL_GetDisplayName")]
   private static partial byte* _GetDisplayName(SDL3_DisplayId display);

   public static string? GetDisplayName(SDL3_DisplayId display)
   {
      byte* native = _GetDisplayName(display);
      return Utf8StringMarshaller.ConvertToManaged(native);
   }

   [LibraryImport(LibName, EntryPoint = "SDL_GetCurrentDisplayMode")]
   public static partial SDL3_DisplayMode* GetCurrentDisplayMode(SDL3_DisplayId display);

   [LibraryImport(LibName, EntryPoint = "SDL_GetDesktopDisplayMode")]
   public static partial SDL3_DisplayMode* GetDesktopDisplayMode(SDL3_DisplayId display);

   public static bool GetDesktopDisplayMode(SDL3_DisplayId display, out SDL3_DisplayMode mode)
   {
      SDL3_DisplayMode* native = GetDesktopDisplayMode(display);
      if (native is null)
      {
         mode = default;
         return false;
      }

      mode = *native;
      return true;
   }

   [LibraryImport(LibName, EntryPoint = "SDL_GetCurrentDisplayOrientation")]
   public static partial SDL3_DisplayOrientation GetCurrentDisplayOrientation(SDL3_DisplayId display);

   [LibraryImport(LibName, EntryPoint = "SDL_GetDisplayContentScale")]
   public static partial float GetDisplayContentScale(SDL3_DisplayId display);

   [LibraryImport(LibName, EntryPoint = "SDL_GetPrimaryDisplay")]
   public static partial SDL3_DisplayId GetPrimaryDisplay();

   [LibraryImport(LibName, EntryPoint = "SDL_GetDisplayBounds")]
   [return: MarshalAs(Bool)]
   public static partial bool GetDisplayBounds(SDL3_DisplayId display, out SDL3_Rect rect);

   [LibraryImport(LibName, EntryPoint = "SDL_GetDisplayUsableBounds")]
   [return: MarshalAs(Bool)]
   public static partial bool GetDisplayUsableBounds(SDL3_DisplayId display, out SDL3_Rect rect);
   #endregion
}
