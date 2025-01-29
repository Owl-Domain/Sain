namespace Sain.Applications.Versions;

/// <summary>
///   Represents an application version that follows the Owl versioning scheme (Owlver for short).
/// </summary>
/// <param name="design">
///   The primary number of the version, should only be incremented on full rewrites and redesigns,
///   usually indicates a whole new application (that may or may not be compatible) instead of just a simple update.
/// </param>
/// <param name="feature">The secondary number of the version, this should be incremented when a new update either adds, removes, or modifies features.</param>
/// <param name="tweaks">The tertiary number of the version, this should be incremented when a new update only has smaller tweaks or bug fixes.</param>
/// <param name="suffix">The (optional) suffix of the version, used to indicate different release phases like alpha/beta/prerelease.</param>
/// <remarks>This is the versioning scheme made up and preferred by <see href="https://github.com/nightowl286">Nightowl</see>.</remarks>
public sealed class OwlApplicationVersion(uint design, uint feature, uint tweaks, string? suffix = null) : IOwlApplicationVersion
{
   #region Properties
   /// <inheritdoc/>
   public uint Design { get; } = design;

   /// <inheritdoc/>
   public uint Feature { get; } = feature;

   /// <inheritdoc/>
   public uint Tweaks { get; } = tweaks;

   /// <inheritdoc/>
   public string? Suffix { get; } = suffix;

   /// <inheritdoc/>
   public string DisplayName { get; } = string.IsNullOrWhiteSpace(suffix) ? $"{design}.{feature}.{tweaks}" : $"{design}.{feature}.{tweaks}-{suffix}";
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder"/> and the <see cref="OwlApplicationVersion"/>.
/// </summary>
public static class ApplicationBuilderOwlApplicationVersionExtensions
{
   #region Methods
   /// <summary>Adds an application version that follows the Owl versioning scheme.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="design">
   ///   The primary number of the version, should only be incremented on full rewrites and redesigns,
   ///   usually indicates a whole new application (that may or may not be compatible) instead of just a simple update.
   /// </param>
   /// <param name="feature">The secondary number of the version, this should be incremented when a new update either adds, removes, or modifies features.</param>
   /// <param name="tweaks">The tertiary number of the version, this should be incremented when a new update only has smaller tweaks or bug fixes.</param>
   /// <param name="suffix">The (optional) suffix of the version, used to indicate different release phases like alpha/beta/prerelease.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   /// <remarks>This is the versioning scheme made up and preferred by <see href="https://github.com/nightowl286">Nightowl</see>.</remarks>
   public static TBuilder WithOwlApplicationVersion<TBuilder>(this TBuilder builder, uint design, uint feature, uint tweaks, string? suffix = null)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      OwlApplicationVersion version = new(design, feature, tweaks, suffix);
      return builder.WithApplicationVersion(version);
   }
   #endregion
}
