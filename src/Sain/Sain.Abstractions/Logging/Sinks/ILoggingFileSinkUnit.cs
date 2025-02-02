namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents an application unit that acts as a file based logging sink.
/// </summary>
public interface ILoggingFileSinkUnit : ILoggingSinkUnit
{
   #region Properties
   /// <summary>The path of the file that the log is being saved to.</summary>
   string? FilePath { get; }
   #endregion
}
