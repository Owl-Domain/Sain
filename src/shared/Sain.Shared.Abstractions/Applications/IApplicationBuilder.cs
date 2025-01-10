namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public interface IApplicationBuilder<TSelf> where TSelf : IApplicationBuilder<TSelf>
{
   #region Methods
   /// <summary>Sets the unique id of the application.</summary>
   /// <param name="applicationId">The unique id of the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the id of the application has already been specified.</exception>
   TSelf WithId(string applicationId);

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
   /// <param name="customise">The (optional) callback that can be used to customise the <paramref name="provider"/>.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>Only a single instance for the same context provider type will be added.</remarks>
   TSelf WithProvider(IContextProvider provider, Action<IContextProvider>? customise = null);

   /// <summary>Creates an instance of a context provider for the given type <typeparamref name="T"/> and uses it when resolving requested application contexts.</summary>
   /// <typeparam name="T">The type of the context provider to create and use.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the created provider.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>Only a single instance for the same context provider type will be added.</remarks>
   TSelf WithProvider<T>(Action<IContextProvider>? customise = null) where T : notnull, IContextProvider, new();

   /// <summary>Uses the context providers that the application builder thinks of as the default providers.</summary>
   /// <returns>The used builder instance.</returns>
   TSelf WithDefaultProviders();

   /// <summary>Makes the given <paramref name="context"/> available to the application.</summary>
   /// <param name="context">The context to make available.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if a context of the same kind has already been included.</exception>
   TSelf WithContext(IContext context);

   /// <summary>Provides a context of the given type <typeparamref name="T"/> and customises it with the given callback.</summary>
   /// <typeparam name="T">The type of the context to provide.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the obtained context.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if a context of the given type <typeparamref name="T"/> couldn't be obtained from the registered context providers.
   /// </exception>
   TSelf WithContext<T>(Action<T>? customise = null) where T : notnull, IContext;

   /// <summary>Tries to provide a context of the given type <typeparamref name="T"/> and customises it with the given callback.</summary>
   /// <typeparam name="T">The type of the context to try and provide.</typeparam>
   /// <param name="customise">The (optional) callback that can be used to customise the obtained context.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if a context of the given type <typeparamref name="T"/> couldn't be obtained from the registered context providers.
   /// </exception>
   TSelf TryWithContext<T>(Action<T>? customise = null) where T : notnull, IContext;

   /// <summary>Checks whether the application builder has been provided a context of the given <paramref name="contextKind"/>.</summary>
   /// <param name="contextKind">The kind of the context to check for.</param>
   /// <returns>
   ///   <see langword="true"/> if a context for the given <paramref name="contextKind"/>
   ///   has been provided, <see langword="false"/> otherwise.
   /// </returns>
   bool HasContext(string contextKind);

   /// <summary>Invokes the given <paramref name="customise"/> callback on every context provider of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the context providers to call the <paramref name="customise"/> callback on.</typeparam>
   /// <param name="customise">The callback to call on every context provider of the given type <typeparamref name="T"/>.</param>
   /// <returns>The used builder instance.</returns>
   TSelf CustomiseProviders<T>(Action<T> customise) where T : notnull, IContextProvider;

   /// <summary>Invokes the given <paramref name="customise"/> callback on every context of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the contexts to call the <paramref name="customise"/> callback on.</typeparam>
   /// <param name="customise">The callback to call on every contexts of the given type <typeparamref name="T"/>.</param>
   /// <returns>The used builder instance.</returns>
   TSelf CustomiseContexts<T>(Action<T> customise) where T : notnull, IContext;

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   IApplicationBase Build();
   #endregion
}

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
public interface IApplicationBuilder<TSelf, TContext> : IApplicationBuilder<TSelf>
   where TSelf : IApplicationBuilder<TSelf, TContext>
   where TContext : IApplicationContext
{
   #region Methods
   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   new IApplication<TContext> Build();
   IApplicationBase IApplicationBuilder<TSelf>.Build() => Build();
   #endregion
}

/// <summary>
///   Represents a builder for an application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
public interface IApplicationBuilder<TSelf, TContext, TApplication> : IApplicationBuilder<TSelf, TContext>
   where TSelf : IApplicationBuilder<TSelf, TContext, TApplication>
   where TContext : IApplicationContext
   where TApplication : IApplication<TContext>
{
   #region Methods
   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   new TApplication Build();
   IApplicationBase IApplicationBuilder<TSelf>.Build() => Build();
   IApplication<TContext> IApplicationBuilder<TSelf, TContext>.Build() => Build();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/>.
/// </summary>
public static class IApplicationBuilderExtensions
{
   #region Methods
   /// <summary>Provides the <see cref="ILoggingContext"/> to the application and optionally customises it with the given callback.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The (optional) callback that can be used to customise the logging context.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithLogging<TSelf>(this TSelf builder, Action<ILoggingContext>? customise = null)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithContext(customise);
   }

   /// <summary>Calls the given <paramref name="customise"/> callback on the registered <see cref="ILoggingContext"/>.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The callback to invoke on the registered <see cref="ILoggingContext"/>.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf CustomiseLogging<TSelf>(this TSelf builder, Action<ILoggingContext> customise)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.CustomiseContexts(customise);
   }

   /// <summary>Provides the <see cref="IMouseInputContext"/> to the application and optionally customises it with the given callback.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The (optional) callback that can be used to customise the mouse input context.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithMouseInput<TSelf>(this TSelf builder, Action<IMouseInputContext>? customise = null)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithContext(customise);
   }

   /// <summary>Calls the given <paramref name="customise"/> callback on the registered <see cref="IMouseInputContext"/>.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The callback to invoke on the registered <see cref="ILoggingContext"/>.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf CustomiseMouseInput<TSelf>(this TSelf builder, Action<IMouseInputContext> customise)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.CustomiseContexts(customise);
   }

   /// <summary>Provides the <see cref="IKeyboardInputContext"/> to the application and optionally customises it with the given callback.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The (optional) callback that can be used to customise the keyboard input context.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithKeyboardInput<TSelf>(this TSelf builder, Action<IKeyboardInputContext>? customise = null)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithContext(customise);
   }

   /// <summary>Calls the given <paramref name="customise"/> callback on the registered <see cref="IKeyboardInputContext"/>.</summary>
   /// <typeparam name="TSelf">The type of the <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="customise">The callback to invoke on the registered <see cref="IKeyboardInputContext"/>.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf CustomiseKeyboardInput<TSelf>(this TSelf builder, Action<IKeyboardInputContext> customise)
      where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.CustomiseContexts(customise);
   }
   #endregion
}
