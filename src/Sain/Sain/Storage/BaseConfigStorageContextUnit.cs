namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the base implementation for the application context unit for config related storage.
/// </summary>
/// <param name="provider">The context provider that this unit came from.</param>
public abstract class BaseConfigStorageContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), IConfigStorageContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public override Type Kind => typeof(IConfigStorageContextUnit);

   /// <inheritdoc/>
   public abstract IReadOnlyList<string> ReadFrom { get; }

   /// <inheritdoc/>
   public abstract string WriteTo { get; }
   #endregion
}
