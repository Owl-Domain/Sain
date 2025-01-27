namespace Sain.Applications.Units;

/// <summary>
///   Represents an application unit that is responsible for providing a single functionality for the application.
/// </summary>
/// <remarks>An application may only have one context unit of a given <see cref="IApplicationUnit.Kind"/>.</remarks>
public interface IContextUnit : IApplicationUnit
{
   #region Properties
   /// <summary>The context provider that this unit came from.</summary>
   IContextProviderUnit? Provider { get; }

   /// <summary>Whether the context provider that this unit came from should automatically be initialised before this unit.</summary>
   bool InitialiseAfterProvider { get; }
   #endregion
}
