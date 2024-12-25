using System.Linq;

namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <param name="id">The unique id of the application.</param>
/// <param name="name">The name of the application.</param>
/// <param name="version">The version of the application,</param>
/// <param name="context">The context of the application.</param>
public sealed class Application(string? id, string name, IVersion version, IApplicationContext context) : IApplication
{
   #region Fields
   private readonly IReadOnlyCollection<IContextProvider> _usedProviders = [.. context.Contexts.Select(c => c.Provider).Where(p => p is not null).Distinct()!];
   private volatile bool _shouldBeRunning;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public string? Id { get; } = id;

   /// <inheritdoc/>
   public string Name { get; } = name;

   /// <inheritdoc/>
   public IVersion Version { get; } = version;

   /// <inheritdoc/>
   public IApplicationContext Context { get; } = context;

   /// <inheritdoc/>
   public ApplicationState State { get; private set; }

   /// <inheritdoc/>
   public TimeSpan LastIterationDuration { get; private set; }
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

         Stopwatch watch = new();

         while (_shouldBeRunning)
         {
            watch.Restart();

            Context.
            Dispatcher.Process();

            Iteration?.Invoke(this);
            LastIterationDuration = watch.Elapsed;

            if (LastIterationDuration.TotalMilliseconds < 10)
               Thread.Sleep(10 - (int)LastIterationDuration.TotalMilliseconds);
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

   #region Helpers
   private static string GetShortTime(TimeSpan value)
   {
      if (value.TotalMilliseconds > 0)
         return $"{value.TotalMilliseconds:n2} ms";

#if NET7_0_OR_GREATER
      if (value.TotalMicroseconds > 0)
         return $"{value.TotalMicroseconds:n2} us";

      if (value.TotalNanoseconds > 0)
         return $"{value.TotalNanoseconds:n2} us";
#else
      if (value.TotalMilliseconds / 100 > 0)
         return $"{value.TotalMilliseconds / 100:n2} us";

      if (value.TotalMilliseconds / 100_000 > 0)
         return $"{value.TotalMilliseconds / 100_000:n2} us";
#endif

      return $"{value.TotalMilliseconds:n2} ms";
   }
   #endregion
}
