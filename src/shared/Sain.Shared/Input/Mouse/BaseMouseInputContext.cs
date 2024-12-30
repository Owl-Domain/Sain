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

   /// <inheritdoc/>
   public abstract IReadOnlyCollection<IMouseButtonState> Buttons { get; }

   /// <inheritdoc/>
   public abstract Point GlobalPosition { get; set; }

   /// <inheritdoc/>
   public abstract Point LocalPosition { get; set; }

   /// <inheritdoc/>
   public abstract bool IsCaptured { get; }

   /// <inheritdoc/>
   public abstract bool IsCursorVisible { get; }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event MouseMoveEventHandler? MouseMoved;

   /// <inheritdoc/>
   public event MouseButtonEventHandler? MouseButtonUp;

   /// <inheritdoc/>
   public event MouseButtonEventHandler? MouseButtonDown;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public abstract bool TrySetGlobalPosition(Point position);

   /// <inheritdoc/>
   public abstract bool TrySetLocalPosition(Point position);

   /// <inheritdoc/>
   public abstract bool StartCapture();

   /// <inheritdoc/>
   public abstract bool StopCapture();

   /// <inheritdoc/>
   public abstract bool ShowCursor();

   /// <inheritdoc/>
   public abstract bool HideCursor();

   /// <inheritdoc/>
   public abstract bool IsSupported(MouseButton button);

   /// <inheritdoc/>
   public abstract bool IsButtonUp(MouseButton button);

   /// <inheritdoc/>
   public abstract bool IsButtonDown(MouseButton button);

   /// <summary>Raises the <see cref="MouseMoved"/> event.</summary>
   /// <param name="globalPosition">The new global position of the mouse device.</param>
   /// <param name="localPosition">The new local position of the mouse device.</param>
   protected void RaiseMouseMoved(Point localPosition, Point globalPosition)
   {
      MouseMoved?.Invoke(this, new(localPosition, globalPosition));
   }

   /// <summary>Raises the <see cref="MouseButtonUp"/> event.</summary>
   /// <param name="button">The mouse button that the event is for.</param>
   /// <param name="name">The name of the mouse button.</param>
   protected void RaiseMouseButtonUp(MouseButton button, string name)
   {
      MouseButtonUp?.Invoke(this, new(button, name, false));
   }

   /// <summary>Raises the <see cref="MouseButtonDown"/> event.</summary>
   /// <param name="button">The mouse button that the event is for.</param>
   /// <param name="name">The name of the mouse button.</param>
   protected void RaiseMouseButtonDown(MouseButton button, string name)
   {
      MouseButtonUp?.Invoke(this, new(button, name, true));
   }
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   public virtual void Refresh()
   {
      RefreshDevices();
      RefreshPositions();
      RefreshButtons();
      RefreshIsCaptured();
      RefreshIsCursorVisible();
   }

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
   public abstract void RefreshGlobalPosition();

   /// <inheritdoc/>
   public abstract void RefreshLocalPosition();

   /// <inheritdoc/>
   public virtual void RefreshPositions()
   {
      RefreshGlobalPosition();
      RefreshLocalPosition();
   }

   /// <inheritdoc/>
   public abstract void RefreshButtons();

   /// <inheritdoc/>
   public abstract void RefreshIsCaptured();

   /// <inheritdoc/>
   public abstract void RefreshIsCursorVisible();
   #endregion
}
