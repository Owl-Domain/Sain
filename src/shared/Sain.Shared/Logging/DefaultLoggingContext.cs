namespace Sain.Shared.Logging;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
public sealed class DefaultLoggingContext(IContextProvider? provider) : BaseContext(provider), ILoggingContext
{
   #region Nested types
   private sealed class Comparer : IComparer<int>
   {
      public int Compare(int x, int y) => y.CompareTo(x);
   }
   #endregion

   #region Fields
   private readonly SortedList<int, string> _filePathPrefixes = new(new Comparer());
   private Stopwatch? _watch;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.Logging;

   /// <inheritdoc/>
   public IReadOnlyList<string> FilePathPrefixes => [.. _filePathPrefixes.Values];
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LogEntryEventHandler? NewEntryLogged;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void PreInitialise()
   {
      _watch = Stopwatch.StartNew();
      Debug.Assert(_filePathPrefixes.Count is 0);

      AddFilePathPrefix("/home/nightowl/repos/Sain/repo/");
   }
   /// <inheritdoc/>
   protected override void PostCleanup()
   {
      _watch = null;
      _filePathPrefixes.Clear();
   }

   /// <inheritdoc/>
   public ILoggingContext AddFilePathPrefix(string prefix)
   {
      prefix = prefix.Replace('\\', '/');
      if (prefix.EndsWith('/') is false)
         prefix += '/';

      _filePathPrefixes.Add(prefix.Length, prefix);

      return this;
   }

   /// <inheritdoc/>
   public ILoggingContext Log(LogSeverity severity, string context, string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      ThrowIfNotInitialised();
      Debug.Assert(_watch is not null);

      if (NewEntryLogged is null)
         return this;

      TimeSpan timestamp = _watch.Elapsed;
      DateTimeOffset date = DateTimeOffset.Now;

      file = GetNormalisedFilePath(file);
      LogEntry entry = new(date, timestamp, severity, context, message, member, file, line);
      NewEntryLogged?.Invoke(this, entry);

      return this;
   }

   private string GetNormalisedFilePath(string file)
   {
      if (string.IsNullOrWhiteSpace(file))
         return file;

      file = file.Replace('\\', '/');

      foreach (string prefix in _filePathPrefixes.Values)
      {
         if (file.StartsWith(prefix))
            return file[prefix.Length..];
      }

      return file;
   }
   #endregion
}
