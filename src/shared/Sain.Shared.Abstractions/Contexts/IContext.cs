namespace Sain.Shared.Contexts;

/// <summary>
///   Represents an application's context.
/// </summary>
public interface IContext : IApplicationComponent
{
   #region Properties
   /// <summary>The provider that the context comes from.</summary>
   IContextProvider? Provider { get; }

   /// <summary>The kind of the context.</summary>
   string Kind { get; }

   /// <summary>Whether the context is available for the application to use.</summary>
   bool IsAvailable { get; }
   #endregion
}
