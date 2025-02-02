namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the group for the application's context units related to storage.
/// </summary>
public interface IStorageContextUnitGroup
{
   #region Properties
   /// <summary>The context unit that is responsible for the application's data storage.</summary>
   IDataStorageContextUnit? Data { get; }

   /// <summary>The context unit that is responsible for the application's config storage.</summary>
   IConfigStorageContextUnit? Config { get; }

   /// <summary>The context unit that is responsible for the application's log storage.</summary>
   ILogStorageContextUnit? Log { get; }
   #endregion
}
