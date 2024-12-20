namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents a dispatched operation.
/// </summary>
public interface IDispatcherOperation
{
   #region Properties
   /// <summary>The state of the operation.</summary>
   DispatcherOperationState State { get; }

   /// <summary>The priority that the operation has been dispatched on.</summary>
   DispatchPriority Priority { get; }

   /// <summary>A task which can be used to wait until the operation has finished.</summary>
   Task Task { get; }
   #endregion
}

/// <summary>
///   Represents a dispatched operation.
/// </summary>
/// <typeparam name="T">The type of the result of the operation.</typeparam>
public interface IDispatcherOperation<T> : IDispatcherOperation
{
   #region Properties
   /// <summary>A task which can be used to wait until the operation has finished.</summary>
   new Task<T> Task { get; }
   Task IDispatcherOperation.Task => Task;

   /// <summary>The result of the operation.</summary>
   T Result => Task.Result;
   #endregion
}
