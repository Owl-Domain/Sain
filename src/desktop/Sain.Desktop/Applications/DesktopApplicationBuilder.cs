namespace Sain.Desktop.Applications;

/// <summary>
///   Represents a builder for a desktop application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the desktop application.</typeparam>
public abstract class DesktopApplicationBuilder<TSelf, TContext, TApplication> : BaseApplicationBuilder<TSelf, TContext, TApplication>
   where TSelf : DesktopApplicationBuilder<TSelf, TContext, TApplication>
   where TContext : IDesktopApplicationContext
   where TApplication : IDesktopApplication<TContext, TApplication>
{
   #region Properties
   /// <summary>The shutdown mode of the desktop application.</summary>
   protected DesktopApplicationShutdownMode? ShutdownMode { get; private set; }

   /// <summary>The type of the window to open when the application starts.</summary>
   protected Type? StartupWindowType { get; private set; }
   #endregion

   #region Methods
   /// <summary>Sets the <paramref name="shutdownMode"/> of the application.</summary>
   /// <param name="shutdownMode">The desktop's application shutdown mode.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the shutdown mode has already been specified.</exception>
   public TSelf WithShutdownMode(DesktopApplicationShutdownMode shutdownMode)
   {
      if (ShutdownMode is not null)
         throw new InvalidOperationException("The shutdown mode of the application has already been specified.");

      ShutdownMode = shutdownMode;
      return Instance;
   }

   /// <summary>Sets the type of the window to open when the application starts.</summary>
   /// <param name="startupWindowType">The type of the window to open when the application starts.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the startup window type has already been specified.</exception>
   public TSelf WithStartupWindowType(Type startupWindowType)
   {
      if (StartupWindowType is not null)
         throw new InvalidOperationException("The type of the startup window has already been specified.");

      StartupWindowType = startupWindowType;
      return Instance;
   }

   /// <summary>Sets the type of the window to open when the application starts.</summary>
   /// <typeparam name="T">The type of the window to open when the application starts.</typeparam>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the startup window type has already been specified.</exception>
   public TSelf WithStartupWindowType<T>() => WithStartupWindowType(typeof(T));

   /// <inheritdoc/>
   protected override void AddDefaultContexts()
   {
      base.AddDefaultContexts();

      TryRequestContext<IDisplayContext>(CoreContextKinds.Display);
      TryRequestContext<IMouseInputContext>(CoreContextKinds.MouseInput);
      TryRequestContext<IKeyboardInputContext>(CoreContextKinds.KeyboardInput);
      TryRequestContext<IDesktopWindowingContext>(DesktopContextKinds.Windowing);
   }
   #endregion
}

/// <summary>
///   Represents a builder for a desktop application.
/// </summary>
public sealed class DesktopApplicationBuilder : DesktopApplicationBuilder<DesktopApplicationBuilder, IDesktopApplicationContext, IDesktopApplication>
{
   #region Methods

   /// <inheritdoc/>
   protected override IDesktopApplication BuildCore(IApplicationInfo info)
   {
      DesktopApplicationShutdownMode shutdownMode = ShutdownMode ?? DesktopApplicationShutdownMode.OnLastWindowClose;

      DesktopApplicationContext context = new(shutdownMode, StartupWindowType, Providers, Contexts);
      DesktopApplication application = new(info, context);

      return application;
   }
   #endregion
}
