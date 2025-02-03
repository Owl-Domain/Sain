namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the context of a Sain application.
/// </summary>
public class ApplicationContext : IApplicationContext
{
   #region Fields
   private readonly object _initLock = new();
   private IApplication? _application;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public IApplication Application
   {
      get
      {
         if (_application is not null)
            return _application;

         throw new InvalidOperationException("The application context hasn't been attached to an application yet.");
      }
   }

   /// <inheritdoc/>
   public bool IsInitialised { get; private set; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IApplicationUnit> AllUnits { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IApplicationUnit> GeneralUnits { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IContextUnit> Contexts { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IContextProviderUnit> ContextProviders { get; }

   /// <inheritdoc/>
   public IReadOnlyList<IApplicationUnit> InitialisationOrder { get; }
   #endregion

   #region Standard context properties
   /// <inheritdoc/>
   public ITimeContextUnit? Time { get; }

   /// <inheritdoc/>
   public ILoggingContextUnit? Logging { get; }

   /// <inheritdoc/>
   public IStorageContextUnitGroup Storage { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationContext"/>.</summary>
   /// <param name="allUnits">The collection of all the units that have been added to the application.</param>
   /// <param name="initialisationOrder">The order in which the application units will be initialised in.</param>
   public ApplicationContext(
      IReadOnlyCollection<IApplicationUnit> allUnits,
      IReadOnlyList<IApplicationUnit> initialisationOrder)
   {
      AllUnits = allUnits;
      GeneralUnits = [.. allUnits.Where(unit => unit is not IContextUnit and not IContextProviderUnit)];
      Contexts = [.. allUnits.OfType<IContextUnit>()];
      ContextProviders = [.. allUnits.OfType<IContextProviderUnit>()];
      InitialisationOrder = initialisationOrder;

      Time = this.TryGetContext<ITimeContextUnit>();
      Logging = this.TryGetContext<ILoggingContextUnit>();
      Storage = StorageContextUnitGroup.Create(this);
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Initialise(IApplication application)
   {
      lock (_initLock)
      {
         if (_application is not null)
            throw new InvalidOperationException($"The application context already belongs to an application ({_application}).");

         _application = application;
         IsInitialised = true;

         try
         {
            foreach (IApplicationUnit unit in InitialisationOrder)
               unit.Initialise();
         }
         catch
         {
            _application = null;
            IsInitialised = false;

            throw;
         }
      }
   }

   /// <inheritdoc/>
   public void Cleanup()
   {
      lock (_initLock)
      {
         if (_application is null)
            throw new InvalidOperationException($"The application context hasn't been initialised yet.");

         try
         {
            for (int i = InitialisationOrder.Count - 1; i >= 0; i--)
            {
               IApplicationUnit unit = InitialisationOrder[i];
               unit.Cleanup();
            }
         }
         finally
         {
            IsInitialised = false;
            _application = null;
         }
      }
   }

   /// <inheritdoc/>
   public bool TryGetUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit) => TryGetUnit(AllUnits, kind, out unit);

   /// <inheritdoc/>
   public bool TryGetGeneralUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit) => TryGetUnit(GeneralUnits, kind, out unit);

   /// <inheritdoc/>
   public bool TryGetContext(Type kind, [NotNullWhen(true)] out IContextUnit? context)
   {
      if (TryGetUnit(Contexts, kind, out IApplicationUnit? untyped))
      {
         context = (IContextUnit)untyped;
         return true;
      }

      context = default;
      return false;
   }

   /// <inheritdoc/>
   public bool TryGetContextProvider(Type kind, [NotNullWhen(true)] out IContextProviderUnit? provider)
   {
      if (TryGetUnit(ContextProviders, kind, out IApplicationUnit? untyped))
      {
         provider = (IContextProviderUnit)untyped;
         return true;
      }

      provider = default;
      return false;
   }
   #endregion

   #region Helpers
   private static bool TryGetUnit(IEnumerable<IApplicationUnit> enumerable, Type kind, [NotNullWhen(true)] out IApplicationUnit? unit)
   {
      foreach (IApplicationUnit current in enumerable)
      {
         if (current.Kind == kind)
         {
            unit = current;
            return true;
         }
      }

      unit = default;
      return false;
   }
   #endregion
}
