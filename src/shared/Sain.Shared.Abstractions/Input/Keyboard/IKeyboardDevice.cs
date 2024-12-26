namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents information about a keyboard device.
/// </summary>
public interface IKeyboardDevice : IDevice
{
   #region Properties
   /// <summary>The collection of the available keyboard keys.</summary>
   /// <remarks>
   ///   This collection might not have all of the keys that the keyboard actually has, just the
   ///   ones that are recognised. This also means that there might be more keys in the collection
   ///   than the keyboard actually has, and it should therefore not be used for representing
   ///   the physical state of the keyboard, instead it should only be used for collecting input.
   /// </remarks>
   IReadOnlyCollection<IKeyboardKeyState> Keys { get; }
   #endregion

   #region Methods
   /// <summary>Checks whether the given physical <paramref name="key"/> is currently up.</summary>
   /// <param name="key">The physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical <paramref name="key"/> is currently up, <see langword="false"/> otherwise.</returns>
   /// <remarks>The keyboard key being up means that it is not currently being pressed down.</remarks>
   bool IsKeyUp(PhysicalKey key);

   /// <summary>Checks whether the given physical <paramref name="key"/> is currently pressed down.</summary>
   /// <param name="key">The physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical <paramref name="key"/> is currently pressed down, <see langword="false"/> otherwise.</returns>
   bool IsKeyDown(PhysicalKey key);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IKeyboardDevice"/>.
/// </summary>
public static class IKeyboardDeviceExtensions
{
   #region Methods
   /// <summary>Checks whether the given physical key <paramref name="kind"/> is currently up.</summary>
   /// <param name="device">The keyboard device to check the key <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical key <paramref name="kind"/> is currently up, <see langword="false"/> otherwise.</returns>
   /// <remarks>The keyboard key being up means that it is not currently being pressed down.</remarks>
   public static bool IsKeyUp(this IKeyboardDevice device, PhysicalKeyKind kind) => device.IsKeyUp(new(kind));

   /// <summary>Checks whether the given physical key <paramref name="kind"/> is currently pressed down.</summary>
   /// <param name="device">The keyboard device to check the key <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical key <paramref name="kind"/> is currently pressed down, <see langword="false"/> otherwise.</returns>
   public static bool IsKeyDown(this IKeyboardDevice device, PhysicalKeyKind kind) => device.IsKeyDown(new(kind));
   #endregion
}
