namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the application's context for dispatching operations.
/// </summary>
public sealed class DefaultDispatcherContext : BaseDispatcherContext
{
   #region Nested types
   private sealed class OperationQueue
   {
      #region Fields
      private readonly SemaphoreSlim _lock = new(1, 1);
      private readonly Queue<IOperation> _queue = [];
      #endregion

      #region Methods
      public void Enqueue(IOperation operation)
      {
         using (_lock.Lock())
            _queue.Enqueue(operation);
      }
      public bool TryDequeue([NotNullWhen(true)] out IOperation? operation)
      {
         using (_lock.Lock())
            return _queue.TryDequeue(out operation);
      }
      #endregion
   }
   #endregion

   #region Fields
   private readonly SortedDictionary<DispatchPriority, OperationQueue> _queues = [];
   private readonly SortedDictionary<DispatchPriority, OperationQueue> _backgroundQueues = [];
   private readonly List<Thread> _backgroundThreads = [];
   private volatile bool _shouldBackgroundThreadsRun;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private int _backgroundThreadCount = 4;

   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private ThreadPriority _backgroundThreadPriority = ThreadPriority.BelowNormal;
   #endregion

   #region Properties
   /// <summary>The amount of threads used to process background tasks.</summary>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the set thread count is less than 1.</exception>
   /// <exception cref="InvalidOperationException">Thrown if the thread count is changed while the context is initialised.</exception>
   public int BackgroundThreadCount
   {
      get => _backgroundThreadCount;
      set
      {
         if (value < 1)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"There must be at least one background thread.");

         if (IsInitialised)
            throw new InvalidOperationException($"The background thread count cannot be changed while the context is initialised.");

         _backgroundThreadCount = value;
      }
   }

   /// <summary>The priority that the background threads run on.</summary>
   /// <exception cref="InvalidOperationException">Thrown if the thread priority is changed while the context is initialised.</exception>
   public ThreadPriority BackgroundThreadPriority
   {
      get => _backgroundThreadPriority;
      set
      {
         if (IsInitialised)
            throw new InvalidOperationException($"The background thread priority cannot be changed while the context is initialised.");

         _backgroundThreadPriority = value;
      }
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   protected override void Initialise()
   {
      _shouldBackgroundThreadsRun = true;

#if NET5_0_OR_GREATER
      foreach (DispatchPriority priority in Enum.GetValues<DispatchPriority>())
      {
         OperationQueue queue = new();
         OperationQueue backgroundQueue = new();

         _queues.Add(priority, queue);
         _backgroundQueues.Add(priority, backgroundQueue);
      }
#else
      foreach (DispatchPriority priority in Enum.GetValues(typeof(DispatchPriority)))
      {
         OperationQueue queue = new();
         OperationQueue backgroundQueue = new();

         _queues.Add(priority, queue);
         _backgroundQueues.Add(priority, backgroundQueue);
      }
#endif

      for (int i = 0; i < BackgroundThreadCount; i++)
      {
         Thread thread = new(ProcessBackground)
         {
            Name = $"{nameof(DefaultDispatcherContext)} Background Thread #{i + 1}",
            IsBackground = true,
            Priority = BackgroundThreadPriority
         };

         _backgroundThreads.Add(thread);
         thread.Start();
      }
   }

   /// <inheritdoc/>
   protected override void Cleanup()
   {
      _queues.Clear();

      _shouldBackgroundThreadsRun = false;
      while (AreBackgroundThreadsRunning())
      {
         if (Thread.Yield() is false)
            Thread.Sleep(10);
      }

      _backgroundQueues.Clear();
      _backgroundThreads.Clear();
   }

   /// <inheritdoc/>
   protected override void ProcessCore()
   {
      while (TryGetNextOperation(out IOperation? operation))
         operation.Process();
   }

   private void ProcessBackground()
   {
      while (_shouldBackgroundThreadsRun)
      {
         while (_shouldBackgroundThreadsRun && TryGetNextBackgroundOperation(out IOperation? operation))
            operation.Process();

         if (Thread.Yield() is false)
            Thread.Sleep(10);
      }
   }

   /// <inheritdoc/>
   protected override void Schedule(IOperation operation) => _queues[operation.Priority].Enqueue(operation);

   /// <inheritdoc/>
   protected override void ScheduleBackground(IOperation operation) => _backgroundQueues[operation.Priority].Enqueue(operation);
   #endregion

   #region Helpers
   private bool TryGetNextOperation([NotNullWhen(true)] out IOperation? operation)
   {
      foreach (KeyValuePair<DispatchPriority, OperationQueue> pair in _queues)
      {
         if (pair.Value.TryDequeue(out operation))
            return true;
      }

      operation = default;
      return false;
   }
   private bool TryGetNextBackgroundOperation([NotNullWhen(true)] out IOperation? operation)
   {
      foreach (KeyValuePair<DispatchPriority, OperationQueue> pair in _backgroundQueues)
      {
         if (pair.Value.TryDequeue(out operation))
            return true;
      }

      operation = default;
      return false;
   }
   private bool AreBackgroundThreadsRunning()
   {
      foreach (Thread thread in _backgroundThreads)
      {
         if (thread.IsAlive)
            return true;
      }

      return false;
   }
   #endregion
}
