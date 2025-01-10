namespace Sain.Shared.Logging;

/// <summary>
///   Represents a log entry.
/// </summary>
public interface ILogEntry
{
   #region Properties
   /// <summary>The date that the log entry was created on.</summary>
   DateTimeOffset Date { get; }

   /// <summary>The timestamp associated with the log entry.</summary>
   /// <remarks>This will usually be the time since the application has started.</remarks>
   TimeSpan Timestamp { get; }

   /// <summary>The severity of the log entry.</summary>
   LogSeverity Severity { get; }

   /// <summary>The context that logged the entry.</summary>
   string Context { get; }

   /// <summary>The message of the log entry.</summary>
   string Message { get; }

   /// <summary>The member (inside the <see cref="Context"/>) that logged the entry.</summary>
   /// <remarks>A value of <see cref="string.Empty"/> means no member was specified.</remarks>
   string Member { get; }

   /// <summary>The source file in which the entry was logged in.</summary>
   /// <remarks>A value of <see cref="string.Empty"/> means no file was specified.</remarks>
   string File { get; }

   /// <summary>The (optional) log path prefix that was used to turn the full path of the source file into a relative one.</summary>
   /// <remarks>This is related to the <see cref="File"/> property.</remarks>
   ILogPathPrefix? FilePrefix { get; }

   /// <summary>The line number (inside of the source <see cref="File"/>) that the entry was logged on.</summary>
   /// <remarks>A value of <c>0</c> means no line number was specified.</remarks>
   int Line { get; }
   #endregion
}
