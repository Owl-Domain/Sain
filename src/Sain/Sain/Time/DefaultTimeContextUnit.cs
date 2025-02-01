namespace OwlDomain.Sain.Time;

/// <summary>
///   Represents the default implementation for a time unit context.
/// </summary>
/// <param name="provider">The context provider that the context unit comes from.</param>
public sealed class DefaultTimeContextUnit(IContextProviderUnit? provider) : BaseTimeContextUnit(provider)
{
   #region Properties
   /// <inheritdoc/>
   public override DateTimeOffset LocalNow => DateTimeOffset.Now;
   #endregion
}
