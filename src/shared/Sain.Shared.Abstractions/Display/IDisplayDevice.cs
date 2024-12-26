namespace Sain.Shared.Display;

/// <summary>
///   Represents information about a display device.
/// </summary>
public interface IDisplayDevice : IDevice
{
   #region Properties
   /// <summary>The resolution of the display.</summary>
   Size Resolution { get; }

   /// <summary>The area that the display takes up in the virtual screen space.</summary>
   Rectangle Area { get; }

   /// <summary>The bounds of the device which are safe for displaying information.</summary>
   /// <remarks>This area is relative to the position of the display in the virtual screen space.</remarks>
   Rectangle Bounds { get; }

   /// <summary>The rate at which the display is refreshed.</summary>
   /// <remarks>If unknown, then the value should be equal to <c>0</c>.</remarks>
   double RefreshRate { get; }

   /// <summary>The scaling of the display.</summary>
   /// <remarks>If unknown, then the value should be equal to <c>1</c>.</remarks>
   double DisplayScale { get; }

   /// <summary>Whether the current display device is marked as the primary display.</summary>
   bool IsPrimary { get; }
   #endregion

   #region Methods
   /// <summary>Refreshes the resolution of the display.</summary>
   void RefreshResolution();

   /// <summary>Refreshes the area of the display.</summary>
   void RefreshArea();

   /// <summary>Refreshes the bounds of the display.</summary>
   void RefreshBounds();

   /// <summary>Refreshes the refresh rate of the display.</summary>
   void RefreshRefreshRate();

   /// <summary>Refreshes the display scale of the display.</summary>
   void RefreshDisplayScale();

   /// <summary>Refreshes the <see cref="IsPrimary"/> information about the display.</summary>
   void RefreshIsPrimary();
   #endregion
}
