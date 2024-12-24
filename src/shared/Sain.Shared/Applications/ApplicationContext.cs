namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public class ApplicationContext : IApplicationContext
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IApplication? _application;

   private bool _initialised;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IReadOnlyCollection<IContext> Contexts { get; }

   /// <inheritdoc/>
   public IAudioContextGroup Audio { get; }

   /// <inheritdoc/>
   public IDispatcherContext Dispatcher { get; }

   /// <summary>Whether the context has been initialised.</summary>
   protected bool IsInitialised => _initialised;

   /// <summary>The application that the context belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the context has not been initialised.</exception>
   [NotNull]
   protected IApplication? Application
   {
      get => _application ?? throw new InvalidOperationException($"The context doesn't belong to an application yet, wait for it to be initialised.");
      private set => _application = value;
   }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationContext"/>.</summary>
   /// <param name="contexts">The contexts that are available to the application.</param>
   public ApplicationContext(IReadOnlyCollection<IContext> contexts)
   {
      Contexts = contexts;

      IAudioPlaybackContext audioPlayback = GetContext<IAudioPlaybackContext>();
      IAudioCaptureContext audioCapture = GetContext<IAudioCaptureContext>();
      Audio = new AudioContextGroup(audioPlayback, audioCapture);

      Dispatcher = GetContext<IDispatcherContext>();
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
      foreach (IContext current in Contexts)
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
   public void Initialise(IApplication application)
   {
      if (_initialised)
      {
         if (Application != application)
            throw new ArgumentException($"The application context has already been initialised for a different application.", nameof(application));

         return;
      }

      Application = application;
      foreach (IContext context in Contexts)
         context.Initialise(application);

      Initialise();
      _initialised = true;
   }

   /// <summary>Initialises the application context.</summary>
   protected virtual void Initialise() { }

   /// <inheritdoc/>
   public void Cleanup(IApplication application)
   {
      if (_initialised is false)
         return;

      if (Application != application)
         throw new ArgumentException($"The application context has already been initialised for a different application.", nameof(application));

      _initialised = false;
      Cleanup();

      foreach (IContext context in Contexts)
         context.Cleanup(application);

      Application = null;
   }

   /// <summary>Cleans up the application context.</summary>
   protected virtual void Cleanup() { }
   #endregion
}
