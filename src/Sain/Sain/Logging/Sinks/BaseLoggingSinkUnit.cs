namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents the base implementation for an application unit that acts as a logging sink.
/// </summary>
public abstract class BaseLoggingSinkUnit : BaseApplicationUnit, ILoggingSinkUnit
{
   #region Properties
   /// <inheritdoc/>
   public override Type Kind => typeof(ILoggingSinkUnit);

   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> InitialiseBeforeUnits { get; } = [typeof(ILoggingContextUnit)]; // Initialise before, so that the sink is cleaned up after the logging context.
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnInitialise()
   {
      base.OnInitialise();

      if (Context.Logging is not null)
      {
         Context.Logging.LogEntryAdded += OnLogEntryAdded;
         Context.Logging.LogFileAttached += OnLogFileAttached;

         Open();
      }
   }

   /// <inheritdoc/>
   protected override void OnCleanup()
   {
      if (Context.Logging is not null)
      {
         Context.Logging.LogEntryAdded -= OnLogEntryAdded;
         Context.Logging.LogFileAttached -= OnLogFileAttached;

         Close();
      }
   }

   /// <summary>Called when the log should be opened.</summary>
   protected abstract void Open();

   /// <summary>Called when the log should be closed.</summary>
   protected abstract void Close();

   /// <summary>Called when a new log entry is added.</summary>
   /// <param name="context">The logging context that the entry came from.</param>
   /// <param name="entry">The entry that was added.</param>
   protected abstract void OnLogEntryAdded(ILoggingContextUnit context, ILogEntry entry);

   /// <summary>Called when a new file is attached to the log.</summary>
   /// <param name="context">The logging context that the file was attached to.</param>
   /// <param name="path">The path of the file that was added.</param>
   protected virtual void OnLogFileAttached(ILoggingContextUnit context, string path) { }
   #endregion
}
