namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents information about a Sain application.
/// </summary>
/// <param name="name">The name of the application.</param>
/// <param name="ids">The ids of the application.</param>
/// <param name="versions">The versions of the application.</param>
public sealed class ApplicationInfo(
   string name,
   IApplicationIdCollection ids,
   IApplicationVersionCollection versions)
   : IApplicationInfo
{
   #region Properties
   /// <inheritdoc/>
   public string Name { get; } = name;

   /// <inheritdoc/>
   public IApplicationIdCollection Ids { get; } = ids;

   /// <inheritdoc/>
   public IApplicationVersionCollection Versions { get; } = versions;
   #endregion
}
