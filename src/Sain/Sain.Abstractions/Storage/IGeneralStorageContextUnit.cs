namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the application's context unit for general storage operations.
/// </summary>
public interface IGeneralStorageContextUnit : IContextUnit
{
   #region Methods
   /// <summary>Derives a new file name in case the requested file is a duplicate.</summary>
   /// <param name="fileName">The preferred file name to derive the duplicate name from (without the extension).</param>
   /// <param name="extension">The extension that should be given to the duplicate file name, with or without the leading <c>.</c> full stop.</param>
   /// <param name="attempt">The number of attempts that the file has been attempted.</param>
   /// <returns>The derived duplicate name.</returns>
   string GetDuplicateFileName(ReadOnlySpan<char> fileName, ReadOnlySpan<char> extension, int attempt);

   /// <summary>Gets the extension from the given <paramref name="fileName"/>.</summary>
   /// <param name="fileName">The file name to get the extension from.</param>
   /// <param name="multipartExtension">Whether the method should act as if a multi-part extension might be present in the given <paramref name="fileName"/>.</param>
   /// <returns>The extension from the obtained file, including the leading <c>.</c> full stop.</returns>
   /// <remarks>This method will return <see cref="string.Empty"/> if the file didn't have an extension.</remarks>
   ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> fileName, bool multipartExtension = true);

   /// <summary>Gets the name of the file (including the extension) that the given <paramref name="path"/> points to.</summary>
   /// <param name="path">The path to get the file name from.</param>
   /// <returns>The name of the file that the given <paramref name="path"/> points to.</returns>
   ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path);

   /// <summary>Gets the name of the file (excluding the extension) that the given <paramref name="path"/> points to.</summary>
   /// <param name="path">The path to get the file name from.</param>
   /// <param name="multipartExtension">Whether the method should act as if a multi-part extension might be present in the given <paramref name="path"/>.</param>
   /// <returns>The name of the file that the given <paramref name="path"/> points to.</returns>
   ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path, bool multipartExtension = true);
   #endregion
}
