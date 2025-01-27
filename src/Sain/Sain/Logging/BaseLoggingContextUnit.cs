namespace Sain.Logging;

/// <summary>
///   Represents the base implementation for a logging context unit.
/// </summary>
/// <param name="provider">The context provider that the context unit comes from.</param>
public abstract class BaseLoggingContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), ILoggingContextUnit
{
   #region Nested types
   private sealed class Comparer : IComparer<int>
   {
      public int Compare(int x, int y) => y.CompareTo(x);
   }
   #endregion

   #region Fields
   private readonly SortedList<int, ILogPathPrefix> _filePathPrefixes = new(new Comparer());
   private readonly List<string> _files = [];
   private readonly List<ILogEntry> _preInitEntries = [];
   private bool _isPreInit = false;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public sealed override Type Kind => typeof(ILoggingContextUnit);

   /// <inheritdoc/>
   public IReadOnlyList<ILogPathPrefix> PathPrefixes => [.. _filePathPrefixes.Values];

   /// <inheritdoc/>
   public IReadOnlyList<string> Files => _files;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LoggingContextLogEntryEventHandler? LogEntryAdded;

   /// <inheritdoc/>
   public event LoggingContextFileEventHandler? LogFileAttached;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnAttach()
   {
      base.OnAttach();
      _isPreInit = true;

      WithPathPrefix("/home/nightowl/repos/Sain/repo/", "Sain");
   }

   /// <inheritdoc/>
   protected override void OnInitialise()
   {
      base.OnInitialise();

      Debug.Assert(_files.Count is 0);

      // Note(Nightowl): Specifically use a for loop instead of a foreach loop here in case sinks / event callbacks add extra log entries;
      for (int i = 0; i < _preInitEntries.Count; i++)
      {
         ILogEntry entry = _preInitEntries[i];
         OnNewEntry(entry);
      }

      _isPreInit = false;
      _preInitEntries.Clear();
   }

   /// <inheritdoc/>
   protected override void OnCleanup()
   {
      base.OnCleanup();
      Debug.Assert(_preInitEntries.Count is 0);

      _filePathPrefixes.Clear();
      _files.Clear();
   }

   /// <inheritdoc/>
   public ILoggingContextUnit WithPathPrefix(string prefix, string project)
   {
      prefix = prefix.Replace('\\', '/');
      if (prefix.EndsWith('/') is false)
         prefix += '/';

      LogPathPrefix pathPrefix = new(prefix, project);

      _filePathPrefixes.Add(prefix.Length, pathPrefix);

      return this;
   }

   /// <inheritdoc/>
   public bool TryGetRelative(string fullPath, [NotNullWhen(true)] out string? relativePath, [NotNullWhen(true)] out ILogPathPrefix? prefix)
   {
      foreach (ILogPathPrefix current in _filePathPrefixes.Values)
      {
         if (fullPath.StartsWith(current.Prefix))
         {
            relativePath = fullPath[current.Prefix.Length..];
            prefix = current;

            return true;
         }
      }

      relativePath = default;
      prefix = default;

      return false;
   }

   /// <inheritdoc/>
   public ILoggingContextUnit WithFile(string path)
   {
      ThrowIfNotInitialised();

      _files.Add(path);
      LogFileAttached?.Invoke(this, path);

      return this;
   }

   /// <inheritdoc/>
   public ILoggingContextUnit Log(LogSeverity severity, string context, string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      if (LogEntryAdded is null)
         return this;

      file = GetNormalisedFilePath(file);

      if (TryGetRelative(file, out string? relativePath, out ILogPathPrefix? prefix))
         file = relativePath;

      ILogEntry entry = CreateEntry(severity, context, message, member, file, prefix, line);

      if (_isPreInit)
         _preInitEntries.Add(entry);
      else
         OnNewEntry(entry);

      return this;
   }

   /// <summary>Creates a new message in the log.</summary>
   /// <param name="severity">The severity of the log message.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="prefix">The (optional) log path prefix that was used to turn the full path of the source file into a relative one.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   protected virtual ILogEntry CreateEntry(LogSeverity severity, string context, string message, string member, string file, ILogPathPrefix? prefix, int line)
   {
      TimeSpan timestamp = GetCurrentTimestamp();
      DateTimeOffset date = GetCurrentTime();

      return new LogEntry(date, timestamp, severity, context, message, member, file, prefix, line);
   }

   // Todo(Nightowl): This will be replaced once a system time context is added;

   /// <summary>Gets the current time.</summary>
   /// <returns>The current time.</returns>
   protected abstract DateTimeOffset GetCurrentTime();

   /// <summary>Gets the current timestamp.</summary>
   /// <returns>The current timestamp.</returns>
   protected abstract TimeSpan GetCurrentTimestamp();
   #endregion

   #region Helpers
   private static string GetNormalisedFilePath(string file)
   {
      if (string.IsNullOrWhiteSpace(file))
         return file;

      file = file.Replace('\\', '/');

      return file;
   }
   private void OnNewEntry(ILogEntry entry) => LogEntryAdded?.Invoke(this, entry);
   #endregion
}
