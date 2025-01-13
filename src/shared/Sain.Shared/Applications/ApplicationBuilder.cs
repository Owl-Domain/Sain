namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
public abstract class ApplicationBuilder<TSelf, TContext, TApplication> : BaseApplicationBuilder<TSelf, TContext, TApplication>
   where TSelf : ApplicationBuilder<TSelf, TContext, TApplication>
   where TContext : IApplicationContext
   where TApplication : IApplication<TContext>
{
}

/// <summary>
///   Represents a builder for a general application.
/// </summary>
public sealed class ApplicationBuilder : ApplicationBuilder<ApplicationBuilder, IApplicationContext, IApplication>
{
   #region Methods
   /// <inheritdoc/>
   protected override IApplication BuildCore(IApplicationInfo info)
   {
      ApplicationContext context = new(ApplicationProviders, Contexts);
      Application application = new(info, context);

      return application;
   }
   #endregion
}
