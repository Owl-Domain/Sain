namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the application's context for keyboard input.
/// </summary>
public interface IKeyboardInputContext : IContext
{
   #region Properties
   /// <summary>A collection of the available keyboard devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IKeyboardDevice> Devices { get; }

   /// <summary>The collection of the available keyboard keys.</summary>
   /// <remarks>
   ///   This collection might not have all of the keys that the keyboard actually has, just the
   ///   ones that are recognised. This also means that there might be more keys in the collection
   ///   than the keyboard actually has, and it should therefore not be used for representing
   ///   the physical state of the keyboard, instead it should only be used for collecting input.
   /// </remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IReadOnlyCollection<IKeyboardPhysicalKeyState> Keys { get; }
   #endregion

   #region Methods
   /// <summary>Checks whether the given physical <paramref name="key"/> is currently up.</summary>
   /// <param name="key">The physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical <paramref name="key"/> is currently up, <see langword="false"/> otherwise.</returns>
   /// <remarks>The keyboard key being up means that it is not currently being pressed down.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool IsKeyUp(PhysicalKey key);

   /// <summary>Checks whether the given physical <paramref name="key"/> is currently pressed down.</summary>
   /// <param name="key">The physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical <paramref name="key"/> is currently pressed down, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool IsKeyDown(PhysicalKey key);
   #endregion

   #region Refresh methods
   /// <summary>Refresh the state of the entire context.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void Refresh();

   /// <summary>Refreshes all of the keyboard devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of all of the keyboard devices.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the names of all of the keyboard devices.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshNames();

   /// <summary>Refreshes the physical key state of the keyboard.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshKeys();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IKeyboardInputContext"/>.
/// </summary>
public static class IKeyboardInputContextExtensions
{
   #region Methods
   /// <summary>Checks whether the given physical key <paramref name="kind"/> is currently up.</summary>
   /// <param name="context">The keyboard input context to use to check the key <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical key <paramref name="kind"/> is currently up, <see langword="false"/> otherwise.</returns>
   /// <remarks>The keyboard key being up means that it is not currently being pressed down.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the <paramref name="context"/> is unavailable.</exception>
   public static bool IsKeyUp(this IKeyboardInputContext context, PhysicalKeyKind kind) => context.IsKeyUp(new(kind));

   /// <summary>Checks whether the given physical key <paramref name="kind"/> is currently pressed down.</summary>
   /// <param name="context">The keyboard input context to use to check the key <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the physical key to check.</param>
   /// <returns><see langword="true"/> if the given physical key <paramref name="kind"/> is currently pressed down, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the <paramref name="context"/> is unavailable.</exception>
   public static bool IsKeyDown(this IKeyboardInputContext context, PhysicalKeyKind kind) => context.IsKeyDown(new(kind));
   #endregion
}
