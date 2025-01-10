namespace Sain.Shared.System.Time;

/// <summary>
///   Represents the base implementation for a system time.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>

public abstract class BaseSystemTimeContext(IContextProvider? provider) : BaseContext(provider), ISystemTimeContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override string Kind => CoreContextKinds.SystemTime;

   /// <inheritdoc/>
   public abstract DateTimeOffset Now { get; }
   #endregion
}
