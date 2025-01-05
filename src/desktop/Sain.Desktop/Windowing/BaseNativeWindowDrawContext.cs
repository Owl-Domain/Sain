namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the base implementation for the native window drawing context.
/// </summary>
public abstract class BaseNativeWindowDrawContext : INativeWindowDrawContext
{
   #region Methods
   /// <inheritdoc/>
   public abstract void Clear(Color color);

   /// <inheritdoc/>
   public abstract void DrawPixel(Point position, Color color);

   /// <inheritdoc/>
   public abstract void DrawLine(Point start, Point end, Color color);

   /// <inheritdoc/>
   public abstract void DrawRectangleFilled(Rectangle rectangle, Color color);

   /// <inheritdoc/>
   public abstract void DrawRectangleOutline(Rectangle rectangle, Color color);
   #endregion
}
