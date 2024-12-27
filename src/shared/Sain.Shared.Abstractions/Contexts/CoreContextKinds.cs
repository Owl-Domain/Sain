namespace Sain.Shared.Contexts;

/// <summary>
///   Represents the different kinds of contexts that are available by default.
/// </summary>
public static class CoreContextKinds
{
   #region Constants
   /// <summary>The kind of the <see cref="IAudioPlaybackContext"/>.</summary>
   public const string AudioPlayback = "core.audio.playback";

   /// <summary>The kind of the <see cref="IAudioCaptureContext"/>.</summary>
   public const string AudioCapture = "core.audio.capture";

   /// <summary>The kind of the <see cref="IDispatcherContext"/>.</summary>
   public const string Dispatcher = "core.dispatcher";

   /// <summary>The kind of the <see cref="ILoggingContext"/>.</summary>
   public const string Logging = "core.logging";

   /// <summary>The kind of the <see cref="IDisplayContext"/>.</summary>
   public const string Display = "core.display";

   /// <summary>The kind of the <see cref="IMouseInputContext"/>.</summary>
   public const string MouseInput = "core.input.mouse";

   /// <summary>The kind of the <see cref="IKeyboardInputContext"/>.</summary>
   public const string KeyboardInput = "core.input.keyboard";
   #endregion
}
