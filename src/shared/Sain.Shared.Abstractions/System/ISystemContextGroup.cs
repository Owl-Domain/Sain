namespace Sain.Shared.System;

/// <summary>
///   Represents a context group for the application's system contexts.
/// </summary>
public interface ISystemContextGroup
{
   #region Properties
   /// <summary>The application's context for accessing system time information.</summary>
   ISystemTimeContext Time { get; }
   #endregion
}
