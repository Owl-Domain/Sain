namespace Sain.Shared.Input.Mouse;

// Todo(Nightowl): Lock enum values upon release;

/// <summary>
///   Represents the different known mouse buttons.
/// </summary>
public enum MouseButtonKind : byte
{
   /// <summary>The mouse button is unknown.</summary>
   Unknown,

   /// <summary>The mouse button is known, but it has not been defined.</summary>
   /// <remarks>This represents a category and not just a single key.</remarks>
   Other,

   /// <summary>The mouse button is reserved.</summary>
   /// <remarks>This represents a category and not just a single key.</remarks>
   Reserved,

   /// <summary>The left button on the mouse.</summary>
   Left,

   /// <summary>The middle button on the mouse.</summary>
   /// <remarks>This is usually the mouse wheel.</remarks>
   Middle,

   /// <summary>The right button on the mouse.</summary>
   Right,
}

/// <summary>
///   Contains various extension methods related to the <see cref="MouseButtonKind"/>.
/// </summary>
public static class MouseButtonKindExtensions
{
   #region Methods
   /// <summary>Checks whether the given mouse button <paramref name="kind"/> represents a category.</summary>
   /// <param name="kind">The kind of the mouse button to check.</param>
   /// <returns><see langword="true"/> if the given mouse button <paramref name="kind"/> represents a category, <see langword="false"/> otherwise.</returns>
   public static bool IsCategory(this MouseButtonKind kind)
   {
      return kind switch
      {
         MouseButtonKind.Other => true,
         MouseButtonKind.Reserved => true,

         _ => false,
      };
   }

   #endregion
}
