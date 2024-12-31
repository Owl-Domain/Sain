namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the base implementation for the application's keyboard input context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public abstract class BaseKeyboardInputContext(IContextProvider? provider) : BaseContext(provider), IKeyboardInputContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override string Kind => CoreContextKinds.KeyboardInput;

   /// <inheritdoc/>
   public abstract IDeviceCollection<IKeyboardDevice> Devices { get; }

   /// <inheritdoc/>
   public abstract IReadOnlyCollection<IPhysicalKeyboardKeyState> Keys { get; }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event KeyboardKeyEventHandler? KeyboardKeyUp;

   /// <inheritdoc/>
   public event KeyboardKeyEventHandler? KeyboardKeyDown;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public abstract bool IsKeyUp(PhysicalKey key);

   /// <inheritdoc/>
   public abstract bool IsKeyDown(PhysicalKey key);

   /// <summary>Raises the <see cref="KeyboardKeyUp"/> event.</summary>
   /// <param name="physicalKey">The physical representation of the key.</param>
   /// <param name="physicalKeyName">The name of the <paramref name="physicalKey"/>.</param>
   /// <param name="virtualKey">The virtual representation of the key.</param>
   /// <param name="virtualKeyName">The name of the <paramref name="virtualKey"/>.</param>
   /// <param name="modifiers">Any of the used key modifiers.</param>
   /// <param name="isRepeat">Whether the key event is a repeat event.</param>
   protected void RaiseKeyboardKeyUp(
      PhysicalKey physicalKey,
      string physicalKeyName,
      VirtualKey virtualKey,
      string virtualKeyName,
      KeyModifiers modifiers,
      bool isRepeat = false) // Note(Nightowl): Is it even possible to have a repeated up event?
   {
      KeyboardKeyUp?.Invoke(this, new(physicalKey, physicalKeyName, virtualKey, virtualKeyName, modifiers, false, isRepeat));
   }

   /// <summary>Raises the <see cref="KeyboardKeyUp"/> event.</summary>
   /// <param name="physicalKey">The physical representation of the key.</param>
   /// <param name="physicalKeyName">The name of the <paramref name="physicalKey"/>.</param>
   /// <param name="virtualKey">The virtual representation of the key.</param>
   /// <param name="virtualKeyName">The name of the <paramref name="virtualKey"/>.</param>
   /// <param name="modifiers">Any of the used key modifiers.</param>
   /// <param name="isRepeat">Whether the key event is a repeat event.</param>
   protected void RaiseKeyboardKeyDown(
      PhysicalKey physicalKey,
      string physicalKeyName,
      VirtualKey virtualKey,
      string virtualKeyName,
      KeyModifiers modifiers,
      bool isRepeat)
   {
      KeyboardKeyDown?.Invoke(this, new(physicalKey, physicalKeyName, virtualKey, virtualKeyName, modifiers, true, isRepeat));
   }
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   public virtual void Refresh()
   {
      RefreshDevices();
      RefreshKeys();
   }

   /// <inheritdoc/>
   public virtual void RefreshDevices()
   {
      foreach (IKeyboardDevice device in Devices)
         device.Refresh();
   }

   /// <inheritdoc/>
   public void RefreshDeviceIds()
   {
      foreach (IKeyboardDevice device in Devices)
         device.RefreshDeviceId();
   }

   /// <inheritdoc/>
   public void RefreshNames()
   {
      foreach (IKeyboardDevice device in Devices)
         device.RefreshName();
   }

   /// <inheritdoc/>
   public abstract void RefreshKeys();
   #endregion
}
