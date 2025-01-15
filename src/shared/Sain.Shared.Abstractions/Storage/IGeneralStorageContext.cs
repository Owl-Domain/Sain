namespace Sain.Shared.Storage;

/// <summary>
///   Represents the various reasons why an IO operation might fail.
/// </summary>
public enum IOFailureReasonKind : byte
{
   /// <summary>No failure, operation was successful.</summary>
   None,

   /// <summary>The failure reason couldn't be determined.</summary>
   Unknown,

   /// <summary>The failure reason was known, but it has not been defined.</summary>
   Other,

   /// <summary>The current application didn't have the sufficient permissions to perform the operation.</summary>
   NoPermissions,

   /// <summary>The file/directory requested by the operation did not exist.</summary>
   NotFound,

   /// <summary>The file/directory requested by the operation already exists.</summary>
   AlreadyExists,

   /// <summary>The source file/directory is in use by a different process and sharing was not enabled.</summary>
   SourceInUse,

   /// <summary>The destination file/directory is in use by a different process and sharing was not enabled.</summary>
   DestinationInUse,

   /// <summary>There wasn't enough space left on the disk to perform the operation.</summary>
   NotEnoughSpace,
}

/// <summary>
///   Represents a failure reason for an IO operation.
/// </summary>
/// <param name="kind">The kind of the failure.</param>
/// <param name="exception">The exception that might have been thrown to cause the failure.</param>
/// <param name="reason">The reason as to why the operation failed.</param>
public readonly struct IOFailureReason(IOFailureReasonKind kind, Exception? exception, string? reason = null)
{
   #region Fields
   /// <summary>A reusable <see cref="IOFailureReason"/> that represents a successful operation.</summary>
   public static readonly IOFailureReason None = new(IOFailureReasonKind.None, null, null);
   #endregion

   #region Properties
   /// <summary>The kind of the failure.</summary>
   public readonly IOFailureReasonKind Kind { get; } = kind;

   /// <summary>The exception that might have been thrown to cause the failure.</summary>
   public readonly Exception? Exception { get; } = exception;

   /// <summary>The reason as to why the operation failed.</summary>
   /// <remarks>This will usually be the message of the <see cref="Exception"/>.</remarks>
   public readonly string? Reason { get; } = reason ?? exception?.Message;
   #endregion
}

/// <summary>
///   Represents the different options that can be used when opening a file for writing.
/// </summary>
[Flags]
public enum FileWriteOptions : byte
{
   /// <summary>No special options.</summary>
   None = 0,

   /// <summary>If the file is missing then it will be created.</summary>
   CreateIfMissing = 1,

   /// <summary>If the file already exists then it will be overwritten.</summary>
   /// <remarks>This will also truncate the file first, making sure that no old data stays.</remarks>
   OverrideIfExists = 2,
}

/// <summary>
///   Represents the application's context for general purpose storage operations.
/// </summary>
public interface IGeneralStorageContext : IContext
{
   #region Validation methods
   /// <summary>Checks whether the given <paramref name="filename"/> is valid on the current platform.</summary>
   /// <param name="filename">The filename to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="filename"/> is
   ///   valid on the current platform, <see langword="false"/> otherwise.
   /// </returns>
   bool IsFilenameValid(ReadOnlySpan<char> filename);

   /// <summary>Checks whether the given <paramref name="filename"/> would be valid on most platforms.</summary>
   /// <param name="filename">The filename to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="filename"/> would
   ///   be valid on most platforms, <see langword="false"/> otherwise.
   /// </returns>
   bool IsFilenameCrossPlatform(ReadOnlySpan<char> filename);

   /// <summary>Checks whether the given <paramref name="path"/> is valid on the current platform.</summary>
   /// <param name="path">The path to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="path"/> is valid
   ///   on the current platform, <see langword="false"/> otherwise.
   /// </returns>
   bool IsPathValid(ReadOnlySpan<char> path);

   /// <summary>Fills the given <paramref name="destination"/> with random characters that are considered valid on the current platform.</summary>
   /// <param name="destination">The destination span to fill with the random characters.</param>
   void CreateRandomFilename(Span<char> destination);
   #endregion

