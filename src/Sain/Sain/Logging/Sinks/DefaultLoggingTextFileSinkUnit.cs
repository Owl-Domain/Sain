namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents the default implementation for an application unit that acts as a text file based logging sink.
/// </summary>
public class DefaultLoggingTextFileSinkUnit : BaseLoggingTextFileSinkUnit
{
   #region Fields
   private readonly object fileLock = new();
   private bool _hadEntry;
   #endregion

   #region Properties
   /// <inheritdoc/>
   public override IReadOnlyCollection<Type> RequiresUnits { get; } = [typeof(ILogStorageContextUnit)];
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Open()
   {
      base.Open();
      _hadEntry = false;
   }

   /// <inheritdoc/>
   protected override void GetLogPath(out string directory, out string preferredFileName)
   {
      Debug.Assert(Context.Storage.Log is not null);

      directory = Context.Storage.Log.WriteTo;

      DateTimeOffset time = Application.StartedOn;
      string name = time.ToString("yyyy-MM-dd HH-mm-ss");
      preferredFileName = name + ".txt";
   }

   /// <inheritdoc/>
   protected override void OnLogEntryAddedCore(ILoggingContextUnit context, ILogEntry entry)
   {
      lock (fileLock)
      {
         if (_hadEntry is false)
         {
            Writer.WriteLine("# OwlDomain/Sain/Default");
            Writer.WriteLine($"# Sain text log for '{Application.Info.Name}' on {Application.StartedOn:yyyy/MM/dd HH:mm:ss zzz}.");
            _hadEntry = true;
         }

         Writer.WriteLine();
         Writer.WriteLine($"[{entry.Date:yyyy/MM/dd HH:mm:ss.fff zzz}][{entry.Timestamp}][{entry.Severity}]");

         if (string.IsNullOrWhiteSpace(entry.Member))
            Writer.Write($"[{entry.Context}]");
         else
            Writer.Write($"[{entry.Context}.{entry.Member}]");

         if (entry.FileConverter is not null)
            Writer.Write($"[@{entry.FileConverter.Project}/{entry.File}]");
         else if (string.IsNullOrWhiteSpace(entry.File) is false)
            Writer.Write($"[{entry.File}]");
         else if (entry.Line > 0)
            Writer.Write("[<unknown-file>]");

         if (entry.Line > 0)
            Writer.Write($":{entry.Line:n0}");

         Writer.WriteLine();

         Writer.Write("   ");

         foreach (char ch in entry.Message)
         {
            if (ch is '\r') Writer.Write("\\r");
            else if (ch is '\n') Writer.Write("\\r");
            else if (ch is '\t') Writer.Write("\\t");
            else if (ch is '\a') Writer.Write("\\a");
            else if (ch is '\b') Writer.Write("\\b");
            else if (ch is '\f') Writer.Write("\\f");
            else if (ch is '\v') Writer.Write("\\v");
            else if (ch is '\0') Writer.Write("\\0");
            else
               Writer.Write(ch);
         }

         Writer.WriteLine();
      }
   }
   #endregion
}
