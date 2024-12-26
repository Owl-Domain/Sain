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
   private string? _name;
   private IVersion? _version;
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
   protected string? Id { get; private set; }

   /// <summary>The version of the application.</summary>
   protected IVersion Version => _version ?? throw new InvalidOperationException("The version of the application has not been set yet.");
   #endregion

   #region Methods
   /// <inheritdoc/>
   public TSelf WithId(string applicationId)
   {
      if (Id is not null)
         throw new InvalidOperationException("The unique id of the application has already been set.");

      Id = applicationId;

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
   public IApplication Build()
   {
      Assembly targetAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
      _name ??= targetAssembly.GetName().Name ?? throw new InvalidOperationException("Couldn't extract the application name from the assembly information.");

      if (_version is null)
         Instance.WithVersionFromAssembly(targetAssembly);

      AddRequired<IDispatcherContext>(CoreContextKinds.Dispatcher);

      TryAddDefault<ILoggingContext, UnavailableLoggingContext>(CoreContextKinds.Logging);
      TryAddDefault<IDisplayContext, UnavailableDisplayContext>(CoreContextKinds.Display);

      TryAddUnavailable<UnavailableAudioPlaybackContext>(CoreContextKinds.AudioPlayback);
      TryAddUnavailable<UnavailableAudioCaptureContext>(CoreContextKinds.AudioCapture);

      return BuildCore();
   }

   /// <summary>Builds the application.</summary>
   /// <returns>The built application.</returns>
   protected abstract IApplication BuildCore();
   #endregion

   #region Helpers
   private void AddRequired<T>(string kind) where T : notnull, IContext
   {
      if (HasContext(kind) is false)
         WithContext<T>();
   }
   private void TryAddDefault<TContext, TUnavailable>(string kind)
      where TContext : notnull, IContext
      where TUnavailable : notnull, TContext, new()
   {
      if (HasContext(kind) is false)
         TryWithContext<TContext>();

      TryAddUnavailable<TUnavailable>(kind);
   }
   private void TryAddUnavailable<T>(string kind) where T : notnull, IContext, new()
   {
      if (HasContext(kind) is false)
      {
         T context = new();
         WithContext(context);
      }
   }
   #endregion
}
