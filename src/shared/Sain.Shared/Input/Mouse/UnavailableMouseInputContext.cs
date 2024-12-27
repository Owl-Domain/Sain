namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents an <see cref="IMouseInputContext"/> that is always marked as unavailable.
/// </summary>
public sealed class UnavailableMouseInputContext : BaseUnavailableContext, IMouseInputContext
{
   #region Properties
   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.MouseInput;

   /// <inheritdoc/>
   public IDeviceCollection<IMouseDevice> Devices
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
   public void RefreshPositions() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshButtons() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshIsCaptured() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshCursorVisibility() => ThrowForUnavailable();
   #endregion
}
