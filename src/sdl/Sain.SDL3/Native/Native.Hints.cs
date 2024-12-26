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
   #endregion
}

internal static class SDL3_Hints
{
   #region Constants
   /// <summary>
   ///   <list type="bullet">
   ///      <item><c>0</c> - disable screensaver (default).</item>
   ///      <item><c>1</c> - enabled screensaver.</item>
   ///   </list>
   /// </summary>
   public const string SDL_HINT_VIDEO_ALLOW_SCREENSAVER = "SDL_VIDEO_ALLOW_SCREENSAVER";
   #endregion
}
