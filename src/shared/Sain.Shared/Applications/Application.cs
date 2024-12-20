namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <param name="context">The context of the application.</param>
public sealed class Application(IApplicationContext context) : IApplication
{
   #region Properties
   /// <inheritdoc/>
   public IApplicationContext Context { get; } = context;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public async Task RunAsync(params string[] arguments)
   {
      await Context.InitialiseAsync();
      try
      {
         // Todo(Nightowl): Add dispatcher processing here;
      }
      finally
      {
         await Context.CleanupAsync();
      }
   }
   #endregion

   #region Functions
   /// <summary>Creates a builder for a new application.</summary>
   /// <returns>The application builder which can be used to configure the application.</returns>
   public static IApplicationBuilder New() => new ApplicationBuilder();
   #endregion
}
