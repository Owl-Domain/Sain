namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the arguments used for keyboard key events.
/// </summary>
/// <param name="physicalKey">The physical representation of the key.</param>
/// <param name="physicalKeyName">The name of the <paramref name="physicalKey"/>.</param>
/// <param name="virtualKey">The virtual representation of the key.</param>
/// <param name="virtualKeyName">The name of the <paramref name="virtualKey"/>.</param>
/// <param name="modifiers">Any of the used key modifiers.</param>
/// <param name="isDown">Whether the key was pressed down.</param>
/// <param name="isRepeat">Whether the key event is a repeat event.</param>
public readonly struct KeyboardKeyEventArgs(
   PhysicalKey physicalKey,
   string physicalKeyName,
   VirtualKey virtualKey,
   string virtualKeyName,
   KeyModifiers modifiers,
   bool isDown,
   bool isRepeat)
{
   #region Properties
   /// <summary>The physical representation of the key.</summary>
   public readonly PhysicalKey PhysicalKey { get; } = physicalKey;

   /// <summary>The name of the physical key.</summary>
   public readonly string PhysicalKeyName { get; } = physicalKeyName;

   /// <summary>The virtual representation of the key.</summary>
   public readonly VirtualKey VirtualKey { get; } = virtualKey;

   /// <summary>Any of the used key modifiers.</summary>
   public readonly KeyModifiers Modifiers { get; } = modifiers;

   /// <summary>The name of the virtual key.</summary>
   public readonly string VirtualKeyName { get; } = virtualKeyName;

   /// <summary>Whether the key was pressed down.</summary>
   public readonly bool IsDown { get; } = isDown;

   /// <summary>Whether the key event is a repeat event.</summary>
   /// <remarks>A repeat event is typically caused by holding down a key.</remarks>
   public readonly bool IsRepeat { get; } = isRepeat;
   #endregion
}

/// <summary>
///   Represents the event handler delegate for keyboard key events.
/// </summary>
/// <param name="context">The keyboard input context that raised the event.</param>
/// <param name="args">The arguments that contain information about the keyboard key event.</param>
public delegate void KeyboardKeyEventHandler(IKeyboardInputContext context, KeyboardKeyEventArgs args);
