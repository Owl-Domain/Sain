namespace Sain.Applications.Units;

/// <summary>
///   Represents an application unit that is responsible for providing other context units to the application.
/// </summary>
public interface IContextProviderUnit : IApplicationUnit
{
   #region Properties
   /// <summary>
   ///   Whether this context provider unit should be omitted from the application
   ///   if it hasn't been used to provide any context units to the application.
   /// </summary>
   bool OmitIfUnused { get; }
   #endregion

   #region Methods
   /// <summary>Tries to provide a context <paramref name="unit"/> of the given <paramref name="kind"/>.</summary>
   /// <param name="kind">The kind of the context unit to try and provide.</param>
   /// <param name="unit">The provided context unit, or <see langword="null"/> if no context unit could be provided for the given <paramref name="kind"/>.</param>
   /// <returns><see langword="true"/> if the context <paramref name="unit"/> could be provided, <see langword="false"/> otherwise.</returns>
   bool TryProvide(Type kind, [NotNullWhen(true)] out IContextUnit? unit);
   #endregion
}
