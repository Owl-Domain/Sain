namespace Sain.Shared.Storage.Application;

/// <summary>
///   Represents a context group for the application specific storage contexts.
/// </summary>
public interface IApplicationStorageContextGroup
{
   #region Properties
   /// <summary>The context for the application specific log storage.</summary>
   IApplicationLogStorageContext Logs { get; }

   /// <summary>The context for the application specific cache storage.</summary>
   IApplicationCacheStorageContext Cache { get; }

   /// <summary>The context for the application specific state storage.</summary>
   IApplicationStateStorageContext State { get; }

   /// <summary>The context for the application specific config storage.</summary>
   IApplicationConfigStorageContext Config { get; }
   #endregion
}
