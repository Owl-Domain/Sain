namespace Sain.Shared.Storage;

/// <summary>
///   Represents the application's context for temporary storage.
/// </summary>
public interface ITemporaryStorageContext : IContext
{
   #region Methods
   /// <summary>Creates a new temporary file, with a random filename and the given <paramref name="extension"/>.</summary>
   /// <param name="extension">
   ///   The extension to give to the random file, with or without the leading
   ///   period (<c>.</c>). If empty then no extension will be given to the file.
   /// </param>
   /// <returns>The newly created temporary file.</returns>
   FileStream CreateRandomFile(ReadOnlySpan<char> extension = default);
   #endregion
}
