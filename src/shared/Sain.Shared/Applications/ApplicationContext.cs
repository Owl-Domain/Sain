namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public class ApplicationContext : BaseHasApplicationInit, IApplicationContext
{
   #region Nested types
   [DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
   private sealed class Component(IApplicationComponent? ApplicationInit)
   {
      #region Properties
      public IApplicationComponent? ApplicationInit { get; } = ApplicationInit;
      public List<Component> Dependencies { get; } = [];
      #endregion

      #region Methods
      private string DebuggerDisplay() => $"Component: {ApplicationInit?.GetType().Name ?? "EntryPoint"}";
      #endregion
   }
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IReadOnlyCollection<IContextProvider> ContextProviders { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IContext> Contexts { get; }

   /// <inheritdoc/>
   public IReadOnlyList<IApplicationComponent> InitialisationOrder { get; }

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

   /// <inheritdoc/>
   public ISystemContextGroup System { get; }
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
      System = SystemContextGroup.Create(this);

      InitialisationOrder = CalculateInitialisationOrder();
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
   protected override void OnAttach()
   {
      // Note(Nightowl):
      // Attachment order doesn't matter, but we already have a list
      // of both contexts and providers so might as well use it;

      foreach (IApplicationComponent component in InitialisationOrder)
         component.Attach(Application);
   }

   /// <inheritdoc/>
   protected override void OnInitialise()
   {
      foreach (IApplicationComponent component in InitialisationOrder)
         component.Initialise();
   }

   /// <inheritdoc/>
   protected override void OnCleanup()
   {
      for (int i = InitialisationOrder.Count - 1; i >= 0; i--)
      {
         IApplicationComponent component = InitialisationOrder[i];
         component.Cleanup();
      }
   }

   /// <inheritdoc/>
   protected override void OnDetach()
   {
      // Note(Nightowl):
      // Detachment order doesn't matter, but we already have a list
      // of both contexts and providers so might as well use it;

      foreach (IApplicationComponent component in InitialisationOrder)
         component.Detach();
   }
   #endregion

   #region Helpers
   private List<IApplicationComponent> CalculateInitialisationOrder()
   {
      List<Component> components = [];
      HashSet<Component> resolved = [];
      HashSet<Component> unresolved = [];

      bool TryGetProvider(IContext context, [NotNullWhen(true)] out Component? component)
      {
         if (context.Provider is null)
         {
            component = default;
            return false;
         }

         foreach (Component current in components)
         {
            if (current.ApplicationInit == context.Provider)
            {
               component = current;
               return true;
            }
         }

         component = default;
         return false;
      }
      bool TryGetComponent(Type type, [NotNullWhen(true)] out Component? component)
      {
         foreach (Component current in components)
         {
            Debug.Assert(current.ApplicationInit is not null);

            Type currentType = current.ApplicationInit.GetType();
            if (type.IsAssignableFrom(currentType))
            {
               component = current;
               return true;
            }
         }

         component = default;
         return false;
      }
      void Resolve(Component component)
      {
         unresolved.Add(component);

         foreach (Component edge in component.Dependencies)
         {
            if (resolved.Contains(edge))
               continue;

            if (unresolved.Contains(edge))
               throw new InvalidOperationException($"Circular dependency detected ({component.ApplicationInit}) -> ({edge.ApplicationInit}).");

            Resolve(edge);
         }

         resolved.Add(component);
         unresolved.Remove(component);
      }

      foreach (IContextProvider provider in ContextProviders)
         components.Add(new(provider));

      foreach (IContext context in Contexts)
      {
         // Note(Nightowl): Unavailable contexts cannot have dependencies;

         if (context.IsAvailable)
            components.Add(new(context));
      }

      // Note(Nightowl): Make every component depend on the context provider that it comes from;
      foreach (Component component in components)
      {
         if (component.ApplicationInit is IContext context && TryGetProvider(context, out Component? provider))
            component.Dependencies.Add(provider);
      }

      // Add the dependencies based on the context kinds;
      foreach (Component component in components)
      {
         Debug.Assert(component.ApplicationInit is not null);

         foreach (Type type in component.ApplicationInit.InitialiseAfter)
         {
            if (TryGetComponent(type, out Component? context))
               component.Dependencies.Add(context);
         }

         foreach (Type type in component.ApplicationInit.InitialiseBefore)
         {
            if (TryGetComponent(type, out Component? context))
               context.Dependencies.Add(component);
         }
      }

      // Note(Nightowl): Make a final component that relies on everything;
      Component final = new(null);
      foreach (Component component in components)
         final.Dependencies.Add(component);

      Resolve(final);

      List<IApplicationComponent> order = [];
      foreach (Component component in resolved)
      {
         if (component.ApplicationInit is not null)
            order.Add(component.ApplicationInit);
      }

      return order;
   }
   #endregion
}
