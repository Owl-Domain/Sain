namespace Sain.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application <paramref name="context"/>.</typeparam>
/// <param name="info">The information about the application.</param>
/// <param name="configuration">The configuration options for the application.</param>
/// <param name="context">The context of the application.</param>
public abstract class Application<TContext>(
   IApplicationInfo info,
   IApplicationConfiguration configuration,
   TContext context)
   : IApplication<TContext>
   where TContext : notnull, IApplicationContext
{
   #region Constants
   private const string LogContext = nameof(Application);
   #endregion

   #region Fields
   private readonly object _runLock = new();
   private readonly Stopwatch _runTimeWatch = new();
   private readonly Stopwatch _generalWatch = new();
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IApplicationInfo Info { get; } = info;

   /// <inheritdoc/>
   public IApplicationConfiguration Configuration { get; } = configuration;

   /// <inheritdoc/>
   public TContext Context { get; } = context;

   /// <inheritdoc/>
   public bool IsRunning { get; private set; }

   /// <inheritdoc/>
   public bool IsStopRequested { get; private set; }

   /// <inheritdoc/>
   public TimeSpan RunTime => _runTimeWatch.Elapsed;

   /// <inheritdoc/>
   public TimeSpan LastIterationTime { get; private set; }

   /// <inheritdoc/>
   public TimeSpan ActualLastIterationTime { get; private set; }

   /// <inheritdoc/>
   public TimeSpan StartupTime { get; private set; }
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
   public void Run(ApplicationRunThread thread, ApplicationRunMode mode)
   {
      if (Enum.IsDefined(typeof(ApplicationRunThread), thread) is false)
         throw new ArgumentOutOfRangeException(nameof(thread), thread, $"The given application run thread is invalid.");

      if (Enum.IsDefined(typeof(ApplicationRunMode), mode) is false)
         throw new ArgumentOutOfRangeException(nameof(mode), mode, $"The given application run mode is invalid.");

      lock (_runLock)
      {
         if (IsRunning)
            throw new InvalidOperationException("The application is already running.");

         StartupTime = default;
         LastIterationTime = default;
         ActualLastIterationTime = default;

         _generalWatch.Restart();
         _runTimeWatch.Restart();
         IsStopRequested = false;
         IsRunning = true;
      }

      if (thread is ApplicationRunThread.RunOnCurrentThread)
         RunOnCurrentThread(mode);
      else if (thread is ApplicationRunThread.RunOnNewThread)
         RunOnNewThread(false, mode);
      else if (thread is ApplicationRunThread.RunOnBackgroundThread)
         RunOnNewThread(true, mode);
      else
         throw new NotImplementedException($"The given application run thread ({thread}) has not been implemented yet.");
   }
   /// <inheritdoc/>
   public void Stop()
   {
      lock (_runLock)
      {
         if (IsRunning)
            IsStopRequested = true;
      }
   }
   private void RunOnCurrentThread(ApplicationRunMode mode)
   {
      try
      {
         Initialise(mode);

         if (mode is ApplicationRunMode.UntilStopped)
         {
            while (IsStopRequested is false)
               RunIteration();
         }
         else if (IsStopRequested is false)
            RunIteration();
      }
      finally
      {
         Cleanup();
      }
   }
   private void RunOnOtherThreadStart(object? state)
   {
      Debug.Assert(state is not null);
      RunOnCurrentThread((ApplicationRunMode)state);
   }
   private void RunOnNewThread(bool isBackground, ApplicationRunMode mode)
   {
      try
      {
         Thread thread = new(RunOnOtherThreadStart)
         {
            Name = isBackground ? $"Sain Application Background Thread ({Info.Name})" : $"Sain Application Thread ({Info.Name})",
            IsBackground = isBackground,
         };

         thread.Start(mode);
      }
      catch
      {
         Cleanup();
         throw;
      }
   }
   private void RunIteration()
   {
      _generalWatch.Restart();
      Iteration?.Invoke(this);

      TimeSpan iterationTime = _generalWatch.Elapsed;
      if (iterationTime < Configuration.MinimumIterationTime)
      {
         TimeSpan difference = Configuration.MinimumIterationTime - iterationTime;
         Thread.Sleep(difference);
      }

      ActualLastIterationTime = iterationTime;
      LastIterationTime = _generalWatch.Elapsed;
   }
   private void Initialise(ApplicationRunMode mode)
   {
      foreach (IApplicationUnit unit in Context.AllUnits)
         unit.Attach(this);

      if (Context.Logging is not null)
      {
         LogApplicationInfo(mode);
         LogApplicationThreadInfo();
         LogApplicationUnitInfo();
         LogApplicationInitialisationOrder();

         Context.Logging.Trace(LogContext, $"Application is about to start.");
      }

      Starting?.Invoke(this);
      Context.Initialise(this);

      Started?.Invoke(this);
      StartupTime = _generalWatch.Elapsed;

      Context.Logging?.Trace(LogContext, $"Application has finished starting, took {GetNiceTimeFormat(StartupTime)}.");
   }
   private void Cleanup()
   {
      lock (_runLock)
      {
         Context.Logging?.Trace(LogContext, $"Application is stopping after running for {GetNiceTimeFormat(RunTime)}.");

         Stopping?.Invoke(this);
         IsStopRequested = false;
         IsRunning = false;

         try
         {
            if (Context.IsInitialised)
               Context.Cleanup();

            foreach (IApplicationUnit unit in Context.AllUnits)
               unit.Detach();
         }
         finally
         {
            _generalWatch.Stop();
            _runTimeWatch.Stop();

            Stopped?.Invoke(this);
         }
      }
   }
   #endregion

   #region Helpers
   private void LogApplicationInfo(ApplicationRunMode mode)
   {
      Debug.Assert(Context.Logging is not null);

      Context.Logging.Info(LogContext, $"Running application ({Info.Name}) in the '{mode}' mode.");

      foreach (IApplicationId id in Info.Ids)
         Context.Logging.Info(LogContext, $"Application id ({id.GetType().Name}): {id.DisplayName}");

      foreach (IApplicationVersion version in Info.Versions)
         Context.Logging.Info(LogContext, $"Application version ({version.GetType().Name}): {version.DisplayName}");
   }
   private void LogApplicationUnitInfo()
   {
      Debug.Assert(Context.Logging is not null);

      if (Context.AllUnits.Count is 1)
         Context.Logging.Info(LogContext, "Application has 1 available unit in total.");
      else
         Context.Logging.Info(LogContext, $"Application has {Context.AllUnits.Count:n0} available units in total.");

      if (Context.ContextProviders.Count is 1)
         Context.Logging.Info(LogContext, "1 context provider unit.");
      else
         Context.Logging.Info(LogContext, $"{Context.ContextProviders.Count:n0} context provider units.");

      if (Context.Contexts.Count is 1)
         Context.Logging.Info(LogContext, "1 context unit.");
      else
         Context.Logging.Info(LogContext, $"{Context.Contexts.Count:n0} context units.");

      if (Context.GeneralUnits.Count is 1)
         Context.Logging.Info(LogContext, "1 general unit.");
      else
         Context.Logging.Info(LogContext, $"{Context.GeneralUnits.Count:n0} general units.");

      foreach (IContextProviderUnit provider in Context.ContextProviders)
      {
         Type type = provider.GetType();
         Context.Logging.Info(LogContext, $"Context provider unit ({type.FullName ?? type.Name}) of the kind ({provider.Kind}).");
      }

      foreach (IContextUnit context in Context.Contexts)
      {
         Type type = context.GetType();
         Context.Logging.Info(LogContext, $"Context unit ({type.FullName ?? type.Name}) of the kind ({context.Kind}).");
      }

      foreach (IApplicationUnit unit in Context.GeneralUnits)
      {
         Type type = unit.GetType();
         Context.Logging.Info(LogContext, $"General application unit ({type.FullName ?? type.Name}) of the kind ({unit.Kind}).");
      }
   }
   private void LogApplicationInitialisationOrder()
   {
      Debug.Assert(Context.Logging is not null);


      if (Context.InitialisationOrder.Count is 1)
         Context.Logging.Debug(LogContext, "Application has 1 unit to initialise.");
      else
         Context.Logging.Debug(LogContext, $"Application has {Context.InitialisationOrder.Count:n0} units to initialise, in the following order.");

      int i = 1;
      foreach (IApplicationUnit unit in Context.InitialisationOrder)
      {
         Type type = unit.GetType();
         Context.Logging.Debug(LogContext, $"#{i++:n0} - {type.FullName ?? type.Name}.");
      }
   }
   private void LogApplicationThreadInfo()
   {
      Debug.Assert(Context.Logging is not null);

      Thread thread = Thread.CurrentThread;
      Context.Logging.Debug(LogContext, $"Application is starting on thread #{thread.ManagedThreadId:n0}.");

      if (thread.Name is not null)
         Context.Logging.Debug(LogContext, $"Application thread name: ({thread.Name}).");

      if (thread.IsBackground)
         Context.Logging.Debug(LogContext, $"Application thread is a background thread.");
      else
         Context.Logging.Debug(LogContext, $"Application thread is not a background thread.");
   }
   private static string GetNiceTimeFormat(TimeSpan time)
   {
      List<string> parts = [];

      int hours = (time.Days * 24) + time.Hours;

      if (hours > 0)
         parts.Add($"{hours:n0}h");

      if (time.Minutes > 0)
         parts.Add($"{time.Minutes}m");

      if (time.Days > 0)
         parts.Add($"{time.Seconds}s");

      if (parts.Count is 0 || time.Milliseconds > 0)
         parts.Add($"{time.Milliseconds:n0}ms");

      return string.Join(' ', parts);
   }
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <param name="info">The information about the application.</param>
/// <param name="configuration">The configuration options for the application.</param>
/// <param name="context">The context of the application.</param>
public sealed class Application(
   IApplicationInfo info,
   IApplicationConfiguration configuration,
   IApplicationContext context)
   : Application<IApplicationContext>(info, configuration, context)
{
   #region Functions
   /// <summary>Starts building a new Sain application.</summary>
   /// <returns>The application builder used to build and customise the new application.</returns>
   public static ApplicationBuilder New() => new();
   #endregion
}
