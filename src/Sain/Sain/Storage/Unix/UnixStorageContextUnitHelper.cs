namespace OwlDomain.Sain.Storage.Unix;

/// <summary>
///   Represents the helper for unix specific storage context units.
/// </summary>
public static class UnixStorageContextUnitHelper
{
   #region Properties
   /// <summary>The user's home directory.</summary>
   public static string Home { get; }

   /// <summary>The main directory where data files should be saved to.</summary>
   /// <remarks>Higher priority than <see cref="XdgDataDirs"/> for reading.</remarks>
   public static string XdgDataHome { get; }

   /// <summary>The directories from which data files should be read form.</summary>
   /// <remarks>Most specific to least specific.</remarks>
   public static IReadOnlyList<string> XdgDataDirs { get; }

   /// <summary>The main directory where config files should be saved to.</summary>
   /// <remarks>Higher priority than <see cref="XdgConfigDirs"/> for reading.</remarks>
   public static string XdgConfigHome { get; }

   /// <summary>The directories from which config files should be read from.</summary>
   /// <remarks>Most specific to least specific.</remarks>
   public static IReadOnlyList<string> XdgConfigDirs { get; }
   #endregion

   #region Constructors
   static UnixStorageContextUnitHelper()
   {
      Home = GetPath("HOME", "/home", Environment.UserName);
      XdgDataHome = GetPath("XDG_DATA_HOME", Home, ".local", "share");
      XdgConfigHome = GetPath("XDG_CONFIG_HOME", Home, ".config");
      XdgDataDirs = GetDirectories("XDG_DATA_DIRS", ["/usr/local/share", "/usr/share"]);
      XdgConfigDirs = GetDirectories("XDG_CONFIG_DIRS", ["/etc/xdg"]);
   }
   #endregion

   #region Functions
   /// <summary>Gets the name for the application specific directory.</summary>
   /// <param name="application">The application to get the name for.</param>
   /// <returns>The name for the application specific directory.</returns>
   public static string GetApplicationDirectoryName(this IApplication application)
   {
      if (application.Info.Ids.Count > 0)
         return application.Info.Ids.Default.DisplayName;

      return application.Info.Name;
   }
   #endregion

   #region Helpers
   private static IReadOnlyList<string> GetDirectories(string variableName, ReadOnlySpan<string> fallback)
   {
      if (TryGetVariable(variableName, out string? value))
         return value.Split(':');

      if (Path.DirectorySeparatorChar == '/')
         return [.. fallback];

      // Note(Nightowl): This path should *hopefully* never actually happen;

      List<string> directories = [];
      foreach (string directory in fallback)
      {
         string actualDirectory = Path.Combine(directory.Split('/'));
         directories.Add(actualDirectory);
      }

      return directories;
   }
   private static string GetPath(string variableName, params ReadOnlySpan<string> fallbackComponents)
   {
      if (TryGetVariable(variableName, out string? variable))
         return variable;

      return Path.Combine([.. fallbackComponents]);
   }
   private static bool TryGetVariable(string name, [NotNullWhen(true)] out string? value)
   {
      value = Environment.GetEnvironmentVariable(name);
      return value is not null;
   }
   #endregion
}
