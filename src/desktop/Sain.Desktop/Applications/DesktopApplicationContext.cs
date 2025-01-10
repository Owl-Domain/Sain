namespace Sain.Desktop.Applications;

/// <summary>
///   Represents the context of a desktop application.
/// </summary>
public class DesktopApplicationContext : ApplicationContext, IDesktopApplicationContext
{
   #region Properties
   /// <inheritdoc/>
   public DesktopApplicationShutdownMode ShutdownMode { get; }

   /// <inheritdoc/>
   public Type? StartupWindowType { get; }

   /// <inheritdoc/>
   public IDesktopWindowingContext Windowing { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="DesktopApplicationContext"/>.</summary>
   /// <param name="shutdownMode">The shutdown mode of the application.</param>
   /// <param name="startupWindowType">The type of the window to open when the application starts.</param>
   /// <param name="contextProviders">The context providers that are available to the application.</param>
   /// <param name="contexts">The contexts that are available to the desktop application.</param>
   public DesktopApplicationContext(
      DesktopApplicationShutdownMode shutdownMode,
      Type? startupWindowType,
      IReadOnlyCollection<IContextProvider> contextProviders,
      IReadOnlyCollection<IContext> contexts)
      : base(contextProviders, contexts)
   {
      ShutdownMode = shutdownMode;
      StartupWindowType = startupWindowType;

      Windowing = GetContext<IDesktopWindowingContext>();

      if (Windowing.IsAvailable)
         Windowing.Windows.CollectionChanged += WindowCollectionChanged;
   }
   #endregion

   #region Methods
   private void WindowCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
   {
      Debug.Assert(Windowing.IsAvailable);

      if (Windowing.Windows.Count is not 0)
         return;

      if (ShutdownMode is DesktopApplicationShutdownMode.OnLastWindowClose)
      {
         if (Logging.IsAvailable)
            Logging.Debug<DesktopApplicationContext>($"Last window has closed. Due to the shutdown mode being ({DesktopApplicationShutdownMode.OnLastWindowClose}), the application will now be stopped.");

         Application.Stop();
      }
      else if (ShutdownMode is DesktopApplicationShutdownMode.OnExplicitShutdown)
      {
         if (Logging.IsAvailable)
            Logging.Debug<DesktopApplicationContext>($"Last window has closed. Due to the shutdown mode being ({DesktopApplicationShutdownMode.OnExplicitShutdown}), the application will now run in the background.");
      }
   }
   #endregion
}
