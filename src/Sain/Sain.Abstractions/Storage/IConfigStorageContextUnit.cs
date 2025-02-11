namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the application's context unit for config related storage.
/// </summary>
public interface IConfigStorageContextUnit : IContextUnit
{
   #region Properties
   /// <summary>The collection of the directories where config files for the current application should be read from.</summary>
   /// <remarks>This collection is ordered from most specific to least specific.</remarks>
   IReadOnlyList<string> ReadFrom { get; }

   /// <summary>The path to the directory in which new config files for the current application should be written to.</summary>
   string WriteTo { get; }
   #endregion
}
