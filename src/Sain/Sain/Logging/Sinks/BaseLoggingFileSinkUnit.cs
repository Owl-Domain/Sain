using System.IO;

namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents the base implementation for an application unit that acts as a file based logging sink.
/// </summary>
public abstract class BaseLoggingFileSinkUnit : BaseLoggingSinkUnit, ILoggingFileSinkUnit
{
   #region Fields
   private FileStream? _stream;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public override Type Kind => typeof(ILoggingFileSinkUnit);

   /// <inheritdoc/>
   public string? FilePath { get; private set; }

   /// <summary>The stream for the created log file.</summary>
   /// <exception cref="InvalidOperationException">Thrown if accessed before the stream has been opened, or if accessed after it has been closed.</exception>
   protected FileStream Stream => _stream ?? throw new InvalidOperationException($"The logging sink hasn't been opened yet.");
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Open()
   {
      GetLogPath(out string directory, out string preferredFileName);

      string extension = Path.GetExtension(preferredFileName);
      preferredFileName = Path.GetFileNameWithoutExtension(preferredFileName);

      int attempt = 2;

      FileStream? stream;
      while (TryCreateFile(directory, preferredFileName, extension, attempt, out stream) is false)
         attempt++;

      FilePath = stream.Name;
      _stream = stream;
   }

   /// <inheritdoc/>
   protected override void Close()
   {
      _stream?.Close();
      _stream = null;
      FilePath = null;
   }

   /// <inheritdoc/>
   protected sealed override void OnLogEntryAdded(ILoggingContextUnit context, ILogEntry entry)
   {
      Debug.Assert(_stream is not null);

      OnLogEntryAddedCore(context, entry);
      _stream.Flush(true);
   }

   /// <summary>Called when a new log entry is added.</summary>
   /// <param name="context">The logging context that the entry came from.</param>
   /// <param name="entry">The entry that was added.</param>
   protected abstract void OnLogEntryAddedCore(ILoggingContextUnit context, ILogEntry entry);
   #endregion

   #region Helpers
   /// <summary>Gets the path information about where the log file should be created.</summary>
   /// <param name="directory">The directory in which the file should be created.</param>
   /// <param name="preferredFileName">The preferred name of the log file (including the extension).</param>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>
   ///         The <paramref name="preferredFileName"/> is only a hint, sometimes the file may already
   ///         exist, in which case <see cref="CreateLogPath"/> is used to derive a new name.
   ///      </item>
   ///      <item>The reason the methods are separated is so that <see cref="GetLogPath"/> can perform more expensive operations without having to worry.</item>
   ///   </list>
   /// </remarks>
   protected abstract void GetLogPath(out string directory, out string preferredFileName);

   /// <summary>Creates the path where the log file should be created at.</summary>
   /// <param name="directory">The directory in which the log file should be placed in.</param>
   /// <param name="preferredFileName">The preferred name for the log file (without the extension).</param>
   /// <param name="extension">The extension that should be given to the file.</param>
   /// <param name="creationAttempt">The amount of times creating the log file has been attempted.</param>
   /// <returns>The full path where the log file should be created.</returns>
   protected virtual string CreateLogPath(string directory, string preferredFileName, string extension, int creationAttempt)
   {
      string name = $"{preferredFileName} ({extension:n0}){extension}";
      return Path.Combine(directory, name);
   }

   /// <summary>Tries to create the log file.</summary>
   /// <param name="directory">The directory in which the log file should be placed in.</param>
   /// <param name="preferredFileName">The preferred name for the log file (without the extension).</param>
   /// <param name="extension">The extension that should be given to the file.</param>
   /// <param name="creationAttempt">The amount of times creating the log file has been attempted.</param>
   /// <param name="stream">The stream for the created file, or <see langword="null"/> if creating the file fails.</param>
   /// <returns><see langword="true"/> if creating the log file was successful, <see langword="false"/> otherwise.</returns>
   protected virtual bool TryCreateFile(string directory, string preferredFileName, string extension, int creationAttempt, [NotNullWhen(true)] out FileStream? stream)
   {
      if (Directory.Exists(directory) is false)
         Directory.CreateDirectory(directory);

      string path = CreateLogPath(directory, preferredFileName, extension, creationAttempt);
      if (File.Exists(path))
      {
         stream = default;
         return false;
      }

      try
      {
         stream = TryCreateFile(path);
         return true;
      }
      catch (IOException)
      {
         stream = default;
         return false;
      }
   }

   /// <summary>Tries to create the log file at the given <paramref name="path"/>.</summary>
   /// <param name="path">The path to create the file at.</param>
   /// <returns>The stream for the created file.</returns>
   /// <remarks>
   ///   This method should only try to create the file at the given
   ///   <paramref name="path"/>, error handling should be performed by the caller.
   /// </remarks>
   protected virtual FileStream TryCreateFile(string path) => File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
   #endregion
}
