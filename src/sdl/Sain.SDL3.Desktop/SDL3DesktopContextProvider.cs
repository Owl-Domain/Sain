namespace Sain.SDL3.Desktop;

/// <summary>
///   Represents the SDL3.Desktop specific context provider.
/// </summary>
public class SDL3DesktopContextProvider : SDL3ContextProvider
{
   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide<T>([MaybeNullWhen(false)] out T context)
   {
      Type type = typeof(T);

      if (type == typeof(IDesktopWindowingContext) || type == typeof(SDL3DesktopWindowingContext))
      {
         context = (T)(IContext)new SDL3DesktopWindowingContext(this);
         return true;
      }

      context = default;
      return false;
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/> and the <see cref="SDL3DesktopContextProvider"/>.
/// </summary>
public static class IApplicationBuilderSDL3ContextProviderExtensions
{
   #region Methods
   /// <summary>Uses the SDL3.Desktop specific context provider.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSDL3Desktop<TSelf>(this TSelf builder)
   where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithProvider<TSelf, SDL3DesktopContextProvider>();
   }
   #endregion
}
