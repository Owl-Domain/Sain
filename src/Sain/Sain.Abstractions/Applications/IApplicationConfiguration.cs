namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the configuration options for a Sain application.
/// </summary>
public interface IApplicationConfiguration
{
   #region Properties
   /// <summary>The minimum time that each application iteration should last.</summary>
   TimeSpan MinimumIterationTime { get; }
   #endregion
}
