namespace Sain.Shared.Devices;

/// <summary>
///   Represents a unique device id.
/// </summary>
/// <param name="components">The components that make up the device id.</param>
public sealed class DeviceId(params IReadOnlyList<string> components) :
#if NET7_0_OR_GREATER
IEqualityOperators<DeviceId, DeviceId, bool>,
#endif
   IEquatable<DeviceId?>,
   IDeviceId
{
   #region Properties
   /// <inheritdoc/>
   [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
   public IReadOnlyList<string> Components { get; } = components;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool Equals(IDeviceId? other)
   {
      if (other is null)
         return false;

      return this.IsFullMatch(other);
   }

   /// <inheritdoc/>
   public bool Equals(DeviceId? other) => Equals(other as IDeviceId);

   /// <inheritdoc/>
   public override bool Equals(object? obj) => Equals(obj as IDeviceId);

   /// <inheritdoc/>
   public override int GetHashCode()
   {
      HashCode code = new();
      code.Add(Components.Count);

      foreach (string component in Components)
         code.Add(component);

      return code.ToHashCode();
   }

   /// <inheritdoc/>
   /// <remarks>This should be only used for display purposes and not for serialisation.</remarks>
   public override string ToString() => string.Concat('{', string.Join('.', Components), '}');
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(DeviceId? left, DeviceId? right)
   {
      if (left is not null)
         return left.Equals(right);

      if (right is not null)
         return right.Equals(left);

      return true; // Both null
   }

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(DeviceId? left, DeviceId? right) => !(left == right);
   #endregion
}
