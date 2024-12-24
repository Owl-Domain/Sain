namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the various states of a desktop window.
/// </summary>
public enum DesktopWindowState : byte
{
   /// <summary>The window has no special state.</summary>
   Normal,

   /// <summary>The window is minimized.</summary>
   Minimized,

   /// <summary>The window is maximized.</summary>
   Maximimzed,
}
