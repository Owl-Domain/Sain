
namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public class ApplicationContext : BaseHasApplicationInit, IApplicationContext
{
   #region Properties
   /// <inheritdoc/>
   public IReadOnlyCollection<IContextProvider> ContextProviders { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IContext> Contexts { get; }

   /// <inheritdoc/>
   public IAudioContextGroup Audio { get; }

   /// <inheritdoc/>
   public IDispatcherContext Dispatcher { get; }

   /// <inheritdoc/>
   public ILoggingContext Logging { get; }

   /// <inheritdoc/>
   public IDisplayContext Display { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationContext"/>.</summary>
   /// <param name="contextProviders">The context providers that are available to the application.</param>
   /// <param name="contexts">The contexts that are available to the application.</param>
   public ApplicationContext(IReadOnlyCollection<IContextProvider> contextProviders, IReadOnlyCollection<IContext> contexts)
   {
      ContextProviders = contextProviders;
      Contexts = contexts;

      Audio = AudioContextGroup.Create(this);

      Dispatcher = GetContext<IDispatcherContext>();
      Logging = GetContext<ILoggingContext>();
      Display = GetContext<IDisplayContext>();
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
   protected override void PreInitialise()
   {
      foreach (IContextProvider provider in ContextProviders)
         provider.PreInitialise(Application);

      foreach (IContext context in Contexts)
         context.PreInitialise(Application);
   }

   /// <inheritdoc/>
   protected override void Initialise()
   {
      foreach (IContextProvider provider in ContextProviders)
         provider.Initialise(Application);

      foreach (IContext context in Contexts)
         context.Initialise(Application);
   }

   /// <inheritdoc/>
   protected override void PostInitialise()
   {
      foreach (IContextProvider provider in ContextProviders)
         provider.PostInitialise(Application);

      foreach (IContext context in Contexts)
         context.PostInitialise(Application);
   }

   /// <inheritdoc/>
   protected override void PreCleanup()
   {
      foreach (IContext context in Contexts)
         context.PreCleanup(Application);

      foreach (IContextProvider provider in ContextProviders)
         provider.PreCleanup(Application);
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      foreach (IContext context in Contexts)
         context.Cleanup(Application);

      foreach (IContextProvider provider in ContextProviders)
         provider.Cleanup(Application);
   }

   /// <inheritdoc/>
   protected override void PostCleanup()
   {
      foreach (IContext context in Contexts)
         context.PostCleanup(Application);

      foreach (IContextProvider provider in ContextProviders)
         provider.PostCleanup(Application);
   }
   #endregion
}
