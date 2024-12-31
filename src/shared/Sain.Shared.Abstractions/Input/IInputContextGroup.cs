namespace Sain.Shared.Input;

/// <summary>
///   Represents a context group for the application's input contexts.
/// </summary>
public interface IInputContextGroup
{
   #region Properties
   /// <summary>The application's context for keyboard input.</summary>
   IKeyboardInputContext Keyboard { get; }

   /// <summary>The application's context for mouse input.</summary>
   IMouseInputContext Mouse { get; }
   #endregion
}
