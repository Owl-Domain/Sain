namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a collection of native windows.
/// </summary>
/// <typeparam name="T">The type of the native windows to store.</typeparam>
public interface INativeWindowCollection<out T> : IReadOnlyCollection<T>, INotifyCollectionChanged
   where T : INativeWindow
{
}
