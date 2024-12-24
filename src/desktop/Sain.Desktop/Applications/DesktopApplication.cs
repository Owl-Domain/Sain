namespace Sain.Desktop.Applications;

/// <summary>
///   Contains functionality for creating new desktop applications.
/// </summary>
public static class DesktopApplication
{
   #region Functions
   /// <summary>Creates a builder for a new desktop application.</summary>
   /// <returns>The desktop application builder which can be used to configure the desktop application.</returns>
   public static DesktopApplicationBuilder New() => new();
   #endregion
}
