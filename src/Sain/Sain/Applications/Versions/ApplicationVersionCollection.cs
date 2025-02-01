namespace OwlDomain.Sain.Applications.Versions;

/// <summary>
///   Represents a collection of the versions for a Sain application.
/// </summary>
public class ApplicationVersionCollection : List<IApplicationVersion>, IApplicationVersionCollection
{
   #region Properties
   /// <inheritdoc/>
   public IApplicationVersion Default
   {
      get
      {
         if (Count > 0)
            return this[0];

         throw new InvalidOperationException($"The application doesn't have any versions.");
      }
   }
   #endregion
}
