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
   /// <inheritdoc/>
   public bool IsInitialised { get; private set; }

   /// <summary>The application that the component belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the component has not been initialised.</exception>
   [NotNull]
   protected IApplicationBase? Application
   {
      get => _application ?? throw new InvalidOperationException($"The {GetName()} doesn't belong to an application yet, wait for it to be initialised.");
      private set => _application = value;
   }

   /// <summary>The context of the application that the component belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the component has not been initialised.</exception>
   [NotNull]
   protected IApplicationContext? Context
   {
      get => _application?.Context ?? throw new InvalidOperationException($"The {GetName()} doesn't belong to an application yet, wait for it to be initialised.");
   }

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<string> DependsOnContexts { get; } = [];
   #endregion

   #region Methods

   /// <inheritdoc/>
   public void Initialise(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (IsInitialised)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been initialised for a different application.", nameof(application));

            return;
         }

         Application = application;

         try
         {
            Initialise();
            IsInitialised = true;
         }
         catch
         {
            Application = null;
            throw;
         }
      }
   }
   /// <summary>Initialises the component.</summary>
   protected virtual void Initialise() { }

   /// <inheritdoc/>
   public void Cleanup(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been cleaned up for a different application.", nameof(application));

            return;
         }

         IsInitialised = false;

         try
         {
            Cleanup();
         }
         finally
         {
            Application = null;
         }
      }
   }

   /// <summary>Cleans up the component.</summary>
   protected virtual void Cleanup() { }
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
