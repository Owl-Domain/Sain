namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents a prefix based log path converter.
/// </summary>
/// <param name="prefix">The prefix that should be used when turning the logged source file paths into project relative ones.</param>
/// <param name="project">The name of the project that the prefix belongs to.</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class LogPathPrefixConverter(string prefix, string project) : ILogPathConverter
{
   #region Properties
   /// <summary>The prefix that should be used when turning the logged source file paths into project relative ones.</summary>
   public string Prefix { get; } = prefix;

   /// <inheritdoc/>
   public string Project { get; } = project;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public int CompareTo(ILogPathConverter? other)
   {
      if (other is LogPathPrefixConverter prefix)
         return CompareTo(prefix);

      return 0;
   }
   private int CompareTo(LogPathPrefixConverter other) => other.Prefix.Length.CompareTo(Prefix.Length);

   /// <inheritdoc/>
   public bool TryGetRelative(string path, [NotNullWhen(true)] out string? relative)
   {
      if (path.StartsWith(Prefix))
      {
         relative = path[Prefix.Length..];
         return true;
      }

      relative = default;
      return false;
   }
   private string DebuggerDisplay() => $"LogPathPrefix {{ Prefix = (\"{Prefix}\"), Project = (\"{Project}\") }}";
   #endregion
}
