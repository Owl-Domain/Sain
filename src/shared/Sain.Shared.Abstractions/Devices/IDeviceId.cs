namespace Sain.Shared.Devices;

/// <summary>
///   Represents a unique device id.
/// </summary>
public interface IDeviceId
{
   #region Properties
   /// <summary>The components that make up the device id.</summary>
   /// <remarks>
   ///   Each component can be of any length (even empty), and it can contain
   ///   any character, but there will always be at least 1 component.
   /// </remarks>
   IReadOnlyList<string> Components { get; }
   #endregion
}
