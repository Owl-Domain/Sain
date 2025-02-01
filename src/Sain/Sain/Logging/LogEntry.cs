namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents a log entry.
/// </summary>
/// <param name="date">The date that the log entry was created on.</param>
/// <param name="timestamp">The timestamp associated with the log entry.</param>
/// <param name="severity">The severity of the log entry.</param>
/// <param name="context">The context that logged the entry.</param>
/// <param name="message">The message of the log entry.</param>
/// <param name="member">The member (inside the <paramref name="context"/>) that logged the entry.</param>
/// <param name="file">The source file in which the entry was logged in.</param>
/// <param name="fileConverter">The (optional) log path converter that was used to turn the full path of the source file into a relative one.</param>
/// <param name="line">The line number (inside of the source <paramref name="file"/>) that the entry was logged on.</param>
public sealed class LogEntry(
   DateTimeOffset date,
   TimeSpan timestamp,
   LogSeverity severity,
   string context,
   string message,
   string member,
   string file,
   ILogPathConverter? fileConverter,
   int line)
   : ILogEntry
{
   #region Properties
   /// <inheritdoc/>
   public DateTimeOffset Date { get; } = date;

   /// <inheritdoc/>
   public TimeSpan Timestamp { get; } = timestamp;

   /// <inheritdoc/>
   public LogSeverity Severity { get; } = severity;

   /// <inheritdoc/>
   public string Context { get; } = context;

   /// <inheritdoc/>
   public string Message { get; } = message;

   /// <inheritdoc/>
   public string Member { get; } = member;

   /// <inheritdoc/>
   public string File { get; } = file;

   /// <inheritdoc/>
   public ILogPathConverter? FileConverter { get; } = fileConverter;

   /// <inheritdoc/>
   public int Line { get; } = line;
   #endregion
}
