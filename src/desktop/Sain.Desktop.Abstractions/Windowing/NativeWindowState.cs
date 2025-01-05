namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the different states of a desktop window.
/// </summary>
public enum NativeWindowState : byte
{
   /// <summary>The window is in it's normal state.</summary>
   Normal,

   /// <summary>The window is in it's minimized state.</summary>
   Minimized,

   /// <summary>The window is in it's maximized state.</summary>
   Maximized,
}
