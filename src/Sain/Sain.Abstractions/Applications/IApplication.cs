namespace Sain.Applications;

/// <summary>
///   Represents the different ways a Sain application can be ran.
/// </summary>
public enum ApplicationRunMode : byte
{
   /// <summary>Runs the application on the current thread.</summary>
   RunOnCurrentThread,

   /// <summary>Runs the application on the current thread, but only for a single iteration.</summary>
   RunSingleIterationOnCurrentThread,

   /// <summary>Runs the application on a new thread.</summary>
   RunOnNewThread,
}

/// <summary>
///   Represents the event handler for application events.
/// </summary>
/// <param name="application">The application that raised the event.</param>
public delegate void ApplicationEventHandler(IApplication application);

/// <summary>
///   Represents a Sain application.
/// </summary>
public interface IApplication
{
   #region Properties
   /// <summary>The information about the application.</summary>
   IApplicationInfo Info { get; }

   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }

   /// <summary>Whether the application is currently running.</summary>
   bool IsRunning { get; }

   /// <summary>Whether the application has been requested to be stopped.</summary>
   bool IsStopRequested { get; }

   /// <summary>The minimum time that a single application iteration should last.</summary>
   TimeSpan MinimumIterationTime { get; }

   /// <summary>The time that the application has been running for.</summary>
   TimeSpan RunTime { get; }

   /// <summary>The time it took for the last iteration to finish.</summary>
   TimeSpan IterationTime { get; }

   /// <summary>The time it took for the application to start.</summary>
   TimeSpan StartupTime { get; }
   #endregion

   #region Events
   /// <summary>Raised when the application is starting.</summary>
   event ApplicationEventHandler? Starting;

   /// <summary>Raised when the application has started.</summary>
   event ApplicationEventHandler? Started;

   /// <summary>Raised when the application is stopping.</summary>
   event ApplicationEventHandler? Stopping;

   /// <summary>Raised when the application has stopped.</summary>
   event ApplicationEventHandler? Stopped;

   /// <summary>Raised for every iteration of the application.</summary>
   event ApplicationEventHandler? Iteration;
   #endregion

   #region Methods
   /// <summary>Runs the application with the given run <paramref name="mode"/>.</summary>
   /// <param name="mode">The mode to run the application with.</param>
   /// <exception cref="InvalidOperationException">Thrown if the application is already running.</exception>
   void Run(ApplicationRunMode mode = ApplicationRunMode.RunOnCurrentThread);

   /// <summary>Requests for the application to stop.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application is not running.</exception>
   void Stop();
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
public interface IApplication<TContext> : IApplication
   where TContext : notnull, IApplicationContext
{
   #region Properties
   /// <summary>The context of the application.</summary>
   new TContext Context { get; }
   IApplicationContext IApplication.Context => Context;
   #endregion
}
