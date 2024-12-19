namespace Sain.Shared.Devices;

/// <summary>
///   Represents a collection of devices.
/// </summary>
/// <typeparam name="T">The type of the devices in the collection.</typeparam>
public interface IDeviceCollection<out T> : IReadOnlyCollection<T>, INotifyCollectionChanged
   where T : class, IDevice
{
   #region Indexers
   /// <summary>Tries to get the device with the given <paramref name="id"/>.</summary>
   /// <param name="id">The id of the device to get.</param>
   /// <returns>
   ///   The device with the given <paramref name="id"/>, or <see langword="null"/>
   ///   if no device with the given <paramref name="id"/> was found.
   /// </returns>
   T? this[Guid id] { get; }
   #endregion

   #region Methods
   /// <summary>Tries to get the device with the given <paramref name="id"/>.</summary>
   /// <param name="id">The id of the device to get.</param>
   /// <returns>
   ///   The device with the given <paramref name="id"/>, or <see langword="null"/>
   ///   if no device coudl be found for the given <paramref name="id"/>.
   /// </returns>
   T? TryGet(Guid id);

   /// <summary>Tries to get the devices that matches the given device <paramref name="id"/> the best.</summary>
   /// <param name="id">The device id to try and find a device for.</param>
   /// <param name="wasPartial">Whether the found device was only a partial match.</param>
   /// <returns>
   ///   The device that best matched the given <paramref name="id"/>, or <see langword="null"/>
   ///   if no device could be found for the given <paramref name="id"/>.
   /// </returns>
   T? TryGet(IDeviceId id, out bool wasPartial);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IDeviceCollection{T}"/>.
/// </summary>
public static class IDeviceCollectionExtensions
{
   #region Methods
   /// <summary>Tries to get the device with the given <paramref name="id"/>.</summary>
   /// <typeparam name="T">The type of the devices in the <paramref name="collection"/>.</typeparam>
   /// <param name="collection">The device collection to look for the device in.</param>
   /// <param name="id">The id of the device to get.</param>
   /// <param name="device">The found device, or <see langword="null"/>.</param>
   /// <returns>
   ///   <see langword="true"/> if a <paramref name="device"/> with the given
   ///   <paramref name="id"/> could be found, <see langword="false"/> otherwise.
   /// </returns>
   public static bool TryGet<T>(this IDeviceCollection<T> collection, Guid id, [NotNullWhen(true)] out T? device)
      where T : class, IDevice
   {
      device = collection.TryGet(id);
      return device is not null;
   }

   /// <summary>Tries to get the devices that matches the given device <paramref name="id"/> the best.</summary>
   /// <typeparam name="T">The type of the devices in the <paramref name="collection"/>.</typeparam>
   /// <param name="collection">The device collection to look for the device in.</param>
   /// <param name="id">The device id to try and find a device for.</param>
   /// <param name="device">
   ///   The device that best matched the given <paramref name="id"/>, or <see langword="null"/>
   ///   if no device could be found for the given <paramref name="id"/>.
   /// </param>
   /// <param name="wasPartial">Whether the found device was only a partial match.</param>
   /// <returns>
   ///   <see langword="true"/> if a <paramref name="device"/> with the given
   ///   <paramref name="id"/> could be found, <see langword="false"/> otherwise.
   /// </returns>
   public static bool TryGet<T>(this IDeviceCollection<T> collection, IDeviceId id, [NotNullWhen(true)] out T? device, out bool wasPartial)
      where T : class, IDevice
   {
      device = collection.TryGet(id, out wasPartial);
      return device is not null;
   }
   #endregion
}
