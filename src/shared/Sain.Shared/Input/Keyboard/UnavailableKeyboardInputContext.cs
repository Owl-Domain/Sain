namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents an <see cref="IKeyboardInputContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableKeyboardInputContext : BaseUnavailableContext, IKeyboardInputContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.KeyboardInput;

   /// <inheritdoc/>
   public IDeviceCollection<IKeyboardDevice> Devices => ThrowForUnavailable<IDeviceCollection<IKeyboardDevice>>();

   /// <inheritdoc/>
   public IReadOnlyCollection<IPhysicalKeyboardKeyState> Keys => ThrowForUnavailable<IReadOnlyCollection<IPhysicalKeyboardKeyState>>();
   #endregion

   #region Events
   /// <inheritdoc/>
   public event KeyboardKeyEventHandler? KeyboardKeyUp
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public event KeyboardKeyEventHandler? KeyboardKeyDown
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool IsKeyUp(PhysicalKey key) => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool IsKeyDown(PhysicalKey key) => ThrowForUnavailable<bool>();
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   public void Refresh() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshDevices() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshDeviceIds() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshNames() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshKeys() => ThrowForUnavailable();
   #endregion
}
