using System.Reflection;

namespace OwlDomain.Sain.Applications.Versions;

/// <summary>
///   Represents an application version that comes from an assembly.
/// </summary>
public interface IAssemblyApplicationVersion : IApplicationVersion
{
   #region Properties
   /// <summary>The assembly that the version comes from.</summary>
   public Assembly Assembly { get; }

   /// <summary>The application version.</summary>
   public Version Version { get; }
   #endregion
}
