namespace Sain.Shared;

/// <summary>
///   Represents a point in a two dimensional space.
/// </summary>
/// <param name="x">The horizontal position of the point.</param>
/// <param name="y">The vertical position of the point.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct Point(double x, double y) :
#if NET7_0_OR_GREATER
   IEqualityOperators<Point, Point, bool>,
   IAdditionOperators<Point, Point, Point>,
   IAdditionOperators<Point, Size, Point>,
   ISubtractionOperators<Point, Point, Point>,
   ISubtractionOperators<Point, Size, Point>,
   IMultiplyOperators<Point, double, Point>,
   IDivisionOperators<Point, double, Point>,
   IUnaryPlusOperators<Point, Point>,
   IUnaryNegationOperators<Point, Point>,
#endif
   IEquatable<Point>
{
   #region Properties
   /// <summary>The horizontal position of the point.</summary>
   public readonly double X { get; } = x;

   /// <summary>The vertical position of the point.</summary>
   public readonly double Y { get; } = y;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(Point other) => other.X == X && other.Y == Y;

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is Point other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public override int GetHashCode() => HashCode.Combine(X, Y);

   /// <inheritdoc/>
   public override string ToString() => $"{X}, {Y}";
   private string DebuggerDisplay() => $"Point {{ X = ({X}), Y = ({Y}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(Point left, Point right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(Point left, Point right) => left.Equals(right) is false;

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked +(Point left, Point right) => new(checked(left.X + right.X), checked(left.Y + right.Y));

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Point operator +(Point left, Size right) => new(left.X + right.Width, left.Y + right.Height);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked +(Point left, Size right) => new(checked(left.X + right.Width), checked(left.Y + right.Height));

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Point operator -(Point left, Point right) => new(left.X - right.X, left.Y - right.Y);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked -(Point left, Point right) => new(checked(left.X - right.X), checked(left.Y - right.Y));

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Point operator -(Point left, Size right) => new(left.X - right.Width, left.Y - right.Height);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked -(Point left, Size right) => new(checked(left.X - right.Width), checked(left.Y - right.Height));

   /// <summary>Multiplies two values together to compute their product.</summary>
   /// <param name="left">The value which <paramref name="right"/> multiplies.</param>
   /// <param name="right">The value which multiplies <paramref name="left"/>.</param>
   /// <returns>The prouct of left multiplied by <paramref name="right"/>.</returns>
   public static Point operator *(Point left, double right) => new(left.X * right, left.Y * right);

   /// <summary>Multiplies two values together to compute their product.</summary>
   /// <param name="left">The value which <paramref name="right"/> multiplies.</param>
   /// <param name="right">The value which multiplies <paramref name="left"/>.</param>
   /// <returns>The prouct of left multiplied by <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked *(Point left, double right) => new(checked(left.X * right), checked(left.Y * right));

   /// <summary>Divides two values together to compute their quotient.</summary>
   /// <param name="left">The value which <paramref name="right"/> divides.</param>
   /// <param name="right">The value which divides <paramref name="left"/>.</param>
   /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
   public static Point operator /(Point left, double right) => new(left.X / right, left.Y / right);

   /// <summary>Divides two values together to compute their quotient.</summary>
   /// <param name="left">The value which <paramref name="right"/> divides.</param>
   /// <param name="right">The value which divides <paramref name="left"/>.</param>
   /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked /(Point left, double right) => new(checked(left.X / right), checked(left.Y / right));

   /// <summary>Computes the unary plus of a value.</summary>
   /// <param name="value">The value for which to compute the unary plus.</param>
   /// <returns>The unary plus of <paramref name="value"/>.</returns>
   public static Point operator +(Point value) => new(+value.X, +value.Y);

   /// <summary>Computes the unary negation of a value.</summary>
   /// <param name="value">The value for which to compute the unary negation.</param>
   /// <returns>The unary negation of <paramref name="value"/>.</returns>
   public static Point operator -(Point value) => new(-value.X, -value.Y);

   /// <summary>Computes the unary negation of a value.</summary>
   /// <param name="value">The value for which to compute the unary negation.</param>
   /// <returns>The unary negation of <paramref name="value"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Point operator checked -(Point value) => new(checked(-value.X), checked(-value.Y));
   #endregion
}
