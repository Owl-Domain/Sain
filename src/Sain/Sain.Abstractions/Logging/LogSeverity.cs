namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents a log severity level.
/// </summary>
/// <param name="severity">The severity level.</param>
public readonly struct LogSeverity(string severity) :
#if NET7_0_OR_GREATER
   IEqualityOperators<LogSeverity, LogSeverity, bool>,
#endif
   IEquatable<LogSeverity>
{
   #region Fields
   /// <summary>The log level for fatal errors.</summary>
   public static readonly LogSeverity Fatal = new("fatal");

   /// <summary>The log level for errors.</summary>
   public static readonly LogSeverity Error = new("error");

   /// <summary>The log level for warnings.</summary>
   public static readonly LogSeverity Warning = new("warning");

   /// <summary>The log level for general information.</summary>
   public static readonly LogSeverity Info = new("info");

   /// <summary>The log level for debug information.</summary>
   public static readonly LogSeverity Debug = new("debug");

   /// <summary>The log level for trace information.</summary>
   public static readonly LogSeverity Trace = new("trace");
   #endregion

   #region Properties
   /// <summary>The severity level.</summary>
   public readonly string Severity { get; } = severity;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public readonly bool Equals(LogSeverity other) => other.Severity == Severity;

   /// <inheritdoc/>
   public override bool Equals([NotNullWhen(true)] object? obj)
   {
      if (obj is LogSeverity other)
         return Equals(other);

      return false;
   }

   /// <inheritdoc/>
   public override int GetHashCode() => HashCode.Combine(Severity);

   /// <inheritdoc/>
   public override string ToString() => Severity;
   #endregion

   #region Operators
   /// <summary>Compares two values to determine equality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator ==(LogSeverity left, LogSeverity right) => left.Equals(right);

   /// <summary>Compares two values to determine inequality.</summary>
   /// <param name="left">The value to compare with <paramref name="right"/>.</param>
   /// <param name="right">The value to compare with <paramref name="left"/>.</param>
   /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>, <see langword="false"/> otherwise.</returns>
   public static bool operator !=(LogSeverity left, LogSeverity right) => left.Equals(right) is false;
   #endregion
}
