namespace Sain.Time;

/// <summary>
///   Represents the base implementation for a time context unit.
/// </summary>
/// <param name="provider">The context provider that the context unit comes from.</param>
public abstract class BaseTimeContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), ITimeContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public sealed override Type Kind => typeof(ITimeContextUnit);

   /// <inheritdoc/>
   public abstract DateTimeOffset LocalNow { get; }
   #endregion
}
