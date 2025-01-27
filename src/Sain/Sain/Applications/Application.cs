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
   #endregion

   #region Helpers
   private void RunOnCurrentThread(ApplicationRunMode mode)
   {
      try
      {
         Initialise();

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
