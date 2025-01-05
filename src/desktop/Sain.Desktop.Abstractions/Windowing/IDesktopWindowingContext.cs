namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the desktop application's context for managing windows.
/// </summary>
public interface IDesktopWindowingContext : IContext
{
   #region Properties
   /// <summary>The collection of the currently open windows.</summary>
   INativeWindowCollection<INativeWindow> Windows { get; }
   #endregion

   #region Methods
   INativeWindow CreateWindow(Point position, Size size, string title);
   #endregion
}
