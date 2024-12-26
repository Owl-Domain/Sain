namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents information about a mouse device.
/// </summary>
public interface IMouseDevice : IDevice
{
   #region Properties
   /// <summary>The collection of the available mouse buttons.</summary>
   /// <remarks>
   ///   This collection might not have all of the buttons that the mouse actually has, just
   ///   the ones that are recognised. This also means that there might be more buttons in the
   ///   collection than the mouse actually has, and it should therefore not be used for representing
   ///   the physical state of the mouse, instead it should only be used for collecting input.
   /// </remarks>
   IReadOnlyCollection<IMouseButtonState> Buttons { get; }

   /// <summary>The position of the mouse in the virtual screen space.</summary>
   /// <exception cref="NotSupportedException">
   ///   Thrown if setting the mouse position is not supported, if you want to
   ///   check for this you should use <see cref="TrySetPosition(Point)"/> instead.
   /// </exception>
   Point Position { get; set; }

   /// <summary>Whether the mouse events for this mouse device are captured.</summary>
   /// <remarks>Capturing mouse events means that only the current application (and only the window that currently has mouse focus) will receive mouse events.</remarks>
   bool IsCaptured { get; }

   /// <summary>Whether the mouse cursor is currently visible.</summary>
   bool IsCursorVisible { get; }
   #endregion

   #region Methods
   /// <summary>Tries to set the mouse position in the virtual screen space.</summary>
   /// <param name="position">The position to move the mouse to.</param>
   /// <returns><see langword="true"/> if setting the mouse position was successful, <see langword="false"/> otherwise.</returns>
   bool TrySetPosition(Point position);

   /// <summary>Starts capturing mouse events from this mouse device.</summary>
   /// <returns><see langword="true"/> if capturing was successfully enabled, <see langword="false"/> otherwise.</returns>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   bool StartCapture();

   /// <summary>stops capturing mouse events from this mouse device.</summary>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   void StopCapture();

   /// <summary>Makes the mouse cursor visible.</summary>
   /// <returns><see langword="true"/> if making the mouse cursor visible was successful, <see langword="false"/> otherwise.</returns>
   bool ShowCursor();

   /// <summary>Makes the mouse cursor invisible.</summary>
   /// <returns><see langword="true"/> if making the mouse cursor invisible was successful, <see langword="false"/> otherwise.</returns>
   bool HideCursor();

   /// <summary>Checks whether the given mouse <paramref name="button"/> is currently up.</summary>
   /// <param name="button">The mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse <paramref name="button"/> is
   ///   currently up, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>The mouse button being up means that it is not currently being pressed down.</remarks>
   bool IsButtonUp(MouseButton button);

   /// <summary>Checks whether the given mouse <paramref name="button"/> is currently pressed down.</summary>
   /// <param name="button">The mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse <paramref name="button"/> is
   ///   currently pressed down, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   bool IsButtonDown(MouseButton button);

   /// <summary>Refreshes the position of the mouse.</summary>
   void RefreshPosition();

   /// <summary>Refreshes the visibility of the mouse cursor.</summary>
   void RefreshIsCursorVisible();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IMouseDevice"/>.
/// </summary>
public static class IMouseDeviceExtensions
{
   #region Methods
   /// <summary>Sets the visible of the cursor for the given <paramref name="mouse"/>.</summary>
   /// <param name="mouse">The mouse device to set the visibility of the cursor for.</param>
   /// <param name="isVisible">Whether the mouse cursor should be visible.</param>
   /// <returns>
   ///   <see langword="true"/> if the visibility state was successfully set to the
   ///   given <paramref name="isVisible"/> value, <see langword="false"/> otherwise.
   /// </returns>
   public static bool SetCursorVisibility(this IMouseDevice mouse, bool isVisible) => isVisible ? mouse.ShowCursor() : mouse.HideCursor();

   /// <summary>Checks whether the given mouse button <paramref name="kind"/> is currently up.</summary>
   /// <param name="device">The mouse device to check the button <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently up, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>The mouse button being up means that it is not currently being pressed down.</remarks>
   public static bool IsButtonUp(this IMouseDevice device, MouseButtonKind kind) => device.IsButtonUp(new(kind));

   /// <summary>Checks whether the given mouse button <paramref name="kind"/> is currently pressed down.</summary>
   /// <param name="device">The mouse device to check the button <paramref name="kind"/> on.</param>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently pressed down, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsButtonDown(this IMouseDevice device, MouseButtonKind kind) => device.IsButtonDown(new(kind));
   #endregion
}
