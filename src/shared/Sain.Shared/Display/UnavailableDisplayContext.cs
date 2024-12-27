namespace Sain.Shared.Display;

/// <summary>
///   Represents an <see cref="IDisplayContext"/> that is always marked as unavailable.
/// </summary>
public class UnavailableDisplayContext : BaseUnavailableContext, IDisplayContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.Display;

   /// <inheritdoc/>
   public IDeviceCollection<IDisplayDevice> Devices
   {
      get
      {
         ThrowForUnavailable();
         Debug.Fail("Should never be reached.");
         return null; // Never reached but needed for the compiler.
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void RefreshDevices() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshDeviceIds() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshNames() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshResolutions() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshAreas() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshBounds() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshDisplayScales() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshRefreshRates() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshIsPrimary() => ThrowForUnavailable();
   #endregion
}
