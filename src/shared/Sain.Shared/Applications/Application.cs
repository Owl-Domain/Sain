namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <param name="info">The information about the application.</param>
/// <param name="context">The context of the application.</param>
public abstract class Application<TContext>(IApplicationInfo info, TContext context) :
   ApplicationBase(info, context),
   IApplication<TContext>
   where TContext : IApplicationContext
{
   #region Fields
   private ApplicationEventHandler<TContext>? _starting;
   private ApplicationEventHandler<TContext>? _started;
   private ApplicationEventHandler<TContext>? _stopping;
   private ApplicationEventHandler<TContext>? _stopped;
   private ApplicationEventHandler<TContext>? _iteration;
   #endregion

   #region Properties
   TContext IApplication<TContext>.Context => (TContext)Context;
   #endregion

   #region Events
   event ApplicationEventHandler<TContext>? IApplication<TContext>.Starting
   {
      add => _starting += value;
      remove => _starting -= value;
   }
   event ApplicationEventHandler<TContext>? IApplication<TContext>.Started
   {
      add => _started += value;
      remove => _started -= value;
   }
   event ApplicationEventHandler<TContext>? IApplication<TContext>.Stopping
   {
      add => _stopping += value;
      remove => _stopping -= value;
   }
   event ApplicationEventHandler<TContext>? IApplication<TContext>.Stopped
   {
      add => _stopped += value;
      remove => _stopped -= value;
   }
   event ApplicationEventHandler<TContext>? IApplication<TContext>.Iteration
   {
      add => _iteration += value;
      remove => _iteration -= value;
   }
   #endregion

   #region Helpers
   /// <inheritdoc/>
   protected override void RaiseStarting()
   {
      base.RaiseStarting();
      _starting?.Invoke(this);
   }

   /// <inheritdoc/>
   protected override void RaiseStarted()
   {
      base.RaiseStarted();
      _started?.Invoke(this);
   }

   /// <inheritdoc/>
   protected override void RaiseStopping()
   {
      base.RaiseStopping();
      _stopping?.Invoke(this);
   }

   /// <inheritdoc/>
   protected override void RaiseStopped()
   {
      base.RaiseStopped();
      _stopped?.Invoke(this);
   }

   /// <inheritdoc/>
   protected override void RaiseIteration()
   {
      base.RaiseIteration();
      _iteration?.Invoke(this);
   }
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
/// <param name="info">The information about the application.</param>
/// <param name="context">The context of the application.</param>
public abstract class Application<TContext, TApplication>(IApplicationInfo info, TContext context) :
   Application<TContext>(info, context),
   IApplication<TContext, TApplication>
   where TContext : IApplicationContext
   where TApplication : IApplication<TContext, TApplication>
{
   #region Fields
   private ApplicationEventHandler<TContext, TApplication>? _starting;
   private ApplicationEventHandler<TContext, TApplication>? _started;
   private ApplicationEventHandler<TContext, TApplication>? _stopping;
   private ApplicationEventHandler<TContext, TApplication>? _stopped;
   private ApplicationEventHandler<TContext, TApplication>? _iteration;
   #endregion

   #region Properties
   private TApplication Self => (TApplication)(IApplicationBase)this;
   #endregion

   #region Events
   event ApplicationEventHandler<TContext, TApplication>? IApplication<TContext, TApplication>.Starting
   {
      add => _starting += value;
      remove => _starting -= value;
   }
   event ApplicationEventHandler<TContext, TApplication>? IApplication<TContext, TApplication>.Started
   {
      add => _started += value;
      remove => _started -= value;
   }
   event ApplicationEventHandler<TContext, TApplication>? IApplication<TContext, TApplication>.Stopping
   {
      add => _stopping += value;
      remove => _stopping -= value;
   }
   event ApplicationEventHandler<TContext, TApplication>? IApplication<TContext, TApplication>.Stopped
   {
      add => _stopped += value;
      remove => _stopped -= value;
   }
   event ApplicationEventHandler<TContext, TApplication>? IApplication<TContext, TApplication>.Iteration
   {
      add => _iteration += value;
      remove => _iteration -= value;
   }
   #endregion

   #region Helpers
   /// <inheritdoc/>
   protected override void RaiseStarting()
   {
      base.RaiseStarting();
      _starting?.Invoke(Self);
   }

   /// <inheritdoc/>
   protected override void RaiseStarted()
   {
      base.RaiseStarted();
      _started?.Invoke(Self);
   }

   /// <inheritdoc/>
   protected override void RaiseStopping()
   {
      base.RaiseStopping();
      _stopping?.Invoke(Self);
   }

   /// <inheritdoc/>
   protected override void RaiseStopped()
   {
      base.RaiseStopped();
      _stopped?.Invoke(Self);
   }

   /// <inheritdoc/>
   protected override void RaiseIteration()
   {
      base.RaiseIteration();
      _iteration?.Invoke(Self);
   }
   #endregion
}

/// <summary>
///   Represents a general Sain application.
/// </summary>
/// <param name="info">The information about the application.</param>
/// <param name="context">The context of the application.</param>
public sealed class Application(IApplicationInfo info, IApplicationContext context) :
   Application<IApplicationContext, IApplication>(info, context),
   IApplication
{
   #region Functions
   /// <summary>Creates a builder for a new application.</summary>
   /// <returns>The application builder which can be used to configure the application.</returns>
   public static ApplicationBuilder New() => new();
   #endregion
}
