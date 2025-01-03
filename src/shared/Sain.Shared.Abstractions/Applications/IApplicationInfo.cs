namespace Sain.Shared.Applications;

/// <summary>
///   Represents information about a Sain application.
/// </summary>
public interface IApplicationInfo
{
   #region Properties
   /// <summary>The unique id of the application.</summary>
   string Id { get; }

   /// <summary>The name of the application.</summary>
   string Name { get; }

   /// <summary>The version of the application.</summary>
   IVersion Version { get; }
   #endregion
}
