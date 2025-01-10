namespace Sain.Shared.Logging;

/// <summary>
///   Represents information about a log path prefix.
/// </summary>
/// <param name="prefix">The prefix that should be used when turning the logged source file paths into relative ones.</param>
/// <param name="project">The name of the project that the prefix belongs to.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class LogPathPrefix(string prefix, string project) : ILogPathPrefix
{
   #region Properties
   /// <inheritdoc/>
   public string Prefix { get; } = prefix;
   /// <inheritdoc/>
   public string Project { get; } = project;
   #endregion

   #region Methods
   private string DebuggerDisplay() => $"LogPathPrefix {{ Prefix = (\"{Prefix}\"), Project = (\"{Project}\") }}";
   #endregion
}
