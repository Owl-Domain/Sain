namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the default context provider.
/// </summary>
public sealed class DefaultContextProvider : BaseContextProvider
{
   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide<T>([MaybeNullWhen(false)] out T context)
   {
      context = default;
      return false;
   }
   #endregion
}
