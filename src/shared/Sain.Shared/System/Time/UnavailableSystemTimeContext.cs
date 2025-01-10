namespace Sain.Shared.System.Time;

/// <summary>
///   Represents an <see cref="ISystemTimeContext"/> that is always marked as unavailable.
/// </summary>
public class UnavailableSystemTimeContext : BaseUnavailableContext, ISystemTimeContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.SystemTime;

   /// <inheritdoc/>
   public DateTimeOffset Now => ThrowForUnavailable<DateTimeOffset>();
   #endregion
}
