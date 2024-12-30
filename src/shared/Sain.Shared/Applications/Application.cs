namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <param name="id">The unique id of the application.</param>
/// <param name="name">The name of the application.</param>
/// <param name="version">The version of the application,</param>
/// <param name="context">The context of the application.</param>
public sealed class Application(string? id, string name, IVersion version, IApplicationContext context) : IApplication
{
   #region Fields
   private volatile bool _shouldBeRunning;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public string? Id { get; } = id;

   /// <inheritdoc/>
   public string Name { get; } = name;

   /// <inheritdoc/>
   public IVersion Version { get; } = version;

   /// <inheritdoc/>
   public IApplicationContext Context { get; } = context;

   /// <inheritdoc/>
   public ApplicationState State { get; private set; }

   /// <inheritdoc/>
   public TimeSpan LastIterationDuration { get; private set; }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event ApplicationEventHandler? Starting;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Started;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Stopping;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Stopped;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Iteration;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Run()
   {
      if (State is not ApplicationState.Stopped)
         throw new InvalidOperationException($"The application is either currently running or has not fully stopped yet, meaning it cannot be started again.");

      _shouldBeRunning = true;
      State = ApplicationState.Starting;
      Starting?.Invoke(this);

      Context.PreInitialise(this);
      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>($"Between {nameof(Context.PreInitialise)} and {nameof(Context.Initialise)} steps.");
      Context.Initialise(this);
      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>($"Between {nameof(Context.Initialise)} and {nameof(Context.PostInitialise)} steps.");
      Context.PostInitialise(this);
      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>($"Application started (after {nameof(Context.PostInitialise)}).");

      State = ApplicationState.Running;
      Started?.Invoke(this);

      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>("Application loop starting.");

      Stopwatch watch = new();
      while (_shouldBeRunning)
      {
         watch.Restart();

         Context.Dispatcher.Process();

         Iteration?.Invoke(this);
         LastIterationDuration = watch.Elapsed;

         if (LastIterationDuration.TotalMilliseconds < 10)
            Thread.Sleep(10 - (int)LastIterationDuration.TotalMilliseconds);
         else if (Context.Logging.IsAvailable)
         {
            if (LastIterationDuration.TotalMilliseconds > 250)
               Context.Logging.Warning<Application>($"Last iteration took over 250ms, something is affecting performance a lot ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 100)
               Context.Logging.Warning<Application>($"Last iteration took over 100ms, something is affecting performance ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 50)
               Context.Logging.Debug<Application>($"Last iteration took over 50ms ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 30)
               Context.Logging.Debug<Application>($"Last iteration took over 30ms ({LastIterationDuration.TotalMilliseconds:n2}ms).");
         }
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Trace<Application>("Application stopping.");

      State = ApplicationState.Stopping;
      Stopping?.Invoke(this);

      State = ApplicationState.Stopped;

      if (Context.Logging.IsAvailable)
         Context.Logging.Trace<Application>($"Application stopped (before {nameof(Context.PreCleanup)}).");

      Context.PreCleanup(this);
      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>($"Between {nameof(Context.PreCleanup)} and {nameof(Context.Cleanup)} steps.");
      Context.Cleanup(this);
      if (Context.Logging.IsAvailable) Context.Logging.Trace<Application>($"Between {nameof(Context.Cleanup)} and {nameof(Context.PostCleanup)} steps.");
      Context.PostCleanup(this);

      Stopped?.Invoke(this);
   }

   /// <inheritdoc/>
   public void Stop() => _shouldBeRunning = false;
   #endregion

   #region Functions
   /// <summary>Creates a builder for a new application.</summary>
   /// <returns>The application builder which can be used to configure the application.</returns>
   public static ApplicationBuilder New() => new();
   #endregion
}
