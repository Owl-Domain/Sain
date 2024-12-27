namespace Sain.SDL3.Input.Mouse;

/// <summary>
///   Represents the SDL3 specific context for mouse input.
/// </summary>
/// <param name="provider">The context provider that the context comes from.</param>
public sealed class SDL3MouseInputContext(IContextProvider? provider) : BaseMouseInputContext(provider), IMouseInputContext, ISDL3Context
{
   #region Fields
   private readonly DeviceCollection<SDL3MouseDevice> _devices = [];
   private readonly Dictionary<SDL3_MouseId, SDL3MouseDevice> _deviceLookup = [];
   #endregion

   #region Properties
   SDL3_InitFlags ISDL3Context.Flags => SDL3_InitFlags.Events;

   /// <inheritdoc/>
   public override IDeviceCollection<IMouseDevice> Devices => _devices;
   internal bool IsMouseCaptured { get; set; }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      foreach (SDL3_MouseId id in EnumerateMouseIds())
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
      HashSet<SDL3_MouseId> seen = [];

      foreach (SDL3_MouseId id in EnumerateMouseIds())
      {
         if (_deviceLookup.TryGetValue(id, out SDL3MouseDevice? device))
            device.Refresh();
         else
            AddDevice(id);

         seen.Add(id);
      }

      HashSet<SDL3_MouseId> toRemove = [];
      foreach (SDL3_MouseId id in _deviceLookup.Keys)
      {
         if (seen.Contains(id) is false)
            toRemove.Add(id);
      }

      foreach (SDL3_MouseId id in toRemove)
         RemoveDevice(id);
   }
   #endregion

   #region Helpers
   private IEnumerable<SDL3_MouseId> EnumerateMouseIds()
   {
      nint ptr;
      SDL3_MouseId[] ids;

      unsafe
      {
         SDL3_MouseId* native = Native.GetMice(out int count);
         if (native is null)
         {
            if (Context.Logging.IsAvailable)
               Context.Logging.Error<SDL3DisplayContext>($"Couldn't get the mouse devices. ({Native.LastError})");

            yield break;
         }

         Debug.Assert(count >= 0);

         ptr = new(native);
         ReadOnlySpan<SDL3_MouseId> span = new(native, count);
         ids = [.. span];
      }

      foreach (SDL3_MouseId id in ids)
         yield return id;

      unsafe { Native.Free(ptr.ToPointer()); }
   }
   private void RemoveDevice(SDL3_MouseId id)
   {
      if (_deviceLookup.TryGetValue(id, out SDL3MouseDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"A display mouse with an unknown mouse id was removed ({id}).");

         return;
      }

      _deviceLookup.Remove(device.MouseId);
      _devices.Remove(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3MouseInputContext>($"Mouse device removed, id = ({device.Id}), mouse id = ({device.MouseId})");
   }
   private void AddDevice(SDL3_MouseId id)
   {
      SDL3MouseDevice device = new(Context, this, id);
      if (_deviceLookup.TryAdd(id, device) is false)
      {
         SDL3MouseDevice old = _deviceLookup[id];

         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"A device with a duplicate mouse id ({id}) was detected, old id = ({old.Id}), new id = ({device.Id}).");

         _devices.Remove(old);
         _deviceLookup[id] = device; // Override
      }

      _devices.Add(device);

      if (Context.Logging.IsAvailable)
         Context.Logging.Debug<SDL3MouseInputContext>($"Mouse device added, id = ({device.Id}), mouse id = ({device.MouseId})");
   }
   private void RouteEvent<T>(in T ev, SDL3_MouseId id, SDL3_EventType type)
   {
      if (_deviceLookup.TryGetValue(id, out SDL3MouseDevice? device) is false)
      {
         if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3DisplayContext>($"Received a mouse event ({type}) for an unknown mouse id ({id}).");

         return;
      }

      if (device is ISDL3EventHandler<T> handler)
         handler.RouteEvent(ev);
   }
   void ISDL3Context.OnEvent(in SDL3_Event ev)
   {
      if (ev.IsMouseDeviceEvent(out SDL3_MouseDeviceEvent device))
      {
         if (device.Type is SDL3_EventType.MouseAdded)
            AddDevice(device.MouseId);
         else if (device.Type is SDL3_EventType.MouseRemoved)
            RemoveDevice(device.MouseId);
         else if (Context.Logging.IsAvailable)
            Context.Logging.Warning<SDL3MouseInputContext>($"Unknown mouse device event ({device.Type}), this should've been known but wasn't handled properly here.");

         return;
      }

      if (ev.IsMouseMotionEvent(out SDL3_MouseMotionEvent motion))
         RouteEvent(motion, motion.MouseId, motion.Type);
      else if (ev.IsMouseButtonEvent(out SDL3_MouseButtonEvent button))
         RouteEvent(button, button.MouseId, button.Type);
      else if (ev.IsMouseWheelEvent(out SDL3_MouseWheelEvent wheel))
         RouteEvent(wheel, wheel.MouseId, wheel.Type);
   }
   #endregion
}
