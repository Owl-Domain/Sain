namespace Sain.SDL3;

static partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_SetAppMetadata", StringMarshalling = String)]
   [return: MarshalAs(Bool)]
   public static partial bool SetAppMetadata(string? appName, string? appVersion, string? appIdentifier);
   #endregion
}
