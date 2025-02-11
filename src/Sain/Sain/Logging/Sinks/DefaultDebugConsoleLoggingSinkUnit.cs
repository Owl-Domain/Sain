namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents the default implementation for an application unit that prints the log to the debug console.
/// </summary>
public class DefaultDebugConsoleLoggingSinkUnit : BaseLoggingSinkUnit
{
   #region Methods
   /// <inheritdoc/>
   protected override void OnLogEntryAdded(ILoggingContextUnit context, ILogEntry entry)
   {
      if (entry.FileConverter is not null)
         Debug.WriteLine($"[{entry.FileConverter.Project}@{entry.Context}.{entry.Member}:{entry.Line:n0}][{entry.Severity}]: {entry.Message}");
      else
         Debug.WriteLine($"[{entry.Context}.{entry.Member}:{entry.Line:n0}][{entry.Severity}]: {entry.Message}");
   }
   #endregion
}

/// <summary>
///   Contains various extension methods that are related to the <see cref="IApplicationBuilder"/>
///   and to the <see cref="DefaultDebugConsoleLoggingSinkUnit"/>.
/// </summary>
public static class ApplicationBuilderDefaultDebugConsoleLoggingSinkUnitExtensions
{
   #region Methods
   /// <summary>Adds the built-in context provider units to the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TSelf WithDefaultDebugConsoleLoggingSinkUnit<TSelf>(this TSelf builder)
      where TSelf : IApplicationBuilder<TSelf>
   {
      if (Debugger.IsAttached)
         builder.WithUnit<DefaultDebugConsoleLoggingSinkUnit>();

      return builder;
   }
   #endregion
}
