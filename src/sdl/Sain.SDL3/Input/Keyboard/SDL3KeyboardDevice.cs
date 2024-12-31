namespace Sain.SDL3.Input.Keyboard;

/// <summary>
///   Represents SDL3 specific information about a keyboard device.
/// </summary>
public class SDL3KeyboardDevice : ObservableBase, IKeyboardDevice
{
   #region Fields
   private readonly IApplicationContext _context;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IDeviceId _deviceId;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string _name;
   #endregion

   #region Properties
   internal SDL3_KeyboardId KeyboardId { get; }

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
   internal SDL3KeyboardDevice(IApplicationContext context, SDL3_KeyboardId id)
   {
      _context = context;

      KeyboardId = id;
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
      DeviceId = new DeviceId($"{KeyboardId}", Name);
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_name))]
   public void RefreshName()
   {
      string? name = Native.GetKeyboardNameForId(KeyboardId);

      if (name is null && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3KeyboardDevice>($"Failed to get the name for the keyboard ({KeyboardId}). ({Native.LastError}).");

      Name = name ?? string.Empty;
   }
   #endregion
}
