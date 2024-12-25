namespace Sain.Shared.Contexts;

/// <summary>
///   Represents a provider for new application contexts.
/// </summary>
public interface IContextProvider
{
   #region Methods
   /// <summary>Tries to get a new application context of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the context to try and provide.</typeparam>
   /// <param name="context">The obtained context.</param>
   /// <returns>
   ///   <see langword="true"/> if the requested <paramref name="context"/>
   ///   could be provided, <see langword="false"/> otherwise.
   /// </returns>
   bool TryProvide<T>([MaybeNullWhen(false)] out T context) where T : notnull, IContext;

   /// <summary>Initialises the context provider.</summary>
   /// <param name="application">The application that the context provider will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context provider has already been initialised for a different application.</exception>
   void Initialise(IApplication application);

   /// <summary>Cleans up the context provider.</summary>
   /// <param name="application">The application that the context provider will belong to.</param>
   /// <remarks>It is safe to call this method multiple times for the same <paramref name="application"/>.</remarks>
   /// <exception cref="ArgumentException">Thrown if the context provider has been initialised for a different application than the given one.</exception>
   void Cleanup(IApplication application);
   #endregion
}
