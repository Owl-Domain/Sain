namespace Sain.Shared.Applications;

/// <summary>
///   Represents a delegate that is used for application events.
/// </summary>
/// <param name="application">The application that the event is being raised for.</param>
public delegate void ApplicationEventHandler(IApplication application);

/// <summary>
///   Represents a Sain application.
/// </summary>
public interface IApplication
{
   #region Properties
   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }

   /// <summary>The current state of the application.</summary>
   ApplicationState State { get; }
   #endregion

   #region Events
   /// <summary>Represents the event that is raised when the application is starting, before any initialisation has occured.</summary>
   event ApplicationEventHandler? Starting;

   /// <summary>Represents the event that is raised when the application has started, after initialisation has occured.</summary>
   event ApplicationEventHandler? Started;

   /// <summary>Represents the event that is raised when the application is stopping, before any cleanup has occured.</summary>
   event ApplicationEventHandler? Stopping;

   /// <summary>Represents the event that is raised when the application has stopped, after cleanup has occured.</summary>
   event ApplicationEventHandler? Stopped;

   /// <summary>Represents the event that is raised when the application has processed all awaiting events.</summary>
   /// <remarks>This is the event that should be used for polling any events that may be required by context providers.</remarks>
   event ApplicationEventHandler? Iteration;
   #endregion

   #region Methods
   /// <summary>Runs the application.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application is already running or it hasn't fully stopped yet.</exception>
   void Run();

   /// <summary>Stops the application.</summary>
   void Stop();
   #endregion
}
