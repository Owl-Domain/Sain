namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the various kinds of desktop windows.
/// </summary>
public enum DesktopWindowKind : byte
{
   /// <summary>The window is a regular window.</summary>
   Normal,

   /// <summary>The window is a tooltip window.</summary>
   Tooltip,

   /// <summary>The window is a popup window.</summary>
   Popup,
}
