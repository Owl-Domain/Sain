namespace Sain.Shared.Applications;

/// <summary>
///   Represents information about a Sain application.
/// </summary>
/// <param name="id">The unique id of the application.</param>
/// <param name="name">The name of the application.</param>
/// <param name="version">The version of the application.</param>
public sealed class ApplicationInfo(string id, string name, IVersion version) : IApplicationInfo
{
   #region Properties
   /// <inheritdoc/>
   public string Id { get; } = id;

   /// <inheritdoc/>
   public string Name { get; } = name;

   /// <inheritdoc/>
   public IVersion Version { get; } = version;
   #endregion
}
