namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public class ApplicationContext : BaseHasApplicationInit, IApplicationContext
{
   #region Fields
   private readonly List<IHasApplicationInit> _initialisationOrder;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IReadOnlyCollection<IContextProvider> ContextProviders { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IContext> Contexts { get; }

   /// <inheritdoc/>
   public IDispatcherContext Dispatcher { get; }

   /// <inheritdoc/>
   public ILoggingContext Logging { get; }

   /// <inheritdoc/>
   public IDisplayContext Display { get; }

   /// <inheritdoc/>
   public IInputContextGroup Input { get; }

   /// <inheritdoc/>
   public IAudioContextGroup Audio { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationContext"/>.</summary>
   /// <param name="contextProviders">The context providers that are available to the application.</param>
   /// <param name="contexts">The contexts that are available to the application.</param>
   public ApplicationContext(IReadOnlyCollection<IContextProvider> contextProviders, IReadOnlyCollection<IContext> contexts)
   {
      ContextProviders = contextProviders;
      Contexts = contexts;

      Dispatcher = GetContext<IDispatcherContext>();
      Logging = GetContext<ILoggingContext>();
      Display = GetContext<IDisplayContext>();

      Input = InputContextGroup.Create(this);
      Audio = AudioContextGroup.Create(this);

      _initialisationOrder = CalculateInitialisationOrder();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool HasContext<T>() where T : notnull, IContext
   {
      foreach (IContext current in Contexts)
      {
         if (current is T)
            return true;
      }

      return false;
   }

   /// <inheritdoc/>
   public bool IsContextAvailable<T>() where T : notnull, IContext
   {
      foreach (IContext current in Contexts)
      {
         if (current is T typed && typed.IsAvailable)
            return true;
      }

      return false;
   }

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
   public bool TryGetContextIfAvailable<T>([MaybeNullWhen(false)] out T context) where T : notnull, IContext
   {
      foreach (IContext current in Contexts)
      {
         if (current is T typed && typed.IsAvailable)
         {
            context = typed;
            return true;
         }
      }

      context = default;
      return false;
   }

   /// <inheritdoc/>
   protected override void Initialise()
   {
      foreach (IHasApplicationInit component in _initialisationOrder)
      {
         if (component.IsInitialised is false)
            component.Initialise(Application);
      }
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      for (int i = _initialisationOrder.Count - 1; i >= 0; i--)
      {
         IHasApplicationInit component = _initialisationOrder[i];

         if (component.IsInitialised)
            component.Cleanup(Application);
      }
   }
   #endregion

   #region Helpers
   private List<IHasApplicationInit> CalculateInitialisationOrder()
   {
      return [
         .. ContextProviders,
         .. Contexts
      ];
   }
   #endregion
}
