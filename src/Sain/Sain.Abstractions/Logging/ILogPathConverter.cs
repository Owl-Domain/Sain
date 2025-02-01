namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents a converter for log entry paths.
/// </summary>
public interface ILogPathConverter : IComparable<ILogPathConverter?>
{
   #region Properties
   /// <summary>The name of the project that the produced relative paths are for.</summary>
   string Project { get; }
   #endregion

   #region Methods
   /// <summary>Tries to convert the given <paramref name="path"/> into a project <paramref name="relative"/> path.</summary>
   /// <param name="path">The path to try and convert.</param>
   /// <param name="relative">The given <paramref name="path"/> converted to a project relative path, or <see langword="null"/> if the path couldn't be converted.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="path"/> could be
   ///   converted to a project relative path, <see langword="false"/> otherwise.
   /// </returns>
   bool TryGetRelative(string path, [NotNullWhen(true)] out string? relative);
   #endregion
}
