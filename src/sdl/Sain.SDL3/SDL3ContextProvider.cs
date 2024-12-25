namespace Sain.SDL3;

/// <summary>
///   Represents the SDL3 specific context provider.
/// </summary>
public unsafe class SDL3ContextProvider : BaseContextProvider
{
   #region Fields
   private IReadOnlyCollection<ISDL3Context> _providedContexts = [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide<T>([MaybeNullWhen(false)] out T context)
   {
      context = default;
      return false;
   }

   /// <inheritdoc/>
   protected override void Initialise()
   {
      _providedContexts = GetProvidedContexts<ISDL3Context>();

      SDL3_InitFlags flags = GetInitFlags();
      if (flags is SDL3_InitFlags.None)
         return;

      Application.Iteration += ApplicationIteration;

      if (Native.SetAppMetadata(Application.Name, Application.Version.DisplayName, Application.Id) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3ContextProvider>($"Couldn't set the application metadata. ({Native.LastError})");
      }

      if (Native.InitSubSystem(flags) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Fatal<SDL3ContextProvider>($"Couldn't initialise the application with the flags ({flags}). ({Native.LastError})");

         // Todo(Nightowl): Show fatal error warning;
      }

      if (Native.SetHint(SDL3_Hints.SDL_HINT_VIDEO_ALLOW_SCREENSAVER, "1") is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3ContextProvider>($"Couldn't set the hint to allow the screensaver by default. ({Native.LastError})");
      }
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      try
      {
         SDL3_InitFlags flags = GetInitFlags();

         if (flags is SDL3_InitFlags.None)
            return;

         Application.Iteration -= ApplicationIteration;

         Native.QuitSubSystem(flags);
         Native.Quit();
      }
      finally
      {
         _providedContexts = [];
      }
   }
   private void ApplicationIteration(IApplication application)
   {
      while (Native.WaitEvent(out SDL3_Event ev, 1))
      {
         DispatchPriority priority = DispatchPriority.Normal;

         if (ev.Type is SDL3_EventType.WindowCloseRequested or SDL3_EventType.WindowDestroyed)
            priority = DispatchPriority.Highest;
         else if (ev.IsWindowEvent(out _))
            priority = DispatchPriority.Visual;

         application.Context.Dispatcher.Dispatch(OnEvent, ev, priority);
      }
   }
   private void OnEvent(SDL3_Event ev)
   {
      foreach (ISDL3Context context in _providedContexts)
         context.OnEvent(&ev);
   }
   #endregion

   #region Helpers
   private SDL3_InitFlags GetInitFlags()
   {
      SDL3_InitFlags flags = SDL3_InitFlags.None;

      foreach (ISDL3Context context in _providedContexts)
         flags |= context.Flags;

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
