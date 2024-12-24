namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the various possible modes for determing the startup position of a window.
/// </summary>
public enum WindowStartupPositionMode : byte
{
   /// <summary>The window should be centered on the current screen.</summary>
   CenterScreen,

   /// <summary>The window should be centered on the primary screen.</summary>
   CenterPrimaryScreen,

   /// <summary>The window should be centered on the parent window.</summary>
   CenterParent,

   /// <summary>The window's startup position is manually specified.</summary>
   Manual,
}
