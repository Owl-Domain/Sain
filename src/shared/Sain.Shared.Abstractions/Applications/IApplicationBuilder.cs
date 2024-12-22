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

   /// <summary>Makes the given <paramref name="context"/> available to the application.</summary>
   /// <param name="context">The context to make available.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if a context of the same kind has already been included.</exception>
   IApplicationBuilder WithContext(IContext context);

   /// <summary>Requests a context of the given type <typeparamref name="T"/> and customises it with the given callback.</summary>
   /// <typeparam name="T">The type of the context to create.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the obtained context.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if a context of the given type <typeparamref name="T"/> couldn't be obtained from the registered context providers.
   /// </exception>
   IApplicationBuilder WithContext<T>(Action<T>? customise = null) where T : notnull, IContext;

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
   /// <param name="customise">The (optional) callback that can be used to customise the built context provider.</param>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static IApplicationBuilder WithProvider<T>(this IApplicationBuilder builder, Action<T>? customise = null) where T : IContextProvider, new()
   {
      T provider = new();
      customise?.Invoke(provider);

      return builder.WithProvider(provider);
   }
   #endregion
}
