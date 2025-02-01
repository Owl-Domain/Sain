namespace OwlDomain.Sain.Providers;

/// <summary>
///   Contains various extension methods that are related to the <see cref="IApplicationBuilder"/> and to the built-in context provider units.
/// </summary>
public static class ApplicationBuilderBuiltinContextProviderUnitExtensions
{
   #region Methods
   /// <summary>Adds the built-in context provider units to the application.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TSelf WithBuiltinProviders<TSelf>(this TSelf builder)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithContextProvider<BuiltinContextProviderUnit>();
   }
   #endregion
}
