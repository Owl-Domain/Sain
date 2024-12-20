namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a application's context.
/// </summary>
public abstract class BaseContext : IContext
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IApplication? _application;
   private bool _initialised;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public virtual bool IsAvailable => true;

   /// <summary>The application that the context belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the context has not been initialised.</exception>
   [NotNull]
   protected IApplication? Application
   {
      get => _application ?? throw new InvalidOperationException($"The context doesn't belong to an application yet, wait for it to be initialised.");
      private set => _application = value;
   }

   /// <summary>The context of the application that the context belongs to.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the property is accessed when the context has not been initialised.</exception>
   [NotNull]
   protected IApplicationContext? Context
   {
      get => _application?.Context ?? throw new InvalidOperationException($"The context doesn't belong to an application yet, wait for it to be initialised.");
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public async Task InitialiseAsync(IApplication application)
   {
      if (_initialised && _application != application)
         throw new ArgumentException($"The context has already been initialised for a different application.", nameof(application));

      if (_initialised is false && IsAvailable)
      {
         Application = application;
         await InitialiseAsync();
         _initialised = true;
      }
   }

   /// <summary>Initialises the context.</summary>
   /// <returns>A task representing the asynchronous operation.</returns>
   protected abstract Task InitialiseAsync();

   /// <inheritdoc/>
   public async Task CleanupAsync(IApplication application)
   {
      if (_initialised)
      {
         Debug.Assert(IsAvailable);

         if (Application != application)
            throw new ArgumentException($"The context has already been initialised for a different application.", nameof(application));

         _initialised = false;
         await CleanupAsync();
         Application = null;
      }
   }

   /// <summary>Cleans up the context.</summary>
   /// <returns>A task representing the asynchronous operation.</returns>
   protected abstract Task CleanupAsync();
   #endregion

   #region Helpers
   /// <summary>Throws an exception if the context is unavailable.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the context is unavailable.</exception>
   protected void ThrowIfUnavailable()
   {
      if (IsAvailable is false)
         ThrowForUnavailable();
   }

   /// <summary>Throws an exception indicating that the context is unavailable.</summary>
   /// <exception cref="InvalidOperationException">Thrown to show that the context is unavailable.</exception>
   [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
   protected static void ThrowForUnavailable() => throw new InvalidOperationException("The context is marked as unavailable and cannot be used.");
   #endregion
}
