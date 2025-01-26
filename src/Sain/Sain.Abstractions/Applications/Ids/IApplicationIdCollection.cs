namespace Sain.Applications.Ids;

/// <summary>
///   Represents a collection of ids of a Sain application.
/// </summary>
public interface IApplicationIdCollection : IReadOnlyCollection<IApplicationId>
{
   #region Properties
   /// <summary>The default id of the application.</summary>
   IApplicationId Default { get; }
   #endregion
}
