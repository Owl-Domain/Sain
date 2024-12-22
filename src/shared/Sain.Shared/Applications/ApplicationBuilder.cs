namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
public sealed class ApplicationBuilder : IApplicationBuilder
{
   #region Fields
   private readonly HashSet<IContextProvider> _availableProviders = [];
   private readonly Dictionary<string, IContext> _providedContexts = [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   public IApplicationBuilder WithProvider(IContextProvider provider)
   {
      _availableProviders.Add(provider);
      return this;
   }

   /// <inheritdoc/>
   public IApplicationBuilder WithContext(IContext context)
   {
      if (_providedContexts.TryAdd(context.Kind, context) is false)
         throw new ArgumentException($"A context of the same kind ({context.Kind}) has already been included.", nameof(context));

      return this;
   }

   /// <inheritdoc/>
   public IApplicationBuilder WithContext<T>(Action<T>? customise = null) where T : notnull, IContext
   {
      foreach (IContextProvider provider in _availableProviders)
      {
         if (provider.TryProvide<T>(out T? context))
         {
            WithContext(context);
            customise?.Invoke(context);

            return this;
         }
      }

      throw new InvalidOperationException($"A context of the requested type ({typeof(T)}) could not be obtained.");
   }

   /// <inheritdoc/>
   public IApplication Build()
   {
      if (_providedContexts.ContainsKey(CoreContextKinds.AudioPlayback) is false) WithContext(new UnavailableAudioPlaybackContext());
      if (_providedContexts.ContainsKey(CoreContextKinds.AudioCapture) is false) WithContext(new UnavailableAudioCaptureContext());
      if (_providedContexts.ContainsKey(CoreContextKinds.Dispatcher) is false) WithContext<IDispatcherContext>();

      ApplicationContext context = new(_providedContexts.Values);
      Application application = new(context);

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
   public static IApplicationBuilder WithDefaultProvider(this IApplicationBuilder builder) => builder.WithProvider<DefaultContextProvider>();
   #endregion
}
