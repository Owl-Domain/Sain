namespace Sain.Shared.Input;

/// <summary>
///   Represents information about an input device.
/// </summary>
public interface IInputDevice : IDevice
{
   #region Properties
   /// <summary>The time since the input device was last used by the user.</summary>
   /// <remarks>Should return <see cref="TimeSpan.MaxValue"/> if the device hasn't been used yet.</remarks>
   TimeSpan LastUsed { get; }
   #endregion
}
