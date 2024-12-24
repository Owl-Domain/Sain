namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public class ApplicationContext : IApplicationContext
{
   #region Fields
   private bool _initialised;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IReadOnlyCollection<IContext> Contexts { get; }

   /// <inheritdoc/>
   public IAudioContextGroup Audio { get; }

   /// <inheritdoc/>
   public IDispatcherContext Dispatcher { get; }
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
         return;

      foreach (IContext context in Contexts)
         context.Initialise(application);

      _initialised = true;
   }

   /// <inheritdoc/>
   public void Cleanup(IApplication application)
   {
      if (_initialised is false)
         return;

      _initialised = false;

      foreach (IContext context in Contexts)
         context.Cleanup(application);
   }
   #endregion
}
