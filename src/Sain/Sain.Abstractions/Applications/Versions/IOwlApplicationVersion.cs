namespace Sain.Applications.Versions;

/// <summary>
///   Represents an application version that follows the Owl versioning scheme (Owlver for short).
/// </summary>
/// <remarks>This is the versioning scheme made up and preferred by <see href="https://github.com/nightowl286">Nightowl</see>.</remarks>
public interface IOwlApplicationVersion : IApplicationVersion
{
   #region Properties
   /// <summary>
   ///   The primary number of the version, should only be incremented on full rewrites and redesigns,
   ///   usually indicates a whole new application (that may or may not be compatible) instead of just a simple update.
   /// </summary>
   /// <remarks>Incrementing this number should reset the <see cref="Feature"/> and <see cref="Tweaks"/> numbers.</remarks>
   uint Design { get; }

   /// <summary>The secondary number of the version, this should be incremented when a new update either adds, removes, or modifies features.</summary>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>Incrementing this number should reset the <see cref="Tweaks"/> number.</item>
   ///      <item>If an update has both feature related changes and tweaks, then incrementing this number takes priority.</item>
   ///   </list>
   /// </remarks>
   uint Feature { get; }

   /// <summary>The tertiary number of the version, this should be incremented when a new update only has smaller tweaks or bug fixes.</summary>
   /// <remarks>If an update has both feature related changes and tweaks, then incrementing the <see cref="Feature"/> number takes priority.</remarks>
   uint Tweaks { get; }

   /// <summary>The (optional) suffix of the version, used to indicate different release phases like alpha/beta/prerelease.</summary>
   string? Suffix { get; }
   #endregion
}