   #region Try/OpenForReading methods
   /// <summary>Opens the file at the given <paramref name="filePath"/> for reading.</summary>
   /// <param name="filePath">The path of the file to open for reading.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <returns>The file stream that can read from the file at the given <paramref name="filePath"/>.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   /// <exception cref="FileNotFoundException">Thrown if the file at the given <paramref name="filePath"/> doesn't exist.</exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application doesn't have the permissions
   ///   to read from the file at the given <paramref name="filePath"/>.
   /// </exception>
   FileStream OpenForReading(ReadOnlySpan<char> filePath, FileShare share = FileShare.Read);

   /// <summary>Tires to opens the file at the given <paramref name="filePath"/> for reading.</summary>
   /// <param name="filePath">The path of the file to try and open for reading.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to read from the file, or <see langword="null"/> if opening the file failed.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   bool TryOpenForReading(ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason);
   #endregion

   #region Try/OpenForWriting methods
   /// <summary>Opens the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="flags">The flags used to customise the behaviour for opening the file.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <returns>The file stream that can write to the file at the given <paramref name="filePath"/>.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   /// <exception cref="FileNotFoundException">
   /// Thrown if the file at the given <paramref name="filePath"/> doesn't exist,
   /// whether this is thrown will depending on the given <paramref name="flags"/>.
   /// </exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application either doesn't have the permissions to write to the file at the given <paramref name="filePath"/>,
   ///   or if the file was missing and the current application doesn't have the permissions to create it.
   /// </exception>
   FileStream OpenForWriting(ReadOnlySpan<char> filePath, FileWriteOptions flags = FileWriteOptions.CreateIfMissing, FileShare share = FileShare.None);

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="flags">The flags used to customise the behaviour for opening the file.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   bool TryOpenForWriting(ReadOnlySpan<char> filePath, FileWriteOptions flags, FileShare share, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason);
   #endregion

   #region Try/OpenForAppending methods
   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="createIfMissing">Whether the file should be created if it was missing.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <returns>The file stream that can be used to append to the file.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   /// <exception cref="FileNotFoundException">
   /// Thrown if the file at the given <paramref name="filePath"/> doesn't exist,
   /// whether this is thrown will depending on the given <paramref name="createIfMissing"/> value.
   /// </exception>
   FileStream OpenForAppending(ReadOnlySpan<char> filePath, bool createIfMissing, FileShare share = FileShare.None);

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="createIfMissing">Whether the file should be created if it was missing.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   bool TryOpenForAppending(ReadOnlySpan<char> filePath, bool createIfMissing, FileShare share, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason);
   #endregion

   #region Try/RenameFile methods
   /// <summary>Renames the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to rename.</param>
   /// <param name="newName">The new name of the file.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <exception cref="ArgumentException">Thrown if either the given <paramref name="filePath"/> or the given <paramref name="newName"/> were not considered valid on the current platform.</exception>
   /// <exception cref="FileNotFoundException">Thrown if the file at the given <paramref name="filePath"/> doesn't exist.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the new file already exists, and replacing it was not allowed.</exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application doesn't have the permissions
   ///   to rename the file at the given <paramref name="filePath"/>.
   /// </exception>
   void RenameFile(ReadOnlySpan<char> filePath, ReadOnlySpan<char> newName, bool replace = false);

   /// <summary>Tries to Rename the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to rename.</param>
   /// <param name="newName">The new name to rename the file to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <param name="failureReason">The reason why renaming the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if renaming the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if either the given <paramref name="filePath"/> or the given <paramref name="newName"/> were not considered valid on the current platform.</exception>
   bool TryRenameFile(ReadOnlySpan<char> filePath, ReadOnlySpan<char> newName, bool replace, out IOFailureReason failureReason);
   #endregion

   #region Try/MoveFile methods
   /// <summary>Moves the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="sourceFilePath">The path to the file to move.</param>
   /// <param name="destinationFilePath">The path that the file should be moved to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   /// <exception cref="FileNotFoundException">Thrown if the file at the given <paramref name="sourceFilePath"/> doesn't exist.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the <paramref name="destinationFilePath"/> already exists, and replacing it was not allowed.</exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application either doesn't have the permissions to move the file at the given <paramref name="sourceFilePath"/>,
   ///   or if it doesn't have the permissions to move the file to the given <paramref name="destinationFilePath"/>, or if
   ///   the destination directory doesn't exist and the current application doesn't have the permissions to create it.
   /// </exception>
   void MoveFile(ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace = false);

