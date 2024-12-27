namespace Sain.Shared.Display;

/// <summary>
///   Represents the base implementation for the application's display context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public abstract class BaseDisplayContext(IContextProvider? provider) : BaseContext(provider), IDisplayContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override string Kind => CoreContextKinds.Display;

   /// <inheritdoc/>
   public abstract IDeviceCollection<IDisplayDevice> Devices { get; }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public virtual void RefreshDevices()
   {
      foreach (IDisplayDevice device in Devices)
         device.Refresh();
   }

   /// <inheritdoc/>
   public void RefreshDeviceIds()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshDeviceId();
   }

   /// <inheritdoc/>
   public void RefreshNames()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshName();
   }

   /// <inheritdoc/>
   public void RefreshAreas()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshArea();
   }

   /// <inheritdoc/>
   public void RefreshBounds()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshBounds();
   }

   /// <inheritdoc/>
   public void RefreshDisplayScales()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshDisplayScale();
   }

   /// <inheritdoc/>
   public void RefreshIsPrimary()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshIsPrimary();
   }

   /// <inheritdoc/>
   public void RefreshRefreshRates()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshRefreshRate();
   }

   /// <inheritdoc/>
   public void RefreshResolutions()
   {
      foreach (IDisplayDevice device in Devices)
         device.RefreshResolution();
   }
   #endregion
}
