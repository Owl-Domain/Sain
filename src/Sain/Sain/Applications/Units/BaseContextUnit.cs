namespace OwlDomain.Sain.Applications.Units;

/// <summary>
///   Represents the base implementation for a context unit.
/// </summary>
/// <param name="provider">he context provider that this unit came from.</param>
public abstract class BaseContextUnit(IContextProviderUnit? provider) : BaseApplicationUnit, IContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public abstract override Type Kind { get; }

   /// <inheritdoc/>
   public IContextProviderUnit? Provider { get; } = provider;

   /// <inheritdoc/>
   public virtual bool InitialiseAfterProvider => true;

   /// <inheritdoc/>
   public sealed override bool CanCoexist => false;
   #endregion
}
