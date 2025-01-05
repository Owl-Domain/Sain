namespace Sain.Shared.Dispatch;

// Note(Nightowl): Do not include any aliases in the enum;

/// <summary>
///   Represents the different available priorities for dispatch operations.
/// </summary>
public enum DispatchPriority : byte
{
   /// <summary>A priority meant for the highest priority operations.</summary>
   Highest,

   /// <summary>The dispatched operation will be processed at the same priority as audio events.</summary>
   Audio,

   /// <summary>The dispatched operation will be processed along with other regular application operations.</summary>
   Normal,

   /// <summary>The dispatched operation will be processed at the same priority as data related events.</summary>
   Data,

   /// <summary>The dispatched operation will be processed at the same priority as visual events.</summary>
   Visual,

   /// <summary>The dispatched operation will be processed at the same priority as input events.</summary>
   Input,

   /// <summary>The dispatched operation will be completed in the background.</summary>
   /// <remarks>This means that the operation will be processed only after all non-idle operations have finished.</remarks>
   Background,

   /// <summary>A priority meant for the lowest priority operations.</summary>
   Lowest,
}
