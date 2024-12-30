namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for a application's context.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public abstract class BaseContext(IContextProvider? provider) : BaseHasApplicationInit, IContext
{
   #region Properties
   /// <inheritdoc/>
   public IContextProvider? Provider { get; } = provider;

   /// <inheritdoc/>
   public abstract string Kind { get; }

   /// <inheritdoc/>
   public virtual bool IsAvailable => true;
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

   /// <summary>Throws an exception indicating that the context is unavailable.</summary>
   /// <typeparam name="T">The type to fake return.</typeparam>
   /// <returns>The value that isn't actually returned.</returns>
   /// <exception cref="InvalidOperationException">Thrown to show that the context is unavailable.</exception>
   [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
   protected static T ThrowForUnavailable<T>() => throw new InvalidOperationException("The context is marked as unavailable and cannot be used.");
   #endregion
}
