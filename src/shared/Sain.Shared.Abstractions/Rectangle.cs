namespace Sain.Shared;

/// <summary>
///   Represents a rectangular area in a two dimensional space.
/// </summary>
/// <param name="x">The horizontal position of the rectangle.</param>
/// <param name="y">The vertical position of the rectangle.</param>
/// <param name="width">The width of the rectangle.</param>
/// <param name="height">The height of the rectangle.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct Rectangle(double x, double y, double width, double height) :
#if NET7_0_OR_GREATER
   IEqualityOperators<Rectangle, Rectangle, bool>,
   IAdditionOperators<Rectangle, Point, Rectangle>,
   IAdditionOperators<Rectangle, Size, Rectangle>,
   ISubtractionOperators<Rectangle, Point, Rectangle>,
   ISubtractionOperators<Rectangle, Size, Rectangle>,
#endif
   IEquatable<Rectangle>
{
   #region Nested types
   /// <summary>
   ///   Represents the equality comparer for the <see cref="Rectangle"/> type.
   /// </summary>
   public sealed class EqualityComparer : IEqualityComparer<Rectangle>
   {
      #region Properties
      /// <summary>The shared instance of the equality comparer for the <see cref="Rectangle"/> type.</summary>
      public static EqualityComparer Instance { get; } = new();
      #endregion

      #region Methods
      /// <inheritdoc/>
      public bool Equals(Rectangle x, Rectangle y) => x.Equals(y);

      /// <inheritdoc/>
      public int GetHashCode([DisallowNull] Rectangle obj) => obj.GetHashCode();
      #endregion
   }
   #endregion

   #region Properties
   /// <summary>The horizontal position of the rectangle.</summary>
   public readonly double X { get; } = x;

   /// <summary>The vertical position of the rectangle.</summary>
   public readonly double Y { get; } = y;

   /// <summary>The width of the rectangle.</summary>
   public readonly double Width { get; } = width;

   /// <summary>The height of the rectangle.</summary>
   public readonly double Height { get; } = height;

   /// <summary>The position of the rectangle.</summary>
   public readonly Point Position => new(X, Y);

   /// <summary>The size of the rectangle.</summary>
   public readonly Size Size => new(Width, Height);

   /// <summary>The left edge of the rectangle.</summary>
   public readonly double Left => X;

   /// <summary>The right edge of the rectangle.</summary>
   public readonly double Right => X + Width;

   /// <summary>The top edge of the rectangle.</summary>
   public readonly double Top => Y;

   /// <summary>The bottom edge of the rectangle.</summary>
   public readonly double Bottom => Y + Height;

   /// <summary>The top left corner of the rectangle.</summary>
   public readonly Point TopLeft => new(Left, Top);

   /// <summary>The top right corner of the rectangle.</summary>
   public readonly Point TopRight => new(Right, Top);

   /// <summary>The bottom left corner of the rectangle.</summary>
   public readonly Point BottomLeft => new(Left, Bottom);

   /// <summary>The bottom right corner of the rectangle.</summary>
   public readonly Point BottomRight => new(Right, Bottom);
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="Rectangle"/>.</summary>
   /// <param name="x">The horizontal position of the rectangle.</param>
   /// <param name="y">The vertical position of the rectangle.</param>
   /// <param name="size">The size of the rectangle.</param>
   public Rectangle(double x, double y, Size size) : this(x, y, size.Width, size.Height) { }

   /// <summary>Creates a new instance of the <see cref="Rectangle"/>.</summary>
   /// <param name="position">The position of the rectangle.</param>
   /// <param name="width">The width of the rectangle.</param>
   /// <param name="height">The height of the rectangle.</param>
   public Rectangle(Point position, double width, double height) : this(position.X, position.Y, width, height) { }

   /// <summary>Creates a new instance of the <see cref="Rectangle"/>.</summary>
   /// <param name="position">The position of the rectangle.</param>
   /// <param name="size">The size of the rectangle.</param>
   public Rectangle(Point position, Size size) : this(position.X, position.Y, size.Width, size.Height) { }

   /// <summary>Creates a new instance of the <see cref="Rectangle"/>.</summary>
   /// <param name="width">The width of the rectangle.</param>
   /// <param name="height">The height of the rectangle.</param>
   public Rectangle(double width, double height) : this(0, 0, width, height) { }

   /// <summary>Creates a new instance of the <see cref="Rectangle"/>.</summary>
   /// <param name="size">The size of the rectangle.</param>
   public Rectangle(Size size) : this(0, 0, size.Width, size.Height) { }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(Rectangle other) => other.X == X && other.Y == Y && other.Width == Width && other.Height == Height;

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is Rectangle other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

   /// <inheritdoc/>
   public override string ToString() => $"{X}, {Y} {Width}x{Height}";
   private string DebuggerDisplay() => $"Rectangle {{ X = ({X}), Y = ({Y}), Width = ({Width}), Height = ({Height}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(Rectangle left, Rectangle right) => left.Equals(right) is false;

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Rectangle operator +(Rectangle left, Point right) => new(left.Position + right, left.Size);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Rectangle operator checked +(Rectangle left, Point right) => new(checked(left.Position + right), left.Size);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Rectangle operator +(Rectangle left, Size right) => new(left.Position, left.Size + right);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Rectangle operator checked +(Rectangle left, Size right) => new(left.Position, checked(left.Size + right));

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Rectangle operator -(Rectangle left, Point right) => new(left.Position - right, left.Size);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Rectangle operator checked -(Rectangle left, Point right) => new(checked(left.Position - right), left.Size);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Rectangle operator -(Rectangle left, Size right) => new(left.Position, left.Size - right);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Rectangle operator checked -(Rectangle left, Size right) => new(left.Position, checked(left.Size - right));
   #endregion
}
