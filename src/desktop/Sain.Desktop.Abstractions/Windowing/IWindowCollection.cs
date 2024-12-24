namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a collection of desktop windows.
/// </summary>
public interface IDesktopWindowCollection : IReadOnlyCollection<IDesktopWindow>, INotifyCollectionChanged
{

}
