namespace Sain.Applications;

/// <summary>
///   Represents a Sain application.
/// </summary>
public interface IApplication
{
   #region Properties
   /// <summary>The information about the application.</summary>
   IApplicationInfo Info { get; }

   /// <summary>The context of the application.</summary>
   IApplicationContext Context { get; }
   #endregion
}

/// <summary>
///   Represents a Sain application.
/// </summary>
/// <typeparam name="TContext">The type of the application's context.</typeparam>
public interface IApplication<TContext> : IApplication
   where TContext : notnull, IApplicationContext
{
   #region Properties
   /// <summary>The context of the application.</summary>
   new TContext Context { get; }
   IApplicationContext IApplication.Context => Context;
   #endregion
}
