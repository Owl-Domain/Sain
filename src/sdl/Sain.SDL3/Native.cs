namespace Sain.SDL3;

internal static unsafe partial class Native
{
   #region Constants
   private const string LibName = "SDL3";
   private const UnmanagedType Bool = UnmanagedType.U1;
   private const StringMarshalling String = StringMarshalling.Utf8;
   #endregion

   #region Functions
   [LibraryImport(LibName, EntryPoint = "SDL_Delay")]
   public static partial void Delay(uint ms);

   [LibraryImport(LibName, EntryPoint = "SDL_free")]
   public static partial void Free(void* pointer);
   #endregion
}

internal readonly unsafe struct Pointer<T>(T* ptr) where T : unmanaged
{
   #region Fields
   public readonly T* Ptr = ptr;
   #endregion
}