   /// <summary>Tries to move the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="sourceFilePath">The path to the file to move.</param>
   /// <param name="destinationFilePath">The path that the file should be moved to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <param name="failureReason">The reason why moving the file at the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/> failed.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   bool TryMoveFile(ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace, out IOFailureReason failureReason);
   #endregion

   #region Try/CopyFile methods
   /// <summary>Copies the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="sourceFilePath">The path to the file to copy.</param>
   /// <param name="destinationFilePath">The path that the file should be copied to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   /// <exception cref="FileNotFoundException">Thrown if the file at the given <paramref name="sourceFilePath"/> doesn't exist.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the <paramref name="destinationFilePath"/> already exists, and replacing it was not allowed.</exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application either doesn't have the permissions to copy the file at the given <paramref name="sourceFilePath"/>,
   ///   or if it doesn't have the permissions to copy the file to the given <paramref name="destinationFilePath"/>, or if
   ///   the destination directory doesn't exist and the current application doesn't have the permissions to create it.
   /// </exception>
   void CopyFile(ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace = false);

   /// <summary>Tries to copy the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="sourceFilePath">The path to the file to copy.</param>
   /// <param name="destinationFilePath">The path that the file should be copied to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <param name="failureReason">The reason why moving the file at the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/> failed.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   bool TryCopyFile(ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace, out IOFailureReason failureReason);
   #endregion

   #region Try/DeleteFile methods
   /// <summary>Deletes the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to delete.</param>
   /// <param name="throwIfMissing">Whether the <see cref="FileNotFoundException"/> should be thrown if the file was missing.</param>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   /// <exception cref="FileNotFoundException">
   ///   Thrown if the file at the given <paramref name="filePath"/> didn't exist, this will depending on the given <paramref name="throwIfMissing"/> value.
   /// </exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application doesn't have the permissions
   ///   to delete the file at the given <paramref name="filePath"/>.
   /// </exception>
   void DeleteFile(ReadOnlySpan<char> filePath, bool throwIfMissing = false);

   /// <summary>Tries to delete the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="filePath">The path of the file to delete.</param>
   /// <param name="failIfMissing">Whether the method should return <see langword="false"/> if the file was already missing.</param>
   /// <param name="failureReason">The reason why deleting the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if the file was successfully deleted or missing, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   bool TryDeleteFile(ReadOnlySpan<char> filePath, bool failIfMissing, out IOFailureReason failureReason);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IGeneralStorageContext"/>.
/// </summary>
public static class IGeneralStorageContextExtensions
{
   #region Try/Reading methods
   /// <summary>Tires to opens the file at the given <paramref name="filePath"/> for reading.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to try and open for reading.</param>
   /// <param name="file">The file stream that can be used to read from the file, or <see langword="null"/> if reading from the file failed.</param>
   /// <param name="failureReason">The reason why reading the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileShare.Read"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForReading(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForReading(filePath, FileShare.Read, out file, out failureReason);
   }

