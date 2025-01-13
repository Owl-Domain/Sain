namespace Sain.Shared.Logging;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class DefaultLoggingContext(IContextProvider? provider) : BaseLoggingContext(provider)
{
   #region Properties
   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> InitialiseAfter { get; } = [typeof(ISystemTimeContext)];
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override DateTimeOffset GetCurrentTime()
   {
      if (Context.System.Time.IsAvailable)
         return Context.System.Time.Now;

      return DateTimeOffset.Now;
   }

   /// <inheritdoc/>
   protected override TimeSpan GetCurrentTimestamp() => Application.Uptime;
   #endregion
}
