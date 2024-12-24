namespace Sain.Shared.Versioning;

/// <summary>
///   Represents a semantic version.
/// </summary>
public interface ISemanticVersion : IVersion
{
   #region Properties
   /// <summary>The major version number.</summary>
   int Major { get; }

   /// <summary>The minor version number.</summary>
   int? Minor { get; }

   /// <summary>The revision number.</summary>
   int? Revision { get; }

   /// <summary>The build number.</summary>
   int? Build { get; }

   /// <summary>An optional suffix for the version.</summary>
   string? Suffix { get; }
   #endregion
}
