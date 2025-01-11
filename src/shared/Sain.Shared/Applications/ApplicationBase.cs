using System.Linq;

namespace Sain.Shared.Applications;

/// <summary>
///   Represents a base Sain application.
/// </summary>
/// <param name="info">The information about the application.</param>
/// <param name="context">The context of the application.</param>
public abstract class ApplicationBase(IApplicationInfo info, IApplicationContext context) : IApplicationBase
{
   #region Fields
   private volatile bool _shouldBeRunning;
   private readonly Stopwatch _uptimeWatch = new();
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IApplicationInfo Info { get; } = info;

   /// <inheritdoc/>
   public IApplicationContext Context { get; } = context;

   /// <inheritdoc/>
   public ApplicationState State { get; private set; }

   /// <inheritdoc/>
   public TimeSpan LastIterationDuration { get; private set; }

   /// <inheritdoc/>
   public TimeSpan Uptime => _uptimeWatch.Elapsed;
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
      _uptimeWatch.Restart();
      try
      {
         Initialise();
         MainLoop();
         Cleanup();
      }
      finally
      {
         _uptimeWatch.Stop();
      }
   }

   /// <inheritdoc/>
   public void Stop() => _shouldBeRunning = false;
   #endregion

   #region Helpers
   private void Initialise()
   {
      State = ApplicationState.Starting;
      RaiseStarting();

      if (Context.Logging.IsAvailable)
      {
         LogInitialisationInformation();
         LogInitialisationOrder();
         Context.Logging.Trace<ApplicationBase>($"Application is starting.");
      }

      Context.Initialise(this);

      if (Context.Logging.IsAvailable)
         Context.Logging.Trace<ApplicationBase>($"Application started.");

      State = ApplicationState.Running;
      RaiseStarted();
   }
   private void MainLoop()
   {
      if (Context.Logging.IsAvailable)
         Context.Logging.Trace<ApplicationBase>("Application loop starting.");

      Stopwatch watch = new();
      while (_shouldBeRunning)
      {
         watch.Restart();

         Context.Dispatcher.Process();

         RaiseIteration();
         LastIterationDuration = watch.Elapsed;

         if (LastIterationDuration.TotalMilliseconds < 10)
            Thread.Sleep(10 - (int)LastIterationDuration.TotalMilliseconds);
         else if (Context.Logging.IsAvailable)
         {
            if (LastIterationDuration.TotalMilliseconds > 250)
               Context.Logging.Warning<ApplicationBase>($"Last iteration took over 250ms, something is affecting performance a lot ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 100)
               Context.Logging.Warning<ApplicationBase>($"Last iteration took over 100ms, something is affecting performance ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 50)
               Context.Logging.Debug<ApplicationBase>($"Last iteration took over 50ms ({LastIterationDuration.TotalMilliseconds:n2}ms).");
            else if (LastIterationDuration.TotalMilliseconds > 30)
               Context.Logging.Debug<ApplicationBase>($"Last iteration took over 30ms ({LastIterationDuration.TotalMilliseconds:n2}ms).");
         }
      }

      if (Context.Logging.IsAvailable)
         Context.Logging.Trace<ApplicationBase>("Application loop stopping.");
   }
   private void Cleanup()
   {
      State = ApplicationState.Stopping;
      RaiseStopping();

      try
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Trace<ApplicationBase>($"Cleaning up application.");

         Context.Cleanup(this);
      }
      finally
      {
         State = ApplicationState.Stopped;
         RaiseStopped();
      }
   }
   #endregion

   #region Helpers
   /// <summary>Raises the application starting event.</summary>
   protected virtual void RaiseStarting() => Starting?.Invoke(this);

   /// <summary>Raises the application started event.</summary>
   protected virtual void RaiseStarted() => Started?.Invoke(this);

   /// <summary>Raises the application stopping event.</summary>
   protected virtual void RaiseStopping() => Stopping?.Invoke(this);

   /// <summary>Raises the application stopped event.</summary>
   protected virtual void RaiseStopped() => Stopped?.Invoke(this);

   /// <summary>Raises the application iteration event.</summary>
   protected virtual void RaiseIteration() => Iteration?.Invoke(this);

   private void LogInitialisationInformation()
   {
      Debug.Assert(Context.Logging.IsAvailable);

      Context.Logging.Info<ApplicationBase>($"Running application Id = ({Info.Id}), Name = ({Info.Name}), Version = ({Info.Version.DisplayName}).");

      Context.Logging.Info<ApplicationBase>($"Available context providers: {Context.ContextProviders.Count:n0}");
      foreach (IContextProvider provider in Context.ContextProviders)
      {
         Type type = provider.GetType();
         Context.Logging.Debug<ApplicationBase>($"Available context provider: {type.FullName ?? type.Name}.");
      }

      Context.Logging.Info<ApplicationBase>($"Available contexts: {Context.Contexts.Count(c => c.IsAvailable):n0}");
      foreach (IContext context in Context.Contexts)
      {
         if (context.IsAvailable is false)
            continue;

         Type type = context.GetType();
         Context.Logging.Debug<ApplicationBase>($"Available context ({context.Kind}): {type.FullName ?? type.Name}.");
      }
   }
   private void LogInitialisationOrder()
   {
      Debug.Assert(Context.Logging.IsAvailable);

      int i = 1;

      Context.Logging.Info<ApplicationBase>($"Components to initialise: {Context.InitialisationOrder.Count:n0}");
      foreach (IHasApplicationInit init in Context.InitialisationOrder)
      {
         Type type = init.GetType();
         string name = type.FullName ?? type.Name;

         Context.Logging.Trace<ApplicationBase>($"Initialisation order ({i++}): {name}");
      }
   }
   #endregion
}
