namespace OwlDomain.Sain.Storage.Unix;

/// <summary>
///   Represents the context provider unit for provoding unix specific storage context units.
/// </summary>
public class UnixStorageContextProviderUnit : BaseContextProviderUnit
{
   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide(Type kind, [NotNullWhen(true)] out IContextUnit? unit)
   {
      unit = default;

      if (kind == typeof(IDataStorageContextUnit)) unit = new UnixDataStorageContextUnit(this);
      else if (kind == typeof(IConfigStorageContextUnit)) unit = new UnixConfigStorageContextUnit(this);
      else if (kind == typeof(ILogStorageContextUnit)) unit = new UnixLogStorageContextUnit(this);

      return unit is not null;
   }
   #endregion
}
