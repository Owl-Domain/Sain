namespace Sain.Shared.Applications;

/// <summary>
///   Represents the different life cycle states of an application.
/// </summary>
public enum ApplicationState : byte
{
   /// <summary>The application is currently stopped.</summary>
   Stopped,

   /// <summary>The application is currently starting.</summary>
   Starting,

   /// <summary>The application is currently running.</summary>
   Running,

   /// <summary>The application is currently stopping.</summary>
   Stopping,
}
