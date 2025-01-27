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
   /// <remarks>This is a collection to allow for different id formats.</remarks>
   IApplicationIdCollection Ids { get; }

   /// <summary>The versions of the application.</summary>
   /// <remarks>This is a collection to allow for different version formats.</remarks>
   IApplicationVersionCollection Versions { get; }
   #endregion
}
