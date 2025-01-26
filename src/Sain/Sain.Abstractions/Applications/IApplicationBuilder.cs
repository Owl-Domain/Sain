namespace Sain.Applications;

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
public interface IApplicationBuilder
{
   #region Methods
   /// <summary>Builds the configured Sain application.</summary>
   /// <returns>The built application.</returns>
   IApplication Build();
   #endregion
}

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
public interface IApplicationBuilder<TSelf> : IApplicationBuilder
   where TSelf : notnull, IApplicationBuilder<TSelf>
{
   #region Methods
   /// <summary>Sets the application name to the given <paramref name="name"/>.</summary>
   /// <param name="name">The name of the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <exception cref="InvalidOperationException">Thrown if the application name has already been set.</exception>
   TSelf WithApplicationName(string name);

   /// <summary>Adds the given <paramref name="id"/> as an extra application id.</summary>
   /// <param name="id">The id to add to the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>The first id that is provided will be considered the default one.</item>
   ///      <item>This method will not perform any duplicate checking, make sure to not add duplicate ids yourself.</item>
   ///   </list>
   /// </remarks>
   TSelf WithApplicationId(IApplicationId id);

   /// <summary>Adds the given <paramref name="version"/> as an extra application version.</summary>
   /// <param name="version">The version to add to the application.</param>
   /// <returns>The used builder instance.</returns>
   /// <remarks>
   ///   <list type="bullet">
   ///      <item>The first version that is provided will be considered the default one.</item>
   ///      <item>This method will not perform any duplicate checking, make sure to not add duplicate versions yourself.</item>
   ///   </list>
   /// </remarks>
   TSelf WithApplicationVersion(IApplicationVersion version);
   #endregion
}

/// <summary>
///   Represents the builder for a Sain application.
/// </summary>
/// <typeparam name="TSelf">The type of the application builder.</typeparam>
/// <typeparam name="TApplication">The type of the built application.</typeparam>
public interface IApplicationBuilder<TSelf, TApplication> : IApplicationBuilder<TSelf>
   where TSelf : IApplicationBuilder<TSelf, TApplication>
   where TApplication : IApplication
{
   #region Methods
   /// <summary>Builds the configured Sain application.</summary>
   /// <returns>The built application.</returns>
   new TApplication Build();
   IApplication IApplicationBuilder.Build() => Build();
   #endregion
}
