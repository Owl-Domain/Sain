namespace Sain.Logging;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class DefaultLoggingContextUnit(IContextProviderUnit? provider) : BaseLoggingContextUnit(provider)
{
   #region Properties
   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> InitialiseAfterUnits { get; } = [typeof(ITimeContextUnit)];
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override DateTimeOffset GetCurrentTime()
   {
      if (Context.Time is not null)
         return Context.Time.LocalNow;

      return DateTimeOffset.Now;
   }

   /// <inheritdoc/>
   protected override TimeSpan GetCurrentTimestamp() => Application.RunTime;
   #endregion
}
