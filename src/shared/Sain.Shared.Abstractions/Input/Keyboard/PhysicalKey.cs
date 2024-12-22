namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents a physical key on a keyboard.
/// </summary>
/// <param name="kind">The kind of the physical key.</param>
/// <param name="id">The internal id of the physical key.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct PhysicalKey(PhysicalKeyKind kind, uint id) :
#if NET7_0_OR_GREATER
   IEqualityOperators<PhysicalKey, PhysicalKey, bool>,
#endif
   IEquatable<PhysicalKey>
{
   #region Properties
   /// <summary>The kind of the physical key.</summary>
   public readonly PhysicalKeyKind Kind { get; } = kind;

   /// <summary>The internal id of the physical key, primarily useful when the key has not yet been defined as a <see cref="PhysicalKeyKind"/>.</summary>
   /// <remarks>I really hope <see cref="uint"/> will be enough.</remarks>
   public readonly uint Id { get; } = id;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(PhysicalKey other) => other.Kind == Kind && other.Id == Id;

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is PhysicalKey other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public override int GetHashCode() => HashCode.Combine(Kind, Id);

   /// <inheritdoc/>
   public override string ToString()
   {
      if (Kind.IsCategory())
         return $"{Kind}: {Id:n0}";

#if NET5_0_OR_GREATER
      if (Enum.IsDefined(Kind))
         return Kind.ToString();
#else
      if (Enum.IsDefined(typeof(PhysicalKey), Kind))
         return Kind.ToString();
#endif

      return $"Other: {Id:n0}";
   }
   private string DebuggerDisplay() => $"Physical Key {{ Kind = ({Kind}), Id = ({Id:n0}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(PhysicalKey left, PhysicalKey right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(PhysicalKey left, PhysicalKey right) => left.Equals(right) is false;
   #endregion
}
