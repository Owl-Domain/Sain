namespace OwlDomain.Sain.Storage;

/// <summary>
///   Creates a new instance of the <see cref="StorageContextUnitGroup"/>.
/// </summary>
/// <param name="data">The context unit that is responsible for the application's data storage.</param>
/// <param name="config">The context unit that is responsible for the application's config storage.</param>
/// <param name="log">The context unit that is responsible for the application's log storage.</param>
public sealed class StorageContextUnitGroup(
   IDataStorageContextUnit? data,
   IConfigStorageContextUnit? config,
   ILogStorageContextUnit? log)
   : IStorageContextUnitGroup
{
   #region Properties
   /// <inheritdoc/>
   public IDataStorageContextUnit? Data { get; } = data;

   /// <inheritdoc/>
   public IConfigStorageContextUnit? Config { get; } = config;

   /// <inheritdoc/>
   public ILogStorageContextUnit? Log { get; } = log;
   #endregion

   #region Methods
   /// <summary>Creates a new <see cref="StorageContextUnitGroup"/> with the context units from the given <paramref name="applicationContext"/>.</summary>
   /// <param name="applicationContext">The application context to use for obtaining the storage context units.</param>
   /// <returns>The created storage context unit group.</returns>
   public static StorageContextUnitGroup Create(IApplicationContext applicationContext)
   {
      applicationContext.TryGetContext(out IDataStorageContextUnit? data);
      applicationContext.TryGetContext(out IConfigStorageContextUnit? config);
      applicationContext.TryGetContext(out ILogStorageContextUnit? log);

      return new(data, config, log);
   }
   #endregion
}
