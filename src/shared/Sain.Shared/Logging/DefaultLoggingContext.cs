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
   protected override void Initialise()
   {
      base.Initialise();

      _watch ??= Stopwatch.StartNew();
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      base.Cleanup();

      _watch = null;
   }

   /// <inheritdoc/>
   protected override DateTimeOffset GetCurrentTime()
   {
      if (IsInitialised && Context.System.Time.IsAvailable)
         return Context.System.Time.Now;

      return DateTimeOffset.Now;
   }

   /// <inheritdoc/>
   protected override TimeSpan GetCurrentTimestamp()
   {
      _watch ??= Stopwatch.StartNew();

      return _watch.Elapsed;
   }
   #endregion
}
