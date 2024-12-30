namespace Sain.SDL3;

/// <summary>
///   Represents the SDL3 specific context provider.
/// </summary>
public unsafe class SDL3ContextProvider : BaseContextProvider
{
   #region Fields
   private static readonly Dictionary<nint, SDL3ContextProvider> ActiveProviders = [];
   private static readonly Dictionary<SDL3_LogPriority, LogSeverity> CustomLogSeverities = [];
   private nint _providerFakePointer;
   private IReadOnlyCollection<ISDL3Context> _providedContexts = [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   public override bool TryProvide<T>([MaybeNullWhen(false)] out T context)
   {
      Type type = typeof(T);

      if (type == typeof(IDisplayContext) || type == typeof(SDL3DisplayContext))
      {
         context = (T)(IContext)new SDL3DisplayContext(this);
         return true;
      }

      if (type == typeof(IMouseInputContext) || type == typeof(SDL3MouseInputContext))
      {
         context = (T)(IContext)new SDL3MouseInputContext(this);
         return true;
      }

      context = default;
      return false;
   }

   /// <inheritdoc/>
   protected override void Initialise()
   {
      _providedContexts = GetProvidedContexts<ISDL3Context>();

      _providerFakePointer = new(NativeMemory.Alloc(0));
      ActiveProviders.Add(_providerFakePointer, this);

      if (Context.Logging.IsAvailable)
      {
#if DEBUG
         Native.SetLogPriorities(SDL3_LogPriority.Info);
#else
         Native.SetLogPriorities(SDL3_LogPriority.Warning);
#endif
         Native.SetLogOutputFunction(&LogCallback, _providerFakePointer.ToPointer());
      }

      SDL3_InitFlags flags = GetInitFlags();
      if (flags is SDL3_InitFlags.None)
         return;

      Application.Iteration += ApplicationIteration;

      if (Context.Logging.IsAvailable)
      {
         int version = Native.GetVersion();
         string revision = Native.GetRevision();

         Context.Logging.Debug<SDL3ContextProvider>($"Using SDL3, version = ({version}), revision = ({revision}).");
      }

      if (Native.SetAppMetadata(Application.Name, Application.Version.DisplayName, Application.Id) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Error<SDL3ContextProvider>($"Couldn't set the application metadata. ({Native.LastError})");
      }

      SetHints();

      if (Native.InitSubSystem(flags) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Fatal<SDL3ContextProvider>($"Couldn't initialise the application with the flags ({flags}). ({Native.LastError})");

         // Todo(Nightowl): Show fatal error warning;
      }
   }

   /// <summary>Sets the SDL3 hints before initialising any SDL sub-systems.</summary>
   protected virtual void SetHints()
   {
      if (Native.EnableHint(SDL3_Hints.VIDEO_ALLOW_SCREENSAVER) is false)
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

         ActiveProviders.Remove(_providerFakePointer);
         if (_providerFakePointer != default)
         {
            NativeMemory.Free(_providerFakePointer.ToPointer());
            _providerFakePointer = default;
         }
      }
   }
   private void ApplicationIteration(IApplication application)
   {
      SDL3_Event latestMouseMotion = default;
      bool hasMouseMotion = false;

      while (Native.WaitEvent(out SDL3_Event ev, 1))
      {
         DispatchPriority priority = DispatchPriority.Normal;

         if (ev.Type is SDL3_EventType.WindowCloseRequested or SDL3_EventType.WindowDestroyed)
            priority = DispatchPriority.Highest;
         else if (ev.IsWindowEvent(out _) || ev.IsDisplayEvent(out _))
            priority = DispatchPriority.Visual;
         else if (ev.IsMouseButtonEvent(out _) || ev.IsMouseWheelEvent(out _))
            priority = DispatchPriority.Input;
         else if (ev.IsMouseMotionEvent(out _))
         {
            // Note(Nightowl):
            // Potential problem with mouse events being out of order, i.e. button first, move second?
            // Would require either computer or inhuman speed to actually cause any problems?

            latestMouseMotion = ev;
            hasMouseMotion = true;

            continue;
         }

         application.Context.Dispatcher.Dispatch(OnEvent, ev, priority);
      }

      if (hasMouseMotion)
         application.Context.Dispatcher.Dispatch(OnEvent, latestMouseMotion, DispatchPriority.Input);
   }
   private void OnEvent(SDL3_Event ev)
   {
      Debug.WriteLine($"OnEvent: {ev.Type}");

      foreach (ISDL3Context context in _providedContexts)
         context.OnEvent(ev);
   }
   #endregion

   #region Helpers
   private static void LogCallback(void* userData, SDL3_LogCategory category, SDL3_LogPriority priority, byte* nativeMessage)
   {
      nint contextPtr = new(userData);
      if (ActiveProviders.TryGetValue(contextPtr, out SDL3ContextProvider? provider) is false)
         return;

      string context = $"SDL3/{category}";
      LogSeverity severity = priority switch
      {
         SDL3_LogPriority.Trace => LogSeverity.Trace,
         SDL3_LogPriority.Debug => LogSeverity.Debug,
         SDL3_LogPriority.Info => LogSeverity.Info,
         SDL3_LogPriority.Warning => LogSeverity.Warning,
         SDL3_LogPriority.Error => LogSeverity.Error,
         SDL3_LogPriority.Critical => LogSeverity.Fatal,

         _ => GetCustomLogSeverity(priority)
      };

      if (provider.Context.Logging.IsAvailable)
      {
         string? message = Utf8StringMarshaller.ConvertToManaged(nativeMessage);
         if (string.IsNullOrWhiteSpace(message))
            return;

         provider.Context.Logging.Log(severity, context, message, string.Empty, string.Empty, 0);
      }
   }
   private static LogSeverity GetCustomLogSeverity(SDL3_LogPriority priority)
   {
      if (CustomLogSeverities.TryGetValue(priority, out LogSeverity severity) is false)
      {
         string text = $"custom-sdl3-severity({severity.ToString().ToLower()})";
         severity = new(text);

         CustomLogSeverities.Add(priority, severity);
      }

      return severity;
   }
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
