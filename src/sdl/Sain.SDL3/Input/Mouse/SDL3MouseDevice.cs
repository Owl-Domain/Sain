namespace Sain.SDL3.Input.Mouse;

/// <summary>
///   Represents SDL3 specific information about a mouse device.
/// </summary>
public class SDL3MouseDevice : ObservableBase, IMouseDevice
{
   #region Fields
   private readonly IApplicationContext _context;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IDeviceId _deviceId;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string _name;
   #endregion

   #region Properties
   internal SDL3_MouseId MouseId { get; }

   /// <inheritdoc/>
   public Guid Id { get; }

   /// <inheritdoc/>
   public IDeviceId DeviceId
   {
      get => _deviceId;

      [MemberNotNull(nameof(_deviceId))]
      private set => Set(ref _deviceId!, value);
   }

   /// <inheritdoc/>
   public string Name
   {
      get => _name;

      [MemberNotNull(nameof(_name))]
      private set
      {
         if (Set(ref _name!, value))
            RefreshDeviceId();
      }
   }
   #endregion

   #region Constructors
   internal SDL3MouseDevice(IApplicationContext context, SDL3_MouseId id)
   {
      _context = context;

      MouseId = id;
      Id = Guid.NewGuid();

      Refresh();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool IsMatch(IDeviceId id, out int score) => DeviceId.IsBasicPartialMatch(id, out score);
   #endregion

   #region Refresh methods
   /// <inheritdoc/>
   [MemberNotNull(nameof(_deviceId), nameof(_name))]
   public void Refresh()
   {
      RefreshName();
      RefreshDeviceId();
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_deviceId))]
   public void RefreshDeviceId()
   {
      DeviceId = new DeviceId($"{MouseId}", Name);
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_name))]
   public void RefreshName()
   {
      string? name = Native.GetMouseNameForId(MouseId);
      if (name is null && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3MouseDevice>($"Failed to get the name for the mouse ({MouseId}). ({Native.LastError})");

      Name = name ?? string.Empty;
   }
   #endregion
}
