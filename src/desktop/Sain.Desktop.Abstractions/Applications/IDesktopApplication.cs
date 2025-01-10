namespace Sain.Desktop.Applications;

/// <summary>
///   Represents a Sain desktop application.
/// </summary>
/// <typeparam name="TContext">The type of the applications context.</typeparam>
/// <typeparam name="TApplication">The type of the application.</typeparam>
public interface IDesktopApplication<TContext, TApplication> : IApplication<TContext, TApplication>
   where TContext : IDesktopApplicationContext
   where TApplication : IDesktopApplication<TContext, TApplication>
{
}

/// <summary>
///   Represents a Sain desktop application.
/// </summary>
public interface IDesktopApplication : IDesktopApplication<IDesktopApplicationContext, IDesktopApplication> { }
