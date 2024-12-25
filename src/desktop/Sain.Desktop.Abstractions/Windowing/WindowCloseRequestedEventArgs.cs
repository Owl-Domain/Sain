namespace Sain.Desktop.Windowing;

/// <summary>
///   Represents the event arguments for a window close request.
/// </summary>
public sealed class WindowCloseRequestedEventArgs : EventArgs
{
   #region Properties
   /// <summary>Whether the request should be obeyed or ignored.</summary>
   public bool ShouldCloseWindow { get; set; } = true;
   #endregion
}
