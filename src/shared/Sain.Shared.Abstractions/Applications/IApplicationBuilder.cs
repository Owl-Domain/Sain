namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
public interface IApplicationBuilder
{
   #region Methods
   /// <summary>Uses the given context <paramref name="provider"/> when resolving requested application contexts.</summary>
   /// <param name="provider">The context provider to use.</param>
   /// <returns>The used builder instance.</returns>
   IApplicationBuilder WithProvider(IContextProvider provider);

   /// <summary>Makes the context of the given type <typeparamref name="T"/> available to the built application.</summary>
   /// <typeparam name="T">The type of the context to make available to the built application.</typeparam>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if no context of the given type <typeparamref name="T"/> could be provided.</exception>
   IApplicationBuilder WithContext<T>() where T : notnull, IContext;

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   IApplication Build();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder"/>.
/// </summary>
public static class IApplicationBuilderExtensions
{
   #region Methods
   /// <summary>Creates a new instance of, and uses a context provider of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the context provider to create and use.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static IApplicationBuilder WithProvider<T>(this IApplicationBuilder builder) where T : IContextProvider, new()
   {
      T provider = new();
      return builder.WithProvider(provider);
   }
   #endregion
}
