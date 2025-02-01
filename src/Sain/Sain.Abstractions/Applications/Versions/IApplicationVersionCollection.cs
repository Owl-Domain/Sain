namespace OwlDomain.Sain.Applications.Versions;

/// <summary>
///   Represents a collection of the versions for a Sain application.
/// </summary>
public interface IApplicationVersionCollection : IReadOnlyCollection<IApplicationVersion>
{
   #region Properties
   /// <summary>The default version of the application.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application doesn't have any versions.</exception>
   IApplicationVersion Default { get; }
   #endregion
}
