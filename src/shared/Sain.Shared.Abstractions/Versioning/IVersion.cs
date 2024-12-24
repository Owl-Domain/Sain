namespace Sain.Shared.Versioning;

/// <summary>
///   Represents information about a version.
/// </summary>
public interface IVersion
{
   #region Properties
   /// <summary>The name of the version to use for display.</summary>
   string DisplayName { get; }
   #endregion
}

