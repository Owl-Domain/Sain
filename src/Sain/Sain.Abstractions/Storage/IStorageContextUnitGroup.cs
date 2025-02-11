namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the group for the application's context units related to storage.
/// </summary>
public interface IStorageContextUnitGroup
{
   #region Properties
   /// <summary>Whether all of the context units in this group are available.</summary>
#if NET5_0_OR_GREATER
   [MemberNotNullWhen(true, nameof(General), nameof(Data), nameof(Config), nameof(Log))]
#endif
   bool IsFullyAvailable { get; }

   /// <summary>The context unit that is responsible for general storage operations.</summary>
   IGeneralStorageContextUnit? General { get; }

   /// <summary>The context unit that is responsible for the application's data storage.</summary>
   IDataStorageContextUnit? Data { get; }

   /// <summary>The context unit that is responsible for the application's config storage.</summary>
   IConfigStorageContextUnit? Config { get; }

   /// <summary>The context unit that is responsible for the application's log storage.</summary>
   ILogStorageContextUnit? Log { get; }
   #endregion
}
