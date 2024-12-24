namespace Sain.Shared.Applications;

/// <summary>
///   Represents the base implementation for an application builder.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public abstract class BaseApplicationBuilder<TSelf> : IApplicationBuilder<TSelf>
   where TSelf : BaseApplicationBuilder<TSelf>
{
   #region Fields
   private readonly HashSet<IContextProvider> _availableProviders = [];
   private readonly Dictionary<string, IContext> _providedContexts = [];
   #endregion

   #region Properties
   /// <summary>The instance of the builder.</summary>
   protected abstract TSelf Instance { get; }

   /// <summary>The collection of the available providers.</summary>
   protected IReadOnlyCollection<IContextProvider> Providers => _availableProviders;

   /// <summary>The collection of the provided contexts.</summary>
   protected IReadOnlyCollection<IContext> Contexts => _providedContexts.Values;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public TSelf WithProvider(IContextProvider provider)
   {
      _availableProviders.Add(provider);
      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithContext(IContext context)
   {
      if (_providedContexts.TryAdd(context.Kind, context) is false)
         throw new ArgumentException($"A context of the same kind ({context.Kind}) has already been included.", nameof(context));

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithContext<T>(Action<T>? customise = null) where T : notnull, IContext
   {
      foreach (IContextProvider provider in _availableProviders)
      {
         if (provider.TryProvide<T>(out T? context))
         {
            WithContext(context);
            customise?.Invoke(context);

            return Instance;
         }
      }

      throw new InvalidOperationException($"A context of the requested type ({typeof(T)}) could not be obtained.");
   }

   /// <inheritdoc/>
   public bool HasContext(string contextKind) => _providedContexts.ContainsKey(contextKind);

   /// <inheritdoc/>
   public IApplication Build()
   {
      if (HasContext(CoreContextKinds.AudioPlayback) is false)
         WithContext(new UnavailableAudioPlaybackContext());

      if (HasContext(CoreContextKinds.AudioCapture) is false)
         WithContext(new UnavailableAudioCaptureContext());

      if (HasContext(CoreContextKinds.Dispatcher) is false)
         WithContext<IDispatcherContext>();

      return BuildCore();
   }

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   protected abstract IApplication BuildCore();
   #endregion
}
