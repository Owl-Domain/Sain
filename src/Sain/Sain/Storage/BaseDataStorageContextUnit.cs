namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the base implementation for the application context unit for data related storage.
/// </summary>
/// <param name="provider">The context provider that this unit came from.</param>
public abstract class BaseDataStorageContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), IDataStorageContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public override Type Kind => typeof(IDataStorageContextUnit);

   /// <inheritdoc/>
   public abstract IReadOnlyList<string> ReadFrom { get; }

   /// <inheritdoc/>
   public abstract string WriteTo { get; }
   #endregion
}
