namespace Sain.Shared.Applications;

/// <summary>
///   Represents the context of an application.
/// </summary>
public interface IApplicationContext : IHasApplicationInit
{
   #region Properties
   /// <summary>The collection of the available context providers.</summary>
   IReadOnlyCollection<IContextProvider> ContextProviders { get; }

   /// <summary>The collection of the available contexts.</summary>
   IReadOnlyCollection<IContext> Contexts { get; }

   /// <summary>The application's context for the dispatcher.</summary>
   IDispatcherContext Dispatcher { get; }

   /// <summary>The application's context for logging.</summary>
   ILoggingContext Logging { get; }

   /// <summary>The application's context for display information.</summary>
   IDisplayContext Display { get; }

   /// <summary>The application's context group for input contexts.</summary>
   IInputContextGroup Input { get; }

   /// <summary>The application's context group for audio.</summary>
   IAudioContextGroup Audio { get; }
   #endregion

   #region Methods
   /// <summary>Checks whether a context of the given type <typeparamref name="T"/> can be obtained.</summary>
   /// <typeparam name="T">The type of the context to check for.</typeparam>
   /// <returns>
   ///   <see langword="true"/> if a context of the given type <typeparamref name="T"/>
   ///   can be obtained, <see langword="false"/> otherwise.
   /// </returns>
   bool HasContext<T>() where T : notnull, IContext;

   /// <summary>Checks whether a context of the given type <typeparamref name="T"/> can be obtained, and it is marked as available.</summary>
   /// <typeparam name="T">The type of the context to check for.</typeparam>
   /// <returns>
   ///   <see langword="true"/> if a context of the given type <typeparamref name="T"/> can
   ///   be obtained, and it is marked as available, <see langword="false"/> otherwise.
   /// </returns>
   bool IsContextAvailable<T>() where T : notnull, IContext;

   /// <summary>Gets the context of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the context to get.</typeparam>
   /// <returns>The obtained context.</returns>
   T GetContext<T>() where T : notnull, IContext;

   /// <summary>Tries to get the context of the given type <typeparamref name="T"/>.</summary>
   /// <typeparam name="T">The type of the context to try and get.</typeparam>
   /// <param name="context">
   ///   The obtained context, or <see langword="null"/> if a context
   ///   of the given type <typeparamref name="T"/> could not be found.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the <paramref name="context"/>
   ///   could be obtained, <see langword="false"/> otherwise.
   /// </returns>
   bool TryGetContext<T>([NotNullWhen(true)] out T? context) where T : notnull, IContext;

   /// <summary>Tries to get a context of the given type <typeparamref name="T"/>, but only if it is marked as being available.</summary>
   /// <typeparam name="T">The type of the context to try and get.</typeparam>
   /// <param name="context">
   ///   The obtained context, or <see langword="null"/> if a context of the given type
   ///   <typeparamref name="T"/> could either not be found, or it was marked as unavailable.
   /// </param>
   /// <returns>
   ///   <see langword="true"/> if the <paramref name="context"/>
   ///   could be obtained, <see langword="false"/> otherwise.
   /// </returns>
   bool TryGetContextIfAvailable<T>([NotNullWhen(true)] out T? context) where T : notnull, IContext;
   #endregion
}
