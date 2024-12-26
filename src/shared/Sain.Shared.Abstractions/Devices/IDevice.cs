namespace Sain.Shared.Devices;

/// <summary>
///   Represents information about a device.
/// </summary>
public interface IDevice : INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Properties
   /// <summary>The unique id for the device.</summary>
   /// <remarks>
   ///   This id will be unique across the entire application, but it will not be persisted
   ///   across application sessions and should therefore only be used for quick lookups.
   /// </remarks>
   Guid Id { get; }

   /// <summary>The unique id for the device.</summary>
   /// <remarks>
   ///   This id is only guaranteed to be unique among the devices of the same type, and it will
   ///   be persisted across application sessions, however it might not always be a full match
   ///   therefore <see cref="IsMatch(IDeviceId, out int)"/> should be used for equality checks.
   ///   It is also possible for the device id to change at runtime, so any changes should be
   ///   reacted to and persisted.
   /// </remarks>
   IDeviceId DeviceId { get; }

   /// <summary>The friendly display name for the device.</summary>
   string Name { get; }
   #endregion

   #region Methods
   /// <summary>Checks whether the device matches the given <paramref name="id"/>.</summary>
   /// <param name="id">The device id to check.</param>
   /// <param name="score">The score assigned to the match, higher is better.</param>
   /// <returns>
   ///   <see langword="true"/> if the device matches the given device
   ///   <paramref name="id"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>
   ///   The device with the highest <paramref name="score"/> should be considered the expected device,
   ///   and the saved id should be updated to match the new <see cref="DeviceId"/>. It is also possible
   ///   for multiple devices to get the same highest <paramref name="score"/>, determining which device
   ///   to use in that case is left up to the programmer, as the best approach will be application specific.
   /// </remarks>
   bool IsMatch(IDeviceId id, out int score);

   /// <summary>Refreshes all of the information about the device.</summary>
   void Refresh();

   /// <summary>Refreshes the device id of the device.</summary>
   void RefreshDeviceId();

   /// <summary>Refreshes the name of the device.</summary>
   void RefreshName();
   #endregion
}
