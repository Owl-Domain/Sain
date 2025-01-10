namespace Sain.Shared.Contexts;

/// <summary>
///   Represents a component that has application specific initialisation and cleanup.
/// </summary>
public interface IHasApplicationInit
{
   #region Properties
   /// <summary>Whether the component has been initialised in any way.</summary>
   bool IsInitialised { get; }

   /// <summary>The collection of the contexts that the application component relies on for initialisation.</summary>
   IReadOnlyCollection<string> DependsOnContexts { get; }
   #endregion

   #region Methods
   /// <summary>Initialises the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has already been initialised for a different application.</exception>
   void Initialise(IApplicationBase application);

   /// <summary>Cleans up the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has been initialised for a different application than the given one.</exception>
   void Cleanup(IApplicationBase application);
   #endregion
}
