namespace Sain.Desktop.Applications;

/// <summary>
///   Contains functionality for creating new desktop applications.
/// </summary>
/// <param name="info">The information about the application.</param>
/// <param name="context">The context of the application.</param>
public sealed class DesktopApplication(IApplicationInfo info, IDesktopApplicationContext context) :
   Application<IDesktopApplicationContext, IDesktopApplication>(info, context),
   IDesktopApplication
{
   #region Functions
   /// <summary>Creates a builder for a new desktop application.</summary>
   /// <returns>The desktop application builder which can be used to configure the desktop application.</returns>
   public static DesktopApplicationBuilder New() => new();
   #endregion
}
