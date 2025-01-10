namespace Sain.Shared.Logging;

/// <summary>
///   Represents the base implementation for a logging context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public abstract class BaseLoggingContext(IContextProvider? provider) : BaseContext(provider), ILoggingContext
{
   #region Nested types
   private sealed class Comparer : IComparer<int>
   {
      public int Compare(int x, int y) => y.CompareTo(x);
   }
   #endregion

   #region Fields
   private readonly SortedList<int, ILogPathPrefix> _filePathPrefixes = new(new Comparer());
   private readonly List<ILogSink> _sinks = [];
   private readonly List<string> _files = [];
   #endregion

   #region Properties
   /// <inheritdoc/>
   public sealed override string Kind => CoreContextKinds.Logging;

   /// <inheritdoc/>
   public override IReadOnlyCollection<string> DependsOnContexts => [];

   /// <inheritdoc/>
   public IReadOnlyList<ILogPathPrefix> PathPrefixes => [.. _filePathPrefixes.Values];

   /// <inheritdoc/>
   public IReadOnlyList<ILogSink> Sinks => _sinks;

   /// <inheritdoc/>
   public IReadOnlyList<string> Files => _files;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LogEntryEventHandler? NewEntryLogged;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      base.Initialise();

      Debug.Assert(_files.Count is 0);

      WithPathPrefix("/home/nightowl/repos/Sain/repo/", "Sain");

      DateTimeOffset timestamp = GetCurrentTime();

      foreach (ILogSink sink in _sinks)
         sink.Initialise(Application, timestamp);
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      base.Cleanup();

      foreach (ILogSink sink in _sinks)
         sink.Cleanup();

      _filePathPrefixes.Clear();
      _sinks.Clear();
      _files.Clear();
   }

   /// <inheritdoc/>
   public ILoggingContext WithPathPrefix(string prefix, string project)
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
   public ILoggingContext WithSink(ILogSink sink)
   {
      _sinks.Add(sink);

      return this;
   }

   /// <inheritdoc/>
   public ILoggingContext WithFile(string path)
   {
      ThrowIfNotInitialised();

      _files.Add(path);

      foreach (ILogSink sink in _sinks)
         sink.AddFile(path);

      return this;
   }

   /// <inheritdoc/>
   public ILoggingContext Log(LogSeverity severity, string context, string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      ThrowIfNotInitialised();

      if (NewEntryLogged is null && _sinks.Count is 0)
         return this;

      file = GetNormalisedFilePath(file);

      if (TryGetRelative(file, out string? relativePath, out ILogPathPrefix? prefix))
         file = relativePath;

      ILogEntry entry = CreateEntry(severity, context, message, member, file, prefix, line);

      foreach (ILogSink sink in _sinks)
         sink.AddEntry(entry);

      NewEntryLogged?.Invoke(this, entry);

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
   protected abstract ILogEntry CreateEntry(LogSeverity severity, string context, string message, string member, string file, ILogPathPrefix? prefix, int line);

   // Todo(Nightowl): This will be replaced once a system time context is added;

   /// <summary>Gets the current time.</summary>
   /// <returns>The current time.</returns>
   protected virtual DateTimeOffset GetCurrentTime() => DateTimeOffset.Now;
   #endregion

   #region Helpers
   private static string GetNormalisedFilePath(string file)
   {
      if (string.IsNullOrWhiteSpace(file))
         return file;

      file = file.Replace('\\', '/');

      return file;
   }
   #endregion
}
