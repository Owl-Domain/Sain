namespace Sain.Shared.Contexts;

/// <summary>
///   Represents a component that has application specific initialisation and cleanup.
/// </summary>
public interface IHasApplicationInit
{
   #region Methods
   /// <summary>Initialises the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the component has already been initialised for a different application.</exception>
   void Initialise(IApplication application);

   /// <summary>Cleans up the component.</summary>
   /// <param name="application">The application that the component will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the component has been initialised for a different application than the given one.</exception>
   void Cleanup(IApplication application);
   #endregion
}
