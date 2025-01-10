namespace Sain.Shared.System.Time;

/// <summary>
///   Represents the default implementation for a logging context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class DefaultSystemTimeContext(IContextProvider? provider) : BaseSystemTimeContext(provider)
{
   #region Properties
   /// <inheritdoc/>
   public override DateTimeOffset Now => DateTimeOffset.Now;
   #endregion

}
