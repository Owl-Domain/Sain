namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for an unavailable context.
/// </summary>
public abstract class BaseUnavailableContext : BaseContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override bool IsAvailable => false;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected sealed override Task InitialiseAsync() => Task.CompletedTask;

   /// <inheritdoc/>
   protected sealed override Task CleanupAsync() => Task.CompletedTask;
   #endregion
}
