namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public interface IApplicationBuilder<TSelf> where TSelf : IApplicationBuilder<TSelf>
{
   #region Methods
   /// <summary>Sets the name of the application.</summary>
   /// <param name="applicationName">The name of the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the name of the application has already been specified.</exception>
   TSelf WithName(string applicationName);

   /// <summary>Sets the version of the application.</summary>
   /// <param name="applicationVersion">The version of the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the version of the application has already been specified.</exception>
   TSelf WithVersion(IVersion applicationVersion);

   /// <summary>Uses the given context <paramref name="provider"/> when resolving requested application contexts.</summary>
   /// <param name="provider">The context provider to use.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithProvider(IContextProvider provider);

   /// <summary>Makes the given <paramref name="context"/> available to the application.</summary>
   /// <param name="context">The context to make available.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if a context of the same kind has already been included.</exception>
   TSelf WithContext(IContext context);

   /// <summary>Requests a context of the given type <typeparamref name="T"/> and customises it with the given callback.</summary>
   /// <typeparam name="T">The type of the context to create.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the obtained context.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if a context of the given type <typeparamref name="T"/> couldn't be obtained from the registered context providers.
   /// </exception>
   TSelf WithContext<T>(Action<T>? customise = null) where T : notnull, IContext;

   /// <summary>Checks whether the application builder has been provided a context of the given <paramref name="contextKind"/>.</summary>
   /// <param name="contextKind">The kind of the context to check for.</param>
   /// <returns>
   ///   <see langword="true"/> if a context for the given <paramref name="contextKind"/>
   ///   has been provided, <see langword="false"/> otherwise.
   /// </returns>
   bool HasContext(string contextKind);

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   IApplication Build();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/>.
/// </summary>
public static class IApplicationBuilderExtensions
{
   #region Methods
   /// <summary>Creates a new instance of, and uses a context provider of the given type <typeparamref name="TProvider"/>.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <typeparam name="TProvider">The type of the context provider to create and use.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the built context provider.</param>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithProvider<TSelf, TProvider>(this TSelf builder, Action<TProvider>? customise = null)
      where TSelf : IApplicationBuilder<TSelf>
      where TProvider : IContextProvider, new()
   {
      TProvider provider = new();
      customise?.Invoke(provider);

      return builder.WithProvider(provider);
   }
   #endregion
}
