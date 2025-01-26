namespace Sain.Applications.Versions;

/// <summary>
///   Represents information about the version of a Sain application.
/// </summary>
public interface IApplicationVersion
{
   #region Properties
   /// <summary>The display name of the version.</summary>
   string DisplayName { get; }
   #endregion
}
