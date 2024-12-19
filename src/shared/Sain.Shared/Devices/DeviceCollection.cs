namespace Sain.Shared.Devices;

/// <summary>
///   Represents a collection of devices.
/// </summary>
/// <typeparam name="T">The type of the devices in the collection.</typeparam>
/// <remarks>This implementation is thread-safe.</remarks>
public sealed class DeviceCollection<T> : IDeviceCollection<T>, ICollection<T>
   where T : class, IDevice
{
   #region Fields
   private readonly List<T> _collection;
   private readonly ReaderWriterLockSlim _lock = new();
   #endregion

   #region Properties
   /// <inheritdoc/>
   public int Count
   {
      get
      {
         using (_lock.ReadLock())
            return _collection.Count;
      }
   }

   /// <inheritdoc/>
   public bool IsReadOnly => false;
   #endregion

   #region Indexers
   /// <inheritdoc/>
   public T? this[Guid id] => TryGet(id);
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="DeviceCollection{T}"/>.</summary>
   public DeviceCollection() => _collection = [];

   /// <summary>Creates a new instance of the <see cref="DeviceCollection{T}"/>.</summary>
   /// <param name="devices">An enumerable of the initial devices to fill the collection with.</param>
   public DeviceCollection(IEnumerable<T> devices) => _collection = [.. devices];
   #endregion

   #region Events
   /// <inheritdoc/>
   public event NotifyCollectionChangedEventHandler? CollectionChanged;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public T? TryGet(Guid id)
   {
      using (_lock.ReadLock())
      {
         foreach (T device in _collection)
         {
            if (device.Id == id)
               return device;
         }
      }

      return default;
   }

   /// <inheritdoc/>
   public T? TryGet(IDeviceId id, out bool wasPartial)
   {
      using (_lock.ReadLock())
      {
         T? bestDevice = null;
         int bestScore = 0;

         foreach (T device in _collection)
         {
            if (device.IsMatch(id, out int score))
            {
               if (score == id.Components.Count)
               {
                  wasPartial = false;
                  return device;
               }

               if (score > bestScore)
               {
                  bestScore = score;
                  bestDevice = device;
               }
            }
         }

         wasPartial = bestDevice is not null;
         return bestDevice;
      }
   }

   /// <inheritdoc/>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="item"/> already exists in the device collection.</exception>
   public void Add(T item)
   {
      using (_lock.WriteLock())
      {
         if (_collection.Contains(item))
            throw new ArgumentException($"The given device ({item}) already exists in the device collection.", nameof(item));

         _collection.Add(item);
         CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, item));
      }
   }

   /// <inheritdoc/>
   public bool Remove(T item)
   {
      using (_lock.WriteLock())
      {
         if (_collection.Remove(item))
         {
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, item));
            return true;
         }

         return false;
      }
   }

   /// <summary>Tries to remove the device with the given <paramref name="id"/>.</summary>
   /// <param name="id">The id of the device to remove from the collection.</param>
   /// <returns>
   ///   <see langword="true"/> if a device with the given <paramref name="id"/>
   ///   was found and removed, <see langword="false"/> otherwise.
   /// </returns>
   public bool Remove(Guid id)
   {
      using (_lock.WriteLock())
      {
         for (int i = 0; i < _collection.Count; i++)
         {
            if (_collection[i].Id == id)
            {
               T device = _collection[i];
               _collection.RemoveAt(i);

               CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, device));

               return true;
            }
         }

         return false;
      }
   }

   /// <inheritdoc/>
   public void Clear()
   {
      using (_lock.WriteLock())
      {
         bool hadItems = _collection.Count > 0;
         _collection.Clear();

         if (hadItems)
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
      }
   }

   /// <inheritdoc/>
   public bool Contains(T item)
   {
      using (_lock.ReadLock())
         return _collection.Contains(item);
   }

   /// <inheritdoc/>
   public void CopyTo(T[] array, int arrayIndex)
   {
      using (_lock.ReadLock())
         _collection.CopyTo(array, arrayIndex);
   }

   /// <inheritdoc/>
   public IEnumerator<T> GetEnumerator()
   {
      // Todo(Nightowl): Optimise this with a custom iterator;
      // Note(Nightowl): Optimisation is not that high of a priority since device collections should be quite small;

      using (_lock.ReadLock())
      {
         List<T> copy = [.. _collection];
         return copy.GetEnumerator();
      }
   }
   IEnumerator IEnumerable.GetEnumerator()
   {
      // Todo(Nightowl): Optimise this with a custom iterator;
      // Note(Nightowl): Optimisation is not that high of a priority since device collections should be quite small;

      using (_lock.ReadLock())
      {
         List<T> copy = [.. _collection];
         return ((IEnumerable)copy).GetEnumerator();
      }
   }
   #endregion
}
