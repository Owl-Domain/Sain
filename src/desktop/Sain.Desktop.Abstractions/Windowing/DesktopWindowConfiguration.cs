namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the configuration for desktop windows.
/// </summary>
/// <param name="size">The size of the window.</param>
public struct DesktopWindowConfiguration(Size size)
{
   #region Fields
   /// <summary>The kind of the desktop window.</summary>
   public DesktopWindowKind Kind = DesktopWindowKind.Normal;

   /// <summary>The title of the window.</summary>
   /// <remarks>If left <see langword="null"/>, the title should be set to the application's name.</remarks>
   public string? Title;

   /// <summary>The mode used to determine the window's startup position.</summary>
   /// <remarks>If set to <see cref="WindowStartupPositionMode.Manual"/> then the <see cref="Position"/> should be set.</remarks>
   public WindowStartupPositionMode StartupPositionMode = WindowStartupPositionMode.CenterParent;

   /// <summary>The starting position of the window.</summary>
   /// <remarks>Ignored if the <see cref="StartupPositionMode"/> is not set to <see cref="WindowStartupPositionMode.Manual"/>.</remarks>
   public Point? Position;

   /// <summary>The size of the window.</summary>
   public Size Size = size;

   /// <summary>Whether the window can be resized.</summary>
   public bool CanResize = true;

   /// <summary>Whether the window allows transparency.</summary>
   public bool AllowsTransparency = false;

   /// <summary>Whether the window is always on top.</summary>
   public bool IsAlwaysOnTop = false;

   /// <summary>The parent window.</summary>
   public IDesktopWindow? Parent;
   #endregion
}
