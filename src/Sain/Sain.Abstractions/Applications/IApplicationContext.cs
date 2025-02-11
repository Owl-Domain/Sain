namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the context of a Sain application.
/// </summary>
public interface IApplicationContext
{
   #region Properties
   /// <summary>The application that the application context has been attached to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the application context hasn't been attached to an application yet.</exception>
   IApplication Application { get; }

   /// <summary>Whether the application context has been initialised.</summary>
   bool IsInitialised { get; }

   /// <summary>The collection of all the units that have been added to the application.</summary>
   /// <remarks>This means general units, context units and context provider units.</remarks>
   IReadOnlyCollection<IApplicationUnit> AllUnits { get; }

   /// <summary>The collection of the general units that have been added to the application.</summary>
   /// <remarks>This is all units except context units, and context provider units.</remarks>
   IReadOnlyCollection<IApplicationUnit> GeneralUnits { get; }

   /// <summary>The collection of the context units that have been added to the application.</summary>
   IReadOnlyCollection<IContextUnit> Contexts { get; }

   /// <summary>The collection of the context provider units that have been added to the application.</summary>
   IReadOnlyCollection<IContextProviderUnit> ContextProviders { get; }

   /// <summary>The order in which the application units will be initialised in.</summary>
   /// <remarks>Cleanup (uninitialisation) will happen in the reverse order.</remarks>
   IReadOnlyList<IApplicationUnit> InitialisationOrder { get; }
   #endregion

   #region Standard context properties
   /// <summary>The application's context unit for time information.</summary>
   ITimeContextUnit? Time { get; }

   /// <summary>The application's context unit for logging information.</summary>
   ILoggingContextUnit? Logging { get; }

   /// <summary>The application's context unit for storage information and operations.</summary>
   IStorageContextUnitGroup Storage { get; }
   #endregion

   #region Methods
   /// <summary>Initialises the application context.</summary>
   /// <param name="application">The Sain application that the current application context is for.</param>
   /// <remarks>This will also initialise the application units in the <see cref="InitialisationOrder"/>.</remarks>
   /// <exception cref="InvalidOperationException">Thrown if the application context is already initialised.</exception>
   void Initialise(IApplication application);

   /// <summary>Cleans up (uninitialises) the application context.</summary>
   /// <remarks>This will also cleanup (uninitialise) the application units in the reverse <see cref="InitialisationOrder"/>.</remarks>
   /// <exception cref="InvalidOperationException">Thrown if the application context is not initialised.</exception>
   void Cleanup();
   #endregion

   #region All unit methods
   /// <summary>Gets an application unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the application unit to get.</param>
   /// <returns>The obtained application unit.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IApplicationUnit"/> type.</exception>
   /// <exception cref="InvalidOperationException">Thrown if no application unit of the given <paramref name="kind"/> could be obtained.</exception>
   IApplicationUnit GetUnit(Type kind);

   /// <summary>Tries to get an application unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the application unit to try and get.</param>
   /// <returns>The obtained application unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IApplicationUnit"/> type.</exception>
   IApplicationUnit? TryGetUnit(Type kind);

