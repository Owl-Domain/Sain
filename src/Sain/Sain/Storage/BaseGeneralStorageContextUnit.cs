namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the base implementation for the application context unit that is responsible for general storage operations.
/// </summary>
/// <param name="provider">The context provider that this unit came from.</param>
public abstract class BaseGeneralStorageContextUnit(IContextProviderUnit? provider) : BaseContextUnit(provider), IGeneralStorageContextUnit
{
   #region Properties
   /// <inheritdoc/>
   public sealed override Type Kind => typeof(IGeneralStorageContextUnit);
   #endregion

   #region Methods
   /// <inheritdoc/>
   public abstract string GetDuplicateFileName(ReadOnlySpan<char> fileName, ReadOnlySpan<char> extension, int attempt);

   /// <inheritdoc/>
   public abstract ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> fileName, bool extractMultipart = true);

   /// <inheritdoc/>
   public abstract ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path);

   /// <inheritdoc/>
   public abstract ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path, bool multipartExtension = true);
   #endregion
}
