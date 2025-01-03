namespace Sain.Shared;

/// <summary>
///   Represents a size in a two dimensional space.
/// </summary>
/// <param name="width">The horizontal size.</param>
/// <param name="height">The vertical size.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct Size(double width, double height) :
#if NET7_0_OR_GREATER
   IEqualityOperators<Size, Size, bool>,
   IComparisonOperators<Size, Size, bool>,
   IAdditionOperators<Size, Size, Size>,
   IAdditionOperators<Size, Point, Size>,
   ISubtractionOperators<Size, Size, Size>,
   ISubtractionOperators<Size, Point, Size>,
   IMultiplyOperators<Size, double, Size>,
   IDivisionOperators<Size, double, Size>,
   IUnaryPlusOperators<Size, Size>,
   IUnaryNegationOperators<Size, Size>,
   IModulusOperators<Size, Size, Size>,
#endif
   IEquatable<Size>,
   IComparable<Size>
{
   #region Nested types
   /// <summary>
   ///   Represents the equality comparer for the <see cref="Size"/> type.
   /// </summary>
   public sealed class EqualityComparer : IEqualityComparer<Size>
   {
      #region Properties
      /// <summary>The shared instance of the equality comparer for the <see cref="Size"/> type.</summary>
      public static EqualityComparer Instance { get; } = new();
      #endregion

      #region Methods
      /// <inheritdoc/>
      public bool Equals(Size x, Size y) => x.Equals(y);

      /// <inheritdoc/>
      public int GetHashCode([DisallowNull] Size obj) => obj.GetHashCode();
      #endregion
   }
   #endregion

   #region Properties
   /// <summary>The horizontal size.</summary>
   public readonly double Width { get; } = width;

   /// <summary>The vertical size.</summary>
   public readonly double Height { get; } = height;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(Size other) => other.Width == Width && other.Height == other.Height;

   /// <inheritdoc/>
   public int CompareTo(Size other)
   {
      double width = Width - other.Width;
      double height = Height - other.Height;

      if (width is 0 && height is 0)
         return 0;

      if (width < 0 || height < 0)
         return int.Min((int)double.Min(width, height), -1);

      return int.Max((int)double.Max(width, height), 1);
   }

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is Size other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public override int GetHashCode() => HashCode.Combine(Width, Height);

   /// <inheritdoc/>
   public override string ToString() => $"{Width}x{Height}";
   private string DebuggerDisplay() => $"Size {{ Width = ({Width}), Height = ({Height}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(Size left, Size right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(Size left, Size right) => left.Equals(right) is false;

   /// <summary>Compares two values to determine which is lesser.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator <(Size left, Size right) => left.Width < right.Width || left.Height < right.Height;

   /// <summary>Compares two values to determine which is greater.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator >(Size left, Size right) => left.Width > right.Width && left.Height > right.Height;

   /// <summary>Compares two values to determine which is lesser or equal.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator <=(Size left, Size right) => left.Width <= right.Width || left.Height <= right.Height;

   /// <summary>Compares two values to determine which is greater or equal.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator >=(Size left, Size right) => left.Width >= right.Width && left.Height >= right.Height;

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Size operator +(Size left, Size right) => new(left.Width + right.Width, left.Height + right.Height);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked +(Size left, Size right) => new(checked(left.Width + right.Width), checked(left.Height + right.Height));

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   public static Size operator +(Size left, Point right) => new(left.Width + right.X, left.Height + right.Y);

   /// <summary>Adds two values together to compute their sum.</summary>
   /// <param name="left">The value to which <paramref name="right"/> is added.</param>
   /// <param name="right">The value which is added to <paramref name="left"/>.</param>
   /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked +(Size left, Point right) => new(checked(left.Width + right.X), checked(left.Height + right.Y));

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Size operator -(Size left, Size right) => new(left.Width - right.Width, left.Height - right.Height);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked -(Size left, Size right) => new(checked(left.Width - right.Width), checked(left.Height - right.Height));

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   public static Size operator -(Size left, Point right) => new(left.Width - right.X, left.Height - right.Y);

   /// <summary>Subtracts two values to compute their difference.</summary>
   /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
   /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
   /// <returns>The value of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked -(Size left, Point right) => new(checked(left.Width - right.X), checked(left.Height - right.Y));

   /// <summary>Multiplies two values together to compute their product.</summary>
   /// <param name="left">The value which <paramref name="right"/> multiplies.</param>
   /// <param name="right">The value which multiplies <paramref name="left"/>.</param>
   /// <returns>The prouct of left multiplied by <paramref name="right"/>.</returns>
   public static Size operator *(Size left, double right) => new(left.Width * right, left.Height * right);

   /// <summary>Multiplies two values together to compute their product.</summary>
   /// <param name="left">The value which <paramref name="right"/> multiplies.</param>
   /// <param name="right">The value which multiplies <paramref name="left"/>.</param>
   /// <returns>The prouct of left multiplied by <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked *(Size left, double right) => new(checked(left.Width * right), checked(left.Height * right));

   /// <summary>Divides two values together to compute their quotient.</summary>
   /// <param name="left">The value which <paramref name="right"/> divides.</param>
   /// <param name="right">The value which divides <paramref name="left"/>.</param>
   /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
   public static Size operator /(Size left, double right) => new(left.Width / right, left.Height / right);

   /// <summary>Divides two values together to compute their quotient.</summary>
   /// <param name="left">The value which <paramref name="right"/> divides.</param>
   /// <param name="right">The value which divides <paramref name="left"/>.</param>
   /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked /(Size left, double right) => new(checked(left.Width / right), checked(left.Height / right));

   /// <summary>Computes the unary plus of a value.</summary>
   /// <param name="value">The value for which to compute the unary plus.</param>
   /// <returns>The unary plus of <paramref name="value"/>.</returns>
   public static Size operator +(Size value) => new(+value.Width, +value.Height);

   /// <summary>Computes the unary negation of a value.</summary>
   /// <param name="value">The value for which to compute the unary negation.</param>
   /// <returns>The unary negation of <paramref name="value"/>.</returns>
   public static Size operator -(Size value) => new(-value.Width, -value.Height);

   /// <summary>Computes the unary negation of a value.</summary>
   /// <param name="value">The value for which to compute the unary negation.</param>
   /// <returns>The unary negation of <paramref name="value"/>.</returns>
   /// <exception cref="OverflowException">Thrown if the result is not representable.</exception>
   public static Size operator checked -(Size value) => new(checked(-value.Width), checked(-value.Height));

   /// <summary>Divides two values together to compute their modulus or remainder.</summary>
   /// <param name="left">The value which <paramref name="right"/> divides.</param>
   /// <param name="right">The value which divides <paramref name="left"/>.</param>
   /// <returns>The modulus or remainder of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
   public static Size operator %(Size left, Size right) => new(left.Width % right.Width, left.Height % right.Height);
   #endregion
}
