namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the desktop application's context for managing windows.
/// </summary>
public interface IDesktopWindowingContext : IContext
{
   #region Properties
   /// <summary>The collection of the currently open windows.</summary>
   IDesktopWindowCollection Windows { get; }
   #endregion

   #region Methods
   /// <summary>Creates a new desktop window following the given <paramref name="configuration"/>.</summary>
   /// <param name="configuration">The configuration to use to create the new window.</param>
   /// <returns>The created window.</returns>
   IDesktopWindow CreateWindow(DesktopWindowConfiguration configuration);
   #endregion
}
