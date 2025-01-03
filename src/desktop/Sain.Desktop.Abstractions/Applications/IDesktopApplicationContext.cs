namespace Sain.Desktop.Applications;

/// <summary>
///   Represents the context of a desktop application.
/// </summary>
public interface IDesktopApplicationContext : IApplicationContext
{
   #region Properties
   /// <summary>The shutdown mode of the desktop application.</summary>
   DesktopApplicationShutdownMode ShutdownMode { get; }

   /// <summary>The type of the window to open when the application starts.</summary>
   /// <remarks>Opening this window should be handled by the selected UI framework.</remarks>
   Type? StartupWindowType { get; }
   #endregion
}
