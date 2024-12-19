namespace Sain.Shared.Contexts;

/// <summary>
///   Represents an application's context.
/// </summary>
public interface IContext
{
   #region Properties
   /// <summary>Whether the context is available for the application to use.</summary>
   bool IsAvailable { get; }
   #endregion

   #region Methods
   /// <summary>Initialises the context.</summary>
   /// <param name="application">The context of the application that the context will belong to.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context has already been initialised for a different application.</exception>
   Task InitialiseAsync(IApplicationContext application);

   /// <summary>Cleans up the context.</summary>
   /// <param name="application">The context of the application that the context belonged to.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context has been initialised for a different application than the given one.</exception>
   Task CleanupAsync(IApplicationContext application);
   #endregion
}
