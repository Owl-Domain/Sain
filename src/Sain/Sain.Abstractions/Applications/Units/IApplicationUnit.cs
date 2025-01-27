namespace Sain.Applications.Units;

/// <summary>
///   Represents a Sain application unit.
/// </summary>
public interface IApplicationUnit
{
   #region Properties
   /// <summary>The type that is used to represent the kind of the unit.</summary>
   Type Kind { get; }

   /// <summary>Whether this unit can co-exist with other application units that have the same <see cref="Kind"/>.</summary>
   bool CanCoexist { get; }

   /// <summary>The application units that should be initialised before this unit.</summary>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>These units are not treated as hard requirements, and are only used for determining the initialisation order.</item>
   ///      <item>These units will be cleaned up (uninitialised) after the current unit.</item>
   ///   </list>
   /// </remarks>
   IReadOnlyCollection<Type> InitialiseAfterUnits { get; }

   /// <summary>The application units that should be initialised after this unit.</summary>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>These units are not treated as hard requirements, and are only used for determining the initialisation order.</item>
   ///      <item>These units will be cleaned up (uninitialised) before the current unit.</item>
   ///   </list>
   /// </remarks>
   IReadOnlyCollection<Type> InitialiseBeforeUnits { get; }

   /// <summary>The application units that this unit requires to function.</summary>
   IReadOnlyCollection<Type> RequiresUnits { get; }

   /// <summary>Whether the application unit should automatically be initialised after the required units.</summary>
   bool InitialiseAfterRequiredUnits { get; }

   /// <summary>The application units that this unit cannot function along with.</summary>
   IReadOnlyCollection<Type> ConflictsWithUnits { get; }

   /// <summary>Whether the unit is currently attached to an application.</summary>
   bool IsAttached { get; }

   /// <summary>Whether the unit is currently initialised.</summary>
   bool IsInitialised { get; }
   #endregion

   #region Methods
   /// <summary>Attaches the unit to the given <paramref name="application"/>.</summary>
   /// <param name="application">The application to attach the unit to.</param>
   /// <exception cref="InvalidOperationException">Thrown if the unit is already attached to an application.</exception>
   void Attach(IApplication application);

   /// <summary>Initialises the unit.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the unit is already initialised.</exception>
   void Initialise();

   /// <summary>Cleans up (uninitialises) the unit.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the unit is not initialised.</exception>
   void Cleanup();

   /// <summary>Detaches the unit from the application.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the unit is not attached to an application.</exception>
   void Detach();
   #endregion
}
