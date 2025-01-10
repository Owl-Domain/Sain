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
   public IReadOnlyList<ILogPathPrefix> PathPrefixes => ThrowForUnavailable<IReadOnlyList<ILogPathPrefix>>();

   /// <inheritdoc/>
   public IReadOnlyList<ILogSink> Sinks => ThrowForUnavailable<IReadOnlyList<ILogSink>>();

   /// <inheritdoc/>
   public IReadOnlyList<string> Files => ThrowForUnavailable<IReadOnlyList<string>>();
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
   public ILoggingContext WithPathPrefix(string prefix, string project) => ThrowForUnavailable<ILoggingContext>();

   /// <inheritdoc/>
   public bool TryGetRelative(string fullPath, [NotNullWhen(true)] out string? relativePath, [NotNullWhen(true)] out ILogPathPrefix? prefix)
   {
      relativePath = null;
      prefix = null;

      return ThrowForUnavailable<bool>();
   }

   /// <inheritdoc/>
   public ILoggingContext WithSink(ILogSink sink) => ThrowForUnavailable<ILoggingContext>();

   /// <inheritdoc/>
   public ILoggingContext WithFile(string path) => ThrowForUnavailable<ILoggingContext>();

   /// <inheritdoc/>
   public ILoggingContext Log(
      LogSeverity severity,
      string context,
      string message,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
   {
      return ThrowForUnavailable<ILoggingContext>();
   }
   #endregion
}
