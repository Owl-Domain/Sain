namespace Sain.Applications.Ids;

/// <summary>
///   Represents information about the id of a Sain application.
/// </summary>
public interface IApplicationId
{
   #region Properties
   /// <summary>The display name of the id.</summary>
   string DisplayName { get; }
   #endregion
}
