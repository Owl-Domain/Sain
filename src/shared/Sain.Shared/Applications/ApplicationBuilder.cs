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
