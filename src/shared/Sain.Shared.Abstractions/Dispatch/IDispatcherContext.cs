namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the application's context for dispatching operations.
/// </summary>
public interface IDispatcherContext : IContext
{
   #region Methods
   /// <summary>Processes any remaning operations.</summary>
   /// <remarks>This should only be called from the main application loop.</remarks>
   void Process();

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   IDispatcherOperation Dispatch(Action operation, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <typeparam name="T">The type of the result returned by the <paramref name="operation"/>.</typeparam>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   IDispatcherOperation<T> Dispatch<T>(Func<T> operation, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   /// <remarks>The dispatcher can only guarantee that the <paramref name="operation"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch(Func<Task> operation, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <typeparam name="T">The type of the result returned by the <paramref name="operation"/>.</typeparam>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   /// <remarks>The dispatcher can only guarantee that the <paramref name="operation"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<T> Dispatch<T>(Func<Task<T>> operation, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   /// <remarks>The dispatcher can only guarantee that the <paramref name="operation"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch(Func<ValueTask> operation, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="operation"/> to the dispatcher.</summary>
   /// <typeparam name="T">The type of the result returned by the <paramref name="operation"/>.</typeparam>
   /// <param name="operation">The operation to offload to the dispatcher.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="operation"/>.</returns>
   /// <remarks>The dispatcher can only guarantee that the <paramref name="operation"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<T> Dispatch<T>(Func<ValueTask<T>> operation, DispatchPriority priority = DispatchPriority.Normal);
   #endregion
}