   /// <summary>Tries to get an application unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the application unit to try and get.</param>
   /// <param name="unit">The obtained application unit.</param>
   /// <returns><see langword="true"/> if the application <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IApplicationUnit"/> type.</exception>
   bool TryGetUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit);

   /// <summary>Tries to get all of the application units for the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the application units to get.</param>
   /// <returns>A collection of the obtained application units.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IApplicationUnit"/> type.</exception>
   IReadOnlyCollection<IApplicationUnit> GetUnits(Type kind);
   #endregion

   #region Generic all unit methods
   /// <summary>Gets an application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the application unit to get.</typeparam>
   /// <returns>The obtained application unit.</returns>
   /// <exception cref="InvalidOperationException">Thrown if no application unit of the given kind <typeparamref name="T"/> could be obtained.</exception>
   T GetUnit<T>() where T : notnull, IApplicationUnit;

   /// <summary>Tries to get an application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the application unit to get.</typeparam>
   /// <returns>The obtained application unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   T? TryGetUnit<T>() where T : notnull, IApplicationUnit;

   /// <summary>Tries to get an application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the application unit to get.</typeparam>
   /// <param name="unit">The obtained application unit.</param>
   /// <returns><see langword="true"/> if the application <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   bool TryGetUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit;

   /// <summary>Tries to get all of the application units for the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the application units to get.</typeparam>
   /// <returns>A collection of the obtained application units.</returns>
   IReadOnlyCollection<T> GetUnits<T>() where T : notnull, IApplicationUnit;
   #endregion

   #region General unit methods
   /// <summary>Gets a general application unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the general application unit to get.</param>
   /// <returns>The obtained general application unit.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if either:
   ///   <list type="bullet">
   ///      <item>The given <paramref name="kind"/> is not of the <see cref="IApplicationUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextProviderUnit"/> type.</item>
   ///   </list>
   /// </exception>
   /// <exception cref="InvalidOperationException">Thrown if no general application unit of the given <paramref name="kind"/> could be obtained.</exception>
   IApplicationUnit GetGeneralUnit(Type kind);

   /// <summary>Tries to get a general application unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the general application unit to try and get.</param>
   /// <returns>The obtained general application unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if either:
   ///   <list type="bullet">
   ///      <item>The given <paramref name="kind"/> is not of the <see cref="IApplicationUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextProviderUnit"/> type.</item>
   ///   </list>
   /// </exception>
   IApplicationUnit? TryGetGeneralUnit(Type kind);

   /// <summary>Tries to get all of the general application units for the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the general application units to get.</param>
   /// <returns>A collection of the obtained general application units.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if either:
   ///   <list type="bullet">
   ///      <item>The given <paramref name="kind"/> is not of the <see cref="IApplicationUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextUnit"/> type.</item>
   ///      <item>The given <paramref name="kind"/> is of the <see cref="IContextProviderUnit"/> type.</item>
   ///   </list>
   /// </exception>
   IReadOnlyCollection<IApplicationUnit> GetGeneralUnits(Type kind);
   #endregion

   #region Generic general unit methods
   /// <summary>Gets a general application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the general application unit to get.</typeparam>
   /// <returns>The obtained general application unit.</returns>
   /// <exception cref="InvalidOperationException">Thrown if no general application unit of the given kind <typeparamref name="T"/> could be obtained.</exception>
   /// <exception cref="ArgumentException">
   ///   Thrown if the given kind type <typeparamref name="T"/> is either an
   ///   <see cref="IContextUnit"/> or an <see cref="IContextProviderUnit"/>.
   /// </exception>
   T GetGeneralUnit<T>() where T : notnull, IApplicationUnit;

   /// <summary>Tries to get a general application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the general application unit to get.</typeparam>
   /// <returns>The obtained general application unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if the given kind type <typeparamref name="T"/> is either an
   ///   <see cref="IContextUnit"/> or an <see cref="IContextProviderUnit"/>.
   /// </exception>
   T? TryGetGeneralUnit<T>() where T : notnull, IApplicationUnit;

   /// <summary>Tries to get a general application unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the general application unit to get.</typeparam>
   /// <param name="unit">The obtained general application unit.</param>
   /// <returns><see langword="true"/> if the general application <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if the given kind type <typeparamref name="T"/> is either an
   ///   <see cref="IContextUnit"/> or an <see cref="IContextProviderUnit"/>.
   /// </exception>
   bool TryGetGeneralUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit;

   /// <summary>Tries to get all of the general application units for the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the general application units to get.</typeparam>
   /// <returns>A collection of the obtained general application units.</returns>
   /// <exception cref="ArgumentException">
   ///   Thrown if the given kind type <typeparamref name="T"/> is either an
   ///   <see cref="IContextUnit"/> or an <see cref="IContextProviderUnit"/>.
   /// </exception>
   IReadOnlyCollection<T> GetGeneralUnits<T>() where T : notnull, IApplicationUnit;
   #endregion

   #region Context unit methods
   /// <summary>Gets a context unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context unit to get.</param>
   /// <returns>The obtained context unit.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IContextUnit"/> type.</exception>
   /// <exception cref="InvalidOperationException">Thrown if no context unit of the given <paramref name="kind"/> could be obtained.</exception>
   IContextUnit GetContextUnit(Type kind);

   /// <summary>Tries to get a context unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context unit to try and get.</param>
   /// <returns>The obtained context unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IContextUnit"/> type.</exception>
   IContextUnit? TryGetContextUnit(Type kind);
   #endregion

   #region Generic context unit methods
   /// <summary>Gets a context unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context unit to get.</typeparam>
   /// <returns>The obtained context unit.</returns>
   /// <exception cref="InvalidOperationException">Thrown if no context unit of the given kind <typeparamref name="T"/> could be obtained.</exception>
   T GetContextUnit<T>() where T : notnull, IContextUnit;

   /// <summary>Tries to get a context unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context unit to get.</typeparam>
   /// <returns>The obtained context unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   T? TryGetContextUnit<T>() where T : notnull, IContextUnit;

   /// <summary>Tries to get a context unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context unit to get.</typeparam>
   /// <param name="unit">The obtained context unit.</param>
   /// <returns><see langword="true"/> if the context <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   bool TryGetContextUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IContextUnit;
   #endregion

   #region Context provider unit methods
   /// <summary>Gets a context provider unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context provider unit to get.</param>
   /// <returns>The obtained context provider unit.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IContextProviderUnit"/> type.</exception>
   /// <exception cref="InvalidOperationException">Thrown if no context provider unit of the given <paramref name="kind"/> could be obtained.</exception>
   IContextProviderUnit GetContextProviderUnit(Type kind);

   /// <summary>Tries to get a context provider unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context provider unit to try and get.</param>
   /// <returns>The obtained context provider unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   /// <exception cref="ArgumentException">Thrown if the given kind is not of the <see cref="IContextProviderUnit"/> type.</exception>
   IContextProviderUnit? TryGetContextProviderUnit(Type kind);
   #endregion

   #region Generic context provider unit methods
   /// <summary>Gets a context provider unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context provider unit to get.</typeparam>
   /// <returns>The obtained context provider unit.</returns>
   /// <exception cref="InvalidOperationException">Thrown if no context provider unit of the given kind <typeparamref name="T"/> could be obtained.</exception>
   T GetContextProviderUnit<T>() where T : notnull, IContextProviderUnit;

   /// <summary>Tries to get a context provider unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context provider unit to get.</typeparam>
   /// <returns>The obtained context provider unit, or <see langword="null"/> if the unit couldn't be obtained.</returns>
   T? TryGetContextProviderUnit<T>() where T : notnull, IContextProviderUnit;

   /// <summary>Tries to get a context provider unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context provider unit to get.</typeparam>
   /// <param name="unit">The obtained context provider unit.</param>
   /// <returns><see langword="true"/> if the context provider <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   bool TryGetContextProviderUnit<T>([NotNullWhen(true)] out T? unit) where T : notnull, IContextProviderUnit;
   #endregion
}
