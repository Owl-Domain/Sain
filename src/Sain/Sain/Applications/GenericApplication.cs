namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents a generic Sain application.
/// </summary>
/// <param name="info">The information about the application.</param>
/// <param name="configuration">The configuration options for the application.</param>
/// <param name="context">The context of the application.</param>
public sealed class GenericApplication(
   IApplicationInfo info,
   IApplicationConfiguration configuration,
   IApplicationContext context)
   : Application<IGenericApplication, IApplicationContext>(info, configuration, context), IGenericApplication
{
   #region Functions
   /// <summary>Starts building a new generic Sain application.</summary>
   /// <returns>The application builder used to build and customise the new application.</returns>
   public static GenericApplicationBuilder New() => new();
   #endregion
}
