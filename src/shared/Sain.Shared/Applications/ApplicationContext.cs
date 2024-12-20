namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public sealed class ApplicationContext : IApplicationContext
{
   #region Fields
   private bool _initialised;
   private readonly IReadOnlyCollection<IContext> _contexts;
   private readonly IReadOnlyCollection<IContextProvider> _usedProviders;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IAudioContextGroup Audio { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationContext"/>.</summary>
   /// <param name="contexts">The contexts that are available to the application.</param>
   /// <param name="usedProviders">The provides that have been used to provide the given <paramref name="contexts"/>.</param>
   public ApplicationContext(IReadOnlyCollection<IContext> contexts, IReadOnlyCollection<IContextProvider> usedProviders)
   {
      _contexts = contexts;
      _usedProviders = usedProviders;

      IAudioPlaybackContext audioPlayback = GetContext<IAudioPlaybackContext>();
      IAudioCaptureContext audioCapture = GetContext<IAudioCaptureContext>();
      Audio = new AudioContextGroup(audioPlayback, audioCapture);
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public T GetContext<T>() where T : notnull, IContext
   {
      if (TryGetContext<T>(out T? context))
         return context;

      throw new InvalidOperationException($"No context of the type ({typeof(T)}) is available.");
   }

   /// <inheritdoc/>
   public bool TryGetContext<T>([MaybeNullWhen(false)] out T context) where T : notnull, IContext
   {
      foreach (IContext current in _contexts)
      {
         if (current is T typed)
         {
            context = typed;
            return true;
         }
      }

      context = default;
      return false;
   }

   /// <inheritdoc/>
   public async Task InitialiseAsync()
   {
      if (_initialised)
         return;

      foreach (IContextProvider provider in _usedProviders)
         await provider.BeforeContextsInitialisedAsync(this);

      foreach (IContext context in _contexts)
         await context.InitialiseAsync(this);

      foreach (IContextProvider provider in _usedProviders)
         await provider.AfterContextsInitialisedAsync(this);

      _initialised = true;
   }

   /// <inheritdoc/>
   public async Task CleanupAsync()
   {
      if (_initialised is false)
         return;

      _initialised = false;

      foreach (IContextProvider provider in _usedProviders)
         await provider.BeforeContextsCleanedUpAsync(this);

      foreach (IContext context in _contexts)
         await context.CleanupAsync(this);

      foreach (IContextProvider provider in _usedProviders)
         await provider.AfterContextsCleanedUpAsync(this);
   }
   #endregion
}
