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
   public void Run(ApplicationRunMode mode = ApplicationRunMode.RunOnCurrentThread)
   {
      if (IsRunModeKnown(mode) is false)
         throw new ArgumentOutOfRangeException(nameof(mode), mode, $"The run mode ({mode}) is not known.");

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

      if (mode is ApplicationRunMode.RunOnCurrentThread)
         RunOnCurrentThread();
      else if (mode is ApplicationRunMode.RunSingleIterationOnCurrentThread)
         RunSingleIterationOnCurrentThread();
      else if (mode is ApplicationRunMode.RunOnNewThread)
         RunOnNewThread();
      else if (mode is ApplicationRunMode.RunOnBackgroundThread)
         RunOnBackgroundThread();
      else
         Debug.Fail("This shouldn't happen because of the guard.");
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
   #endregion

   #region Helpers
   private static bool IsRunModeKnown(ApplicationRunMode mode)
   {
      return mode switch
      {
         ApplicationRunMode.RunOnCurrentThread => true,
         ApplicationRunMode.RunSingleIterationOnCurrentThread => true,
         ApplicationRunMode.RunOnNewThread => true,
         ApplicationRunMode.RunOnBackgroundThread => true,

         _ => false,
      };
   }
   private void RunOnCurrentThread()
   {
      try
      {
         Initialise();
         while (IsStopRequested is false)
            RunIteration();
      }
      finally
      {
         Cleanup();
      }
   }
   private void RunSingleIterationOnCurrentThread()
   {
      try
      {
         Initialise();
         RunIteration();
      }
      finally
      {
         Cleanup();
      }
   }
   private void RunOnNewThread()
   {
      try
      {
         Thread thread = new(RunOnCurrentThread)
         {
            Name = $"Sain Application Thread ({Info.Name})"
         };

         thread.Start();
      }
      finally
      {
         Cleanup();
      }
   }
   private void RunOnBackgroundThread()
   {
      try
      {
         Thread thread = new(RunOnCurrentThread)
         {
            Name = $"Sain Application Background Thread ({Info.Name})",
            IsBackground = true,
         };

         thread.Start();
      }
      finally
      {
         Cleanup();
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
   private void Initialise()
   {
      Starting?.Invoke(this);
      Context.Initialise(this);
      Started?.Invoke(this);
      StartupTime = _generalWatch.Elapsed;
   }
   private void Cleanup()
   {
      lock (_runLock)
      {
         Stopping?.Invoke(this);
         IsStopRequested = false;
         IsRunning = false;

         try
         {
            if (Context.IsInitialised)
               Context.Cleanup();
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
