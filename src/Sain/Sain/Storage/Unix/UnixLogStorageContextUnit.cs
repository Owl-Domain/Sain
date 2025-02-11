namespace OwlDomain.Sain.Storage.Unix;

/// <summary>
///   Represents the unix specific context unit for the application's log related storage.
/// </summary>
public sealed class UnixLogStorageContextUnit(IContextProviderUnit? provider) : BaseLogStorageContextUnit(provider)
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private List<string>? _readFrom;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string? _writeTo;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string? _sessionDirectory;
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

   /// <inheritdoc/>
   public override string SessionDirectory
   {
      get
      {
         ThrowIfNotInitialised();
         Debug.Assert(_sessionDirectory is not null);

         return _sessionDirectory;
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnAttach()
   {
      base.OnAttach();

      string appName = UnixStorageContextUnitHelper.GetApplicationDirectoryName(Application);

      _writeTo = Path.Combine(UnixStorageContextUnitHelper.XdgDataHome, appName, "logs");
      _sessionDirectory = Path.Combine(_writeTo, Application.StartedOn.ToString("yyyy-MM-dd HH-mm-ss"));

      _readFrom = [_writeTo];

      foreach (string baseDirectory in UnixStorageContextUnitHelper.XdgDataDirs)
      {
         string directory = Path.Combine(baseDirectory, appName, "logs");
         _readFrom.Add(directory);
      }
   }

   /// <inheritdoc/>
   protected override void OnDetach()
   {
      base.OnDetach();

      _readFrom = null;
      _writeTo = null;
      _sessionDirectory = null;
   }
   #endregion
}
