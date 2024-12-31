using System.Reflection;

namespace Sain.Shared.Applications;

/// <summary>
///   Represents the base implementation for an application builder.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public abstract class BaseApplicationBuilder<TSelf> : IApplicationBuilder<TSelf>
   where TSelf : BaseApplicationBuilder<TSelf>
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string? _name;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string? _id;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IVersion? _version;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly HashSet<IContextProvider> _availableProviders = [];
   private readonly Dictionary<string, IContext> _providedContexts = [];
   #endregion

   #region Properties
   /// <summary>The typed instance of the builder.</summary>
   protected TSelf Instance => (TSelf)this;

   /// <summary>The collection of the available providers.</summary>
   protected IReadOnlyCollection<IContextProvider> Providers => _availableProviders;

   /// <summary>The collection of the provided contexts.</summary>
   protected IReadOnlyCollection<IContext> Contexts => _providedContexts.Values;

   /// <summary>The name of the application.</summary>
   protected string Name => _name ?? throw new InvalidOperationException("The name of the application has not been set yet.");

   /// <summary>The unique id of the application.</summary>
   protected string Id => _id ?? throw new InvalidOperationException("The unique id of the application has not been set yet.");

   /// <summary>The version of the application.</summary>
   protected IVersion Version => _version ?? throw new InvalidOperationException("The version of the application has not been set yet.");
   #endregion

   #region Methods
   /// <inheritdoc/>
   public TSelf WithId(string applicationId)
   {
      if (_id is not null)
         throw new InvalidOperationException("The unique id of the application has already been set.");

      _id = applicationId;

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithName(string applicationName)
   {
      if (_name is not null)
         throw new InvalidOperationException("The name of the application has already been set.");

      _name = applicationName;

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithVersion(IVersion applicationVersion)
   {
      if (_version is not null)
         throw new InvalidOperationException("The version of the application has already been set.");

      _version = applicationVersion;

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithProvider(IContextProvider provider, Action<IContextProvider>? customise = null)
   {
      Type type = provider.GetType();
      foreach (IContextProvider current in _availableProviders)
      {
         // Note(Nightowl): Don't add providers of the same type multiple times;
         if (type == current.GetType())
            break;
      }

      _availableProviders.Add(provider);
      customise?.Invoke(provider);

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithProvider<T>(Action<IContextProvider>? customise = null) where T : notnull, IContextProvider, new()
   {
      foreach (IContextProvider current in _availableProviders)
      {
         if (current is T)
            break;
      }

      T provider = new();
      _availableProviders.Add(provider);
      customise?.Invoke(provider);

      return Instance;
   }

   /// <inheritdoc/>
   public virtual TSelf WithDefaultProviders()
   {
      WithProvider<DefaultContextProvider>();

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
         if (provider.TryProvide<T>(out T? context) is false)
            continue;

         if (_providedContexts.TryAdd(context.Kind, context))
         {
            customise?.Invoke(context);
            return Instance;
         }

         IContext existing = _providedContexts[context.Kind];
         if (existing is T typed)
         {
            customise?.Invoke(context);
            return Instance;
         }

         throw new InvalidOperationException($"A different implementation ({existing}) of the context for the requested type ({typeof(T)}) has already been provided.");
      }

      throw new InvalidOperationException($"A context of the requested type ({typeof(T)}) could not be obtained.");
   }

   /// <inheritdoc/>
   public TSelf TryWithContext<T>(Action<T>? customise = null) where T : notnull, IContext
   {
      foreach (IContextProvider provider in _availableProviders)
      {
         if (provider.TryProvide<T>(out T? context))
         {
            _providedContexts.TryAdd(context.Kind, context);
            customise?.Invoke(context);

            return Instance;
         }
      }

      return Instance;
   }

   /// <inheritdoc/>
   public bool HasContext(string contextKind) => _providedContexts.ContainsKey(contextKind);

   /// <inheritdoc/>
   public TSelf CustomiseProviders<T>(Action<T> customise) where T : notnull, IContextProvider
   {
      foreach (IContextProvider current in _availableProviders)
      {
         if (current is T typed)
            customise.Invoke(typed);
      }

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf CustomiseContexts<T>(Action<T> customise) where T : notnull, IContext
   {
      foreach (IContext current in _providedContexts.Values)
      {
         if (current is T typed)
            customise.Invoke(typed);
      }

      return Instance;
   }

   /// <inheritdoc/>
   public IApplication Build()
   {
      Assembly targetAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
      _name ??= targetAssembly.GetName().Name ?? throw new InvalidOperationException("Couldn't extract the application name from the assembly information.");
      _id ??= _name;

      if (_version is null)
         Instance.WithVersionFromAssembly(targetAssembly);

      AddRequiredContexts();
      AddDefaultContexts();
      AddUnavailableContexts();

      return BuildCore();
   }

   /// <summary>Adds the contexts that are deemed to be required.</summary>
   /// <remarks>You should use the <see cref="AddRequiredContext{T}(string)"/> method inside this method.</remarks>
   protected virtual void AddRequiredContexts()
   {
      AddRequiredContext<IDispatcherContext>(CoreContextKinds.Dispatcher);
   }

   /// <summary>Adds the default implementations for commonly used contexts.</summary>
   /// <remarks>You should use the <see cref="TryRequestContext{T}(string)"/> method inside this method.</remarks>
   protected virtual void AddDefaultContexts()
   {
      TryRequestContext<ILoggingContext>(CoreContextKinds.Logging);
   }

   /// <summary>Adds the unavailable implementations for commonly used contexts (contexts that are directly provided by the <see cref="IApplicationContext"/>).</summary>
   /// <remarks>
   ///   You should use the <see cref="TryAddUnavailableContext{T}(string)"/> method inside this method, this should
   ///   be done for every context that is directly provided by the built <see cref="IApplicationContext"/>.
   /// </remarks>
   protected virtual void AddUnavailableContexts()
   {
      TryAddUnavailableContext<UnavailableLoggingContext>(CoreContextKinds.Logging);
      TryAddUnavailableContext<UnavailableDisplayContext>(CoreContextKinds.Display);

      TryAddUnavailableContext<UnavailableKeyboardInputContext>(CoreContextKinds.KeyboardInput);
      TryAddUnavailableContext<UnavailableMouseInputContext>(CoreContextKinds.MouseInput);

      TryAddUnavailableContext<UnavailableAudioPlaybackContext>(CoreContextKinds.AudioPlayback);
      TryAddUnavailableContext<UnavailableAudioCaptureContext>(CoreContextKinds.AudioCapture);
   }

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   protected abstract IApplication BuildCore();
   #endregion

   #region Helpers
   /// <summary>Adds a context of the given type <typeparamref name="T"/> as a required context.</summary>
   /// <typeparam name="T">The type of the <see cref="IContext"/> to add.</typeparam>
   /// <param name="kind">The kind of the context.</param>
   /// <remarks>Use this when a context is always required.</remarks>
   protected void AddRequiredContext<T>(string kind) where T : notnull, IContext
   {
      if (HasContext(kind) is false)
         WithContext<T>();
   }

   /// <summary>Tries to request a context of the given type <typeparamref name="TContext"/> if a context of the same <paramref name="kind"/> hasn't already been added.</summary>
   /// <typeparam name="TContext">The type of the context to try to add.</typeparam>
   /// <param name="kind">The kind of the context to try to add.</param>
   /// <remarks>
   ///   Use this to add default implementations for contexts that haven't been explicitly added, but that will typically be wanted.
   /// </remarks>
   protected void TryRequestContext<TContext>(string kind)
      where TContext : notnull, IContext
   {
      if (HasContext(kind) is false)
         TryWithContext<TContext>();
   }

   /// <summary>Tries to add an unavailable implementation of a context if a context of the same <paramref name="kind"/> hasn't already been added.</summary>
   /// <typeparam name="T">
   ///   The type of the unavailable implementation. An unavailable implementation is a context implementation that is always
   ///   unavailable, primarily used for contexts that are available by default in the <see cref="IApplicationContext"/>.
   /// </typeparam>
   /// <param name="kind">The kind of the context to try to add.</param>
   /// <exception cref="InvalidOperationException">Thrown if the context of the type <typeparamref name="T"/> isn't marked as unavailable.</exception>
   protected void TryAddUnavailableContext<T>(string kind) where T : notnull, IContext, new()
   {
      if (HasContext(kind) is false)
      {
         T context = new();

         if (context.IsAvailable)
            throw new InvalidOperationException($"The context of the given type ({typeof(T)}) is expected to always be unavailable.");

         WithContext(context);
      }
   }
   #endregion
}
