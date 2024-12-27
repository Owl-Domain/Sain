namespace Sain.SDL3.Display;

/// <summary>
///   Represents information about an SDL3 display device.
/// </summary>
public unsafe sealed class SDL3DisplayDevice : ObservableBase, IDisplayDevice, ISDL3EventHandler<SDL3_DisplayEvent>
{
   #region Fields
   private readonly IApplicationContext _context;
   private Rectangle _globalBounds;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private IDeviceId _deviceId;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string _name;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Size _resolution;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Rectangle _area;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private Rectangle _bounds;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private double _refreshRate;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private double _displayScale;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private bool _isPrimary;
   #endregion

   #region Properties
   internal SDL3_DisplayId DisplayId { get; }

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

   /// <inheritdoc/>
   public Size Resolution
   {
      get => _resolution;
      private set => Set(ref _resolution, value);
   }

   /// <inheritdoc/>
   public Rectangle Area
   {
      get => _area;
      private set
      {
         if (Set(ref _area, value))
            Bounds = _globalBounds - Area.Position;
      }
   }

   /// <inheritdoc/>
   public Rectangle Bounds
   {
      get => _bounds;
      private set => Set(ref _bounds, value);
   }

   /// <inheritdoc/>
   public double RefreshRate
   {
      get => _refreshRate;
      private set => Set(ref _refreshRate, value);
   }

   /// <inheritdoc/>
   public double DisplayScale
   {
      get => _displayScale;
      private set => Set(ref _displayScale, value);
   }

   /// <inheritdoc/>
   public bool IsPrimary
   {
      get => _isPrimary;
      private set => Set(ref _isPrimary, value);
   }
   #endregion

   #region Constructors
   internal SDL3DisplayDevice(SDL3_DisplayId displayId, IApplicationContext context)
   {
      DisplayId = displayId;
      _context = context;

      Id = Guid.NewGuid();

      Refresh();
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public bool IsMatch(IDeviceId id, out int score) => DeviceId.IsBasicPartialMatch(id, out score);

   /// <inheritdoc/>
   [MemberNotNull(nameof(_name), nameof(_deviceId))]
   public void Refresh()
   {
      RefreshName();
      RefreshDeviceId();
      UpdateDesktopMode();
      RefreshResolution();
      RefreshArea();
      RefreshBounds();
      RefreshIsPrimary();
   }

   /// <inheritdoc/>
   [MemberNotNull(nameof(_deviceId))]
   public void RefreshDeviceId() => DeviceId = new DeviceId($"{DisplayId}", Name);

   /// <inheritdoc/>
   [MemberNotNull(nameof(_name))]
   public void RefreshName()
   {
      string? name = Native.GetDisplayName(DisplayId);
      if (name is null && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DisplayDevice>($"Failed to get the name for the display ({DisplayId}). ({Native.LastError})");

      Name = name ?? string.Empty;
   }

   /// <inheritdoc/>
   public void RefreshDisplayScale()
   {
      DisplayScale = Native.GetDisplayContentScale(DisplayId);
      if (DisplayScale is 0 && _context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DisplayDevice>($"Failed to get the display scale for the display ({DisplayId}). ({Native.LastError})");
   }

   /// <inheritdoc/>
   public void RefreshResolution() => UpdateDesktopMode();

   /// <inheritdoc/>
   public void RefreshArea()
   {
      if (Native.GetDisplayBounds(DisplayId, out SDL3_Rect areaRect))
         Area = new(areaRect.X, areaRect.Y, areaRect.Width, areaRect.Height);
      else if (_context.Logging.IsAvailable)
      {
         _context.Logging.Error<SDL3DisplayDevice>($"Failed to get the display area for the display ({DisplayId}). ({Native.LastError})");
         Area = default;
      }
   }

   /// <inheritdoc/>
   public void RefreshBounds()
   {
      if (Native.GetDisplayUsableBounds(DisplayId, out SDL3_Rect boundsRect))
      {
         _globalBounds = new(boundsRect.X, boundsRect.Y, boundsRect.Width, boundsRect.Height);
         Bounds = _globalBounds - Area.Position;
      }
      else if (_context.Logging.IsAvailable)
      {
         _context.Logging.Error<SDL3DisplayDevice>($"Failed to get the display bounds for the display ({DisplayId}). ({Native.LastError})");

         _globalBounds = Area;
         Bounds = new(0, 0, Resolution);
      }
   }

   /// <inheritdoc/>
   public void RefreshRefreshRate() => UpdateDesktopMode();

   /// <inheritdoc/>
   public void RefreshIsPrimary()
   {
      SDL3_DisplayId primary = Native.GetPrimaryDisplay();
      if (primary.Id is 0)
      {
         if (_context.Logging.IsAvailable)
            _context.Logging.Error<SDL3DisplayDevice>($"Couldn't get the primary display id. ({Native.LastError})");

         IsPrimary = false;
         return;
      }

      IsPrimary = primary.Id == DisplayId.Id;
   }

   private void UpdateDesktopMode()
   {
      if (Native.GetDesktopDisplayMode(DisplayId, out SDL3_DisplayMode mode))
      {
         RefreshRate = mode.RefreshRate;
         Resolution = new(mode.Width, mode.Height);

         return;
      }

      if (_context.Logging.IsAvailable)
         _context.Logging.Error<SDL3DisplayDevice>($"Failed to get the display mode for the display ({DisplayId}). ({Native.LastError})");

      RefreshRate = 0;
      Resolution = default;
   }
   void ISDL3EventHandler<SDL3_DisplayEvent>.OnEvent(in SDL3_DisplayEvent ev)
   {
      if (ev.Type is SDL3_EventType.DisplayMoved)
      {
         RefreshArea();
         RefreshBounds();
      }
      else if (ev.Type is SDL3_EventType.DisplayContentScaleChanged)
         RefreshDisplayScale();
      else if (ev.Type is SDL3_EventType.DisplayDesktopModeChanged)
         UpdateDesktopMode();
   }
   #endregion
}
