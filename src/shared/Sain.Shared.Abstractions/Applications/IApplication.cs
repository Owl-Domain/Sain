namespace Sain.Shared.Applications;

/// <summary>
///   Represents a delegate that is used for application events.
/// </summary>
/// <param name="application">The application that the event is being raised for.</param>
public delegate void ApplicationEventHandler(IApplicationBase application);

/// <summary>
///   Represents a delegate that is used for application events.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <param name="application">The application that the event is being raised for.</param>
public delegate void ApplicationEventHandler<TContext>(IApplication<TContext> application) where TContext : IApplicationContext;

/// <summary>
///   Represents a delegate that is used for application events.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
/// <param name="application">The application that the event is being raised for.</param>
public delegate void ApplicationEventHandler<TContext, TApplication>(TApplication application)
   where TContext : IApplicationContext
   where TApplication : IApplication<TContext, TApplication>;

/// <summary>
///   Represents the base Sain application.
/// </summary>
public interface IApplicationBase
{
   #region Properties
   /// <summary>The information about the application.</summary>
   IApplicationInfo Info { get; }

   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }

   /// <summary>The current state of the application.</summary>
   ApplicationState State { get; }

   /// <summary>The duration of the last iteration of the application.</summary>
   TimeSpan LastIterationDuration { get; }
   #endregion

   #region Events
   /// <summary>Represents the event that is raised when the application is starting, before any initialisation has occured.</summary>
   event ApplicationEventHandler? Starting;

   /// <summary>Represents the event that is raised when the application has started, after initialisation has occured.</summary>
   event ApplicationEventHandler? Started;

   /// <summary>Represents the event that is raised when the application is stopping, before any cleanup has occured.</summary>
   event ApplicationEventHandler Stopping;

   /// <summary>Represents the event that is raised when the application has stopped, after cleanup has occured.</summary>
   event ApplicationEventHandler Stopped;

   /// <summary>Represents the event that is raised when the application has processed all awaiting events.</summary>
   /// <remarks>This is the event that should be used for polling any events that may be required by context providers.</remarks>
   event ApplicationEventHandler Iteration;
   #endregion

   #region Methods
   /// <summary>Runs the application.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application is already running or it hasn't fully stopped yet.</exception>
   void Run();

   /// <summary>Stops the application.</summary>
   void Stop();
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
public interface IApplication<TContext> : IApplicationBase
   where TContext : IApplicationContext
{
   #region Properties
   /// <summary>The context of the application.</summary>
   new TContext Context { get; }
   IApplicationContext IApplicationBase.Context => Context;
   #endregion

   #region Events
   /// <summary>Represents the event that is raised when the application is starting, before any initialisation has occured.</summary>
   new event ApplicationEventHandler<TContext>? Starting;

   /// <summary>Represents the event that is raised when the application has started, after initialisation has occured.</summary>
   new event ApplicationEventHandler<TContext>? Started;

   /// <summary>Represents the event that is raised when the application is stopping, before any cleanup has occured.</summary>
   new event ApplicationEventHandler<TContext>? Stopping;

   /// <summary>Represents the event that is raised when the application has stopped, after cleanup has occured.</summary>
   new event ApplicationEventHandler<TContext>? Stopped;

   /// <summary>Represents the event that is raised when the application has processed all awaiting events.</summary>
   /// <remarks>This is the event that should be used for polling any events that may be required by context providers.</remarks>
   new event ApplicationEventHandler<TContext>? Iteration;
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
public interface IApplication<TContext, TApplication> : IApplication<TContext>
   where TContext : IApplicationContext
   where TApplication : IApplication<TContext, TApplication>
{
   #region Events
   /// <summary>Represents the event that is raised when the application is starting, before any initialisation has occured.</summary>
   new event ApplicationEventHandler<TContext, TApplication>? Starting;

   /// <summary>Represents the event that is raised when the application has started, after initialisation has occured.</summary>
   new event ApplicationEventHandler<TContext, TApplication>? Started;

   /// <summary>Represents the event that is raised when the application is stopping, before any cleanup has occured.</summary>
   new event ApplicationEventHandler<TContext, TApplication>? Stopping;

   /// <summary>Represents the event that is raised when the application has stopped, after cleanup has occured.</summary>
   new event ApplicationEventHandler<TContext, TApplication>? Stopped;

   /// <summary>Represents the event that is raised when the application has processed all awaiting events.</summary>
   /// <remarks>This is the event that should be used for polling any events that may be required by context providers.</remarks>
   new event ApplicationEventHandler<TContext, TApplication>? Iteration;
   #endregion
}

/// <summary>
///   Represents a general Sain application.
/// </summary>
public interface IApplication : IApplication<IApplicationContext, IApplication> { }
