namespace Sain.Desktop.Applications;

/// <summary>
///   Represents the context of a desktop application.
/// </summary>
/// <param name="contexts">The contexts that are available to the desktop application.</param>
public class DesktopApplicationContext(IReadOnlyCollection<IContext> contexts) : ApplicationContext(contexts)
{
}
