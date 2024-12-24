namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a desktop's application window.
/// </summary>
public interface IDesktopWindow : INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Properties
   /// <summary>The kind of the desktop window.</summary>
   DesktopWindowKind Kind { get; }

   /// <summary>The title of the window.</summary>
   string Title { get; set; }

   /// <summary>The position of the window.</summary>
   Point Position { get; set; }

   /// <summary>The size of the window.</summary>
   Size Size { get; set; }

   /// <summary>Whether the window can be resized.</summary>
   bool CanResize { get; set; }

   /// <summary>Whether the window is borderless.</summary>
   bool IsBorderless { get; set; }

   /// <summary>Whether the window is currently visible.</summary>
   bool IsVisible { get; set; }

   /// <summary>Whether the window allows transparency.</summary>
   bool AllowsTransparency { get; set; }

   /// <summary>The current state of the window.</summary>
   DesktopWindowState State { get; set; }

   /// <summary>Whether the window is always on top.</summary>
   bool IsAlwaysOnTop { get; set; }
   #endregion
}
