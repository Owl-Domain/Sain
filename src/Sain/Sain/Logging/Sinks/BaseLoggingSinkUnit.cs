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
         Open();
      }
   }

   /// <inheritdoc/>
   protected override void OnCleanup()
   {
      if (Context.Logging is not null)
      {
         Context.Logging.LogEntryAdded -= OnLogEntryAdded;
         Close();
      }
   }

   /// <summary>Called when the log should be opened.</summary>
   protected virtual void Open() { }

   /// <summary>Called when the log should be closed.</summary>
   protected virtual void Close() { }

   /// <summary>Called when a new log entry is added.</summary>
   /// <param name="context">The logging context that the entry came from.</param>
   /// <param name="entry">The entry that was added.</param>
   protected abstract void OnLogEntryAdded(ILoggingContextUnit context, ILogEntry entry);
   #endregion
}
