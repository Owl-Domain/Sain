namespace OwlDomain.Sain.Providers;

/// <summary>
///   Represents the context provider unit that can provide the built-in Sain context units.
/// </summary>
public sealed class BuiltinContextProviderUnit : BaseContextProviderUnit
{
   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide(Type kind, [NotNullWhen(true)] out IContextUnit? unit)
   {
      unit = null;

      if (kind == typeof(ITimeContextUnit)) unit = new DefaultTimeContextUnit(this);
      else if (kind == typeof(ILoggingContextUnit)) unit = new DefaultLoggingContextUnit(this);

      return unit is not null;
   }
   #endregion
}
