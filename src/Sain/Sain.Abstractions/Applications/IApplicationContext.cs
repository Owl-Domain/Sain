namespace Sain.Applications;

/// <summary>
///   Represents the context of a Sain application.
/// </summary>
public interface IApplicationContext
{
   #region Properties
   /// <summary>The collection of all the units that have been added to the application.</summary>
   /// <remarks>This means standalone units, context units and context provider units.</remarks>
   IReadOnlyCollection<IApplicationUnit> AllUnits { get; }

   /// <summary>The collection of the units that have been added to the application.</summary>
   /// <remarks>This is excluding context units, and context provider units.</remarks>
   IReadOnlyCollection<IApplicationUnit> Units { get; }

   /// <summary>The collection of the context units that have been added to the application.</summary>
   IReadOnlyCollection<IContextUnit> Contexts { get; }

   /// <summary>The collection of the context provider units that have been added to the application.</summary>
   IReadOnlyCollection<IContextProviderUnit> ContextProviders { get; }
   #endregion
}
