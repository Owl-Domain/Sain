namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents the base implementation for the application's mouse input context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public abstract class BaseMouseInputContext(IContextProvider? provider) : BaseContext(provider), IMouseInputContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override string Kind => CoreContextKinds.MouseInput;

   /// <inheritdoc/>
   public abstract IDeviceCollection<IMouseDevice> Devices { get; }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public virtual void RefreshDevices()
   {
      foreach (IMouseDevice device in Devices)
         device.Refresh();
   }

   /// <inheritdoc/>
   public void RefreshDeviceIds()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshDeviceId();
   }

   /// <inheritdoc/>
   public void RefreshNames()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshName();
   }

   /// <inheritdoc/>
   public void RefreshPositions()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshPosition();
   }

   /// <inheritdoc/>
   public void RefreshButtons()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshButtons();
   }

   /// <inheritdoc/>
   public void RefreshIsCaptured()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshIsCaptured();
   }

   /// <inheritdoc/>
   public void RefreshCursorVisibility()
   {
      foreach (IMouseDevice device in Devices)
         device.RefreshIsCursorVisible();
   }
   #endregion
}
