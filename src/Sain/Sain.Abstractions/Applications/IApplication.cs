namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the different threads a Sain application can be ran on.
/// </summary>
public enum ApplicationRunThread : byte
{
   /// <summary>The application will run on the current thread.</summary>
   RunOnCurrentThread,

   /// <summary>The application will run on a new thread.</summary>
   RunOnNewThread,

   /// <summary>The application will run on a new background thread.</summary>
   RunOnBackgroundThread,
}

/// <summary>
///   Represents the different ways a Sain application can run in.
/// </summary>
public enum ApplicationRunMode : byte
{
   /// <summary>The application will keep running until it is manually stopped.</summary>
   UntilStopped,

   /// <summary>The application will only run for a single iteration.</summary>
   SingleIteration,
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

   /// <summary>The configuration options for the application.</summary>
   IApplicationConfiguration Configuration { get; }

   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }

   /// <summary>Whether the application is currently running.</summary>
   bool IsRunning { get; }

   /// <summary>Whether the application has been requested to be stopped.</summary>
   bool IsStopRequested { get; }

   /// <summary>The time that the application has been running for.</summary>
   TimeSpan RunTime { get; }

   /// <summary>The time it took for the last iteration to finish.</summary>
   /// <remarks>This will include the time spent waiting for <see cref="IApplicationConfiguration.MinimumIterationTime"/>.</remarks>
   TimeSpan LastIterationTime { get; }

   /// <summary>The time it took for the last iteration to finish.</summary>
   /// <remarks>This will be the actual time the main iteration logic took, without waiting for <see cref="IApplicationConfiguration.MinimumIterationTime"/>.</remarks>
   TimeSpan ActualLastIterationTime { get; }

   /// <summary>The time it took for the application to start.</summary>
   /// <remarks>This will only be available after the <see cref="Started"/> event has been raised.</remarks>
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
   /// <param name="thread">The thread to run the application on.</param>
   /// <param name="mode">The mode to run the application in.</param>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the given <paramref name="thread"/> or <paramref name="mode"/> value is unknown.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the application is already running.</exception>
   void Run(ApplicationRunThread thread, ApplicationRunMode mode);

   /// <summary>Requests for the application to stop.</summary>
   /// <remarks>If the application is not currently running then this will do nothing.</remarks>
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

/// <summary>
///   Contains various extension methods related to the <see cref="IApplication"/>.
/// </summary>
public static class IApplicationExtensions
{
   #region Methods
   /// <summary>Runs the given <paramref name="application"/> on the given <paramref name="thread"/>, with the given <paramref name="mode"/>.</summary>
   /// <param name="application">The application to run.</param>
   /// <param name="mode">The mode to run the application in.</param>
   /// <param name="thread">The thread to run the application on.</param>
   public static void Run(this IApplication application, ApplicationRunMode mode, ApplicationRunThread thread) => application.Run(thread, mode);

   /// <summary>Runs the given <paramref name="application"/> on the given <paramref name="thread"/>, until it is manually stopped.</summary>
   /// <param name="application">The application to run.</param>
   /// <param name="thread">The thread to run the application on.</param>
   public static void Run(this IApplication application, ApplicationRunThread thread) => application.Run(thread, ApplicationRunMode.UntilStopped);

   /// <summary>Runs the given <paramref name="application"/> on the current thread, with the given <paramref name="mode"/>.</summary>
   /// <param name="application">The application to run.</param>
   /// <param name="mode">The mode to run the application in.</param>
   public static void Run(this IApplication application, ApplicationRunMode mode) => application.Run(ApplicationRunThread.RunOnCurrentThread, mode);

   /// <summary>Runs the given <paramref name="application"/> on the current thread, until it is manually stopped.</summary>
   /// <param name="application">The application to run.</param>
   public static void Run(this IApplication application) => application.Run(ApplicationRunThread.RunOnCurrentThread, ApplicationRunMode.UntilStopped);
   #endregion
}
