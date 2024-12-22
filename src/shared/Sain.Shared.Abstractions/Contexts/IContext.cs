namespace Sain.Shared.Contexts;

/// <summary>
///   Represents an application's context.
/// </summary>
public interface IContext
{
   #region Properties
   /// <summary>The provider that the context comes from.</summary>
   IContextProvider? Provider { get; }

   /// <summary>The kind of the context.</summary>
   string Kind { get; }

   /// <summary>Whether the context is available for the application to use.</summary>
   bool IsAvailable { get; }
   #endregion

   #region Methods
   /// <summary>Initialises the context.</summary>
   /// <param name="application">The application that the context will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context has already been initialised for a different application.</exception>
   void Initialise(IApplication application);

   /// <summary>Cleans up the context.</summary>
   /// <param name="application">The application that the context will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context has been initialised for a different application than the given one.</exception>
   void Cleanup(IApplication application);
   #endregion
}
