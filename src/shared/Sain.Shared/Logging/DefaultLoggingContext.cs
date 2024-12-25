namespace Sain.Shared.Logging;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
public sealed class DefaultLoggingContext(IContextProvider? provider) : BaseContext(provider), ILoggingContext
{
   #region Fields
   private Stopwatch? _watch;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.Logging;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LogEntryEventHandler? NewEntryLogged;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void PreInitialise() => _watch = Stopwatch.StartNew();
   /// <inheritdoc/>
   protected override void PostCleanup() => _watch = null;

   /// <inheritdoc/>
   public ILoggingContext Log(LogSeverity severity, string context, string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      ThrowIfNotInitialised();
      Debug.Assert(_watch is not null);

      if (NewEntryLogged is null)
         return this;

      TimeSpan timestamp = _watch.Elapsed;
      DateTimeOffset date = DateTimeOffset.Now;

      LogEntry entry = new(date, timestamp, severity, context, message, member, file, line);
      NewEntryLogged?.Invoke(this, entry);

      return this;
   }
   #endregion
}
