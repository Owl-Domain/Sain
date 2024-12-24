namespace Sain.Shared.Versioning;

/// <summary>
///   Represents a semantic version.
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public class SemanticVersion : ISemanticVersion
{
   #region Properties
   /// <inheritdoc/>
   public string DisplayName { get; }

   /// <inheritdoc/>
   public int Major { get; }

   /// <inheritdoc/>
   public int? Minor { get; }

   /// <inheritdoc/>
   public int? Revision { get; }

   /// <inheritdoc/>
   public int? Build { get; }

   /// <inheritdoc/>
   public string? Suffix { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="build">The build number.</param>
   /// <param name="suffix">The version suffix.</param>
   public SemanticVersion(int major, int minor, int revision, int build, string suffix)
   {
      Major = major;
      Minor = minor;
      Revision = revision;
      Build = build;
      Suffix = suffix;

      DisplayName = $"{major}.{minor}.{revision}.{build}-{suffix}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="build">The build number.</param>
   public SemanticVersion(int major, int minor, int revision, int build)
   {
      Major = major;
      Minor = minor;
      Revision = revision;
      Build = build;

      DisplayName = $"{major}.{minor}.{revision}.{build}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="suffix">The version suffix.</param>
   public SemanticVersion(int major, int minor, int revision, string suffix)
   {
      Major = major;
      Minor = minor;
      Revision = revision;
      Suffix = suffix;

      DisplayName = $"{major}.{minor}.{revision}-{suffix}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   public SemanticVersion(int major, int minor, int revision)
   {
      Major = major;
      Minor = minor;
      Revision = revision;

      DisplayName = $"{major}.{minor}.{revision}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="suffix">The version suffix.</param>
   public SemanticVersion(int major, int minor, string suffix)
   {
      Major = major;
      Minor = minor;
      Suffix = suffix;

      DisplayName = $"{major}.{minor}-{suffix}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   public SemanticVersion(int major, int minor)
   {
      Major = major;
      Minor = minor;

      DisplayName = $"{major}.{minor}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   /// <param name="suffix">The version suffix.</param>
   public SemanticVersion(int major, string suffix)
   {
      Major = major;
      Suffix = suffix;

      DisplayName = $"{major}-{suffix}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="major">The major version number.</param>
   public SemanticVersion(int major)
   {
      Major = major;

      DisplayName = $"{major}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="version">The version to copy the semantic version information from.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the given <paramref name="version"/> doesn't have a major version.</exception>
   public SemanticVersion(Version version, string suffix)
   {
      if (version.Major < 0)
         throw new ArgumentOutOfRangeException(nameof(version), version, $"Expected the version to at least a the major version.");

      Major = version.Major;
      Suffix = suffix;

      if (version.Minor >= 0)
      {
         Minor = version.Minor;
         if (version.Revision >= 0)
         {
            Revision = version.Revision;
            if (version.Build >= 0)
            {
               Build = version.Build;
               DisplayName = $"{Major}.{Minor}.{Revision}.{Build}-{suffix}";
            }
            else
               DisplayName = $"{Major}.{Minor}.{Revision}-{suffix}";
         }
         else
            DisplayName = $"{Major}.{Minor}-{suffix}";
      }
      else
         DisplayName = $"{Major}-{suffix}";
   }

   /// <summary>Creates a new instance of the <see cref="SemanticVersion"/>.</summary>
   /// <param name="version">The version to copy the semantic version information from.</param>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the given <paramref name="version"/> doesn't have a major version.</exception>
   public SemanticVersion(Version version)
   {
      if (version.Major < 0)
         throw new ArgumentOutOfRangeException(nameof(version), version, $"Expected the version to at least a the major version.");

      Major = version.Major;

      if (version.Minor >= 0)
      {
         Minor = version.Minor;
         if (version.Revision >= 0)
         {
            Revision = version.Revision;
            if (version.Build >= 0)
            {
               Build = version.Build;
               DisplayName = $"{Major}.{Minor}.{Revision}.{Build}";
            }
            else
               DisplayName = $"{Major}.{Minor}.{Revision}";
         }
         else
            DisplayName = $"{Major}.{Minor}";
      }
      else
         DisplayName = $"{Major}";
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public override string ToString() => DisplayName;
   private string DebuggerDisplay() => $"SemanticVersion {{ Version = ({DisplayName}) }}";
   #endregion
}
