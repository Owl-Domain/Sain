
namespace Sain.Shared.Logging;

/// <summary>
///   Represents an <see cref="ILoggingContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableLoggingContext : BaseUnavailableContext, ILoggingContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.Logging;

   /// <inheritdoc/>
   public IReadOnlyList<string> FilePathPrefixes => ThrowForUnavailable<IReadOnlyList<string>>();
   #endregion

   #region Events
   /// <inheritdoc/>
   public event LogEntryEventHandler? NewEntryLogged
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public ILoggingContext AddFilePathPrefix(string prefix) => ThrowForUnavailable<ILoggingContext>();

   /// <inheritdoc/>
   public ILoggingContext Log(
      LogSeverity severity,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      ThrowForUnavailable();
      return this;
   }
   #endregion
}
