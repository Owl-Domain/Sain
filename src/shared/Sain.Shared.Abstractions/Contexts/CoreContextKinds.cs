using System.Data.Common;

namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the different kinds of contexts that are available by default.
/// </summary>
public static class CoreContextKinds
{
   #region Regular contexts
   /// <summary>The kind of the <see cref="IDispatcherContext"/>.</summary>
   public const string Dispatcher = "core.dispatcher";

   /// <summary>The kind of the <see cref="ILoggingContext"/>.</summary>
   public const string Logging = "core.logging";

   /// <summary>The kind of the <see cref="IDisplayContext"/>.</summary>
   public const string Display = "core.display";
   #endregion

   #region Audio contexts
   /// <summary>The kind of the <see cref="IAudioPlaybackContext"/>.</summary>
   public const string AudioPlayback = "core.audio.playback";

   /// <summary>The kind of the <see cref="IAudioCaptureContext"/>.</summary>
   public const string AudioCapture = "core.audio.capture";
   #endregion

   #region Input contexts
   /// <summary>The kind of the <see cref="IMouseInputContext"/>.</summary>
   public const string MouseInput = "core.input.mouse";

   /// <summary>The kind of the <see cref="IKeyboardInputContext"/>.</summary>
   public const string KeyboardInput = "core.input.keyboard";
   #endregion

   #region System contexts
   /// <summary>The kind of the <see cref="ISystemTimeContext"/>.</summary>
   public const string SystemTime = "core.system.time";
   #endregion

   #region Storage contexts
   /// <summary>The kind of the <see cref="IGeneralStorageContext"/>.</summary>
   public const string GeneralStorage = "core.storage.general";

   /// <summary>The kind of the <see cref="ITemporaryStorageContext"/>.</summary>
   public const string TemporaryStorage = "core.storage.temporary";

   /// <summary>The kind of the <see cref="IStoragePermissionsContext"/>.</summary>
   public const string StoragePermissions = "core.storage.permissions";

   #region Application storage
   /// <summary>The kind of the <see cref="IApplicationLogStorageContext"/>.</summary>
   public const string ApplicationLogStorage = "core.storage.application.log";

   /// <summary>The kind of the <see cref="IApplicationCacheStorageContext"/>.</summary>
   public const string ApplicationCacheStorage = "core.storage.application.cache";

   /// <summary>The kind of the <see cref="IApplicationConfigStorageContext"/>.</summary>
   public const string ApplicationConfigStorage = "core.storage.application.config";

   /// <summary>The kind of the <see cref="IApplicationStateStorageContext"/>.</summary>
   public const string ApplicationStateStorage = "core.storage.application.state";
   #endregion
   #endregion
}
