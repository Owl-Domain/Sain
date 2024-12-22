namespace Sain.Shared.Input.Mouse;

/// <summary>
///   Represents information about a single mouse button.
/// </summary>
public interface IMouseButton
{
   #region Properties
   /// <summary>The kind of the mouse button.</summary>
   MouseButtonKind Kind { get; }

   /// <summary>The name of the mouse button.</summary>
   /// <remarks>This can be used if the kind of the mouse button is <see cref="MouseButtonKind.Other"/>.</remarks>
   string Name { get; }

   /// <summary>Whether the mouse button is currently pressed down.</summary>
   bool IsDown { get; }
   #endregion
}
