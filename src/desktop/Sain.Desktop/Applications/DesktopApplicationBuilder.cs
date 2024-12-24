namespace Sain.Desktop.Applications;

/// <summary>
///   Represents a builder for a desktop application.
/// </summary>
public class DesktopApplicationBuilder : BaseApplicationBuilder<DesktopApplicationBuilder>
{
   #region Properties
   /// <inheritdoc/>
   protected override DesktopApplicationBuilder Instance => this;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override IApplication BuildCore()
   {

      DesktopApplicationContext context = new(Contexts);
      Application application = new(context);

      return application;
   }
   #endregion
}
