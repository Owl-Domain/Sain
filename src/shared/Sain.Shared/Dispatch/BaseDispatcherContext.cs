namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the base implementation for the application's dispatcher context.
/// </summary>
public abstract class BaseDispatcherContext : BaseContext, IDispatcherContext
{
   #region Nested types
   /// <summary>
   ///   Represents a dispatcher operation.
   /// </summary>
   protected interface IOperation
   {
      #region Properties
      /// <summary>The priority that the operation should be scheduled on.</summary>
      public DispatchPriority Priority { get; }
      #endregion

      #region Methods
      /// <summary>Processes the operation.</summary>
      void Process();
      #endregion
   }

   #region Base operation types
   private abstract class BaseSyncOperation(DispatchPriority priority) : IDispatcherOperation, IOperation
   {
      #region Properties
      public TaskCompletionSource<object?> TaskSource { get; } = new();
      public DispatcherOperationState State { get; set; }
      public DispatchPriority Priority { get; } = priority;
      public Task Task => TaskSource.Task;
      #endregion

      #region Methods
      public void Process()
      {
         State = DispatcherOperationState.Started;
         try
         {
            ProcessCore();

            State = DispatcherOperationState.Finished;
            TaskSource.SetResult(null);
         }
         catch (Exception exception)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(exception);
         }
      }
      protected abstract void ProcessCore();
      #endregion
   }
   private abstract class BaseSyncOperation<TResult>(DispatchPriority priority) : IDispatcherOperation<TResult>, IOperation
   {
      #region Properties
      public TaskCompletionSource<TResult> TaskSource { get; } = new();
      public DispatcherOperationState State { get; set; }
      public DispatchPriority Priority { get; } = priority;
      public Task<TResult> Task => TaskSource.Task;
      #endregion

      #region Methods
      public void Process()
      {
         State = DispatcherOperationState.Started;
         try
         {
            TResult result = ProcessCore();

            State = DispatcherOperationState.Finished;
            TaskSource.SetResult(result);
         }
         catch (Exception exception)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(exception);
         }
      }
      protected abstract TResult ProcessCore();
      #endregion
   }
   private abstract class BaseAsyncOperation(DispatchPriority priority) : IDispatcherOperation, IOperation
   {
      #region Properties
      public TaskCompletionSource<object?> TaskSource { get; } = new();
      public DispatcherOperationState State { get; set; }
      public DispatchPriority Priority { get; } = priority;
      public Task Task => TaskSource.Task;
      #endregion

      #region Methods
      public void Process()
      {
         State = DispatcherOperationState.Started;
         try
         {
            Task task = ProcessCore();
            task.ContinueWith(Continuation);
         }
         catch (Exception exception)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(exception);
         }
      }
      protected abstract Task ProcessCore();
      private void Continuation(Task task)
      {
         if (task.Exception is not null)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(task.Exception);
         }
         else
         {
            State = DispatcherOperationState.Finished;
            TaskSource.SetResult(null);
         }
      }
      #endregion
   }
   private abstract class BaseAsyncOperation<TResult>(DispatchPriority priority) : IDispatcherOperation<TResult>, IOperation
   {
      #region Properties
      public TaskCompletionSource<TResult> TaskSource { get; } = new();
      public DispatcherOperationState State { get; set; }
      public DispatchPriority Priority { get; } = priority;
      public Task<TResult> Task => TaskSource.Task;
      #endregion

      #region Methods
      public void Process()
      {
         State = DispatcherOperationState.Started;
         try
         {
            Task<TResult> task = ProcessCore();
            task.ContinueWith(Continuation);
         }
         catch (Exception exception)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(exception);
         }
      }
      protected abstract Task<TResult> ProcessCore();
      private void Continuation(Task<TResult> task)
      {
         if (task.Exception is not null)
         {
            State = DispatcherOperationState.Failed;
            TaskSource.SetException(task.Exception);
         }
         else
         {
            State = DispatcherOperationState.Finished;
            TaskSource.SetResult(task.Result);
         }
      }
      #endregion
   }
   #endregion
   #region Operation types
   private sealed class SyncOperation(Action callback, DispatchPriority priority) : BaseSyncOperation(priority)
   {
      protected override void ProcessCore() => callback.Invoke();
   }
   private sealed class SyncArgumentOperation<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority) : BaseSyncOperation(priority)
   {
      protected override void ProcessCore() => callback.Invoke(argument);
   }
   private sealed class SyncOperation<TResult>(Func<TResult> callback, DispatchPriority priority) : BaseSyncOperation<TResult>(priority)
   {
      protected override TResult ProcessCore() => callback.Invoke();
   }
   private sealed class SyncArgumentOperation<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority) : BaseSyncOperation<TResult>(priority)
   {
      protected override TResult ProcessCore() => callback.Invoke(argument);
   }
   private sealed class TaskOperation(Func<Task> callback, DispatchPriority priority) : BaseAsyncOperation(priority)
   {
      protected override Task ProcessCore() => callback.Invoke();
   }
   private sealed class TaskArgumentOperation<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority) : BaseAsyncOperation(priority)
   {
      protected override Task ProcessCore() => callback.Invoke(argument);
   }
   private sealed class TaskOperation<TResult>(Func<Task<TResult>> callback, DispatchPriority priority) : BaseAsyncOperation<TResult>(priority)
   {
      protected override Task<TResult> ProcessCore() => callback.Invoke();
   }
   private sealed class TaskArgumentOperation<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority) : BaseAsyncOperation<TResult>(priority)
   {
      protected override Task<TResult> ProcessCore() => callback.Invoke(argument);
   }
   private sealed class ValueTaskOperation(Func<ValueTask> callback, DispatchPriority priority) : BaseAsyncOperation(priority)
   {
      protected override Task ProcessCore() => callback.Invoke().AsTask();
   }
   private sealed class ValueTaskArgumentOperation<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority) : BaseAsyncOperation(priority)
   {
      protected override Task ProcessCore() => callback.Invoke(argument).AsTask();
   }
   private sealed class ValueTaskOperation<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority) : BaseAsyncOperation<TResult>(priority)
   {
      protected override Task<TResult> ProcessCore() => callback.Invoke().AsTask();
   }
   private sealed class ValueTaskArgumentOperation<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority) : BaseAsyncOperation<TResult>(priority)
   {
      protected override Task<TResult> ProcessCore() => callback.Invoke(argument).AsTask();
   }
   #endregion
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Process()
   {
      ThrowIfUnavailable();

      ProcessCore();
   }

   /// <summary>Processes any remaning operations.</summary>
   protected abstract void ProcessCore();

   /// <summary>Schedules the given <paramref name="operation"/> to be executed on the main thread.</summary>
   /// <param name="operation">The operation to schedule on the main thread.</param>
   /// <remarks>This implementation should be thread-safe.</remarks>
   protected abstract void Schedule(IOperation operation);

   /// <summary>Schedules the given <paramref name="operation"/> to be execute in the background, possibly on a different thread.</summary>
   /// <param name="operation">The operation to schedule on the main thread.</param>
   /// <remarks>This implementation should be thread-safe.</remarks>
   protected abstract void ScheduleBackground(IOperation operation);
   #endregion

   #region Dispatch methods
   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Action callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncOperation operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult>(Func<TResult> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncOperation<TResult> operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Func<Task> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskOperation operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult>(Func<Task<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskOperation<TResult> operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Func<ValueTask> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskOperation operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskOperation<TResult> operation = new(callback, priority);
      Schedule(operation);

      return operation;
   }
   #endregion

   #region Dispatch (with argument) methods
   /// <inheritdoc/>
   public IDispatcherOperation Dispatch<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncArgumentOperation<TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskArgumentOperation<TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskArgumentOperation<TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      Schedule(operation);

      return operation;
   }
   #endregion

   #region Offload methods
   /// <inheritdoc/>
   public IDispatcherOperation Offload(Action callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncOperation operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult>(Func<TResult> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncOperation<TResult> operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Offload(Func<Task> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskOperation operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult>(Func<Task<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskOperation<TResult> operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Offload(Func<ValueTask> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskOperation operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskOperation<TResult> operation = new(callback, priority);
      ScheduleBackground(operation);

      return operation;
   }
   #endregion

   #region Offload (with argument) methods
   /// <inheritdoc/>
   public IDispatcherOperation Offload<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncArgumentOperation<TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      SyncArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Offload<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskArgumentOperation<TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      TaskArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Offload<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskArgumentOperation<TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal)
   {
      ThrowIfUnavailable();
      VerifyPriority(priority, nameof(priority));

      ValueTaskArgumentOperation<TResult, TArgument> operation = new(callback, argument, priority);
      ScheduleBackground(operation);

      return operation;
   }
   #endregion

   #region Helpers
   private static void VerifyPriority(DispatchPriority priority, string parameterName)
   {
#if NET5_0_OR_GREATER
      if (Enum.IsDefined(priority))
         return;
#else
      if (Enum.IsDefined(typeof(DispatchPriority), priority))
         return;
#endif

      throw new ArgumentOutOfRangeException(parameterName, priority, $"The given priority was not one of the available priorities defined in the {typeof(DispatchPriority)} enum.");
   }
   #endregion
}
