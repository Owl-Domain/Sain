namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents a dispatched operation.
/// </summary>
/// <param name="priority">The priority that the operation has been dispatched on.</param>
/// <param name="task">A task which can be used to wait until the operation has finished.</param>
public class DispatcherOperation(DispatchPriority priority, Task task) : IDispatcherOperation
{
   #region Properties
   /// <inheritdoc/>
   public DispatcherOperationState State { get; set; } = DispatcherOperationState.Queued;

   /// <inheritdoc/>
   public DispatchPriority Priority { get; } = priority;

   /// <inheritdoc/>
   public Task Task { get; } = task;
   #endregion
}

/// <summary>
///   Represents a dispatched operation.
/// </summary>
/// <param name="priority">The priority that the operation has been dispatched on.</param>
/// <param name="task">A task which can be used to wait until the operation has finished.</param>
public class DispatcherOperation<T>(DispatchPriority priority, Task<T> task) : IDispatcherOperation<T>
{
   #region Properties
   /// <inheritdoc/>
   public DispatcherOperationState State { get; set; } = DispatcherOperationState.Queued;

   /// <inheritdoc/>
   public DispatchPriority Priority { get; } = priority;

   /// <inheritdoc/>
   public Task<T> Task { get; } = task;
   #endregion
}
