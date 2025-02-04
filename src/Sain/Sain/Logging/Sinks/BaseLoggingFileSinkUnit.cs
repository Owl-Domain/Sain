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
   public override IReadOnlyCollection<Type> RequiresUnits { get; } = [typeof(IGeneralStorageContextUnit), typeof(ILogStorageContextUnit)];

   /// <inheritdoc/>
   public string? FilePath { get; private set; }

   /// <summary>The stream for the created log file.</summary>
   /// <exception cref="InvalidOperationException">Thrown if accessed before the stream has been opened, or if accessed after it has been closed.</exception>
   protected FileStream Stream => _stream ?? throw new InvalidOperationException($"The logging sink hasn't been opened yet.");

   /// <summary>The preferred file name for the log file.</summary>
   protected abstract string PreferredFileName { get; }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Open()
   {
      Debug.Assert(Context.Logging is not null, $"The {nameof(BaseLoggingSinkUnit)} ensures this method will only be called if logging is available.");
      Debug.Assert(Context.Storage.Log is not null);
      Debug.Assert(Context.Storage.General is not null);

      string directory = Context.Storage.Log.SessionDirectory;
      string fullPath = Path.Combine(directory, PreferredFileName);

      if (TryCreateFile(directory, fullPath, out FileStream? stream) is false)
      {
         ReadOnlySpan<char> fileName = Context.Storage.General.GetFileNameWithoutExtension(PreferredFileName);
         ReadOnlySpan<char> extension = Context.Storage.General.GetExtension(PreferredFileName);

         for (int attempt = 2; ; attempt++)
         {
            string duplicateFileName = Context.Storage.General.GetDuplicateFileName(fileName, extension, attempt);
            fullPath = Path.Combine(directory, duplicateFileName);

            if (TryCreateFile(directory, fullPath, out stream))
               break;
         }
      }

      _stream = stream;
      FilePath = stream.Name;
   }

   /// <inheritdoc/>
   protected override void Close()
   {
      try
      {
         if (_stream is not null)
         {
            lock (_stream)
               _stream.Close();
         }
      }
      finally
      {
         _stream = null;
         FilePath = null;
      }
   }

   /// <inheritdoc/>
   protected sealed override void OnLogEntryAdded(ILoggingContextUnit context, ILogEntry entry)
   {
      Debug.Assert(_stream is not null);

      lock (_stream)
      {
         OnLogEntryAddedCore(context, entry);
         Flush();
      }
   }

   /// <summary>Called when a new log entry is added.</summary>
   /// <param name="context">The logging context that the entry came from.</param>
   /// <param name="entry">The entry that was added.</param>
   protected abstract void OnLogEntryAddedCore(ILoggingContextUnit context, ILogEntry entry);
   #endregion

   #region Helpers
   private bool TryCreateFile(string? directory, string fullPath, [NotNullWhen(true)] out FileStream? stream)
   {
      if (Directory.Exists(directory) is false && directory is not null)
         Directory.CreateDirectory(directory);

      if (File.Exists(fullPath))
      {
         stream = null;
         return false;
      }

      // Todo(Nightowl): Should this bother catching the IOException? Could that result in potential bugs?

      stream = TryCreateFile(fullPath);
      return true;
   }

   /// <summary>Tries to create the log file at the given <paramref name="path"/>.</summary>
   /// <param name="path">The path to create the file at.</param>
   /// <returns>The stream for the created file.</returns>
   /// <remarks>
   ///   This method should only try to create the file at the given
   ///   <paramref name="path"/>, error handling should be performed by the caller.
   /// </remarks>
   protected virtual FileStream TryCreateFile(string path) => File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);

   /// <summary>Flushes any written data to the file.</summary>
   /// <remarks>This will be automatically called after a new log entry has been added.</remarks>
   protected virtual void Flush() => _stream?.Flush(true);
   #endregion
}
