namespace Sain.Shared.Input;

/// <summary>
///   Represents a context group for the application's input contexts.
/// </summary>
/// <param name="keyboard">The application's context for keyboard input.</param>
/// <param name="mouse">The application's context for mouse input.</param>
public sealed class InputContextGroup(IKeyboardInputContext keyboard, IMouseInputContext mouse) : IInputContextGroup
{
   #region Properties
   /// <inheritdoc/>
   public IKeyboardInputContext Keyboard { get; } = keyboard;

   /// <inheritdoc/>
   public IMouseInputContext Mouse { get; } = mouse;
   #endregion

   #region Functions
   /// <summary>Creates a new <see cref="InputContextGroup"/> using the given application <paramref name="context"/>.</summary>
   /// <param name="context">The application context to use when creating the input context group.</param>
   /// <returns>The created input context group.</returns>
   public static IInputContextGroup Create(IApplicationContext context)
   {
      IKeyboardInputContext keyboard = context.GetContext<IKeyboardInputContext>();
      IMouseInputContext mouse = context.GetContext<IMouseInputContext>();

      return new InputContextGroup(keyboard, mouse);
   }
   #endregion
}
