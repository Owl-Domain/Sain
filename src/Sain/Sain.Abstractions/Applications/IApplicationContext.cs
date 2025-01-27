namespace Sain.Applications;

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

   /// <summary>Tries to get a <paramref name="unit"/> of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the unit to try and get.</param>
   /// <param name="unit">The obtained unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <remarks>This is including general units, context units, and context provider units.</remarks>
   bool TryGetUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit);

   /// <summary>Tries to get a general <paramref name="unit"/> of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the general unit to try and get.</param>
   /// <param name="unit">The obtained general unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the general <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <remarks>This is excluding context units, and context provider units.</remarks>
   bool TryGetGeneralUnit(Type kind, [NotNullWhen(true)] out IApplicationUnit? unit);

   /// <summary>Tries to get a <paramref name="context"/> unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context unit to try and get.</param>
   /// <param name="context">The obtained context unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the <paramref name="context"/> unit could be obtained, <see langword="false"/> otherwise.</returns>
   bool TryGetContext(Type kind, [NotNullWhen(true)] out IContextUnit? context);

   /// <summary>Tries to get a context <paramref name="provider"/> unit of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context provider unit to try and get.</param>
   /// <param name="provider">The obtained context provider unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the context <paramref name="provider"/> unit could be obtained, <see langword="false"/> otherwise.</returns>
   bool TryGetContextProvider(Type kind, [NotNullWhen(true)] out IContextProviderUnit? provider);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationContext"/>.
/// </summary>
public static class IApplicationContextExtensions
{
   #region Methods
   /// <summary>Tries to get a <paramref name="unit"/> of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the unit to try and get.</typeparam>
   /// <param name="applicationContext">The application context to use.</param>
   /// <param name="unit">The obtained unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <remarks>This is including general units, context units, and context provider units.</remarks>
   public static bool TryGetUnit<T>(this IApplicationContext applicationContext, [NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit
   {
      if (applicationContext.TryGetUnit(typeof(T), out IApplicationUnit? untyped))
      {
         unit = (T)untyped;
         return true;
      }

      unit = default;
      return false;
   }

   /// <summary>Tries to get a <paramref name="unit"/> of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the unit to try and get.</typeparam>
   /// <param name="applicationContext">The application context to use.</param>
   /// <param name="unit">The obtained unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the <paramref name="unit"/> could be obtained, <see langword="false"/> otherwise.</returns>
   /// <remarks>This is excluding context units, and context provider units.</remarks>
   public static bool TryGetGeneralUnit<T>(this IApplicationContext applicationContext, [NotNullWhen(true)] out T? unit) where T : notnull, IApplicationUnit
   {
      if (applicationContext.TryGetGeneralUnit(typeof(T), out IApplicationUnit? untyped))
      {
         unit = (T)untyped;
         return true;
      }

      unit = default;
      return false;
   }

   /// <summary>Tries to get a <paramref name="context"/> unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context unit to try and get.</typeparam>
   /// <param name="applicationContext">The application context to use.</param>
   /// <param name="context">The obtained context unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the <paramref name="context"/> unit could be obtained, <see langword="false"/> otherwise.</returns>
   public static bool TryGetContext<T>(this IApplicationContext applicationContext, [NotNullWhen(true)] out T? context) where T : notnull, IContextUnit
   {
      if (applicationContext.TryGetContext(typeof(T), out IContextUnit? untyped))
      {
         context = (T)untyped;
         return true;
      }

      context = default;
      return false;
   }

   /// <summary>Tries to get a context <paramref name="provider"/> unit of the given kind <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The kind of the context provider unit to try and get.</typeparam>
   /// <param name="applicationContext">The application context to use.</param>
   /// <param name="provider">The obtained context provider unit, or <see langword="null"/> if the unit couldn't be obtained.</param>
   /// <returns><see langword="true"/> if the context <paramref name="provider"/> unit could be obtained, <see langword="false"/> otherwise.</returns>
   public static bool TryGetContextProvider<T>(this IApplicationContext applicationContext, [NotNullWhen(true)] out T? provider) where T : notnull, IContextProviderUnit
   {
      if (applicationContext.TryGetContextProvider(typeof(T), out IContextProviderUnit? untyped))
      {
         provider = (T)untyped;
         return true;
      }

      provider = default;
      return false;
   }
   #endregion
}
