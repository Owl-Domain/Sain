namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents the base implementation for a logging context unit.
/// </summary>
public abstract class BaseLoggingContextUnit : BaseContextUnit, ILoggingContextUnit
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly SortedList<ILogPathConverter, ILogPathConverter> _filePathConverters = [];
   private readonly List<ILogEntry> _preInitEntries = [];
   private bool _isPreInit = false;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public sealed override Type Kind => typeof(ILoggingContextUnit);

   /// <inheritdoc/>
   public IReadOnlyList<ILogPathConverter> PathConverters => [.. _filePathConverters.Values];
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LoggingContextLogEntryEventHandler? LogEntryAdded;
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="BaseLoggingContextUnit"/>.</summary>
   /// <param name="provider">The context provider that the context unit comes from.</param>
   public BaseLoggingContextUnit(IContextProviderUnit? provider) : base(provider)
   {
      WithPathPrefixConverter("/home/nightowl/repos/Sain/repo/", "Sain");
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnAttach()
   {
      base.OnAttach();

      _preInitEntries.Clear();
      _isPreInit = true;
   }

   /// <inheritdoc/>
   protected override void OnInitialise()
   {
      base.OnInitialise();

      // Note(Nightowl): Specifically use a for loop instead of a foreach loop here in case event callbacks add extra log entries;
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

      this.Debug<BaseLoggingContextUnit>($"The logging context unit has been cleaned up, no more logging can occur after this point.");
   }

   /// <inheritdoc/>
   public ILoggingContextUnit WithPathConverter(ILogPathConverter converter)
   {
      _filePathConverters.Add(converter, converter);
      return this;
   }

   /// <inheritdoc/>
   public ILoggingContextUnit WithPathPrefixConverter(string prefix, string project)
   {
      prefix = prefix.Replace('\\', '/');
      if (prefix.EndsWith('/') is false)
         prefix += '/';

      LogPathPrefixConverter converter = new(prefix, project);
      WithPathConverter(converter);

      return this;
   }

   /// <inheritdoc/>
   public bool TryGetRelative(string fullPath, [NotNullWhen(true)] out string? relativePath, [NotNullWhen(true)] out ILogPathConverter? converter)
   {
      foreach (ILogPathConverter current in _filePathConverters.Values)
      {
         if (current.TryGetRelative(fullPath, out relativePath))
         {
            converter = current;
            return true;
         }
      }

      relativePath = default;
      converter = default;

      return false;
   }

   /// <inheritdoc/>
   public ILoggingContextUnit Log(LogSeverity severity, string context, string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      if (_isPreInit is false && LogEntryAdded is null)
         return this;

      if ((IsInitialised is false) && (_isPreInit is false))
         return this;

      file = GetNormalisedFilePath(file);

      if (TryGetRelative(file, out string? relativePath, out ILogPathConverter? converter))
         file = relativePath;

      ILogEntry entry = CreateEntry(severity, context, message, member, file, converter, line);

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
   /// <param name="converter">The (optional) log path converter that was used to turn the full path of the source <paramref name="file"/> into a project relative one.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   protected virtual ILogEntry CreateEntry(LogSeverity severity, string context, string message, string member, string file, ILogPathConverter? converter, int line)
   {
      TimeSpan timestamp = GetCurrentTimestamp();
      DateTimeOffset date = GetCurrentTime();

      return new LogEntry(date, timestamp, severity, context, message, member, file, converter, line);
   }

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
