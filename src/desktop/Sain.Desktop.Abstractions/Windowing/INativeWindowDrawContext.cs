namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the drawing context used to draw to a native window.
/// </summary>
public interface INativeWindowDrawContext
{
   #region Methods
   /// <summary>Draws the entire window area in the specified <paramref name="color"/>.</summary>
   /// <param name="color">The color to draw on the entire window.</param>
   /// <remarks>This should be the first thing that is done when a window is redrawn.</remarks>
   void Clear(Color color);

   /// <summary>Draws a single pixel at the given <paramref name="position"/>, with the given <paramref name="color"/>.</summary>
   /// <param name="position">The position to draw the pixel at.</param>
   /// <param name="color">The color to draw the pixel in.</param>
   void DrawPixel(Point position, Color color);

   /// <summary>Draws a line from the given <paramref name="start"/> to the given <paramref name="end"/> position, with the given <paramref name="color"/>.</summary>
   /// <param name="start">The starting position of the line.</param>
   /// <param name="end">The ending position of the line.</param>
   /// <param name="color">The color to draw the line in.</param>
   void DrawLine(Point start, Point end, Color color);

   /// <summary>Draws a filled in <paramref name="rectangle"/>, with the given <paramref name="color"/>.</summary>
   /// <param name="rectangle">The rectangle to draw in.</param>
   /// <param name="color">The color to draw the rectangle in.</param>
   void DrawRectangleFilled(Rectangle rectangle, Color color);

   /// <summary>Draws the outline of the given <paramref name="rectangle"/>, with the given <paramref name="color"/>.</summary>
   /// <param name="rectangle">The rectangle to draw the outline of.</param>
   /// <param name="color">The color to draw the outline of the rectangle in.</param>
   void DrawRectangleOutline(Rectangle rectangle, Color color);
   #endregion
}
