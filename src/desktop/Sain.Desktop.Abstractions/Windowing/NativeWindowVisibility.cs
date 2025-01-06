namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the different visibilities of a native window.
/// </summary>
public enum NativeWindowVisibility : byte
{
   /// <summary>The window is visible on the screen.</summary>
   Visible,

   /// <summary>The window is minimized.</summary>
   /// <remarks>This means that the window itself is not visible, but an icon on the task bar may still be visible.</remarks>
   Collapsed,

   /// <summary>The window is fully hidden.</summary>
   Hidden,
}
