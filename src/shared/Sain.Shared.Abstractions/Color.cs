namespace Sain.Shared;

/// <summary>
///   Represents an RGBA color.
/// </summary>
/// <param name="red">The red channel.</param>
/// <param name="green">The green channel.</param>
/// <param name="blue">The blue channel.</param>
/// <param name="alpha">The alpha channel.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct Color(byte red, byte green, byte blue, byte alpha = 255) :
#if NET7_0_OR_GREATER
   IEqualityOperators<Color, Color, bool>,
#endif
   IEquatable<Color>
{
   #region Nested types
   /// <summary>
   ///   Represents the equality comparer for the <see cref="Color"/> type.
   /// </summary>
   public sealed class EqualityComparer : IEqualityComparer<Color>
   {
      #region Properties
      /// <summary>The shared instance of the equality comparer for the <see cref="Color"/> type.</summary>
      public static EqualityComparer Instance { get; } = new();
      #endregion

      #region Methods
      /// <inheritdoc/>
      public bool Equals(Color x, Color y) => x.Equals(y);

      /// <inheritdoc/>
      public int GetHashCode([DisallowNull] Color obj) => obj.GetHashCode();
      #endregion
   }
   #endregion

   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly byte _red = red;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly byte _green = green;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly byte _blue = blue;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly byte _alpha = alpha;
   #endregion

   #region Properties
   /// <summary>The red channel, as a <see langword="byte"/>.</summary>
   public readonly byte Red => _red;

   /// <summary>The green channel, as a <see langword="byte"/>.</summary>
   public readonly byte Green => _green;

   /// <summary>The blue channel, as a <see langword="byte"/>.</summary>
   public readonly byte Blue => _blue;

   /// <summary>The alpha channel, as a <see langword="byte"/>.</summary>
   public readonly byte Alpha => _alpha;

   /// <summary>The red channel, as a <see langword="float"/>.</summary>
   /// <remarks>This value will be between <c>0</c> and <c>1</c>.</remarks>
   public readonly float RedF => _red / 255f;

   /// <summary>The green channel, as a <see langword="float"/>.</summary>
   /// <remarks>This value will be between <c>0</c> and <c>1</c>.</remarks>
   public readonly float GreenF => _green / 255f;

   /// <summary>The blue channel, as a <see langword="float"/>.</summary>
   /// <remarks>This value will be between <c>0</c> and <c>1</c>.</remarks>
   public readonly float BlueF => _blue / 255f;

   /// <summary>The alpha channel, as a <see langword="float"/>.</summary>
   /// <remarks>This value will be between <c>0</c> and <c>1</c>.</remarks>
   public readonly float AlphaF => _alpha / 255f;
   #endregion

   #region Functions
   /// <summary>Creates an RGBA color from the regular RGBA channels.</summary>
   /// <param name="red">The red channels.</param>
   /// <param name="green">The green channel.</param>
   /// <param name="blue">The blue channel.</param>
   /// <param name="alpha">The alpha channel.</param>
   /// <returns>The created RGBA color.</returns>
   public static Color FromRgb(byte red, byte green, byte blue, byte alpha = 255) => new(red, green, blue, alpha);

   /// <summary>Creates an RGBA color from the RGBA channels scaled to be between <c>0</c> and <c>1</c>.</summary>
   /// <param name="red">The red channels.</param>
   /// <param name="green">The green channel.</param>
   /// <param name="blue">The blue channel.</param>
   /// <param name="alpha">The alpha channel.</param>
   /// <returns>The created RGBA color.</returns>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if any of the channels are not between <c>0</c> and <c>1</c>.</exception>
   public static Color FromRgb(float red, float green, float blue, float alpha = 1)
   {
      if (red < 0 || red > 1)
         throw new ArgumentOutOfRangeException(nameof(red), red, $"Expected the red channel to be a value between 0-1.");

      if (green < 0 || green > 1)
         throw new ArgumentOutOfRangeException(nameof(green), green, $"Expected the green channel to be a value between 0-1.");

      if (blue < 0 || blue > 1)
         throw new ArgumentOutOfRangeException(nameof(blue), blue, $"Expected the blue channel to be a value between 0-1.");

      if (alpha < 0 || alpha > 1)
         throw new ArgumentOutOfRangeException(nameof(alpha), alpha, $"Expected the alpha channel to be a value between 0-1.");

      byte r = (byte)(red * 255);
      byte g = (byte)(green * 255);
      byte b = (byte)(blue * 255);
      byte a = (byte)(alpha * 255);

      return new(r, g, b, a);
   }
   #endregion

   #region Methods
   /// <summary>Extracts the RGBA channels from the color.</summary>
   /// <param name="red">The red channels.</param>
   /// <param name="green">The green channel.</param>
   /// <param name="blue">The blue channel.</param>
   /// <param name="alpha">The alpha channel.</param>
   public readonly void ToRgba(out byte red, out byte green, out byte blue, out byte alpha)
   {
      red = _red;
      green = _green;
      blue = _blue;
      alpha = _alpha;
   }

   /// <inheritdoc/>
   public readonly bool Equals(Color other) => other._red == _red && other._green == Green && other._blue == _blue && other._alpha == _alpha;

   /// <inheritdoc/>
   public readonly override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is Color other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public readonly override int GetHashCode() => HashCode.Combine(_red, _green, _blue, _alpha);

   /// <inheritdoc/>
   public readonly override string ToString()
   {
      if (_alpha is 255)
         return $"({_red},{_green},{_blue})";

      return $"{_red}, {_green}, {_blue}, {_alpha})";
   }

   private readonly string DebuggerDisplay()
   {
      if (_alpha is 255)
         return $"Color {{ Red = ({_red}), Green = ({_green}), Blue = ({_blue}) }}";

      return $"Color {{ Red = ({_red}), Green = ({_green}), Blue = ({_blue}), Alpha = ({_alpha}) }}";
   }
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(Color left, Color right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(Color left, Color right) => left.Equals(right) is false;
   #endregion
}
