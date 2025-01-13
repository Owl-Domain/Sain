namespace Sain.SDL3;

static unsafe partial class Native
{
   #region Properties
   public static string? LastError
   {
      get
      {
         if (TryGetError(out string? error))
            return error;

         return null;
      }
   }
   #endregion

   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_GetError")]
   private static partial byte* _GetError();

   public static string GetError()
   {
      byte* native = _GetError();

      return Utf8StringMarshaller.ConvertToManaged(native) ?? string.Empty;
   }

   [LibraryImport(LibName, EntryPoint = "SDL_ClearError")]
   [return: MarshalAs(Bool)]
   public static partial bool ClearError();

   public static bool TryGetError([NotNullWhen(true)] out string? error)
   {
      error = GetError();
      if (string.IsNullOrEmpty(error) is false)
      {
         ClearError();
         return true;
      }

      error = default;
      return false;
   }
   #endregion
}
