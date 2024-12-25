namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a component that has application specific initialisation and cleanup.
/// </summary>
public abstract class BaseHasApplicationInit : IHasApplicationInit
{
   #region Fields
   // Note(Nightowl): Doesn't have a property, but it's not gonna be useful to see it in the debugger anyway;
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly object _initLock = new();

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IApplication? _application;
   #endregion

   #region Properties
   /// <summary>Whether the component has been initialised.</summary>
   protected bool IsInitialised { get; private set; }

   /// <summary>The application that the component belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the component has not been initialised.</exception>
   [NotNull]
   protected IApplication? Application
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
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Initialise(IApplication application)
   {
      lock (_initLock)
      {
         if (IsInitialised && _application != application)
            throw new ArgumentException($"The {GetName()} has already been initialised for a different application.", nameof(application));

         if (IsInitialised is false)
         {
            Application = application;
            try
            {
               Initialise();
               IsInitialised = true;
            }
            catch
            {
               Application = null;
               IsInitialised = false;

               throw;
            }
         }
      }
   }

   /// <summary>Initialises the component.</summary>
   protected virtual void Initialise() { }

   /// <inheritdoc/>
   public void Cleanup(IApplication application)
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            return;

         if (Application != application)
            throw new ArgumentException($"The {GetName()} has already been initialised for a different application.", nameof(application));

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

   /// <summary>Cleansup the component.</summary>
   protected virtual void Cleanup() { }
   #endregion

   #region Helpers
   private string GetName() => GetType().Name;

   /// <summary>Throws an exception if the component has not been initialised yet.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the component has not been initialised yet.</exception>
#if NET5_0_OR_GREATER
   [MemberNotNull(nameof(Application), nameof(Context))]
#endif
   protected void ThrowIfNotInitialised()
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            throw new InvalidOperationException($"The {GetName()} has not been initialised yet.");

         Debug.Assert(Application is not null);
         Debug.Assert(Context is not null);
      }
   }
   #endregion
}
