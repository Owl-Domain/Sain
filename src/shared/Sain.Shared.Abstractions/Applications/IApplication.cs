namespace Sain.Shared.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
public interface IApplication
{
   #region Properties
   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }
   #endregion

   #region Methods
   /// <summary>Runs the application with the given <paramref name="arguments"/>.</summary>
   /// <param name="arguments">The arguments that have been passed in to the application.</param>
   /// <returns>A task representing the asynchronous operation.</returns>
   Task RunAsync(params string[] arguments);
   #endregion
}
