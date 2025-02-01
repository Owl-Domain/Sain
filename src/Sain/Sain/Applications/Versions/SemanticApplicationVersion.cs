namespace OwlDomain.Sain.Applications.Versions;

/// <summary>
///   Represents an application version that follows the <see href="https://semver.org/">semantic versioning</see> scheme.
/// </summary>
public sealed class SemanticApplicationVersion : ISemanticApplicationVersion
{
   #region Properties
   /// <inheritdoc/>
   public uint Major { get; }

   /// <inheritdoc/>
   public uint Minor { get; }

   /// <inheritdoc/>
   public uint Patch { get; }

   /// <inheritdoc/>
   public string? PreRelease { get; }

   /// <inheritdoc/>
   public string? BuildMetadata { get; }

   /// <inheritdoc/>
   public string DisplayName { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="SemanticApplicationVersion"/>.</summary>
   /// <param name="major">The major version.</param>
   /// <param name="minor">The minor version.</param>
   /// <param name="patch">The patch version.</param>
   /// <param name="preRelease">The pre-release identifier.</param>
   /// <param name="buildMetadata">The build metadata information.</param>
   public SemanticApplicationVersion(uint major, uint minor, uint patch, string? preRelease, string? buildMetadata)
   {
      Major = major;
      Minor = minor;
      Patch = patch;
      PreRelease = preRelease;
      BuildMetadata = buildMetadata;

      bool hasPreRelease = string.IsNullOrWhiteSpace(preRelease) is false;
      bool hasBuildMetadata = string.IsNullOrWhiteSpace(preRelease) is false;

      DisplayName = (hasPreRelease, hasBuildMetadata) switch
      {
         (true, true) => $"{major}.{minor}.{patch}-{preRelease}+{buildMetadata}",
         (true, false) => $"{major}.{minor}.{patch}-{preRelease}",
         (false, true) => $"{major}.{minor}.{patch}+{buildMetadata}",
         (false, false) => $"{major}.{minor}.{patch}",
      };
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder"/> and the <see cref="SemanticApplicationVersion"/>.
/// </summary>
public static class ApplicationBuilderSemanticApplicationVersionExtensions
{
   #region Methods
   /// <summary>Adds an application version that follows the semantic versioning scheme.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="major">The major version.</param>
   /// <param name="minor">The minor version.</param>
   /// <param name="patch">The patch version.</param>
   /// <param name="preRelease">The pre-release identifier.</param>
   /// <param name="buildMetadata">The build metadata information.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TBuilder WithSemanticApplicationVersion<TBuilder>(this TBuilder builder, uint major, uint minor, uint patch, string? preRelease = null, string? buildMetadata = null)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      SemanticApplicationVersion version = new(major, minor, patch, preRelease, buildMetadata);
      return builder.WithApplicationVersion(version);
   }
   #endregion
}
