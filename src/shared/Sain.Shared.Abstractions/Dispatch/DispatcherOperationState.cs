namespace Sain.Shared.Dispatch;

/// <summary>
///   Represents the different possible states for a dispatch request.
/// </summary>
public enum DispatcherOperationState : byte
{
   /// <summary>The operation has been queued but it has not yet started.</summary>
   Queued,

   /// <summary>The operation has started being processed.</summary>
   Started,

   /// <summary>The operation has finished processing.</summary>
   Finished,
}
