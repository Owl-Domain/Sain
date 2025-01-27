namespace Sain.Applications.Ids;

/// <summary>
///   Represents a collection of the ids for a Sain application.
/// </summary>
public class ApplicationIdCollection : List<IApplicationId>, IApplicationIdCollection
{
   #region Properties
   /// <inheritdoc/>
   public IApplicationId Default
   {
      get
      {
         if (Count > 0)
            return this[0];

         throw new InvalidOperationException($"The application doesn't have any ids.");
      }
   }
   #endregion
}
