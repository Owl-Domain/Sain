namespace Sain.Shared.Storage;

/// <summary>
///   Represents the application's context for storage permissions.
/// </summary>
public interface IStoragePermissionsContext : IContext
{
   #region File methods
   /// <summary>Checks whether the current application has the permissions to create a file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to create a
   ///   file at the given <paramref name="filePath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if a file at the given <paramref name="filePath"/> already exists.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered a valid path on the current platform.</exception>
   bool CanCreateFile(ReadOnlySpan<char> filePath);

   /// <summary>Checks whether the current application has the permissions to read from a file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to read from
   ///   a file at the given <paramref name="filePath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the file at the given <paramref name="filePath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered a valid path on the current platform.</exception>
   bool CanReadFile(ReadOnlySpan<char> filePath);

   /// <summary>Checks whether the current application has the permissions to write to a file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to write to
   ///   a file at the given <paramref name="filePath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the file at the given <paramref name="filePath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered a valid path on the current platform.</exception>
   bool CanWriteToFile(ReadOnlySpan<char> filePath);

   /// <summary>Checks whether the current application has the permissions to rename the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to rename the
   ///   file at the given <paramref name="filePath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the file at the given <paramref name="filePath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered a valid path on the current platform.</exception>
   bool CanRenameFile(ReadOnlySpan<char> filePath);

   /// <summary>
   ///   Checks whether the current application has the permissions to move the file at the
   ///   given <paramref name="filePath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <param name="destinationDirectory">The destination directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to move the file at
   ///   the given <paramref name="filePath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </returns>
   /// <remarks>
   ///   This may return <see langword="false"/> if the file at the given <paramref name="filePath"/>
   ///   doesn't exist, or if the given <paramref name="destinationDirectory"/> doesn't exist
   ///   and the current application doesn't have the permissions to create it.
   /// </remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="filePath"/> or the given
   ///   <paramref name="destinationDirectory"/> is not considered valid on the current platform.
   /// </exception>
   bool CanMoveFile(ReadOnlySpan<char> filePath, ReadOnlySpan<char> destinationDirectory);

   /// <summary>
   ///   Checks whether the current application has the permissions to copy the file at the
   ///   given <paramref name="filePath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <param name="destinationDirectory">The destination directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to copy the file at
   ///   the given <paramref name="filePath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </returns>
   /// <remarks>
   ///   This may return <see langword="false"/> if the file at the given <paramref name="filePath"/>
   ///   doesn't exist, or if the given <paramref name="destinationDirectory"/> doesn't exist
   ///   and the current application doesn't have the permissions to create it.
   /// </remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="filePath"/> or the given
   ///   <paramref name="destinationDirectory"/> is not considered valid on the current platform.
   /// </exception>
   bool CanCopyFile(ReadOnlySpan<char> filePath, ReadOnlySpan<char> destinationDirectory);

   /// <summary>Checks whether the current application has the permissions to delete the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to delete
   ///   the file at the given <paramref name="filePath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the file at the given <paramref name="filePath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered a valid path on the current platform.</exception>
   bool CanDeleteFile(ReadOnlySpan<char> filePath);
   #endregion

   #region Directory methods
   /// <summary>Checks whether the current application has the permissions to create a directory at the given <paramref name="directoryPath"/>.</summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to create a
   ///   directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if a directory at the given <paramref name="directoryPath"/> already exists.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanCreateDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>Checks whether the current application has the permissions to enumerate the files in the directory in the given <paramref name="directoryPath"/>.</summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to enumerate the files
   ///   in the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanEnumerateFilesInDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>Checks whether the current application has the permissions to enumerate the directories in the directory in the given <paramref name="directoryPath"/>.</summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to enumerate the directories
   ///   in the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanEnumerateDirectoriesInDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>Checks whether the current application has the permissions to rename the directory at the given <paramref name="directoryPath"/>.</summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to rename the
   ///   directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanRenameDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>
   ///   Checks whether the current application has the permissions to move the directory at the
   ///   given <paramref name="directoryPath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <param name="destinationDirectory">The destination directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to move the directory at
   ///   the given <paramref name="directoryPath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </returns>
   /// <remarks>
   ///   This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/>
   ///   doesn't exist, or if the given <paramref name="destinationDirectory"/> doesn't exist
   ///   and the current application doesn't have the permissions to create it.
   /// </remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="directoryPath"/> or the given
   ///   <paramref name="destinationDirectory"/> is not considered valid on the current platform.
   /// </exception>
   bool CanMoveDirectory(ReadOnlySpan<char> directoryPath, ReadOnlySpan<char> destinationDirectory);

   /// <summary>
   ///   Checks whether the current application has the permissions to copy the directory at the
   ///   given <paramref name="directoryPath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <param name="destinationDirectory">The destination directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to copy the directory at
   ///   the given <paramref name="directoryPath"/>, to the given <paramref name="destinationDirectory"/>.
   /// </returns>
   /// <remarks>
   ///   This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/>
   ///   doesn't exist, or if the given <paramref name="destinationDirectory"/> doesn't exist
   ///   and the current application doesn't have the permissions to create it.
   /// </remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="directoryPath"/> or the given
   ///   <paramref name="destinationDirectory"/> is not considered valid on the current platform.
   /// </exception>
   bool CanCopyDirectory(ReadOnlySpan<char> directoryPath, ReadOnlySpan<char> destinationDirectory);

   /// <summary>
   ///   Checks whether the current application has the permissions to create
   ///   files in the directory at the given <paramref name="directoryPath"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to create files in
   ///   the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanCreateFilesInDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>
   ///   Checks whether the current application has the permissions to create
   ///   directories in the directory at the given <paramref name="directoryPath"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to create directories in
   ///   the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanCreateDirectoriesInDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>Checks whether the current application has the permissions to delete the directory at the given <paramref name="directoryPath"/>.</summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to delete
   ///   the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   bool CanDeleteDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>
   ///   Checks whether the current application has the permissions to delete
   ///   files from the directory at the given <paramref name="directoryPath"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to delete files from
   ///   the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanDeleteFilesFromDirectory(ReadOnlySpan<char> directoryPath);

   /// <summary>
   ///   Checks whether the current application has the permissions to delete
   ///   directories from the directory at the given <paramref name="directoryPath"/>.
   /// </summary>
   /// <param name="directoryPath">The path of the directory to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the current application has the permissions to delete directories from
   ///   the directory at the given <paramref name="directoryPath"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This may return <see langword="false"/> if the directory at the given <paramref name="directoryPath"/> doesn't exist.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="directoryPath"/> is not considered a valid path on the current platform.</exception>
   bool CanDeleteDirectoriesFromDirectory(ReadOnlySpan<char> directoryPath);
   #endregion
}
