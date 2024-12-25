namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a collection of desktop windows.
/// </summary>
/// <typeparam name="T">The type of the desktop windows to store.</typeparam>
public interface IDesktopWindowCollection<out T> : IReadOnlyCollection<T>, INotifyCollectionChanged
   where T : IDesktopWindow
{
}
