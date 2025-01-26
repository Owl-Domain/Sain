namespace Sain.Applications.Versions;

/// <summary>
///   Represents a collection of versions of a Sain application.
/// </summary>
public interface IApplicationVersionCollection : IReadOnlyCollection<IApplicationVersion>
{
   #region Properties
   /// <summary>The default version of the application.</summary>
   IApplicationVersion Default { get; }
   #endregion
}
