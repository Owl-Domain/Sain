namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public abstract class ApplicationBuilder<TSelf> : BaseApplicationBuilder<TSelf>
   where TSelf : ApplicationBuilder<TSelf>
{
   #region Methods
   /// <inheritdoc/>
   protected override IApplication BuildCore()
   {
      ApplicationContext context = new(Providers, Contexts);
      Application application = new(Id, Name, Version, context);

      return application;
   }
   #endregion
}

/// <summary>
///   Represents a builder for a general application.
/// </summary>
public sealed class ApplicationBuilder : ApplicationBuilder<ApplicationBuilder> { }

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
   #region Methods
   /// <summary>Uses the default context provider.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithDefaultProvider<TSelf>(this TSelf builder)
   where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithProvider<TSelf, DefaultContextProvider>();
   }
   #endregion
}
