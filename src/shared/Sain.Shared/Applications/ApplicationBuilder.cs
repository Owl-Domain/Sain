namespace Sain.Shared.Applications;

/// <summary>
///   Represents a builder for an application.
/// </summary>
public sealed class ApplicationBuilder : IApplicationBuilder
{
   #region Fields
   private readonly HashSet<IContextProvider> _availableProviders = [];
   private readonly HashSet<IContextProvider> _usedProviders = [];
   private readonly HashSet<IContext> _providedContexts = [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   public IApplicationBuilder WithProvider(IContextProvider provider)
   {
      _availableProviders.Add(provider);
      return this;
   }

   /// <inheritdoc/>
   public IApplicationBuilder WithContext<T>() where T : notnull, IContext
   {
      foreach (IContextProvider provider in _availableProviders)
      {
         if (provider.TryProvide<T>(out T? context))
         {
            _usedProviders.Add(provider);
            _providedContexts.Add(context);

            return this;
         }
      }

      throw new InvalidOperationException($"Could not find any contexts for the requested type ({typeof(T)}).");
   }

   /// <inheritdoc/>
   public IApplication Build()
   {
      if (HasContext<IAudioPlaybackContext>() is false) _providedContexts.Add(new UnavailableAudioPlaybackContext());
      if (HasContext<IAudioCaptureContext>() is false) _providedContexts.Add(new UnavailableAudioCaptureContext());

      ApplicationContext context = new(_providedContexts);
      Application application = new(context, _usedProviders);

      return application;
   }
   #endregion

   #region Helpers
   private bool HasContext<T>() where T : notnull, IContext
   {
      foreach (IContext context in _providedContexts)
      {
         if (context is T)
            return true;
      }

      return false;
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
