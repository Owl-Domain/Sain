namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the base implementation for an unavailable context.
/// </summary>
public abstract class BaseUnavailableContext : BaseContext
{
   #region Properties
   /// <inheritdoc/>
   public sealed override bool IsAvailable => false;

   /// <inheritdoc/>
   public sealed override IReadOnlyCollection<Type> InitialiseAfter => [];

   /// <inheritdoc/>
   public sealed override IReadOnlyCollection<Type> InitialiseBefore => [];
   #endregion

   #region Constructors
   /// <summary>Initialises the instance of the <see cref="BaseUnavailableContext"/>.</summary>
   protected BaseUnavailableContext() : base(null) { }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected sealed override void OnInitialise() { }

   /// <inheritdoc/>
   protected sealed override void OnCleanup() { }
   #endregion
}
