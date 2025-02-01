namespace OwlDomain.Sain.Applications.Units;

/// <summary>
///   Represents the base implementation for a context provider unit.
/// </summary>
public abstract class BaseContextProviderUnit : BaseApplicationUnit, IContextProviderUnit
{
   #region Properties
   /// <inheritdoc/>
   public virtual bool OmitIfUnused => true;

   /// <inheritdoc/>
   public override bool CanCoexist => false;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public abstract bool TryProvide(Type kind, [NotNullWhen(true)] out IContextUnit? unit);
   #endregion
}
