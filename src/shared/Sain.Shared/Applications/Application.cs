using System.Linq;

namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <param name="name">The name of the application.</param>
/// <param name="version">The version of the application,</param>
/// <param name="context">The context of the application.</param>
public sealed class Application(string name, IVersion version, IApplicationContext context) : IApplication
{
   #region Fields
   private readonly IReadOnlyCollection<IContextProvider> _usedProviders = [.. context.Contexts.Select(c => c.Provider).Where(p => p is not null).Distinct()!];
   private volatile bool _shouldBeRunning;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public string Name { get; } = name;

   /// <inheritdoc/>
   public IVersion Version { get; } = version;

   /// <inheritdoc/>
   public IApplicationContext Context { get; } = context;

   /// <inheritdoc/>
   public ApplicationState State { get; private set; }
   #endregion

   #region Events
   /// <inheritdoc/>
   public event ApplicationEventHandler? Starting;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Started;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Stopping;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Stopped;

   /// <inheritdoc/>
   public event ApplicationEventHandler? Iteration;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Run()
   {
      if (State is not ApplicationState.Stopped)
         throw new InvalidOperationException($"The application is either currently running or has not fully stopped yet, meaning it cannot be started again.");

      bool hasInitialised = false;
      State = ApplicationState.Starting;

      try
      {
         _shouldBeRunning = true;

         foreach (IContextProvider provider in _usedProviders)
            provider.Attach(this);

         Starting?.Invoke(this);
         Context.Initialise(this);
         hasInitialised = true;

         State = ApplicationState.Running;

         Started?.Invoke(this);

         while (_shouldBeRunning)
         {
            Context.Dispatcher.Process();

            Iteration?.Invoke(this);

            if (Thread.Yield() is false)
               Thread.Sleep(1);
         }

         State = ApplicationState.Stopping;
         Stopping?.Invoke(this);
      }
      finally
      {
         try
         {
            State = ApplicationState.Stopped;
            if (hasInitialised)
               Context.Cleanup(this);

            Stopped?.Invoke(this);
         }
         finally
         {
            foreach (IContextProvider provider in _usedProviders)
               provider.Detach(this);
         }
      }
   }

   /// <inheritdoc/>
   public void Stop() => _shouldBeRunning = false;
   #endregion

   #region Functions
   /// <summary>Creates a builder for a new application.</summary>
   /// <returns>The application builder which can be used to configure the application.</returns>
   public static ApplicationBuilder New() => new();
   #endregion
}
