namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the application's context unit for log related storage.
/// </summary>
public interface ILogStorageContextUnit : IContextUnit
{
   #region Properties
   /// <summary>The collection of the directories where logs for the current application should be read from.</summary>
   /// <remarks>This collection is ordered from most specific to least specific.</remarks>
   IReadOnlyList<string> ReadFrom { get; }

   /// <summary>The path to the directory in which new logs for the current application should be written to.</summary>
   string WriteTo { get; }

   /// <summary>The directory in which the logs for the current application session should be saved to.</summary>
   string SessionDirectory { get; }
   #endregion
}
