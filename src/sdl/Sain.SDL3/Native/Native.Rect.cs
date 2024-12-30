namespace Sain.SDL3;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_Rect(int x, int y, int width, int height)
{
   #region Fields
   public readonly int X = x;
   public readonly int Y = y;
   public readonly int Width = width;
   public readonly int Height = height;
   #endregion

   #region Operators
   public static implicit operator SDL3_Rect(Rectangle rectangle)
   {
      return new(
         (int)rectangle.X,
         (int)rectangle.Y,
         (int)rectangle.Width,
         (int)rectangle.Height);
   }
   #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_FRect(float x, float y, float width, float height)
{
   #region Fields
   public readonly float X = x;
   public readonly float Y = y;
   public readonly float Width = width;
   public readonly float Height = height;
   #endregion

   #region Operators
   public static implicit operator SDL3_FRect(Rectangle rectangle)
   {
      return new(
         (float)rectangle.X,
         (float)rectangle.Y,
         (float)rectangle.Width,
         (float)rectangle.Height);
   }
   #endregion
}

internal readonly struct SDL3_Point(int x, int y)
{
   #region Fields
   public readonly int X = x;
   public readonly int Y = y;
   #endregion

   #region Operators
   public static implicit operator SDL3_Point(Point point)
   {
      return new((int)point.X, (int)point.Y);
   }
   #endregion
}

internal readonly struct SDL3_FPoint(float x, float y)
{
   #region Fields
   public readonly float X = x;
   public readonly float Y = y;
   #endregion

   #region Operators
   public static implicit operator SDL3_FPoint(Point point)
   {
      return new((float)point.X, (float)point.Y);
   }
   #endregion
}

static partial class Native
{

}
