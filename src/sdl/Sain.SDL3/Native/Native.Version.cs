namespace Sain.SDL3;

static unsafe partial class Native
{
   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_GetVersion")]
   public static partial int GetVersion();

   [LibraryImport(LibName, EntryPoint = "SDL_GetRevision")]
   private static partial byte* _GetRevision();

   public static string GetRevision()
   {
      byte* native = _GetRevision();
      return Utf8StringMarshaller.ConvertToManaged(native) ?? string.Empty;
   }
   #endregion
}
