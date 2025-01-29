using System.Reflection;

namespace Sain.Applications.Versions;

/// <summary>
///   Represents an application version that comes from an assembly.
/// </summary>
/// <param name="assembly">The assembly that the <paramref name="version"/> comes from.</param>
/// <param name="version">The version of the application.</param>
public sealed class AssemblyApplicationVersion(Assembly assembly, Version version) : IAssemblyApplicationVersion
{
   #region Properties
   /// <inheritdoc/>
   public Assembly Assembly { get; } = assembly;

   /// <inheritdoc/>
   public Version Version { get; } = version;

   /// <inheritdoc/>
   public string DisplayName { get; } = version.ToString();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder"/> and the <see cref="AssemblyApplicationVersion"/>.
/// </summary>
public static class ApplicationBuilderAssemblyApplicationVersionExtensions
{
   #region Methods
   /// <summary>Adds an application version that comes from the given <paramref name="assembly"/>.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="assembly">The assembly that the <paramref name="version"/> comes from.</param>
   /// <param name="version">The version of the application.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TBuilder WithAssemblyApplicationVersion<TBuilder>(this TBuilder builder, Assembly assembly, Version version)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      AssemblyApplicationVersion appVersion = new(assembly, version);
      return builder.WithApplicationVersion(appVersion);
   }

   /// <summary>Adds an application version that is extracted from the given <paramref name="assembly"/>.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="assembly">The assembly to extract the application version from.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   /// <exception cref="ArgumentException">Thrown if a version couldn't be extracted from the given <paramref name="assembly"/>.</exception>
   public static TBuilder WithAssemblyApplicationVersion<TBuilder>(this TBuilder builder, Assembly assembly)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      Version? version = assembly.GetName().Version ?? throw new ArgumentException($"Couldn't extract a version from the given assembly ({assembly}).", nameof(assembly));

      AssemblyApplicationVersion appVersion = new(assembly, version);
      return builder.WithApplicationVersion(appVersion);
   }

   /// <summary>Adds an application version entry/calling assemblies.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   /// <remarks>This will first try to extract a version from the entry assembly, if that fails then the calling assembly is used.</remarks>
   /// <exception cref="InvalidOperationException">Thrown if a version couldn't be extracted from the entry/calling assemblies.</exception>
   public static TBuilder WithAssemblyApplicationVersion<TBuilder>(this TBuilder builder)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      Assembly? assembly = Assembly.GetEntryAssembly();
      Version? version = assembly?.GetName().Version;
      if (version is null)
      {
         assembly = Assembly.GetCallingAssembly();
         version = assembly.GetName().Version;

         if (version is null)
            throw new InvalidOperationException($"Couldn't automatically extract a version a version from the entry/calling assemblies.");
      }
      else
         Debug.Assert(assembly is not null, "Only way version is not null is if the assembly wasn't null.");

      AssemblyApplicationVersion appVersion = new(assembly, version);
      return builder.WithApplicationVersion(appVersion);
   }
   #endregion
}
