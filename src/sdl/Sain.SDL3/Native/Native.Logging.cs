namespace Sain.SDL3;

internal enum SDL3_LogCategory : int
{
   Application,
   Error,
   Assert,
   System,
   Video,
   Render,
   Input,
   Test,
   GPU,

   Reserved2,
   Reserved3,
   Reserved4,
   Reserved5,
   Reserved6,
   Reserved7,
   Reserved8,
   Reserved9,
   Reserved10,

   Custom,
}

internal enum SDL3_LogPriority : byte
{
   Invalid,
   Trace,
   Verbose,
   Debug,
   Info,
   Warning,
   Error,
   Critical
}

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_SetLogOutputFunction")]
   public static partial void SetLogOutputFunction(delegate*<void*, SDL3_LogCategory, SDL3_LogPriority, byte*, void> callback, void* userData);

   [LibraryImport(LibName, EntryPoint = "SDL_SetLogPriority")]
   public static partial void SetLogPriority(SDL3_LogCategory category, SDL3_LogPriority priority);

   [LibraryImport(LibName, EntryPoint = "SDL_SetLogPriorities")]
   public static partial void SetLogPriorities(SDL3_LogPriority priority);
   #endregion
}
