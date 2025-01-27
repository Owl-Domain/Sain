namespace Sain.Applications;

/// <summary>
///   Represents the configuration options for a Sain application.
/// </summary>
/// <param name="minimumIterationTime">The minimum time that each application iteration should last.</param>
public sealed class ApplicationConfiguration(TimeSpan minimumIterationTime) : IApplicationConfiguration
{
   #region Properties
   /// <inheritdoc/>
   public TimeSpan MinimumIterationTime { get; } = minimumIterationTime;
   #endregion
}
