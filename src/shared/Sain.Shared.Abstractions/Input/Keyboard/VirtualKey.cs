namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents a virtual key.
/// </summary>
/// <param name="kind">The kind of the virtual key.</param>
/// <param name="id">The internal id of the virtual key.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct VirtualKey(VirtualKeyKind kind, uint id) :
#if NET7_0_OR_GREATER
   IEqualityOperators<VirtualKey, VirtualKey, bool>,
#endif
   IEquatable<VirtualKey>
{
   #region Fields
   /// <summary>Represents a reusable <see cref="VirtualKey"/> that represents an unknown key.</summary>
   public static readonly VirtualKey Unknown = new(VirtualKeyKind.Unknown, 0);
   #endregion

   #region Properties
   /// <summary>The kind of the virtual key.</summary>
   public readonly VirtualKeyKind Kind { get; } = kind;

   /// <summary>The internal id of the virtual key, primarily useful when the key has not yet been defined as a <see cref="VirtualKeyKind"/>.</summary>
   /// <remarks>I really hope <see cref="uint"/> will be enough.</remarks>
   public readonly uint Id { get; } = id;
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="VirtualKey"/>.</summary>
   /// <param name="kind">The kind of the virtual key.</param>
   public VirtualKey(VirtualKeyKind kind) : this(kind, 0) { }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(VirtualKey other)
   {
      if (other.Kind != Kind)
         return false;

      if (Kind is VirtualKeyKind.Other or VirtualKeyKind.Reserved)
         return other.Id == Id;

      return true; // Same kind, id is irrelevant.
   }

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is VirtualKey other)
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
      if (Enum.IsDefined(typeof(VirtualKey), Kind))
         return Kind.ToString();
#endif

      return $"Other: {Id:n0}";
   }
   private string DebuggerDisplay() => $"VirtualKey {{ Kind = ({Kind}), Id = ({Id:n0}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(VirtualKey left, VirtualKey right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(VirtualKey left, VirtualKey right) => left.Equals(right) is false;
   #endregion
}
