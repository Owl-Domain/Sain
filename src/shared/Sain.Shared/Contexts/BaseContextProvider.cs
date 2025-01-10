namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a context provider.
/// </summary>
public abstract class BaseContextProvider : BaseHasApplicationInit, IContextProvider
{
   #region Methods
   /// <inheritdoc/>
   public abstract bool TryProvide<T>([MaybeNullWhen(false)] out T context) where T : notnull, IContext;

   /// <summary>Enumerate through the contexts that the current provider has provided to the application.</summary>
   /// <returns>An enumerated through the contexts provided to the application.</returns>
   protected IEnumerable<IContext> EnumerateProvidedContexts()
   {
      foreach (IContext context in Context.Contexts)
      {
         if (context.Provider == this)
            yield return context;
      }
   }

   /// <summary>Enumerate through the contexts that the current provider has provided to the application.</summary>
   /// <typeparam name="T">The type of the contexts to enumerate through.</typeparam>
   /// <returns>An enumerated through the contexts provided to the application.</returns>
   protected IEnumerable<T> EnumerateProvidedContexts<T>() where T : IContext
   {
      foreach (IContext context in Context.Contexts)
      {
         if (context is T typed && typed.Provider == this)
            yield return typed;
      }
   }

   /// <summary>Gets a collection of the contexts that the current provider has provided to the application.</summary>
   /// <returns>A collection of the contexts provided to the application.</returns>
   protected IReadOnlyCollection<IContext> GetProvidedContexts() => [.. EnumerateProvidedContexts()];

   /// <summary>Gets a collection of the contexts that the current provider has provided to the application.</summary>
   /// <typeparam name="T">The type of the contexts to retrieve.</typeparam>
   /// <returns>A collection of the contexts provided to the application.</returns>
   protected IReadOnlyCollection<T> GetProvidedContexts<T>() where T : IContext
   {
      return [.. EnumerateProvidedContexts<T>()];
   }
   #endregion
}
