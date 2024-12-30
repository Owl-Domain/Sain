namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the application's context for dispatching operations.
/// </summary>
public interface IDispatcherContext : IContext
{
   #region Properties
   /// <summary>Checks whether the current thread is different from the main thread.</summary>
   /// <remarks>The main thread is which thread the dispatcher was initialised on.</remarks>
   bool NeedsDispatching { get; }

   /// <summary>Checks whether the current thread is the main thread.</summary>
   /// <remarks>The main thread is which thread the dispatcher was initialised on.</remarks>
   bool IsOnMainThread { get; }
   #endregion

   #region Methods
   /// <summary>Processes any remaning operations.</summary>
   /// <remarks>This should only be called from the main application loop, which is expected to be the main thread.</remarks>
   /// <exception cref="InvalidOperationException">Thrown if the method is not called from the main thread.</exception>
   void Process();
   #endregion

   #region Dispatch methods
   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation Dispatch(Action callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> Dispatch<TResult>(Func<TResult> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch(Func<Task> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Dispatch<TResult>(Func<Task<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch(Func<ValueTask> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Dispatch<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);
   #endregion

   #region Dispatch (with argument) methods
   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation Dispatch<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Dispatch<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Dispatches the given <paramref name="callback"/> to the dispatcher to be executed on the main thread.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Dispatch<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);
   #endregion

   #region TryDispatch methods
   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation TryDispatch(Action callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> TryDispatch<TResult>(Func<TResult> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation TryDispatch(Func<Task> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> TryDispatch<TResult>(Func<Task<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation TryDispatch(Func<ValueTask> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> TryDispatch<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);
   #endregion

   #region TryDispatch (with argument) methods
   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation TryDispatch<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> TryDispatch<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation TryDispatch<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> TryDispatch<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation TryDispatch<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>
   ///   Tries to dispatch the given <paramref name="callback"/> to be executed on the main thread, if called on the main thread,
   ///   then the given <paramref name="callback"/> is executed instantly, ignoring the given <paramref name="priority"/>.
   /// </summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to dispatch.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> TryDispatch<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);
   #endregion

   #region Offload methods
   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation Offload(Action callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> Offload<TResult>(Func<TResult> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Offload(Func<Task> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Offload<TResult>(Func<Task<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Offload(Func<ValueTask> callback, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Offload<TResult>(Func<ValueTask<TResult>> callback, DispatchPriority priority = DispatchPriority.Normal);
   #endregion

   #region Offload (with argument) methods
   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation Offload<TArgument>(Action<TArgument> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, TResult> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Offload<TArgument>(Func<TArgument, Task> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, Task<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation Offload<TArgument>(Func<TArgument, ValueTask> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);

   /// <summary>Offloads the given <paramref name="callback"/> to the dispatcher to be executed in the background.</summary>
   /// <typeparam name="TResult">The type of the result returned by the <paramref name="callback"/>.</typeparam>
   /// <typeparam name="TArgument">The type of the <paramref name="argument"/> to pass to the given <paramref name="callback"/>.</typeparam>
   /// <param name="callback">The operation to offload to the dispatcher, may be executed on a different thread.</param>
   /// <param name="argument">The argument to pass to the given <paramref name="callback"/> when it's being executed.</param>
   /// <param name="priority">The priority that the operation should be processed at.</param>
   /// <returns>A dispatcher operation instance used to represent the offloaded <paramref name="callback"/>.</returns>
   /// <remarks>The dispatcher might only be able to guarantee that the given <paramref name="callback"/> will start at the given <paramref name="priority"/>.</remarks>
   IDispatcherOperation<TResult> Offload<TResult, TArgument>(Func<TArgument, ValueTask<TResult>> callback, TArgument argument, DispatchPriority priority = DispatchPriority.Normal);
   #endregion
}
