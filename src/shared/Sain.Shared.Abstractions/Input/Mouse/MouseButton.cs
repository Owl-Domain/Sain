namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents a mouse button.
/// </summary>
/// <param name="kind">The kind of the mouse button.</param>
/// <param name="id">The internal id of the mouse button.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct MouseButton(MouseButtonKind kind, uint id) :
#if NET7_0_OR_GREATER
   IEqualityOperators<MouseButton, MouseButton, bool>,
#endif
   IEquatable<MouseButton>
{
   #region Fields
   /// <summary>Represents a reusable <see cref="MouseButton"/> that represents an unknown mouse button.</summary>
   public static readonly MouseButton Unknown = new(MouseButtonKind.Unknown, 0);
   #endregion

   #region Properties
   /// <summary>The kind of the mouse button.</summary>
   public readonly MouseButtonKind Kind { get; } = kind;

   /// <summary>The internal id of the mouse button, primarily useful when the key has not yet been defined as a <see cref="MouseButtonKind"/>.</summary>
   /// <remarks>I really hope <see cref="uint"/> will be enough.</remarks>
   public readonly uint Id { get; } = id;
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="MouseButton"/>.</summary>
   /// <param name="kind">The kind of the mouse button.</param>
   public MouseButton(MouseButtonKind kind) : this(kind, 0) { }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(MouseButton other)
   {
      if (other.Kind != Kind)
         return false;

      if (Kind is MouseButtonKind.Other or MouseButtonKind.Reserved)
         return other.Id == Id;

      return true; // Same kind, id is irrelevant.
   }

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is MouseButton other)
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
      if (Enum.IsDefined(typeof(MouseButton), Kind))
         return Kind.ToString();
#endif

      return $"Other: {Id:n0}";
   }
   private string DebuggerDisplay() => $"MouseButton {{ Kind = ({Kind}), Id = ({Id:n0}) }}";
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(MouseButton left, MouseButton right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(MouseButton left, MouseButton right) => left.Equals(right) is false;
   #endregion
}
