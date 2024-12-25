
namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents a collection of desktop windows.
/// </summary>
public sealed class DesktopWindowCollection<T> : IDesktopWindowCollection<T>, IReadOnlyCollection<T>
   where T : IDesktopWindow
{
   #region Fields
   private readonly List<T> _windows = [];
   #endregion

   #region Properties
   /// <inheritdoc/>
   public int Count => _windows.Count;

   /// <inheritdoc/>
   public bool IsReadOnly => false;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event NotifyCollectionChangedEventHandler? CollectionChanged;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Add(T item)
   {
      _windows.Add(item);
      CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, item));
   }

   /// <inheritdoc/>
   public bool Contains(T item) => _windows.Contains(item);

   /// <inheritdoc/>
   public void CopyTo(T[] array, int arrayIndex) => _windows.CopyTo(array, arrayIndex);

   /// <inheritdoc/>
   public bool Remove(T item)
   {
      if (_windows.Remove(item))
      {
         CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, item));
         return true;
      }

      return false;
   }

   /// <inheritdoc/>
   public void Clear()
   {
      _windows.Clear();
      CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
   }

   /// <inheritdoc/>
   public IEnumerator<T> GetEnumerator() => _windows.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_windows).GetEnumerator();
   #endregion
}
