namespace Sain.Shared.Logging;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class DefaultLoggingContext(IContextProvider? provider) : BaseLoggingContext(provider)
{
   #region Fields
   private Stopwatch? _watch;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void PreInitialise()
   {
      base.PreInitialise();

      _watch = Stopwatch.StartNew();
   }

   /// <inheritdoc/>
   protected override void PostCleanup()
   {
      base.PostCleanup();

      _watch = null;
   }

   /// <inheritdoc/>
   protected override ILogEntry CreateEntry(LogSeverity severity, string context, string message, string member, string file, ILogPathPrefix? prefix, int line)
   {
      Debug.Assert(_watch is not null);
      TimeSpan timestamp = _watch.Elapsed;
      DateTimeOffset date = DateTimeOffset.Now;

      return new LogEntry(date, timestamp, severity, context, message, member, file, prefix, line);
   }
   #endregion
}
