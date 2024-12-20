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

   /// <summary>Called before any of the contexts from this provider are initialised.</summary>
   /// <param name="context">The context of the application in which the provided context has been initialised.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   Task BeforeContextsInitialisedAsync(IApplicationContext context);

   /// <summary>Called after all of the contexts from this provider are initialised.</summary>
   /// <param name="context">The context of the application in which the provided context has been initialised.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   Task AfterContextsInitialisedAsync(IApplicationContext context);

   /// <summary>Called before any of the contexts from this provider have been cleaned up.</summary>
   /// <param name="context">The context of the application in which the provided context has been initialised.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   Task BeforeContextsCleanedUpAsync(IApplicationContext context);

   /// <summary>Called after all of the contexts from this provider have been cleaned up.</summary>
   /// <param name="context">The context of the application in which the provided context has been initialised.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   Task AfterContextsCleanedUpAsync(IApplicationContext context);
   #endregion
}
