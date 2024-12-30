namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents the application's context for mouse input.
/// </summary>
public interface IMouseInputContext : IContext, INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Properties
   /// <summary>A collection of the available mouse devices.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IDeviceCollection<IMouseDevice> Devices { get; }

   /// <summary>The collection of the available mouse buttons.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   IReadOnlyCollection<IMouseButtonState> Buttons { get; }

   /// <summary>The position of the mouse in the virtual screen space.</summary>
   /// <remarks>
   ///   Depending on the implementation, changes to the mouse position in the virtual screen
   ///   space might not be automatically reported, and the position must be manually requested.
   /// </remarks>
   /// <exception cref="NotSupportedException">
   ///   Thrown if setting the mouse position is not supported, if you want to
   ///   check for this you should use <see cref="TrySetGlobalPosition(Point)"/> instead.
   /// </exception>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   Point GlobalPosition { get; set; }

   /// <summary>The position of the mouse relative to the active window.</summary>
   /// <exception cref="NotSupportedException">
   ///   Thrown if setting the mouse position is not supported, if you want to
   ///   check for this you should use <see cref="TrySetLocalPosition(Point)"/> instead.
   /// </exception>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   Point LocalPosition { get; set; }

   /// <summary>Whether the mouse events are currently being captured.</summary>
   /// <remarks>Capturing mouse events means that only the current application (and only the window that currently has mouse focus) will receive mouse events.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool IsCaptured { get; }

   /// <summary>Whether the mouse cursor is currently visible.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool IsCursorVisible { get; }
   #endregion

   #region Events
   /// <summary>Raised whenever the mouse is moved.</summary>
   /// <remarks>
   ///   Depending on the implementation, this event might only be raised when the mouse is moved
   ///   over an active window, and not just when it's moved in the virtual screen space.
   /// </remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   event MouseMoveEventHandler? MouseMoved;

   /// <summary>Raised whenever a mouse button is released.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   event MouseButtonEventHandler? MouseButtonUp;

   /// <summary>Raised whenever a mouse button is pressed down.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   event MouseButtonEventHandler? MouseButtonDown;

   /// <summary>Raised whenever a mouse wheel is scrolled.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   event MouseWheelScrollEventHandler? MouseWheelScrolled;
   #endregion

   #region Methods
   /// <summary>Tries to set the mouse position in the virtual screen space.</summary>
   /// <param name="position">The position to move the mouse to.</param>
   /// <returns><see langword="true"/> if setting the mouse position was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool TrySetGlobalPosition(Point position);

   /// <summary>Tries to set the mouse position relative to the active window..</summary>
   /// <param name="position">The position to move the mouse to.</param>
   /// <returns><see langword="true"/> if setting the mouse position was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool TrySetLocalPosition(Point position);

   /// <summary>Starts capturing mouse events from this mouse device.</summary>
   /// <returns><see langword="true"/> if capturing was successfully enabled, <see langword="false"/> otherwise.</returns>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool StartCapture();

   /// <summary>stops capturing mouse events from this mouse device.</summary>
   /// <returns><see langword="true"/> if capturing was successfully disabled, <see langword="false"/> otherwise.</returns>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool StopCapture();

   /// <summary>Makes the mouse cursor visible.</summary>
   /// <returns><see langword="true"/> if making the mouse cursor visible was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool ShowCursor();

   /// <summary>Makes the mouse cursor invisible.</summary>
   /// <returns><see langword="true"/> if making the mouse cursor invisible was successful, <see langword="false"/> otherwise.</returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   bool HideCursor();

   /// <summary>Checks whether the given mouse <paramref name="button"/> is supported.</summary>
   /// <param name="button">The mouse button to check.</param>
   /// <returns><see langword="true"/> if the given mouse <paramref name="button"/> is supported, <see langword="false"/> otherwise.</returns>
   bool IsSupported(MouseButton button);

   /// <summary>Checks whether the given mouse <paramref name="button"/> is currently up.</summary>
   /// <param name="button">The mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse <paramref name="button"/> is
   ///   currently up, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>The mouse button being up means that it is not currently being pressed down.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   /// <exception cref="ArgumentException">Thrown if the given mouse <paramref name="button"/> is not supported.</exception>
   bool IsButtonUp(MouseButton button);

   /// <summary>Checks whether the given mouse <paramref name="button"/> is currently pressed down.</summary>
   /// <param name="button">The mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse <paramref name="button"/> is
   ///   currently pressed down, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   /// <exception cref="ArgumentException">Thrown if the given mouse <paramref name="button"/> is not supported.</exception>
   bool IsButtonDown(MouseButton button);
   #endregion

   #region Refresh methods
   /// <summary>Refresh the state of the entire context.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void Refresh();

   /// <summary>Refreshes all of the mouse devices.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDevices();

   /// <summary>Refreshes the device ids of all of the mouse devices.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshDeviceIds();

   /// <summary>Refreshes the names of all of the mouse devices.</summary>
   ///  <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshNames();

   /// <summary>Refreshes the global position of the mouse.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshGlobalPosition();

   /// <summary>Refreshes the local position of the mouse.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshLocalPosition();

   /// <summary>Refreshes both the local and global positions of the mouse.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshPositions();

   /// <summary>Refreshes the button state of the mouse.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshButtons();

   /// <summary>Refreshes the capture state of the mouse.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshIsCaptured();

   /// <summary>Refreshes the visibility of the mouse cursor.</summary>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   void RefreshIsCursorVisible();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IMouseInputContext"/>.
/// </summary>
public static class IMouseInputContextExtensions
{
   #region Methods
   /// <summary>Sets the visibility of the mouse cursor.</summary>
   /// <param name="context">The mouse input context to set the visibility of the cursor for.</param>
   /// <param name="isVisible">Whether the mouse cursor should be visible.</param>
   /// <returns>
   ///   <see langword="true"/> if the visibility state was successfully set to the
   ///   given <paramref name="isVisible"/> value, <see langword="false"/> otherwise.
   /// </returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   public static bool SetCursorVisibility(this IMouseInputContext context, bool isVisible) => isVisible ? context.ShowCursor() : context.HideCursor();

   /// <summary>Checks whether the given mouse button <paramref name="kind"/> is currently up.</summary>
   /// <param name="context">The mouse input context to check the button <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently up, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>The mouse button being up means that it is not currently being pressed down.</remarks>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   public static bool IsButtonUp(this IMouseInputContext context, MouseButtonKind kind) => context.IsButtonUp(new(kind));

   /// <summary>Checks whether the given mouse button <paramref name="kind"/> is currently pressed down.</summary>
   /// <param name="context">The mouse input context to check the button <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently pressed down, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <exception cref="InvalidOperationException">Might be thrown if accessed when the context is unavailable.</exception>
   public static bool IsButtonDown(this IMouseInputContext context, MouseButtonKind kind) => context.IsButtonDown(new(kind));
   #endregion
}
