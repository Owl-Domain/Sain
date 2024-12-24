namespace Sain.Desktop.Applications;

/// <summary>
///   Represents the shutdown mode of a desktop application.
/// </summary>
public enum DesktopApplicationShutdownMode : byte
{
   /// <summary>The desktop application should close when the last window has been closed.</summary>
   OnLastWindowClose,

   /// <summary>The desktop application should only close when explicitly shutdown by calling <see cref="IApplication.Stop"/>.</summary>
   OnExplicitShutdown,
}
