using System.IO.Compression;
using System.Text;

namespace OwlDomain.Sain.Logging;

/// <summary>
///   Represents a general application unit that will automatically convert
///   the log directory for the current application into a compressed archive.
/// </summary>
public class LogFileZippingUnit : BaseApplicationUnit
{
   #region Properties
   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> RequiresUnits { get; } = [typeof(IGeneralStorageContextUnit), typeof(ILogStorageContextUnit)];

   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> InitialiseBeforeUnits { get; } = [typeof(ILoggingFileSinkUnit)];

   /// <summary>The encoding to use for the names in the archive.</summary>
   public Encoding EntryNameEncoding { get; set; } = Encoding.UTF8;

   /// <summary>The compression level to use for the archive.</summary>
   public CompressionLevel CompressionLevel { get; set; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="LogFileZippingUnit"/>.</summary>
   public LogFileZippingUnit()
   {
#if NET6_0_OR_GREATER
      CompressionLevel = CompressionLevel.SmallestSize;
#else
      CompressionLevel = CompressionLevel.Optimal;
#endif
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void OnCleanup()
   {
      Debug.Assert(Context.Storage.Log is not null);
      Debug.Assert(Context.Storage.General is not null);

      base.OnCleanup();

      if (Context.Logging is null)
         return;

      string source = Context.Storage.Log.SessionDirectory;
      string destination = source + ".zip";

      if (File.Exists(destination))
      {
         string? directory = Path.GetDirectoryName(source);
         ReadOnlySpan<char> name = Context.Storage.General.GetFileName(source);

         Debug.Assert(directory is not null, "Session directory will never be the root of the file system, if it is that's on you.");

         for (int attempt = 2; File.Exists(destination); attempt++)
         {
            string duplicateName = Context.Storage.General.GetDuplicateFileName(name, ".zip", attempt);
            destination = Path.Combine(directory, duplicateName);
         }
      }

      ZipFile.CreateFromDirectory(source, destination, CompressionLevel, true, EntryNameEncoding);
      Directory.Delete(source, true);
   }
   #endregion
}
