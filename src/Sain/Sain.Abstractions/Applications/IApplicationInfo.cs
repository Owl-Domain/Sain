namespace Sain.Applications;

/// <summary>
///   Represents information about a Sain application.
/// </summary>
public interface IApplicationInfo
{
   #region Properties
   /// <summary>The name of the application.</summary>
   string Name { get; }

   /// <summary>The ids of the application.</summary>
   IApplicationIdCollection Ids { get; }

   /// <summary>The versions of the application.</summary>
   IApplicationVersionCollection Versions { get; }
   #endregion
}
