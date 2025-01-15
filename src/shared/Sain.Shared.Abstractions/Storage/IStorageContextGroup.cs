namespace Sain.Shared.Storage;

/// <summary>
///   Represents a context group for the application's storage contexts.
/// </summary>
public interface IStorageContextGroup
{
   #region Properties
   /// <summary>The context for general storage operations.</summary>
   IGeneralStorageContext General { get; }

   /// <summary>The context for temporary storage.</summary>
   ITemporaryStorageContext Temporary { get; }

   /// <summary>The context for application specific storage.</summary>
   IApplicationStorageContextGroup Application { get; }
   #endregion
}
