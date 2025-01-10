namespace Sain.Shared.System.Time;

/// <summary>
///   Represents the application's context for system time information.
/// </summary>
public interface ISystemTimeContext : IContext
{
   #region Properties
   /// <summary>The current system time.</summary>
   DateTimeOffset Now { get; }
   #endregion
}
