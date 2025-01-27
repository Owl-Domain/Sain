namespace Sain.Applications;

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TApplication">The type of the built application.</typeparam>
/// <typeparam name="TContext">The type of the context that the built application uses.</typeparam>
public abstract class ApplicationBuilder<TSelf, TApplication, TContext> : IApplicationBuilder<TSelf, TApplication>
   where TSelf : notnull, ApplicationBuilder<TSelf, TApplication, TContext>
   where TApplication : notnull, IApplication<TContext>
   where TContext : notnull, IApplicationContext
{
   #region Nested types
   [DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
   private sealed class Unit(IApplicationUnit? unit)
   {
      #region Properties
      public IApplicationUnit? ApplicationUnit { get; } = unit;
      public List<Unit> Dependencies { get; } = [];
      #endregion

      #region Methods
      private string DebuggerDisplay()
      {
         if (ApplicationUnit is null)
            return "Entry point unit";

         return $"Unit: {ApplicationUnit?.GetType()?.Name}";
      }
      #endregion
   }
   #endregion

   #region Fields
   private readonly List<IApplicationUnit> _units = [];
   private readonly ApplicationIdCollection _ids = [];
   private readonly ApplicationVersionCollection _versions = [];
   private string? _name;
   private TimeSpan? _minimumIterationTime;
   #endregion

   #region Properties
   /// <summary>The typed instance of the builder.</summary>
   protected TSelf Instance => (TSelf)this;

   /// <summary>The collection of the added application units.</summary>
   protected IReadOnlyCollection<IApplicationUnit> Units => _units;

   /// <summary>The default time to use for the <see cref="IApplicationConfiguration.MinimumIterationTime"/>.</summary>
   protected virtual TimeSpan DefaultMinimumIterationTime => TimeSpan.Zero;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public TSelf WithMinimumIterationTime(TimeSpan time)
   {
      if (time < TimeSpan.Zero)
         throw new ArgumentOutOfRangeException(nameof(time), time, "The minimum iteration time cannot be less than 0 seconds.");

      if (_minimumIterationTime is not null)
         throw new InvalidOperationException($"The minimum iteration time has already been set.");

      _minimumIterationTime = time;
      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithApplicationName(string name)
   {
      if (_name is not null)
         throw new InvalidOperationException($"The application name has already been set ({_name}).");

      _name = name;
      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithApplicationId(IApplicationId id)
   {
      _ids.Add(id);
      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithApplicationVersion(IApplicationVersion version)
   {
      _versions.Add(version);
      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithUnit(IApplicationUnit unit, Action<IApplicationUnit>? customiseCallback = null)
   {
      if (unit is IContextUnit and IContextProviderUnit)
         throw new ArgumentException($"An application unit ({unit}) cannot be both an {nameof(IContextUnit)} and an {nameof(IContextProviderUnit)}.", nameof(unit));

      if (unit is IContextUnit context)
         return WithContext(context, customiseCallback);

      if (unit is IContextProviderUnit provider)
         return WithContextProvider(provider, customiseCallback);

      customiseCallback?.Invoke(unit);
      _units.Add(unit);

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithUnit<T>(Action<T>? customiseCallback = null) where T : notnull, IApplicationUnit, new()
   {
      T unit = new();
      customiseCallback?.Invoke(unit);

      return WithUnit(unit);
   }

   /// <inheritdoc/>
   public TSelf WithContext(IContextUnit unit, Action<IContextUnit>? customiseCallback = null)
   {
      customiseCallback?.Invoke(unit);
      _units.Add(unit);
      SoftValidate();

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithContext<T>(Action<T>? customiseCallback = null) where T : notnull, IContextUnit, new()
   {
      T context = new();
      customiseCallback?.Invoke(context);

      return WithContext(context);
   }

   /// <inheritdoc/>
   public TSelf WithContextProvider(IContextProviderUnit unit, Action<IContextProviderUnit>? customiseCallback = null)
   {
      customiseCallback?.Invoke(unit);
      _units.Add(unit);
      SoftValidate();

      return Instance;
   }

   /// <inheritdoc/>
   public TSelf WithContextProvider<T>(Action<T>? customiseCallback = null) where T : notnull, IContextProviderUnit, new()
   {
      T provider = new();
      customiseCallback?.Invoke(provider);

      return WithContextProvider(provider);
   }

   /// <inheritdoc/>
   public TApplication Build()
   {
      HardValidate();

      if (_name is null)
         throw new InvalidOperationException($"The application name has not been set.");

      IReadOnlyCollection<IApplicationUnit> units = GetFinalApplicationUnits();
      IReadOnlyList<IApplicationUnit> initialisationOrder = GetInitialisationOrder(units);

      TContext context = BuildContext(units, initialisationOrder);

      ApplicationInfo info = new(_name, _ids, _versions);

      TimeSpan minimumIterationTime = _minimumIterationTime ?? DefaultMinimumIterationTime;
      if (minimumIterationTime < TimeSpan.Zero)
      {
         // Note(Nightowl): It's safe to assume the default time is wrong as setting the minimum iteration time is also validated;
         throw new InvalidOperationException($"The default minimum iteration time ({minimumIterationTime}) cannot be less than 0 seconds.");
      }

      ApplicationConfiguration configuration = new(minimumIterationTime);

      TApplication application = BuildApplication(info, configuration, context);
      return application;
   }

   /// <summary>Builds the application context.</summary>
   /// <param name="units">The final units that are being added to the application.</param>
   /// <param name="initialisationOrder">The order in which the given <paramref name="units"/> should be initialised in.</param>
   /// <returns>The built application context.</returns>
   protected abstract TContext BuildContext(IReadOnlyCollection<IApplicationUnit> units, IReadOnlyList<IApplicationUnit> initialisationOrder);

   /// <summary>Builds the application.</summary>
   /// <param name="info">The information about the application.</param>
   /// <param name="configuration">The configuration for the application.</param>
   /// <param name="context">The context of the application.</param>
   /// <returns></returns>
   protected abstract TApplication BuildApplication(IApplicationInfo info, IApplicationConfiguration configuration, TContext context);
   #endregion

   #region Helpers
   /// <summary>Gets the final application units that should be added to the application.</summary>
   /// <returns>The application units that should be added to the application.</returns>
   protected virtual IReadOnlyCollection<IApplicationUnit> GetFinalApplicationUnits()
   {
      HashSet<IContextProviderUnit> usedContextProviders = [];

      foreach (IApplicationUnit unit in _units)
      {
         if (unit is IContextUnit context && context.Provider is not null)
            usedContextProviders.Add(context.Provider);
      }

      List<IApplicationUnit> finalUnits = [];
      foreach (IApplicationUnit unit in _units)
      {
         if (unit is IContextProviderUnit provider && (usedContextProviders.Contains(provider) is false))
            continue;

         finalUnits.Add(unit);
      }

      return finalUnits;
   }

   /// <summary>Gets the initialisation order for the given <paramref name="finalUnits"/>.</summary>
   /// <param name="finalUnits">The final units that will be added to the application.</param>
   /// <returns>The given <paramref name="finalUnits"/>, in the order that they should be initialised in.</returns>
   /// <exception cref="InvalidOperationException">Thrown if it is impossible to get an an initialisation order for the given <paramref name="finalUnits"/>.</exception>
   protected virtual IReadOnlyList<IApplicationUnit> GetInitialisationOrder(IReadOnlyCollection<IApplicationUnit> finalUnits)
   {
      List<Unit> units = [];
      HashSet<Unit> resolved = [];
      HashSet<Unit> unresolved = [];

      #region Helper functions
      bool TryGetProvider(IContextUnit context, [NotNullWhen(true)] out Unit? unit)
      {
         if (context.Provider is null)
         {
            unit = default;
            return false;
         }

         foreach (Unit current in units)
         {
            if (current.ApplicationUnit == context.Provider)
            {
               unit = current;
               return true;
            }
         }

         unit = default;
         return false;
      }
      IEnumerable<Unit> EnumerateUnits(IEnumerable<Type> kinds)
      {
         foreach (Unit unit in units)
         {
            Debug.Assert(unit.ApplicationUnit is not null);
            foreach (Type kind in kinds)
            {
               if (unit.ApplicationUnit.Kind == kind)
               {
                  yield return unit;
                  break;
               }
            }
         }
      }
      void Resolve(Unit unit)
      {
         unresolved.Add(unit);

         foreach (Unit edge in unit.Dependencies)
         {
            if (resolved.Add(edge))
               continue;

            if (unresolved.Contains(edge))
               throw new InvalidOperationException($"Circular dependency detected ({unit.ApplicationUnit}) -> ({edge.ApplicationUnit}).");

            Resolve(edge);
         }

         resolved.Add(unit);
         unresolved.Remove(unit);
      }
      #endregion

      foreach (IApplicationUnit applicationUnit in finalUnits)
      {
         Unit unit = new(applicationUnit);
         units.Add(unit);
      }

      // Note(Nightowl): Add unit dependencies;
      foreach (Unit unit in units)
      {
         Debug.Assert(unit.ApplicationUnit is not null);

         // Note(Nightowl): Context units that automatically initialise after their context provider;
         if (unit.ApplicationUnit is IContextUnit context && TryGetProvider(context, out Unit? provider))
            unit.Dependencies.Add(provider);

         // Note(Nightowl): Automatically add required units as dependencies;
         if (unit.ApplicationUnit.InitialiseAfterRequiredUnits)
         {
            foreach (Unit required in EnumerateUnits(unit.ApplicationUnit.RequiresUnits))
               unit.Dependencies.Add(required);
         }

         // Note(Nightowl): Add dependencies based on initialisation order;
         foreach (Unit dependencyUnit in EnumerateUnits(unit.ApplicationUnit.InitialiseAfterUnits))
            unit.Dependencies.Add(dependencyUnit);

         foreach (Unit dependantUnit in EnumerateUnits(unit.ApplicationUnit.InitialiseBeforeUnits))
            dependantUnit.Dependencies.Add(unit);
      }

      // Note(Nightowl): Create an entry point unit (used only for order resolution);
      Unit entryPoint = new(null);
      foreach (Unit unit in units)
         entryPoint.Dependencies.Add(unit);

      Resolve(entryPoint);

      List<IApplicationUnit> order = [];
      foreach (Unit unit in resolved)
      {
         if (unit.ApplicationUnit is not null)
            order.Add(unit.ApplicationUnit);
      }

      return order;
   }

   /// <summary>Performs soft validation on the state of the application builder.</summary>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>This checks whether units have circular dependencies, and if they conflict with other units.</item>
   ///      <item>If you override this method, you must call the base implementation.</item>
   ///   </list>
   /// </remarks>
   protected virtual void SoftValidate()
   {
      IReadOnlyCollection<IApplicationUnit> finalUnits = GetFinalApplicationUnits();
      Dictionary<Type, int> kinds = [];

      foreach (IApplicationUnit unit in finalUnits)
      {
         if (kinds.TryAdd(unit.Kind, 1) is false)
            kinds[unit.Kind]++;
      }

      // Note(Nightowl): Co-existance & conflict checks;
      foreach (IApplicationUnit unit in finalUnits)
      {
         if (unit.CanCoexist is false && kinds[unit.Kind] > 1)
            throw new InvalidOperationException($"The application unit ({unit}) cannot co-exist with other units of the same kind ({unit.Kind}).");

         foreach (Type kind in unit.ConflictsWithUnits)
         {
            if (kinds.ContainsKey(kind))
               throw new InvalidOperationException($"The application unit ({unit}) conflicts with an added unit ({kind}).");
         }
      }

      // Note(Nightowl): Check if there are any problems with the initialisation order;
      _ = GetInitialisationOrder(finalUnits);
   }

   /// <summary>Performs hard validation on the state of the application builder.</summary>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>This will also call <see cref="SoftValidate"/>.</item>
   ///      <item>This checks whether units have all of the other units that they require.</item>
   ///      <item>If you override this method, you must call the base implementation.</item>
   ///   </list>
   /// </remarks>
   protected virtual void HardValidate()
   {
      SoftValidate();

      IReadOnlyCollection<IApplicationUnit> finalUnits = GetFinalApplicationUnits();
      HashSet<Type> kinds = [];

      foreach (IApplicationUnit unit in finalUnits)
         kinds.Add(unit.Kind);

      // Note(Nightowl): Check required units;
      foreach (IApplicationUnit unit in finalUnits)
      {
         foreach (Type required in unit.RequiresUnits)
         {
            if (kinds.Contains(required) is false)
               throw new InvalidOperationException($"The application unit ({unit}) required a unit that hasn't been added ({required}).");
         }
      }
   }
   #endregion
}

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
public sealed class ApplicationBuilder : ApplicationBuilder<ApplicationBuilder, Application, IApplicationContext>
{
   #region Methods
   /// <inheritdoc/>
   protected override IApplicationContext BuildContext(IReadOnlyCollection<IApplicationUnit> units, IReadOnlyList<IApplicationUnit> initialisationOrder)
   {
      return new ApplicationContext(units, initialisationOrder);
   }

   /// <inheritdoc/>
   protected override Application BuildApplication(IApplicationInfo info, IApplicationConfiguration configuration, IApplicationContext context)
   {
      return new(info, configuration, context);
   }
   #endregion
}
