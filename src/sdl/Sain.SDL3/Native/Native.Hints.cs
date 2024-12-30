namespace Sain.SDL3;

internal enum SDL3_HintPriority : byte
{
   Default,
   Normal,
   Override,
}

static partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_SetHintWithPriority", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool SetHint(string name, string? value, SDL3_HintPriority priority);

   [LibraryImport(LibName, EntryPoint = "SDL_SetHint", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool SetHint(string name, string? value);

   [LibraryImport(LibName, EntryPoint = "SDL_ResetHint", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool ResetHint(string name);

   public static bool EnableHint(string name) => SetHint(name, "1");
   public static bool DisableHint(string name) => SetHint(name, "0");
   #endregion
}

internal static class SDL3_Hints
{
   #region Constants
   /// <summary>
   ///   <list type="bullet">
   ///      <item><c>"0"</c> - disable screensaver (default).</item>
   ///      <item><c>"1"</c> - enabled screensaver.</item>
   ///   </list>
   /// </summary>
   public const string VIDEO_ALLOW_SCREENSAVER = "SDL_VIDEO_ALLOW_SCREENSAVER";

   /// <summary>
   ///   <list type="bullet">
   ///      <item><c>"0"</c> - SDL will not send the <see cref="SDL3_EventType.Quit"/> event when the last window is closed.</item>
   ///      <item><c>"1"</c> - SDL will send the <see cref="SDL3_EventType.Quit"/> event when the last window is closed (default).</item>
   ///   </list>
   /// </summary>
   public const string QUIT_ON_LAST_WINDOW_CLOSE = "SDL_QUIT_ON_LAST_WINDOW_CLOSE";

   /// <summary>
   ///   <list type="bullet">
   ///      <item><c>"0"</c> - Disable VSync (default).</item>
   ///      <item><c>"1"</c> - Enable VSync.</item>
   ///   </list>
   /// </summary>
   public const string RENDER_VSYNC = "SDL_RENDER_VSYNC";

   /// <summary>
   ///   Whether mouse events should generate synthetic touch events.
   ///   <list type="bullet">
   ///      <item><c>"0"</c> - Mouse events will not generate touch events (default for desktop).</item>
   ///      <item><c>"1"</c> - Mouse events will generate touch events (default for mobile).</item>
   ///   </list>
   /// </summary>
   public const string MOUSE_TOUCH_EVENTS = "SDL_MOUSE_TOUCH_EVENTS";

   /// <summary>
   ///   Whether touch events should generate synthetic mouse events.
   ///   <list type="bullet">
   ///      <item><c>"0"</c> - Touch events will not generate mouse events.</item>
   ///      <item><c>"1"</c> - Touch events will generate mouse events (default).</item>
   ///   </list>
   /// </summary>
   public const string TOUCH_MOUSE_EVENTS = "SDL_TOUCH_MOUSE_EVENTS";
   #endregion
}
