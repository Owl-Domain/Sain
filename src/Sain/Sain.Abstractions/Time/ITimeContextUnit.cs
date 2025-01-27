namespace Sain.Time;

/// <summary>
///   Represents the application's context unit for time information.
/// </summary>
public interface ITimeContextUnit : IContextUnit
{
   #region Properties
   /// <summary>The current time in the local time zone.</summary>
   DateTimeOffset LocalNow { get; }
   #endregion
}
