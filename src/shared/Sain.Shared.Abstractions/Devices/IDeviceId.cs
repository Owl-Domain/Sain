namespace Sain.Shared.Devices;

/// <summary>
///   Represents a unique device id.
/// </summary>
public interface IDeviceId : IEquatable<IDeviceId?>
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

/// <summary>
///   Contains various extension methods related to the <see cref="IDeviceId"/>.
/// </summary>
public static class IDeviceIdExtensions
{
   #region Methods
   /// <summary>Checks whether the given <paramref name="deviceId"/> fully matches the <paramref name="other"/> given device id.</summary>
   /// <param name="deviceId">The first device id to check.</param>
   /// <param name="other">The device id to compare the <paramref name="deviceId"/> to.</param>
   /// <returns>
   ///   <see langword="true"/> if the <paramref name="deviceId"/> is a full match to
   ///   the <paramref name="other"/> given device id, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsFullMatch(this IDeviceId deviceId, IDeviceId other)
   {
      if (deviceId.Components.Count != other.Components.Count)
         return false;

      for (int i = 0; i < deviceId.Components.Count; i++)
      {
         if (deviceId.Components[i] != other.Components[i])
            return false;
      }

      return true;
   }

   /// <summary>Checks whether the given <paramref name="deviceId"/> partially matches the <paramref name="other"/> given device id.</summary>
   /// <param name="deviceId">The first device id to check.</param>
   /// <param name="other">The device id to compare the <paramref name="deviceId"/> to.</param>
   /// <param name="score">The score assigned to the match, higher is better.</param>
   /// <returns>
   ///   <see langword="true"/> if the <paramref name="deviceId"/> is a partial match to
   ///   the <paramref name="other"/> given device id, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>
   ///   This method only implements a basic algorithm, and should only be used to
   ///   help implement the <see cref="IDevice.IsMatch(IDeviceId, out int)"/> methods.
   /// </remarks>
   public static bool IsBasicPartialMatch(this IDeviceId deviceId, IDeviceId other, out int score)
   {
      if (deviceId.Components.Count != other.Components.Count)
      {
         score = 0;
         return false;
      }

      score = 0;
      for (int i = 0; i < deviceId.Components.Count; i++)
      {
         if (deviceId.Components[i] == other.Components[i])
            score++;
      }

      return score > 0;
   }
   #endregion
}
