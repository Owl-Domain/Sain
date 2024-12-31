namespace Sain.Shared.Input.Keyboard;

/// <summary>
///   Represents the available keys that can modify the values received from the other keys on the keyboard.
/// </summary>
[Flags]
public enum KeyModifiers : ushort
{
   /// <summary>No key modifiers are used.</summary>
   None = 0,

   /// <summary>The left control (CTRL) key is pressed.</summary>
   LeftControl = 1 << 0,

   /// <summary>The right control (CTRL) key is pressed.</summary>
   RightControl = 1 << 1,

   /// <summary>The left shift key is pressed.</summary>
   LeftShift = 1 << 2,

   /// <summary>The right shift key is pressed.</summary>
   RightShift = 1 << 3,

   /// <summary>The left alt key is pressed.</summary>
   LeftAlt = 1 << 4,

   /// <summary>The right alt key is pressed.</summary>
   RightAlt = 1 << 5,

   /// <summary>The left meta key is pressed.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   LeftMeta = 1 << 6,

   /// <summary>The right meta key is pressed.</summary>
   /// <remarks>This is the OS specific key, e.g: the windows key (on Windows), the command key (on Apple devices), etc.</remarks>
   RightMeta = 1 << 7,

   /// <summary>The Num Lock key is pressed.</summary>
   NumLock = 1 << 8,

   /// <summary>The Caps Lock key is pressed.</summary>
   CapsLock = 1 << 9,

   /// <summary>The mode key is pressed.</summary>
   /// <remarks>I have discovered that mode is the Alt Gr key.</remarks>
   AltGr = 1 << 10,

   /// <summary>The scroll lock modifier key is pressed.</summary>
   /// <remarks>Scroll lock is a modifier key?</remarks>
   ScrollLock = 1 << 11,
}

/// <summary>
///   Contains various extension methods related to the <see cref="KeyModifiers"/>.
/// </summary>
public static class KeyModifiersExtensions
{
   #region Methods
   /// <summary>Checks whether the given key <paramref name="modifiers"/> have either one of the left or right control keys pressed.</summary>
   /// <param name="modifiers">The key modifiers to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="modifiers"/> have either one of the left or right control keys pressed, <see langword="false"/> otherwise.</returns>
   public static bool HasControl(this KeyModifiers modifiers) => modifiers.HasFlag(KeyModifiers.LeftControl) || modifiers.HasFlag(KeyModifiers.RightControl);

   /// <summary>Checks whether the given key <paramref name="modifiers"/> have either one of the left or right shift keys pressed.</summary>
   /// <param name="modifiers">The key modifiers to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="modifiers"/> have either one of the left or right shift keys pressed, <see langword="false"/> otherwise.</returns>
   public static bool HasShift(this KeyModifiers modifiers) => modifiers.HasFlag(KeyModifiers.LeftShift) || modifiers.HasFlag(KeyModifiers.RightShift);

   /// <summary>Checks whether the given key <paramref name="modifiers"/> have either one of the left or right alt keys pressed.</summary>
   /// <param name="modifiers">The key modifiers to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="modifiers"/> have either one of the left or right alt keys pressed, <see langword="false"/> otherwise.</returns>
   public static bool HasAlt(this KeyModifiers modifiers) => modifiers.HasFlag(KeyModifiers.LeftAlt) || modifiers.HasFlag(KeyModifiers.RightAlt);

   /// <summary>Checks whether the given key <paramref name="modifiers"/> have either one of the left or right meta keys pressed.</summary>
   /// <param name="modifiers">The key modifiers to check.</param>
   /// <returns><see langword="true"/> if the given key <paramref name="modifiers"/> have either one of the left or right meta keys pressed, <see langword="false"/> otherwise.</returns>
   public static bool HasMeta(this KeyModifiers modifiers) => modifiers.HasFlag(KeyModifiers.LeftMeta) || modifiers.HasFlag(KeyModifiers.RightMeta);
   #endregion
}
