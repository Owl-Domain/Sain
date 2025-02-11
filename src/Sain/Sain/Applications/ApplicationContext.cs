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

      Time = TryGetContextUnit<ITimeContextUnit>();
      Logging = TryGetContextUnit<ILoggingContextUnit>();
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
   #endregion

   #region All unit methods
   /// <inheritdoc/>
   public IApplicationUnit GetUnit(Type kind)
   {
      ThrowIfNotApplicationUnitType(kind);

      if (TryGetUnit(AllUnits, kind, out IApplicationUnit? unit) is false)
         ThrowForNoUnit(kind);

      return unit;
   }

   /// <inheritdoc/>
   public IApplicationUnit? TryGetUnit(Type kind)
   {
      ThrowIfNotApplicationUnitType(kind);

      return TryGetUnit(AllUnits, kind);
   }

   /// <inheritdoc/>
   public bool TryGetUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit)
   {
      ThrowIfNotApplicationUnitType(kind);

      return TryGetUnit(AllUnits, kind, out unit);
   }

   /// <inheritdoc/>
   public IReadOnlyCollection<IApplicationUnit> GetUnits(Type kind)
   {
      ThrowIfNotApplicationUnitType(kind);

      return [.. AllUnits.Where(unit => unit.Kind == kind)];
   }
   #endregion

   #region Generic all unit methods
   /// <inheritdoc/>
   public T GetUnit<T>() where T : notnull, IApplicationUnit
   {
      if (TryGetUnit(AllUnits, out T? unit) is false)
         ThrowForNoUnit(typeof(T));

      return unit;
   }

   /// <inheritdoc/>
   public T? TryGetUnit<T>() where T : notnull, IApplicationUnit => TryGetUnit<T>(AllUnits);

   /// <inheritdoc/>
   public bool TryGetUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit => TryGetUnit(AllUnits, out unit);

   /// <inheritdoc/>
   public IReadOnlyCollection<T> GetUnits<T>() where T : notnull, IApplicationUnit
   {
      List<T> units = [];

      foreach (IApplicationUnit current in AllUnits)
      {
         if (current.Kind == typeof(T))
         {
            T typed = (T)current;
            units.Add(typed);
         }
      }

      return units;
   }
   #endregion

   #region General unit methods
   /// <inheritdoc/>
   public IApplicationUnit GetGeneralUnit(Type kind)
   {
      ThrowIfNotGeneralUnitType(kind);

      if (TryGetUnit(AllUnits, kind, out IApplicationUnit? unit) is false)
         ThrowForNoUnit(kind);

      return unit;
   }

   /// <inheritdoc/>
   public IApplicationUnit? TryGetGeneralUnit(Type kind)
   {
      ThrowIfNotGeneralUnitType(kind);

      return TryGetUnit(GeneralUnits, kind);
   }

   /// <inheritdoc/>
   public IReadOnlyCollection<IApplicationUnit> GetGeneralUnits(Type kind)
   {
      ThrowIfNotGeneralUnitType(kind);

      return [.. GeneralUnits.Where(unit => unit.Kind == kind)];
   }
   #endregion

   #region Generic general unit methods
   /// <inheritdoc/>
   public T GetGeneralUnit<T>() where T : notnull, IApplicationUnit
   {
      ThrowIfNotGeneralUnitType(typeof(T), nameof(T));

      if (TryGetUnit(GeneralUnits, out T? unit) is false)
         ThrowForNoUnit(typeof(T));

      return unit;
   }

   /// <inheritdoc/>
   public T? TryGetGeneralUnit<T>() where T : notnull, IApplicationUnit
   {
      ThrowIfNotGeneralUnitType(typeof(T), nameof(T));

      return TryGetUnit<T>(GeneralUnits);
   }

   /// <inheritdoc/>
   public bool TryGetGeneralUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit
   {
      ThrowIfNotGeneralUnitType(typeof(T), nameof(T));

      return TryGetUnit(GeneralUnits, out unit);
   }

   /// <inheritdoc/>
   public IReadOnlyCollection<T> GetGeneralUnits<T>() where T : notnull, IApplicationUnit
   {
      ThrowIfNotGeneralUnitType(typeof(T), nameof(T));

      List<T> units = [];

      foreach (IApplicationUnit current in GeneralUnits)
      {
         if (current.Kind == typeof(T))
         {
            T typed = (T)current;
            units.Add(typed);
         }
      }

      return units;
   }
   #endregion

   #region Context unit methods
   /// <inheritdoc/>
   public IContextUnit GetContextUnit(Type kind)
   {
      ThrowIfNotContextUnitType(kind);

      if (TryGetUnit(Contexts, kind, out IApplicationUnit? unit) is false)
         ThrowForNoUnit(kind);

      return (IContextUnit)unit;
   }

   /// <inheritdoc/>
   public IContextUnit? TryGetContextUnit(Type kind)
   {
      ThrowIfNotContextUnitType(kind);

      return (IContextUnit?)TryGetUnit(Contexts, kind);
   }
   #endregion

   #region Generic context unit methods
   /// <inheritdoc/>
   public T GetContextUnit<T>() where T : notnull, IContextUnit
   {
      ThrowIfNotContextUnitType(typeof(T), nameof(T));

      if (TryGetUnit(Contexts, out T? unit) is false)
         ThrowForNoUnit(typeof(T));

      return unit;
   }

   /// <inheritdoc/>
   public T? TryGetContextUnit<T>() where T : notnull, IContextUnit
   {
      ThrowIfNotContextUnitType(typeof(T), nameof(T));

      return TryGetUnit<T>(Contexts);
   }

   /// <inheritdoc/>
   public bool TryGetContextUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IContextUnit
   {
      ThrowIfNotContextUnitType(typeof(T), nameof(T));

      return TryGetUnit(Contexts, out unit);
   }
   #endregion

   #region Context provider unit methods
   /// <inheritdoc/>
   public IContextProviderUnit GetContextProviderUnit(Type kind)
   {
      ThrowIfNotContextProviderUnitType(kind);

      if (TryGetUnit(ContextProviders, kind, out IApplicationUnit? unit) is false)
         ThrowForNoUnit(kind);

      return (IContextProviderUnit)unit;
   }

   /// <inheritdoc/>
   public IContextProviderUnit? TryGetContextProviderUnit(Type kind)
   {
      ThrowIfNotContextProviderUnitType(kind);

      return (IContextProviderUnit?)TryGetUnit(ContextProviders, kind);
   }
   #endregion

   #region Generic context provider unit methods
   /// <inheritdoc/>
   public T GetContextProviderUnit<T>() where T : notnull, IContextProviderUnit
   {
      ThrowIfNotContextProviderUnitType(typeof(T), nameof(T));

      if (TryGetUnit(ContextProviders, out T? unit) is false)
         ThrowForNoUnit(typeof(T));

      return unit;
   }

   /// <inheritdoc/>
   public T? TryGetContextProviderUnit<T>() where T : notnull, IContextProviderUnit
   {
      ThrowIfNotContextProviderUnitType(typeof(T), nameof(T));

      return TryGetUnit<T>(ContextProviders);
   }

   /// <inheritdoc/>
   public bool TryGetContextProviderUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IContextProviderUnit
   {
      ThrowIfNotContextProviderUnitType(typeof(T), nameof(T));

      return TryGetUnit(ContextProviders, out unit);
   }
   #endregion

   #region Helpers
   private static IApplicationUnit? TryGetUnit(IEnumerable<IApplicationUnit> enumerable, Type kind)
   {
      foreach (IApplicationUnit current in enumerable)
      {
         if (current.Kind == kind)
            return current;
      }

      return null;
   }
   private static bool TryGetUnit(IEnumerable<IApplicationUnit> enumerable, Type kind, [NotNullWhen(true)] out IApplicationUnit? unit)
   {
      unit = TryGetUnit(enumerable, kind);
      return unit is not null;
   }
   private static bool TryGetUnit<T>(IEnumerable<IApplicationUnit> enumerable, [NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit
   {
      unit = (T?)TryGetUnit(enumerable, typeof(T));
      return unit is not null;
   }
   private static T? TryGetUnit<T>(IEnumerable<IApplicationUnit> enumerable) where T : notnull, IApplicationUnit
   {
      return (T?)TryGetUnit(enumerable, typeof(T));
   }
   private static void ThrowIfNotApplicationUnitType(Type kind, string parameterName = "kind")
   {
      if (typeof(IApplicationUnit).IsAssignableFrom(kind) is false)
         throw new ArgumentException($"The given kind ({kind}) is not of the ({typeof(IApplicationUnit)}) type.", parameterName);
   }
   private static void ThrowIfNotContextUnitType(Type kind, string parameterName = "kind")
   {
      if (typeof(IContextUnit).IsAssignableFrom(kind) is false)
         throw new ArgumentException($"The given kind ({kind}) is not of the ({typeof(IContextUnit)}) type.", parameterName);
   }
   private static void ThrowIfNotContextProviderUnitType(Type kind, string parameterName = "kind")
   {
      if (typeof(IContextProviderUnit).IsAssignableFrom(kind) is false)
         throw new ArgumentException($"The given kind ({kind}) is not of the ({typeof(IContextProviderUnit)}) type.", parameterName);
   }
   private static void ThrowIfNotGeneralUnitType(Type kind, string parameterName = "kind")
   {
      if (typeof(IContextUnit).IsAssignableFrom(kind))
         throw new ArgumentException($"The given kind ({kind}) is of the ({typeof(IContextUnit)}) type.", parameterName);

      if (typeof(IContextProviderUnit).IsAssignableFrom(kind))
         throw new ArgumentException($"The given kind ({kind}) is of the ({typeof(IContextProviderUnit)}) type.", parameterName);
   }

   [DoesNotReturn]
   private static void ThrowForNoUnit(Type kind)
   {
      throw new InvalidOperationException($"No application unit of the given kind ({kind}) could be obtained.");
   }
   #endregion
}
