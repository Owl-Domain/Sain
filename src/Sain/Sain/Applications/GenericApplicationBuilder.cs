namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the builder for a generic Sain application.
/// </summary>
public sealed class GenericApplicationBuilder : ApplicationBuilder<GenericApplicationBuilder, GenericApplication, IApplicationContext>
{
   #region Methods
   /// <inheritdoc/>
   protected override IApplicationContext BuildContext(IReadOnlyCollection<IApplicationUnit> units, IReadOnlyList<IApplicationUnit> initialisationOrder)
   {
      return new ApplicationContext(units, initialisationOrder);
   }

   /// <inheritdoc/>
   protected override GenericApplication BuildApplication(IApplicationInfo info, IApplicationConfiguration configuration, IApplicationContext context)
   {
      return new(info, configuration, context);
   }
   #endregion
}
