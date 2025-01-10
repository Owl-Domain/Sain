namespace Sain.Shared.Contexts;

/// <summary>
///   Represents a component that has application specific initialisation and cleanup.
/// </summary>
public interface IHasApplicationInit
{
   #region Properties
   /// <summary>Whether the component has been initialised in any way.</summary>
   bool IsInitialised { get; }

   /// <summary>Whether the component has been fully initialised.</summary>
   bool IsFullyInitialised { get; }
   #endregion

   #region Methods
   /// <summary>Initialises the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has already been initialised for a different application.</exception>
   /// <remarks>This is ran before the <see cref="Initialise"/> step for any component has ran.</remarks>
   void PreInitialise(IApplicationBase application);

   /// <summary>Initialises the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has already been initialised for a different application.</exception>
   void Initialise(IApplicationBase application);

   /// <summary>Initialises the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has already been initialised for a different application.</exception>
   /// <remarks>This is ran after the <see cref="Initialise"/> step for all components has ran.</remarks>
   void PostInitialise(IApplicationBase application);

   /// <summary>Cleans up the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has been initialised for a different application than the given one.</exception>
   /// <remarks>This is ran before the <see cref="Cleanup"/> step for any component has ran.</remarks>
   void PreCleanup(IApplicationBase application);

   /// <summary>Cleans up the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has been initialised for a different application than the given one.</exception>
   void Cleanup(IApplicationBase application);

   /// <summary>Cleans up the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <exception cref="ArgumentException">Thrown if the component has been initialised for a different application than the given one.</exception>
   /// <remarks>This is ran after the <see cref="Cleanup"/> step for all components has ran.</remarks>
   void PostCleanup(IApplicationBase application);
   #endregion
}
