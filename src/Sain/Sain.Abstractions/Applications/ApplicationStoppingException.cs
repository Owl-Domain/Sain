namespace OwlDomain.Sain.Applications;

/// <summary>
///   Represents an exception that can be thrown by an application to easily exit out of a long call stack.
/// </summary>
/// <remarks>This eception should not be used for anything else in order to avoid the application suddenly exiting by accident.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ApplicationStoppingException : Exception
{
   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ApplicationStoppingException"/>.</summary>
   public ApplicationStoppingException() { }
   #endregion
}
