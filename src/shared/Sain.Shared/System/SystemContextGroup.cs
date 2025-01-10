namespace Sain.Shared.System;

/// <summary>
///   Represents a context group for the application's system contexts.
/// </summary>
/// <param name="time">The application's context for system time information.</param>
public sealed class SystemContextGroup(ISystemTimeContext time) : ISystemContextGroup
{
   #region Properties
   /// <inheritdoc/>
   public ISystemTimeContext Time { get; } = time;
   #endregion

   #region Functions
   /// <summary>Creates a new <see cref="SystemContextGroup"/> using the given application <paramref name="context"/>.</summary>
   /// <param name="context">The application context to use when creating the system context group.</param>
   /// <returns>The created system context group.</returns>
   public static ISystemContextGroup Create(IApplicationContext context)
   {
      ISystemTimeContext time = context.GetContext<ISystemTimeContext>();

      return new SystemContextGroup(time);
   }
   #endregion
}
