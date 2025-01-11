namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a component that has application specific initialisation and cleanup.
/// </summary>
public abstract class BaseHasApplicationInit : ObservableBase, IHasApplicationInit
{
   #region Fields
   // Note(Nightowl): Doesn't have a property, but it's not gonna be useful to see it in the debugger anyway;
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly object _initLock = new();

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IApplicationBase? _application;
   #endregion

   #region Properties
   /// <summary>The application that the component belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the component has not been attached to an application.</exception>
   [NotNull]
   protected IApplicationBase? Application
   {
      get => _application ?? throw new InvalidOperationException($"The ({GetName()}) component hasn't been attached to an application yet.");
   }

   /// <summary>The context of the application that the component belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the component has not been attached to an application.</exception>
   [NotNull]
   protected IApplicationContext? Context
   {
      get => _application?.Context ?? throw new InvalidOperationException($"The ({GetName()}) component hasn't been attached to an application yet.");
   }

   /// <inheritdoc/>
   public bool IsAttached => _application is not null;

   /// <inheritdoc/>
   public bool IsInitialised { get; private set; }

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<string> InitialiseAfterContexts => [];

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<string> InitialiseBeforeContexts => [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Attach(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_application is not null)
            throw new InvalidOperationException($"The ({GetName()}) component has already been attached to an application.");

         _application = application;
         OnAttach();
      }
   }

   /// <summary>Called when the component has been attached to an application.</summary>
   protected virtual void OnAttach() { }

   /// <inheritdoc/>
   public void Initialise()
   {
      lock (_initLock)
      {
         if (IsInitialised)
            throw new InvalidOperationException($"The ({GetName()}) component has already been initialised.");

         OnInitialise();
         IsInitialised = true;
      }
   }

   /// <summary>Called when the component is initialised.</summary>
   protected virtual void OnInitialise() { }

   /// <inheritdoc/>
   public void Cleanup()
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            throw new InvalidOperationException($"The ({GetName()}) component hasn't been initialised yet.");

         try
         {
            OnCleanup();
         }
         finally
         {
            IsInitialised = false;
         }
      }
   }

   /// <summary>Called when the component is cleaned up.</summary>
   protected virtual void OnCleanup() { }

   /// <inheritdoc/>
   public void Detach()
   {
      lock (_initLock)
      {
         if (_application is null)
            throw new InvalidOperationException($"The ({GetName()}) component hasn't been attached to an application yet.");

         if (IsInitialised)
            throw new InvalidOperationException($"The ({GetName()}) component must before cleaned up before it can be detached from the application.");

         try
         {
            OnDetach();
         }
         finally
         {
            _application = null;
         }
      }
   }

   /// <summary>Called when the component has been detached from the application.</summary>
   protected virtual void OnDetach() { }
   #endregion

   #region Helpers
   private string GetName() => GetType().Name;

   /// <summary>Throws an exception if the component has not been initialised yet.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the component has not been initialised yet.</exception>
   protected void ThrowIfNotInitialised()
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            throw new InvalidOperationException($"The {GetName()} has not been initialised yet.");
      }
   }
   #endregion
}
