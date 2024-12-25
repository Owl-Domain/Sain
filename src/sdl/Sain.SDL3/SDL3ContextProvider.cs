using System.Linq;

namespace Sain.SDL3;

/// <summary>
///   Represents the SDL3 specific context provider.
/// </summary>
public unsafe class SDL3ContextProvider : BaseContextProvider
{
   #region Nested types
   private sealed class State(IApplication application)
   {
      #region Fields
      public readonly IApplication Application = application;
      public readonly List<ISDL3Context> Contexts = [.. application.Context.Contexts.OfType<ISDL3Context>()];
      #endregion

      #region Methods
      public void OnEvent(SDL3_Event ev)
      {
         Console.WriteLine($"Event: {ev.Type}");

         foreach (ISDL3Context context in Contexts)
            context.OnEvent(&ev);
      }
      #endregion
   }
   #endregion

   #region Fields
   private static State? _lastAttached;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide<T>([MaybeNullWhen(false)] out T context)
   {
      context = default;
      return false;
   }

   /// <inheritdoc/>
   public override void Attach(IApplication application)
   {
      if (_lastAttached is not null)
         throw new InvalidOperationException($"Only a single {nameof(SDL3ContextProvider)} may be active at the same time.");

      _lastAttached = new(application);
      application.Iteration += ApplicationIteration;

      SDL3_InitFlags flags = GetInitFlags(application.Context.Contexts);

      if (Native.SetAppMetadata(application.Name, application.Version.DisplayName, application.Id) is false)
      {
         // Todo(Nightowl): handle/log error;
      }

      if (Native.InitSubSystem(flags) is false)
      {
         // Todo(Nightowl): Handle/log error;
      }

      if (Native.SetHint(SDL3_Hints.SDL_HINT_VIDEO_ALLOW_SCREENSAVER, "1") is false)
      {
         // Todo(Nightowl): Handle/log error;
      }
   }

   /// <inheritdoc/>
   public override void Detach(IApplication application)
   {
      application.Iteration -= ApplicationIteration;
      _lastAttached = null;

      SDL3_InitFlags flags = GetInitFlags(application.Context.Contexts);

      Native.QuitSubSystem(flags);
      Native.Quit();
   }
   private static void ApplicationIteration(IApplication application)
   {
      Debug.Assert(_lastAttached is not null);

      while (Native.WaitEvent(out SDL3_Event ev, 1))
      {
         DispatchPriority priority = DispatchPriority.Normal;

         if (ev.Type is SDL3_EventType.WindowCloseRequested or SDL3_EventType.WindowDestroyed)
            priority = DispatchPriority.Highest;
         else if (ev.IsWindowEvent(out _))
            priority = DispatchPriority.Visual;

         application.Context.Dispatcher.Dispatch(_lastAttached.OnEvent, ev, priority);
      }
   }
   #endregion

   #region Helpers
   private static SDL3_InitFlags GetInitFlags(IEnumerable<IContext> contexts)
   {
      SDL3_InitFlags flags = SDL3_InitFlags.None;

      foreach (IContext context in contexts)
      {
         if (context is ISDL3Context typed)
            flags |= typed.Flags;
      }

      return flags;
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder{TSelf}"/> and the <see cref="SDL3ContextProvider"/>.
/// </summary>
public static class IApplicationBuilderSDL3ContextProviderExtensions
{
   #region Methods
   /// <summary>Uses the SDL3 specific context provider.</summary>
   /// <typeparam name="TSelf">The type of the application builder.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <returns>The used builder instance.</returns>
   public static TSelf WithSDL3<TSelf>(this TSelf builder)
   where TSelf : IApplicationBuilder<TSelf>
   {
      return builder.WithProvider<TSelf, SDL3ContextProvider>();
   }
   #endregion
}
