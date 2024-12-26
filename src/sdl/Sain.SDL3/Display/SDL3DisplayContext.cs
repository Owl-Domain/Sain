namespace Sain.SDL3.Display;

/// <summary>
///   Represents the SDL3 specific context for display information.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public unsafe sealed class SDL3DisplayContext(IContextProvider? provider) : BaseDisplayContext(provider), IDisplayContext, ISDL3Context
{
   #region Fields
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private readonly DeviceCollection<SDL3DisplayDevice> _devices = [];
   private readonly Dictionary<SDL3_DisplayId, SDL3DisplayDevice> _deviceLookup = [];
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Video;

   /// <inheritdoc/>
   public override string Kind => CoreContextKinds.Display;

   /// <inheritdoc/>
   public override IDeviceCollection<IDisplayDevice> Devices => _devices;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      foreach (SDL3_DisplayId id in EnumerateDisplayIds())
      {
         SDL3DisplayDevice device = new(id, Context);
         _devices.Add(device);
         _deviceLookup[id] = device;
      }
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      _devices.Clear();
      _deviceLookup.Clear();
   }

   /// <inheritdoc/>
   public override void RefreshDevices()
   {
      HashSet<SDL3_DisplayId> seen = [];

      foreach (SDL3_DisplayId id in EnumerateDisplayIds())
      {
         if (_deviceLookup.TryGetValue(id, out SDL3DisplayDevice? device))
            device.Refresh();
         else
            AddDevice(id);

         seen.Add(id);
      }

      HashSet<SDL3_DisplayId> toRemove = [];
      foreach (SDL3_DisplayId id in _deviceLookup.Keys)
      {
         if (seen.Contains(id) is false)
            toRemove.Add(id);
      }

      foreach (SDL3_DisplayId id in toRemove)
         RemoveDevice(id);
   }
   #endregion

   #region Helpers
   private IEnumerable<SDL3_DisplayId> EnumerateDisplayIds()
   {
      nint ptr;
      ReadOnlySpan<SDL3_DisplayId> ids;

      unsafe
      {
         SDL3_DisplayId* native = Native.GetDisplays(out int count);
         if (native is null)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Error<SDL3DisplayContext>($"Couldn't get the display devices. ({Native.LastError})");

            yield break;
         }

         Debug.Assert(count >= 0);

         ptr = new(native);
         ids = new(native, count);
      }

      foreach (SDL3_DisplayId id in ids)
         yield return id;

      unsafe { Native.Free(ptr.ToPointer()); }
   }
   private void RemoveDevice(SDL3_DisplayId displayId)
   {
      if (_deviceLookup.TryGetValue(displayId, out SDL3DisplayDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"A display device with an unknown display id was removed ({displayId}).");

         return;
      }

      _deviceLookup.Remove(device.DisplayId);
      _devices.Remove(device);
   }
   private void AddDevice(SDL3_DisplayId displayId)
   {
      SDL3DisplayDevice device = new(displayId, Context);
      if (_deviceLookup.TryAdd(displayId, device) is false)
      {
         SDL3DisplayDevice old = _deviceLookup[displayId];

         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"A device with a duplicate display id ({displayId}) was detected, old id = ({old.Id}), new id = ({device.Id}).");

         _devices.Remove(old);
         _deviceLookup[displayId] = device; // Override
      }

      _devices.Add(device);
   }
   private void RouteEvent(in SDL3_DisplayEvent ev)
   {
      Debug.WriteLine($"Routing event {ev.Type}");

      if (_deviceLookup.TryGetValue(ev.DisplayId, out SDL3DisplayDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"Received a display event for an unknown display id ({ev.DisplayId}).");

         return;
      }

      device.RouteEvent(ev);
   }
   unsafe void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsDisplayEvent(out SDL3_DisplayEvent display) is false)
         return;

      if (display.Type is SDL3_EventType.DisplayAdded)
      {
         AddDevice(display.DisplayId);
         return;
      }

      if (display.Type is SDL3_EventType.DisplayRemoved)
      {
         RemoveDevice(display.DisplayId);
         return;
      }

      RouteEvent(display);
   }
   #endregion
}
