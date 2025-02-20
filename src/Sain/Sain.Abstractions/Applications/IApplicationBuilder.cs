namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
public interface IApplicationBuilder
{
   #region Methods
   /// <summary>Builds the configured Sain application.</summary>
   /// <returns>The built application.</returns>
   IApplication Build();

   /// <summary>Checks whether a context unit of the given <paramref name="kind"/> has been added to the application.</summary>
   /// <param name="kind">The kind of the context unit to check for.</param>
   /// <returns><see langword="true"/> if a context unit of the given <paramref name="kind"/> has been added to the application, <see langword="false"/> otherwise.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="kind"/> isn't a <see cref="IContextUnit"/> kind.</exception>
   bool HasContextOfKind(Type kind);

   /// <summary>Checks whether a context unit of the given kind <typeparamref name="T"/> has been added to the application.</summary>
   /// <typeparam name="T">The kind of the context unit to check for.</typeparam>
   /// <returns><see langword="true"/> if a context unit of the given kind <typeparamref name="T"/> has been added to the application, <see langword="false"/> otherwise.</returns>
   bool HasContextOfKind<T>() where T : notnull, IContextUnit;
   #endregion
}

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public interface IApplicationBuilder<TSelf> : IApplicationBuilder
   where TSelf : notnull, IApplicationBuilder<TSelf>
{
   #region Methods
   /// <summary>Sets the time for how long each application iteration should last for at a minimum.</summary>
   /// <param name="time">The minimum time each application iteration should last for.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>Use <see cref="TimeSpan.Zero"/> to not have a minimum iteration time.</remarks>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the given <paramref name="time"/> value is negative.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the minimum iteration time has already been set.</exception>
   TSelf WithMinimumIterationTime(TimeSpan time);

   /// <summary>Sets the application name to the given <paramref name="name"/>.</summary>
   /// <param name="name">The name of the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the application name has already been set.</exception>
   TSelf WithApplicationName(string name);

   /// <summary>Adds the given <paramref name="id"/> as an extra application id.</summary>
   /// <param name="id">The id to add to the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>The first id that is provided will be considered the default one.</item>
   ///      <item>This method will not perform any duplicate checking, make sure to not add duplicate ids yourself.</item>
   ///   </list>
   /// </remarks>
   TSelf WithApplicationId(IApplicationId id);

   /// <summary>Adds the given <paramref name="version"/> as an extra application version.</summary>
   /// <param name="version">The version to add to the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>The first version that is provided will be considered the default one.</item>
   ///      <item>This method will not perform any duplicate checking, make sure to not add duplicate versions yourself.</item>
   ///   </list>
   /// </remarks>
   TSelf WithApplicationVersion(IApplicationVersion version);

   /// <summary>Adds the given application <paramref name="unit"/> to the application.</summary>
   /// <param name="unit">The application unit to add to the application.</param>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added <paramref name="unit"/>.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>If the given <paramref name="unit"/> is an <see cref="IContextUnit"/> then <see cref="WithContext"/> will be called instead.</item>
   ///      <item>If the given <paramref name="unit"/> is an <see cref="IContextProviderUnit"/> then <see cref="WithContextProvider"/> will be called instead.</item>
   ///   </list>
   /// </remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="unit"/> is both an <see cref="IContextUnit"/> and an <see cref="IContextProviderUnit"/>.</exception>
   TSelf WithUnit(IApplicationUnit unit, Action<IApplicationUnit>? customiseCallback = null);

   /// <summary>Creates a new instance of the application unit of the type <typeparamref name="T"/> and adds it to the application.</summary>
   /// <typeparam name="T">The type of the application unit create and add to the application.</typeparam>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the created unit.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>If the created unit is an <see cref="IContextUnit"/> then <see cref="WithContext"/> will be called instead.</item>
   ///      <item>If the created unit is an <see cref="IContextProviderUnit"/> then <see cref="WithContextProvider"/> will be called instead.</item>
   ///   </list>
   /// </remarks>
   /// <exception cref="ArgumentException">Thrown if the created unit is both an <see cref="IContextUnit"/> and an <see cref="IContextProviderUnit"/>.</exception>
   TSelf WithUnit<T>(Action<T>? customiseCallback = null) where T : notnull, IApplicationUnit, new();

   /// <summary>Adds the given context <paramref name="unit"/> to the application.</summary>
   /// <param name="unit">The context unit to add to the application.</param>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added <paramref name="unit"/>.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithContext(IContextUnit unit, Action<IContextUnit>? customiseCallback = null);

   /// <summary>Creates a new instance of the context unit of the type <typeparamref name="T"/> and adds it to the application.</summary>
   /// <typeparam name="T">The type of the context unit create and add to the application.</typeparam>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the created unit.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithContext<T>(Action<T>? customiseCallback = null) where T : notnull, IContextUnit, new();

   /// <summary>Adds the given context provider <paramref name="unit"/> to the application.</summary>
   /// <param name="unit">The context provider unit to add to the application.</param>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added <paramref name="unit"/>.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithContextProvider(IContextProviderUnit unit, Action<IContextProviderUnit>? customiseCallback = null);

   /// <summary>Creates a new instance of the context provider unit of the type <typeparamref name="T"/> and adds it to the application.</summary>
   /// <typeparam name="T">The type of the context provider unit create and add to the application.</typeparam>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the created unit.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithContextProvider<T>(Action<T>? customiseCallback = null) where T : notnull, IContextProviderUnit, new();

   /// <summary>Requests a context unit of the given <paramref name="kind"/> from the added context provider units.</summary>
   /// <param name="kind">The kind of the context unit to try and add.</param>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added unit.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="kind"/> isn't an <see cref="IContextUnit"/> kind.</exception>
   /// <exception cref="InvalidOperationException">Thrown if no context of the given <paramref name="kind"/> could be added.</exception>
   TSelf WithContextOfKind(Type kind, Action<IContextUnit>? customiseCallback = null);

   /// <summary>Requests a context unit of the given kind <typeparamref name="T"/> from the added context provider units.</summary>
   /// <typeparam name="T">The type of the context unit kind to try and add.</typeparam>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added unit.</param>
   /// <returns>The used builder instance.</returns>
   TSelf WithContextOfKind<T>(Action<T>? customiseCallback = null) where T : notnull, IContextUnit;

   /// <summary>Tries to request a context unit of the given <paramref name="kind"/> from the added context provider units.</summary>
   /// <param name="kind">The kind of the context unit to try and add.</param>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added unit.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="kind"/> isn't an <see cref="IContextUnit"/> kind.</exception>
   /// <remarks>This method will not added the context unit if one of the same kind has already been added to the application.</remarks>
   TSelf TryWithContextOfKind(Type kind, Action<IContextUnit>? customiseCallback = null);

   /// <summary>Tries to request a context unit of the given kind <typeparamref name="T"/> from the added context provider units.</summary>
   /// <typeparam name="T">The type of the context unit kind to try and add.</typeparam>
   /// <param name="customiseCallback">The (optional) callback which can be used to customise the added unit.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>This method will not added the context unit if one of the same kind has already been added to the application.</remarks>
   TSelf TryWithContextOfKind<T>(Action<T>? customiseCallback = null) where T : notnull, IContextUnit;

   /// <summary>Tries to customise the application units of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the units to try and customise.</param>
   /// <param name="customiseCallback">The callback which will be used to customise the applcation units.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>This will only customise units that have already been added to the application.</remarks>
   /// <exception cref="ArgumentException">Thrown if the given <paramref name="kind"/> isn't an <see cref="IApplicationUnit"/> kind.</exception>
   TSelf TryCustomiseUnits(Type kind, Action<IApplicationUnit> customiseCallback);

   /// <summary>Tries to customise the application units of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the units to try and customise.</typeparam>
   /// <param name="customiseCallback">The callback which will be used to customise the applcation units.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>This will only customise units that have already been added to the application.</remarks>
   TSelf TryCustomiseUnits<T>(Action<T> customiseCallback) where T : notnull, IApplicationUnit;
   #endregion
}

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TApplication">The type of the built application.</typeparam>
public interface IApplicationBuilder<TSelf, TApplication> : IApplicationBuilder<TSelf>
   where TSelf : IApplicationBuilder<TSelf, TApplication>
   where TApplication : IApplication
{
   #region Methods
   /// <summary>Builds the configured Sain application.</summary>
   /// <returns>The built application.</returns>
   new TApplication Build();
   IApplication IApplicationBuilder.Build() => Build();
   #endregion
}
