
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
   public IDeviceCollection<IMouseDevice> Devices => ThrowForUnavailable<IDeviceCollection<IMouseDevice>>();

   /// <inheritdoc/>
   public IReadOnlyCollection<IMouseButtonState> Buttons => ThrowForUnavailable<IReadOnlyCollection<IMouseButtonState>>();

   /// <inheritdoc/>
   public Point GlobalPosition
   {
      get => ThrowForUnavailable<Point>();
      set => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public Point LocalPosition
   {
      get => ThrowForUnavailable<Point>();
      set => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public bool IsCaptured => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool IsCursorVisible => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public event MouseMoveEventHandler? MouseMoved
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public event MouseButtonEventHandler? MouseButtonUp
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public event MouseButtonEventHandler? MouseButtonDown
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }

   /// <inheritdoc/>
   public event MouseWheelScrollEventHandler? MouseWheelScrolled
   {
      add => ThrowForUnavailable();
      remove => ThrowForUnavailable();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool TrySetGlobalPosition(Point position) => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool TrySetLocalPosition(Point position) => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool StartCapture() => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool StopCapture() => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool ShowCursor() => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool HideCursor() => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool IsSupported(MouseButton button) => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool IsButtonUp(MouseButton button) => ThrowForUnavailable<bool>();

   /// <inheritdoc/>
   public bool IsButtonDown(MouseButton button) => ThrowForUnavailable<bool>();
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
   public void RefreshPositions() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshButtons() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshIsCaptured() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshGlobalPosition() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshLocalPosition() => ThrowForUnavailable();

   /// <inheritdoc/>
   public void RefreshIsCursorVisible() => ThrowForUnavailable();
   #endregion
}
