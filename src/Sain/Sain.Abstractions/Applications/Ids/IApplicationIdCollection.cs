namespace OwlDomain.Sain.Applications.Ids;

/// <summary>
///   Represents a collection of the ids for a Sain application.
/// </summary>
public interface IApplicationIdCollection : IReadOnlyCollection<IApplicationId>
{
   #region Properties
   /// <summary>The default id of the application.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application doesn't have any ids.</exception>
   IApplicationId Default { get; }
   #endregion
}
