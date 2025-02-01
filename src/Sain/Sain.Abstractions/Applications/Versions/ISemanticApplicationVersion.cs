namespace OwlDomain.Sain.Applications.Versions;

/// <summary>
///   Represents an application version that follows the <see href="https://semver.org/">semantic versioning</see> scheme.
/// </summary>
public interface ISemanticApplicationVersion : IApplicationVersion
{
   #region Properties
   /// <summary>The major version.</summary>
   uint Major { get; }

   /// <summary>The minor version.</summary>
   uint Minor { get; }

   /// <summary>The patch version.</summary>
   uint Patch { get; }

   /// <summary>The pre-release identifier.</summary>
   string? PreRelease { get; }

   /// <summary>The build metadata information.</summary>
   string? BuildMetadata { get; }
   #endregion
}
