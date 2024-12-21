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
   IReadOnlyCollection<IMouseButton> Buttons { get; }

   /// <summary>The horizontal position of the mouse in the virtual screen space.</summary>
   double X { get; }

   /// <summary>The vertical position of the mouse in the virtual screen space.</summary>
   double Y { get; }

   /// <summary>Whether the mouse events for this mouse device are captured.</summary>
   /// <remarks>Capturing mouse events means that only the current application (and only the window that currently has mouse focus) will receive mouse events.</remarks>
   bool IsCaptured { get; }
   #endregion

   #region Methods
   /// <summary>Starts capturing mouse events from this mouse device.</summary>
   /// <returns><see langword="true"/> if capturing was successfully enabled, <see langword="false"/> otherwise.</returns>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   bool StartCapture();

   /// <summary>stops capturing mouse events from this mouse device.</summary>
   /// <remarks>Capturing mouse events means that only the current application will receive mouse events.</remarks>
   void StopCapture();

   /// <summary>Checks whether the given button <paramref name="kind"/> is currently up.</summary>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently up, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>The mouse button being up means that it is not currently being pressed down.</remarks>
   bool IsButtonUp(MouseButtonKind kind);

   /// <summary>Checks whether the given button <paramref name="kind"/> is currently pressed down.</summary>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns>
   ///   <see langword="true"/> if the given mouse button <paramref name="kind"/> is
   ///   currently pressed down, <see langword="false"/>, <see langword="false"/> otherwise.
   /// </returns>
   bool IsButtonDown(MouseButtonKind kind);
   #endregion
}
