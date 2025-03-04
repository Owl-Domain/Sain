namespace OwlDomain.Sain.Applications.Units;

/// <summary>
///   Represents the base implementation for an application unit.
/// </summary>
public class BaseApplicationUnit : IApplicationUnit
{
   #region Fields
   private readonly object _initLock = new();
   private IApplication? _application;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public virtual Type Kind => GetType();

   /// <inheritdoc/>
   public virtual bool CanCoexist => true;

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<Type> InitialiseAfterUnits => [];

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<Type> InitialiseBeforeUnits => [];

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<Type> RequiresUnits => [];

   /// <inheritdoc/>
   public virtual bool InitialiseAfterRequiredUnits => true;

   /// <inheritdoc/>
   public virtual IReadOnlyCollection<Type> ConflictsWithUnits => [];

   /// <inheritdoc/>
   public bool IsAttached { get; private set; }

   /// <inheritdoc/>
   public bool IsInitialised { get; private set; }

   /// <summary>Gets the application that the unit has been attached to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the unit hasn't been attached to an application yet.</exception>
   protected IApplication Application
   {
      get
      {
         if (_application is not null)
            return _application;

         throw new InvalidOperationException($"The unit ({this}) has not been attached to an application yet.");
      }
   }

   /// <summary>Gets the context of the application that the unit has been attached to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the unit hasn't been attached to an application yet.</exception>
   protected IApplicationContext Context
   {
      get
      {
         if (_application is not null)
            return _application.Context;

         throw new InvalidOperationException($"The unit ({this}) has not been attached to an application yet.");
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Attach(IApplication application)
   {
      lock (_initLock)
      {
         if (IsAttached)
            throw new InvalidOperationException($"The unit ({this}) has already been attached to an application ({_application}).");

         Debug.Assert(IsInitialised is false);
         Debug.Assert(_application is null);

         try
         {
            _application = application;
            IsAttached = true;
            OnAttach();
         }
         catch
         {
            _application = null;
            IsAttached = false;

            throw;
         }
      }
   }

   /// <summary>Called when the unit has been attached to an application.</summary>
   protected virtual void OnAttach() { }

   /// <inheritdoc/>
   public void Detach()
   {
      lock (_initLock)
      {
         if (IsAttached is false)
            throw new InvalidOperationException($"The unit ({this}) has not been attached to an application yet.");

         if (IsInitialised)
            throw new InvalidOperationException($"The unit ({this}) is still initialised.");

         try
         {
            OnDetach();
         }
         finally
         {
            _application = null;
            IsAttached = false;
         }
      }
   }

   /// <summary>Called when the unit has been detached from the application.</summary>
   protected virtual void OnDetach() { }

   /// <inheritdoc/>
   public void Initialise()
   {
      lock (_initLock)
      {
         if (IsInitialised)
            throw new InvalidOperationException($"The unit ({this}) has already been initialised.");

         if (IsAttached is false)
            throw new InvalidOperationException($"The unit ({this}) has not yet been attached to an application.");

         try
         {
            IsInitialised = true;
            Context.Logging?.Debug<BaseApplicationUnit>($"The unit ({this}) is being initialised.");

            OnInitialise();
         }
         catch
         {
            Context.Logging?.Debug<BaseApplicationUnit>($"The unit ({this}) is being cleaned up (uninitialised) after a failure during initialisation.");
            OnCleanup();

            IsInitialised = false;
            throw;
         }
      }
   }

   /// <summary>Called when the unit has been initialised.</summary>
   protected virtual void OnInitialise() { }

   /// <inheritdoc/>
   public void Cleanup()
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            throw new InvalidOperationException($"The unit ({this}) has not been initialised yet.");

         Debug.Assert(IsAttached);

         try
         {
            Context.Logging?.Debug<BaseApplicationUnit>($"The unit ({this}) is being cleaned up (uninitialised).");
            OnCleanup();
         }
         finally
         {
            IsInitialised = false;
         }
      }
   }

   /// <summary>Called when the unit has been cleaned up (uninitialised).</summary>
   protected virtual void OnCleanup() { }
   #endregion

   #region Helpers
   /// <summary>Throws an exception if the component has not been initialised yet.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the component has not been initialised yet.</exception>
   protected void ThrowIfNotInitialised()
   {
      lock (_initLock)
      {
         if (IsInitialised is false)
            throw new InvalidOperationException($"The unit ({this}) has not been initialised yet.");
      }
   }
   #endregion
}
