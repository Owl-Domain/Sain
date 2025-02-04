namespace OwlDomain.Sain.Applications.Ids;

/// <summary>
///   Represents an application id in the reverse domain name format.
/// </summary>
public interface IReverseDomainNameApplicationId : IApplicationId
{
   #region Properties
   /// <summary>The components that make up the domain name.</summary>
   IReadOnlyList<string> Components { get; }
   #endregion
}
