namespace Sain.Shared.Logging;

/// <summary>
///   Represents information about a log path prefix.
/// </summary>
public interface ILogPathPrefix
{
   #region Properties
   /// <summary>The prefix that should be used when turning the logged source file paths into relative ones.</summary>
   string Prefix { get; }

   /// <summary>The name of the project that the prefix belongs to.</summary>
   string Project { get; }
   #endregion
}