   /// <summary>Tires to opens the file at the given <paramref name="filePath"/> for reading.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to try and open for reading.</param>
   /// <param name="file">The file stream that can be used to read from the file, or <see langword="null"/> if reading from the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileShare.Read"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForReading(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForReading(filePath, out file, out _);
   }

   /// <summary>Tires to opens the file at the given <paramref name="filePath"/> for reading.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to try and open for reading.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to read from the file, or <see langword="null"/> if reading from the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForReading(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForReading(filePath, share, out file, out _);
   }
   #endregion

   #region Try/OpenForWriting methods
   /// <summary>Opens the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <returns>The file stream that can write to the file at the given <paramref name="filePath"/>.</returns>
   /// <remarks>If the file is missing then it will be created, if you want to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   /// <exception cref="UnauthorizedAccessException">
   ///   Thrown if the current application either doesn't have the permissions to write to the file at the given <paramref name="filePath"/>,
   ///   or if the file was missing and the current application doesn't have the permissions to create it.
   /// </exception>
   public static FileStream OpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share = FileShare.None)
   {
      return context.OpenForWriting(filePath, FileWriteOptions.CreateIfMissing, share);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="flags">The flags used to customise the behaviour for opening the file.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileShare.None"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileWriteOptions flags, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForWriting(filePath, flags, FileShare.None, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileWriteOptions.CreateIfMissing"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForWriting(filePath, FileWriteOptions.CreateIfMissing, share, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileWriteOptions.CreateIfMissing"/> and <see cref="FileShare.None"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForWriting(filePath, FileWriteOptions.CreateIfMissing, FileShare.None, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="flags">The flags used to customise the behaviour for opening the file.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileWriteOptions flags, FileShare share, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForWriting(filePath, flags, FileShare.None, out file, out _);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="flags">The flags used to customise the behaviour for opening the file.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileShare.None"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileWriteOptions flags, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForWriting(filePath, flags, FileShare.None, out file, out _);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileWriteOptions.CreateIfMissing"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForWriting(filePath, FileWriteOptions.CreateIfMissing, share, out file, out _);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for writing.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for writing.</param>
   /// <param name="file">The file stream that can be used to write to the file, or <see langword="null"/> if opening the file failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opening with <see cref="FileWriteOptions.CreateIfMissing"/> and <see cref="FileShare.None"/>, to change this, use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForWriting(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForWriting(filePath, FileWriteOptions.CreateIfMissing, FileShare.None, out file, out _);
   }
   #endregion

   #region Try/OpenForAppending methods
   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <returns>The file stream that can be used to append to the file.</returns>
   /// <remarks>If the file is missing then it will be created, to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static FileStream OpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share = FileShare.None)
   {
      return context.OpenForAppending(filePath, true, share);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="createIfMissing">Whether the file should be created if it was missing.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opened with <see cref="FileShare.None"/>, to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, bool createIfMissing, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForAppending(filePath, createIfMissing, FileShare.None, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>If the file is missing then it will be created, to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForAppending(filePath, true, share, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <param name="failureReason">The reason why opening the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>If the file is missing then it will be created, the file will also be opened with <see cref="FileShare.None"/>. To change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file, out IOFailureReason failureReason)
   {
      return context.TryOpenForAppending(filePath, true, FileShare.None, out file, out failureReason);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="createIfMissing">Whether the file should be created if it was missing.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>The file will be opened with <see cref="FileShare.None"/>, to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, bool createIfMissing, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForAppending(filePath, createIfMissing, FileShare.None, out file, out _);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="share">The sharing mode that the file should be opened with.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>If the file is missing then it will be created, to change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, FileShare share, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForAppending(filePath, true, share, out file, out _);
   }

   /// <summary>Tries to open the file at the given <paramref name="filePath"/> for appending.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to open for appending.</param>
   /// <param name="file">The file stream that can be used to append to the file.</param>
   /// <returns><see langword="true"/> if opening the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>If the file is missing then it will be created, the file will also be opened with <see cref="FileShare.None"/>. To change this behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryOpenForAppending(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, [NotNullWhen(true)] out FileStream? file)
   {
      return context.TryOpenForAppending(filePath, true, FileShare.None, out file, out _);
   }
   #endregion

   #region TryRenameFile methods
   /// <summary>Tries to Rename the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to rename.</param>
   /// <param name="newName">The new name to rename the file to.</param>
   /// <param name="failureReason">The reason why renaming the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if renaming the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <remarks>This method will not replace the file if it already exists, if you want to change that behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">Thrown if either the given <paramref name="filePath"/> or the given <paramref name="newName"/> were not considered valid on the current platform.</exception>
   public static bool TryRenameFile(IGeneralStorageContext context, ReadOnlySpan<char> filePath, ReadOnlySpan<char> newName, out IOFailureReason failureReason)
   {
      return context.TryRenameFile(filePath, newName, false, out failureReason);
   }

   /// <summary>Tries to Rename the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to rename.</param>
   /// <param name="newName">The new name to rename the file to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <returns><see langword="true"/> if renaming the file at the given <paramref name="filePath"/> was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if either the given <paramref name="filePath"/> or the given <paramref name="newName"/> were not considered valid on the current platform.</exception>
   public static bool TryRenameFile(IGeneralStorageContext context, ReadOnlySpan<char> filePath, ReadOnlySpan<char> newName, bool replace = false)
   {
      return context.TryRenameFile(filePath, newName, replace, out _);
   }
   #endregion

   #region TryMoveFile methods
   /// <summary>Tries to move the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="sourceFilePath">The path to the file to move.</param>
   /// <param name="destinationFilePath">The path that the file should be moved to.</param>
   /// <param name="failureReason">The reason why moving the file at the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/> failed.</param>
   /// <remarks>This method will not override the <paramref name="destinationFilePath"/> if it already exists, to change that behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   public static bool TryMoveFile(this IGeneralStorageContext context, ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, out IOFailureReason failureReason)
   {
      return context.TryMoveFile(sourceFilePath, destinationFilePath, false, out failureReason);
   }

   /// <summary>Tries to move the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="sourceFilePath">The path to the file to move.</param>
   /// <param name="destinationFilePath">The path that the file should be moved to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   public static bool TryMoveFile(this IGeneralStorageContext context, ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace = false)
   {
      return context.TryMoveFile(sourceFilePath, destinationFilePath, replace, out _);
   }
   #endregion

   #region TryCopyFile methods
   /// <summary>Tries to copy the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="sourceFilePath">The path to the file to copy.</param>
   /// <param name="destinationFilePath">The path that the file should be copied to.</param>
   /// <param name="failureReason">The reason why moving the file at the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/> failed.</param>
   /// <remarks>This method will not override the <paramref name="destinationFilePath"/> if it already exists, to change that behaviour use a different overload.</remarks>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   public static bool TryCopyFile(this IGeneralStorageContext context, ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, out IOFailureReason failureReason)
   {
      return context.TryCopyFile(sourceFilePath, destinationFilePath, false, out failureReason);
   }

   /// <summary>Tries to copy the file from the given <paramref name="sourceFilePath"/> to the given <paramref name="destinationFilePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="sourceFilePath">The path to the file to copy.</param>
   /// <param name="destinationFilePath">The path that the file should be copied to.</param>
   /// <param name="replace">Whether the new file should be replaced if it already exists.</param>
   /// <exception cref="ArgumentException">
   ///   Thrown if either the given <paramref name="sourceFilePath"/> or the given
   ///   <paramref name="destinationFilePath"/> were not considered valid on the current platform.
   /// </exception>
   public static bool TryCopyFile(this IGeneralStorageContext context, ReadOnlySpan<char> sourceFilePath, ReadOnlySpan<char> destinationFilePath, bool replace = false)
   {
      return context.TryCopyFile(sourceFilePath, destinationFilePath, replace, out _);
   }
   #endregion

   #region TryDeleteFile methods
   /// <summary>Tries to delete the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to delete.</param>
   /// <param name="failIfMissing">Whether the method should return <see langword="false"/> if the file was already missing.</param>
   /// <returns><see langword="true"/> if the file was successfully deleted or missing, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryDeleteFile(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, bool failIfMissing = false)
   {
      return context.TryDeleteFile(filePath, failIfMissing, out _);
   }

   /// <summary>Tries to delete the file at the given <paramref name="filePath"/>.</summary>
   /// <param name="context">The general storage context to use.</param>
   /// <param name="filePath">The path of the file to delete.</param>
   /// <param name="failureReason">The reason why deleting the file at the given <paramref name="filePath"/> failed.</param>
   /// <returns><see langword="true"/> if the file was successfully deleted, <see langword="false"/> otherwise.</returns>
   /// <remarks>This method will also return <see langword="true"/> if the file was missing. Use a different overload if you want to change the behaviour.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="filePath"/> is not considered valid on the current platform.</exception>
   public static bool TryDeleteFile(this IGeneralStorageContext context, ReadOnlySpan<char> filePath, out IOFailureReason failureReason)
   {
      return context.TryDeleteFile(filePath, false, out failureReason);
   }
   #endregion
}
