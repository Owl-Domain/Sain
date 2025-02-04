namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the base implementation for the application context unit for log related storage.
/// </summary>
/// <param name="provider">The context provider that this unit came from.</param>
public abstract class BaseLogStorageContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), ILogStorageContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public override Type Kind => typeof(ILogStorageContextUnit);

   /// <inheritdoc/>
   public abstract IReadOnlyList<string> ReadFrom { get; }

   /// <inheritdoc/>
   public abstract string WriteTo { get; }

   /// <inheritdoc/>
   public abstract string SessionDirectory { get; }
   #endregion
}
