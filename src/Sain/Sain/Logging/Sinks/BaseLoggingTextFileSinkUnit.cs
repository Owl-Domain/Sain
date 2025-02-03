namespace OwlDomain.Sain.Logging.Sinks;

/// <summary>
///   Represents the base implementation for an application unit that acts as a text file based logging sink.
/// </summary>
public abstract class BaseLoggingTextFileSinkUnit : BaseLoggingFileSinkUnit
{
   #region Fields
   private StreamWriter? _writer;
   #endregion

   #region Properties
   /// <summary>The stream writer for the created log file.</summary>
   /// <exception cref="InvalidOperationException">Thrown if accessed before the stream has been opened, or if accessed after it has been closed.</exception>
   protected StreamWriter Writer => _writer ?? throw new InvalidOperationException($"The logging sink hasn't been opened yet.");
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Open()
   {
      base.Open();
      _writer = new(Stream);
   }

   /// <inheritdoc/>
   protected override void Close()
   {
      _writer?.Close();
      _writer = null;

      base.Close();
   }
   #endregion

   #region Helpers
   /// <inheritdoc/>
   protected override void Flush()
   {
      _writer?.Flush();
      base.Flush();
   }
   #endregion
}
