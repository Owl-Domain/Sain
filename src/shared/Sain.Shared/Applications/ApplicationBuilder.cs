namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
public sealed class ApplicationBuilder : BaseApplicationBuilder<ApplicationBuilder>
{
   #region Properties
   /// <inheritdoc/>
   protected override ApplicationBuilder Instance => this;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override IApplication BuildCore()
   {
      ApplicationContext context = new(Contexts);
      Application application = new(Name, Version, context);

      return application;
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="ApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
   #region Methods
   /// <summary>Uses the default context provider.</summary>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithDefaultProvider<TSelf>(this TSelf builder)
   where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithProvider<TSelf, DefaultContextProvider>();
   }
   #endregion
}
