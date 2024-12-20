namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a context provider.
/// </summary>
public abstract class BaseContextProvider : IContextProvider
{
   #region Methods
   /// <inheritdoc/>
   public abstract bool TryProvide<T>([MaybeNullWhen(false)] out T context) where T : notnull, IContext;

   /// <inheritdoc/>
   public virtual void Attach(IApplication application) { }

   /// <inheritdoc/>
   public virtual void Detach(IApplication application) { }
   #endregion
}
