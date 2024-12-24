using System.Reflection;

namespace Sain.Shared.Applications;

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/> and the <see cref="ISemanticVersion"/>.
/// </summary>
public static class IApplicationBuilderSemanticVersionExtensions
{
   #region Methods
   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="build">The build number.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor, int revision, int build, string suffix)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor, revision, build, suffix);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="build">The build number.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor, int revision, int build)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor, revision, build);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor, int revision, string suffix)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor, revision, suffix);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="revision">The revision number.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor, int revision)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor, revision);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor, string suffix)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor, suffix);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="minor">The minor version number.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, int minor)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, minor);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major, string suffix)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major, suffix);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="major">The major version number.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, int major)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion version = new(major);
      return builder.WithVersion(version);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="version">The version to copy the semantic version information from.</param>
   /// <param name="suffix">The version suffix.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, Version version, string suffix)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion semver = new(version, suffix);
      return builder.WithVersion(semver);
   }

   /// <summary>Sets the version of the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="version">The version to copy the semantic version information from.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSemanticVersion<TSelf>(this TSelf builder, Version version)
      where TSelf : IApplicationBuilder<TSelf>
   {
      SemanticVersion semver = new(version);
      return builder.WithVersion(semver);
   }

   /// <summary>Uses the given <paramref name="assembly"/> to set the application version.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <param name="assembly">The assembly to get the application version from.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if the application version couldn'TSelf be extracted from the given <paramref name="assembly"/>.</exception>
   public static TSelf WithVersionFromAssembly<TSelf>(this TSelf builder, Assembly assembly) where TSelf : IApplicationBuilder<TSelf>
   {
      Version? version = assembly
         .GetName()
         .Version ?? throw new ArgumentException($"Couldn'TSelf extract the application name from the given assembly ({assembly}).", nameof(assembly)); ;

      return builder.WithSemanticVersion(version);
   }

   /// <summary>Tries to use the entry assembly (<see cref="Assembly.GetEntryAssembly"></see>) to set the application version.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder instance.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if an instance of the entry assembly couldn'TSelf be obtained, or
   ///   the application version couldn'TSelf be extracted from the entry assembly.
   /// </exception>
   public static TSelf WithVersionFromAssembly<TSelf>(this TSelf builder) where TSelf : IApplicationBuilder<TSelf>
   {
      Assembly? assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Couldn'TSelf obtain an instance of the entry assembly.");

      Version? version = assembly
         .GetName()
         .Version ?? throw new InvalidOperationException($"Couldn'TSelf extract the application name from the entry assembly ({assembly}).");

      return builder.WithSemanticVersion(version);
   }
   #endregion
}
