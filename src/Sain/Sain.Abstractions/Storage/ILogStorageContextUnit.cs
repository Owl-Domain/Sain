namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the application's context unit for log related storage.
/// </summary>
public interface ILogStorageContextUnit : IContextUnit
{
   #region Properties
   /// <summary>The collection of the directories where logs for the current application should be read from.</summary>
   IReadOnlyList<string> ReadFrom { get; }

   /// <summary>The path to the directory in which new logs for the current application should be written to.</summary>
   string WriteTo { get; }
   #endregion
}
