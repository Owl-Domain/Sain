namespace Sain.Desktop.Applications;

/// <summary>
///   Represents a builder for a desktop application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public abstract class DesktopApplicationBuilder<TSelf> : BaseApplicationBuilder<TSelf>
   where TSelf : DesktopApplicationBuilder<TSelf>
{
   #region Fields
   private DesktopApplicationShutdownMode? _shutdownMode;
   private Type? _startupWindowType;
   #endregion

   #region Methods
   /// <summary>Sets the <paramref name="shutdownMode"/> of the application.</summary>
   /// <param name="shutdownMode">The desktop's application shutdown mode.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the shutdown mode has already been specified.</exception>
   public TSelf WithShutdownMode(DesktopApplicationShutdownMode shutdownMode)
   {
      if (_shutdownMode is not null)
         throw new InvalidOperationException("The shutdown mode of the application has already been specified.");

      _shutdownMode = shutdownMode;
      return Instance;
   }

   /// <summary>Sets the type of the window to open when the application starts.</summary>
   /// <param name="startupWindowType">The type of the window to open when the application starts.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the startup window type has already been specified.</exception>
   public TSelf WithStartupWindowType(Type startupWindowType)
   {
      if (_startupWindowType is not null)
         throw new InvalidOperationException("The type of the startup window has already been specified.");

      _startupWindowType = startupWindowType;
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

   /// <inheritdoc/>
   protected override IApplication BuildCore()
   {
      _shutdownMode ??= DesktopApplicationShutdownMode.OnLastWindowClose;

      DesktopApplicationContext context = new(_shutdownMode.Value, _startupWindowType, Providers, Contexts);
      Application application = new(Id, Name, Version, context);

      return application;
   }
   #endregion
}

/// <summary>
///   Represents a builder for a desktop application.
/// </summary>
public sealed class DesktopApplicationBuilder : DesktopApplicationBuilder<DesktopApplicationBuilder> { }
