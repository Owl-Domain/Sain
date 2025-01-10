namespace Sain.Shared.Logging;

/// <summary>
///   Represents an event handler delegate for log entry related events.
/// </summary>
/// <param name="context">The logging context that raised the event.</param>
/// <param name="entry">The log entry that the event is for.</param>
public delegate void LogEntryEventHandler(ILoggingContext context, ILogEntry entry);

/// <summary>
///   Represents the application's context for logging.
/// </summary>
public interface ILoggingContext : IContext
{
   #region Properties
   /// <summary>The collection of the log path prefixes that are used to turn the source file paths into relative ones.</summary>
   IReadOnlyList<ILogPathPrefix> PathPrefixes { get; }

   /// <summary>The sinks that have been attached to the logging context.</summary>
   IReadOnlyList<ILogSink> Sinks { get; }
   #endregion

   #region Events
   /// <summary>The event that is raised whenever a new log entry is added.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if the logging context is unavailable.</exception>
   event LogEntryEventHandler? NewEntryLogged;
   #endregion

   #region Methods
   /// <summary>Adds a new log path <paramref name="prefix"/>.</summary>
   /// <param name="prefix">The file path prefix to use when turning the source file paths into relative ones.</param>
   /// <param name="project">The project that the given <paramref name="prefix"/> belongs to.</param>
   /// <returns>The used logging context.</returns>
   /// <remarks>This path should be the entire path of the project that is before the <c>/src/</c> directory.</remarks>
   ILoggingContext WithPathPrefix(string prefix, string project);

   /// <summary>Attaches the given log <paramref name="sink"/> to the logging context.</summary>
   /// <param name="sink">The log sink to attach.</param>
   /// <returns>The used logging context.</returns>
   ILoggingContext WithSink(ILogSink sink);

   /// <summary>Tries to get the <paramref name="relativePath"/> from the given <paramref name="fullPath"/>.</summary>
   /// <param name="fullPath">The full path to get the relative path for.</param>
   /// <param name="relativePath">The calculated relative path.</param>
   /// <param name="prefix">The log path prefix that was used to turn the <paramref name="fullPath"/> into the <paramref name="relativePath"/>.</param>
   /// <returns><see langword="true"/> if the <paramref name="relativePath"/> could be calculated, <see langword="false"/> otherwise.</returns>
   bool TryGetRelative(string fullPath, [NotNullWhen(true)] out string? relativePath, [NotNullWhen(true)] out ILogPathPrefix? prefix);

   /// <summary>Logs a new message in the log.</summary>
   /// <param name="severity">The severity of the log message.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   ILoggingContext Log(
      LogSeverity severity,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="ILoggingContext"/>.
/// </summary>
public static class ILoggingContextExtensions
{
   #region General methods
   /// <summary>Tries to get the <paramref name="relativePath"/> from the given <paramref name="fullPath"/>.</summary>
   /// <param name="context">The logging context to use.</param>
   /// <param name="fullPath">The full path to get the relative path for.</param>
   /// <param name="relativePath">The calculated relative path.</param>
   /// <returns><see langword="true"/> if the <paramref name="relativePath"/> could be calculated, <see langword="false"/> otherwise.</returns>
   public static bool TryGetRelative(this ILoggingContext context, string fullPath, [NotNullWhen(true)] out string? relativePath)
   {
      return context.TryGetRelative(fullPath, out relativePath, out _);
   }

   /// <summary>Attaches the given log <paramref name="sink"/> to the logging context.</summary>
   /// <param name="context">The logging context to use.</param>
   /// <param name="sink">The log sink to attach.</param>
   /// <param name="customise">The (optional) callback that can be used to customise the given <paramref name="sink"/>.</param>
   /// <returns>The used logging context.</returns>
   public static ILoggingContext WithSink(this ILoggingContext context, ILogSink sink, Action<ILogSink> customise)
   {
      customise.Invoke(sink);
      return context.WithSink(sink);
   }

   /// <summary>Creates a new log sink of the given type <typeparamref name="T"/> and attached it to the given logging <paramref name="context"/>.</summary>
   /// <typeparam name="T">The type of the log sink to create and attach to the given logging <paramref name="context"/>.</typeparam>
   /// <param name="context">The logging context to use.</param>
   /// <param name="customise">The (optional) callback that can be used to customise the created sink.</param>
   /// <returns>The used logging context.</returns>
   public static ILoggingContext WithSink<T>(this ILoggingContext context, Action<T>? customise = null)
      where T : ILogSink, new()
   {
      T sink = new();
      customise?.Invoke(sink);

      return context.WithSink(sink);
   }
   #endregion

   #region Log methods
   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Fatal"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Fatal(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Fatal, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Error"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Error(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Error, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Warning"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Warning(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Warning, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Info"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Info(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Info, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Debug"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Debug(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Debug, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Trace"/>.</summary>
   /// <param name="log">The logging context to use.</param>
   /// <param name="context">The context that the log message came from.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Trace(
      this ILoggingContext log,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return log.Log(LogSeverity.Trace, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="severity">The severity of the log message.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Log<TContext>(
      this ILoggingContext log,
      LogSeverity severity,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      string context = typeof(TContext).Name;

      return log.Log(severity, context, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Fatal"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Fatal<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Fatal, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Error"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Error<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Error, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Warning"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Warning<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Warning, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Info"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Info<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Info, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Debug"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Debug<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Debug, message, member, file, line);
   }

   /// <summary>Logs a new message in the log with the severity level <see cref="LogSeverity.Trace"/>.</summary>
   /// <typeparam name="TContext">The context that the log message came from.</typeparam>
   /// <param name="log">The logging context to use.</param>
   /// <param name="message">The message to log.</param>
   /// <param name="member">The member (inside the context) that logged the message.</param>
   /// <param name="file">The source file that the message was logged in.</param>
   /// <param name="line">The line number (inside the source <paramref name="file"/>) that the message was logged on.</param>
   /// <returns>The used logging context.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Might be thrown if the logging context is unavailable, or if it has not been initialised yet.
   /// </exception>
   public static ILoggingContext Trace<TContext>(
      this ILoggingContext log,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return Log<TContext>(log, LogSeverity.Trace, message, member, file, line);
   }
   #endregion
}
