namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the application's context for dispatching operations.
/// </summary>
public sealed class DispatcherContext : BaseContext, IDispatcherContext
{
   #region Nested types
   private sealed class RequestQueue
   {
      #region Fields
      private readonly SemaphoreSlim _lock = new(1, 1);
      private readonly Queue<IRequest> _queue = [];
      #endregion

      #region Methods
      public void Enqueue(IRequest request)
      {
         using (_lock.Lock())
            _queue.Enqueue(request);
      }
      public bool TryDequeue([NotNullWhen(true)] out IRequest? request)
      {
         using (_lock.Lock())
            return _queue.TryDequeue(out request);
      }
      #endregion
   }
   private interface IRequest
   {
      #region Methods
      public abstract void Process();
      #endregion
   }
   private sealed class Request(Action callback, TaskCompletionSource<object?> completionSource, DispatcherOperation operation) : IRequest
   {
      #region Properties
      public Action Callback { get; } = callback;
      public TaskCompletionSource<object?> CompletionSource { get; } = completionSource;
      public DispatcherOperation Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         Callback.Invoke();

         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(null);
      }
      #endregion
   }
   private sealed class Request<T>(Func<T> callback, TaskCompletionSource<T> completionSource, DispatcherOperation<T> operation) : IRequest
   {
      #region Properties
      public Func<T> Callback { get; } = callback;
      public TaskCompletionSource<T> CompletionSource { get; } = completionSource;
      public DispatcherOperation<T> Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         T result = Callback.Invoke();

         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(result);
      }
      #endregion
   }
   private sealed class TaskRequest(Func<Task> callback, TaskCompletionSource<object?> completionSource, DispatcherOperation operation) : IRequest
   {
      #region Properties
      public Func<Task> Callback { get; } = callback;
      public TaskCompletionSource<object?> CompletionSource { get; } = completionSource;
      public DispatcherOperation Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         Task task = Callback.Invoke();
         task.ContinueWith(Continuation);
      }
      private void Continuation(Task task)
      {
         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(null);
      }
      #endregion
   }
   private sealed class TaskRequest<T>(Func<Task<T>> callback, TaskCompletionSource<T> completionSource, DispatcherOperation<T> operation) : IRequest
   {
      #region Properties
      public Func<Task<T>> Callback { get; } = callback;
      public TaskCompletionSource<T> CompletionSource { get; } = completionSource;
      public DispatcherOperation<T> Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         Task<T> task = Callback.Invoke();
         task.ContinueWith(Continuation);
      }
      private void Continuation(Task<T> task)
      {
         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(task.Result);
      }
      #endregion
   }
   private sealed class ValueTaskRequest(Func<ValueTask> callback, TaskCompletionSource<object?> completionSource, DispatcherOperation operation) : IRequest
   {
      #region Properties
      public Func<ValueTask> Callback { get; } = callback;
      public TaskCompletionSource<object?> CompletionSource { get; } = completionSource;
      public DispatcherOperation Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         ValueTask task = Callback.Invoke();
         task.AsTask().ContinueWith(Continuation);
      }
      private void Continuation(Task task)
      {
         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(null);
      }
      #endregion
   }
   private sealed class ValueTaskRequest<T>(Func<ValueTask<T>> callback, TaskCompletionSource<T> completionSource, DispatcherOperation<T> operation) : IRequest
   {
      #region Properties
      public Func<ValueTask<T>> Callback { get; } = callback;
      public TaskCompletionSource<T> CompletionSource { get; } = completionSource;
      public DispatcherOperation<T> Operation { get; } = operation;
      #endregion

      #region Methods
      public void Process()
      {
         Operation.State = DispatcherOperationState.Started;
         ValueTask<T> task = Callback.Invoke();
         task.AsTask().ContinueWith(Continuation);
      }
      private void Continuation(Task<T> task)
      {
         Operation.State = DispatcherOperationState.Finished;
         CompletionSource.SetResult(task.Result);
      }
      #endregion
   }
   #endregion

   #region Fields
   private readonly SortedDictionary<DispatchPriority, RequestQueue> _queues = [];
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override Task InitialiseAsync()
   {
#if NET5_0_OR_GREATER
      foreach (DispatchPriority priority in Enum.GetValues<DispatchPriority>())
      {
         RequestQueue queue = new();
         _queues.Add(priority, queue);
      }
#else
      foreach (DispatchPriority priority in Enum.GetValues(typeof(DispatchPriority)))
      {
         RequestQueue queue = new();
         _queues.Add(priority, queue);
      }
#endif

      return Task.CompletedTask;
   }

   /// <inheritdoc/>
   protected override Task CleanupAsync()
   {
      _queues.Clear();

      return Task.CompletedTask;
   }

   /// <inheritdoc/>
   public void Process()
   {
      foreach (KeyValuePair<DispatchPriority, RequestQueue> pair in _queues)
      {
         while (pair.Value.TryDequeue(out IRequest? request))
            request.Process();
      }
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Action operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<object?> tcs = new();
      DispatcherOperation op = new(priority, tcs.Task);

      Request request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<T> Dispatch<T>(Func<T> operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<T> tcs = new();
      DispatcherOperation<T> op = new(priority, tcs.Task);

      Request<T> request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Func<Task> operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<object?> tcs = new();
      DispatcherOperation op = new(priority, tcs.Task);

      TaskRequest request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<T> Dispatch<T>(Func<Task<T>> operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<T> tcs = new();
      DispatcherOperation<T> op = new(priority, tcs.Task);

      TaskRequest<T> request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }

   /// <inheritdoc/>
   public IDispatcherOperation Dispatch(Func<ValueTask> operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<object?> tcs = new();
      DispatcherOperation op = new(priority, tcs.Task);

      ValueTaskRequest request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }

   /// <inheritdoc/>
   public IDispatcherOperation<T> Dispatch<T>(Func<ValueTask<T>> operation, DispatchPriority priority = DispatchPriority.Normal)
   {
      TaskCompletionSource<T> tcs = new();
      DispatcherOperation<T> op = new(priority, tcs.Task);

      ValueTaskRequest<T> request = new(operation, tcs, op);
      _queues[priority].Enqueue(request);

      return op;
   }
   #endregion
}
