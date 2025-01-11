namespace Sain.Shared.Contexts;

/// <summary>
///   Represents a component that has application specific initialisation and cleanup.
/// </summary>
public interface IHasApplicationInit
{
   #region Properties
   /// <summary>Whether the component is attached to an application.</summary>
   bool IsAttached { get; }

   /// <summary>Whether the component has been initialised.</summary>
   bool IsInitialised { get; }

   /// <summary>The collection of the contexts that the application component relies on for initialisation.</summary>
   IReadOnlyCollection<string> DependsOnContexts { get; }
   #endregion

   #region Methods
   /// <summary>Attaches the component to the given <paramref name="application"/>.</summary>
   /// <param name="application">The application to attach to.</param>
   /// <exception cref="InvalidOperationException">Thrown if the component has already been initialise.</exception>
   void Attach(IApplicationBase application);

   /// <summary>Initialises the component.</summary>
   void Initialise();

   /// <summary>Cleans up the component.</summary>
   void Cleanup();

   /// <summary>Detaches the component from the application it was attached to.</summary>
   /// <exception cref="InvalidOperationException">
   ///   Thrown if the component hasn't been attached to an application yet, or if it hasn't been cleaned up.
   /// </exception>
   void Detach();
   #endregion
}
