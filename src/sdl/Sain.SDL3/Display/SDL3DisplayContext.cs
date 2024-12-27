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
   public override IDeviceCollection<IDisplayDevice> Devices => _devices;
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      foreach (SDL3_DisplayId id in EnumerateDisplayIds())
         AddDevice(id);
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
      SDL3_DisplayId[] ids;

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
         ReadOnlySpan<SDL3_DisplayId> span = new(native, count);
         ids = [.. span];
      }

      foreach (SDL3_DisplayId id in ids)
         yield return id;

      unsafe { Native.Free(ptr.ToPointer()); }
   }
   private void RemoveDevice(SDL3_DisplayId id)
   {
      if (_deviceLookup.TryGetValue(id, out SDL3DisplayDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"A display device with an unknown display id was removed ({id}).");

         return;
      }

      _deviceLookup.Remove(device.DisplayId);
      _devices.Remove(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3DisplayContext>($"Display device removed, id = ({device.Id}), display id = ({device.DisplayId})");
   }
   private void AddDevice(SDL3_DisplayId id)
   {
      SDL3DisplayDevice device = new(id, Context);

      if (_deviceLookup.TryAdd(id, device) is false)
      {
         SDL3DisplayDevice old = _deviceLookup[id];

         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"A device with a duplicate display id ({id}) was detected, old id = ({old.Id}), new id = ({device.Id}).");

         _devices.Remove(old);
         _deviceLookup[id] = device; // Override
      }

      _devices.Add(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3DisplayContext>($"Display device added, id = ({device.Id}), display id = ({device.DisplayId})");
   }
   private void RouteEvent(in SDL3_DisplayEvent ev)
   {
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
