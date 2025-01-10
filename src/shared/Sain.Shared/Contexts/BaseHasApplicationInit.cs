namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a component that has application specific initialisation and cleanup.
/// </summary>
public abstract class BaseHasApplicationInit : ObservableBase, IHasApplicationInit
{
   #region Nested types
   private enum Stage : byte
   {
      Unitialised,
      PreInitialised,
      Initialised,
      PostInitialised,

      PreCleanup,
      Cleanup,
   }
   #endregion

   #region Fields
   // Note(Nightowl): Doesn't have a property, but it's not gonna be useful to see it in the debugger anyway;
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly object _initLock = new();

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IApplicationBase? _application;

   private Stage _stage = Stage.Unitialised;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public bool IsInitialised => _stage is not Stage.Unitialised;

   /// <inheritdoc/>
   public bool IsFullyInitialised => _stage is Stage.PostInitialised;

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
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void PreInitialise(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.PreInitialised)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been pre-initialised for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.Unitialised)
            throw new InvalidOperationException($"Expected the {nameof(PreInitialise)} step to run when the {GetName()} is uninitialised.");

         Application = application;
         try
         {
            PreInitialise();
            _stage = Stage.PreInitialised;
         }
         catch
         {
            Application = null;
            _stage = Stage.Unitialised;

            throw;
         }
      }
   }

   /// <inheritdoc/>
   public void Initialise(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.Initialised)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been initialised for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.PreInitialised)
            throw new InvalidOperationException($"Expected the {nameof(Initialise)} step to run after the {GetName()} has been pre-initialised.");

         try
         {
            Initialise();
            _stage = Stage.Initialised;
         }
         catch
         {
            Application = null;
            _stage = Stage.Unitialised;

            throw;
         }
      }
   }

   /// <inheritdoc/>
   public void PostInitialise(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.PostInitialised)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been post-initialised for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.Initialised)
            throw new InvalidOperationException($"Expected the {nameof(PostInitialise)} step to run after the {GetName()} has been initialised.");

         try
         {
            PostInitialise();
            _stage = Stage.PostInitialised;
         }
         catch
         {
            Application = null;
            _stage = Stage.Unitialised;

            throw;
         }
      }
   }

   /// <summary>Cleans up the component.</summary>
   /// <remarks>Ran before <see cref="Initialise()"/> is ran for any component.</remarks>
   protected virtual void PreInitialise() { }

   /// <summary>Initialises the component.</summary>
   protected virtual void Initialise() { }

   /// <summary>Cleans up the component.</summary>
   /// <remarks>Ran after <see cref="Initialise()"/> has ran for all components.</remarks>
   protected virtual void PostInitialise() { }

   /// <inheritdoc/>
   public void PreCleanup(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.PreCleanup)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been pre-cleaned for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.PostInitialised)
            throw new InvalidOperationException($"Expected the {nameof(PreCleanup)} step to run after the {GetName()} has been post-initialised.");

         _stage = Stage.PreCleanup;
         try
         {
            PreCleanup();
         }
         catch
         {
            Application = null;
            throw;
         }
      }
   }

   /// <inheritdoc/>
   public void Cleanup(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.Cleanup)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been cleaned up for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.PreCleanup)
            throw new InvalidOperationException($"Expected the {nameof(Cleanup)} step to run after the {GetName()} has been pre-cleaned.");

         _stage = Stage.Cleanup;
         try
         {
            Cleanup();
         }
         catch
         {
            Application = null;
            throw;
         }
      }
   }

   /// <inheritdoc/>
   public void PostCleanup(IApplicationBase application)
   {
      lock (_initLock)
      {
         if (_stage is Stage.Unitialised)
         {
            if (_application != application)
               throw new ArgumentException($"The {GetName()} has already been post-cleaned for a different application.", nameof(application));

            return;
         }

         if (_stage is not Stage.Cleanup)
            throw new InvalidOperationException($"Expected the {nameof(PostCleanup)} step to run after the {GetName()} has been cleaned up.");

         _stage = Stage.Unitialised;
         try
         {
            PostCleanup();
         }
         finally
         {
            Application = null;
         }
      }
   }

   /// <summary>Cleans up the component.</summary>
   /// <remarks>Ran before <see cref="Cleanup()"/> is ran for any component.</remarks>
   protected virtual void PreCleanup() { }

   /// <summary>Cleans up the component.</summary>
   protected virtual void Cleanup() { }

   /// <summary>Cleans up the component.</summary>
   /// <remarks>Ran after <see cref="Cleanup()"/> has ran for all components.</remarks>
   protected virtual void PostCleanup() { }
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

   /// <summary>Throws an exception if the component has not been fully initialised yet.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the component has not been fully initialised yet.</exception>
   protected void ThrowIfNotFullyInitialised()
   {
      lock (_initLock)
      {
         if (IsFullyInitialised is false)
            throw new InvalidOperationException($"The {GetName()} has not been fully initialised yet.");
      }
   }
   #endregion
}
