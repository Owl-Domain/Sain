namespace OwlDomain.Sain.Storage.Unix;

/// <summary>
///   Represents the unix specific context unit for the application's data related storage.
/// </summary>
public sealed class UnixDataStorageContextUnit(IContextProviderUnit? provider) : BaseDataStorageContextUnit(provider)
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private List<string>? _readFrom;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string? _writeTo;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public override IReadOnlyList<string> ReadFrom
   {
      get
      {
         ThrowIfNotInitialised();
         Debug.Assert(_readFrom is not null);

         return _readFrom;
      }
   }

   /// <inheritdoc/>
   public override string WriteTo
   {
      get
      {
         ThrowIfNotInitialised();
         Debug.Assert(_writeTo is not null);

         return _writeTo;
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnAttach()
   {
      base.OnAttach();

      string appName = UnixStorageContextUnitHelper.GetApplicationDirectoryName(Application);

      _writeTo = Path.Combine(UnixStorageContextUnitHelper.XdgDataHome, appName, "data");
      _readFrom = [];

      foreach (string baseDirectory in UnixStorageContextUnitHelper.XdgDataDirs)
      {
         string directory = Path.Combine(baseDirectory, appName, "data");
         _readFrom.Add(directory);
      }
   }

   /// <inheritdoc/>
   protected override void OnDetach()
   {
      base.OnDetach();

      _readFrom = null;
      _writeTo = null;
   }
   #endregion
}
