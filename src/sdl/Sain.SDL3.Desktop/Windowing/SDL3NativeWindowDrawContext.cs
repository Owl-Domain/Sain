namespace Sain.SDL3.Desktop.Windowing;

/// <summary>
///   Represents the SDL3 specific native window drawing context.
/// </summary>
public sealed unsafe class SDL3NativeWindowDrawContext : BaseNativeWindowDrawContext
{
   #region Fields
   private readonly IApplicationContext _context;
   private readonly SDL3NativeWindow _window;
   #endregion

   #region Properties
   private SDL3_Renderer* Renderer => _window.Renderer;
   private ILoggingContext Logging => _context.Logging;
   private Guid Id => _window.Id;
   private SDL3_WindowId WindowId => _window.WindowId;
   #endregion
   internal SDL3NativeWindowDrawContext(IApplicationContext context, SDL3NativeWindow window)
   {
      _context = context;
      _window = window;
   }

   #region Methods
   /// <inheritdoc/>
   public override void Clear(Color color)
   {
      SetRenderColor(color);

      if (Native.RenderClear(Renderer) is false && Logging.IsAvailable)
         Logging.Warning<SDL3NativeWindowDrawContext>($"Failed to clear the render for the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }
   /// <inheritdoc/>
   public override void DrawPixel(Point position, Color color)
   {
      SetRenderColor(color);

      if (Native.RenderPoint(Renderer, position) is false && Logging.IsAvailable)
         Logging.Warning<SDL3NativeWindowDrawContext>($"Failed to draw a pixel in the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }

   /// <inheritdoc/>
   public override void DrawLine(Point start, Point end, Color color)
   {
      SetRenderColor(color);

      if (Native.RenderLine(Renderer, start, end) is false && Logging.IsAvailable)
         Logging.Warning<SDL3NativeWindowDrawContext>($"Failed to draw a line in the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }

   /// <inheritdoc/>
   public override void DrawRectangleFilled(Rectangle rectangle, Color color)
   {
      SetRenderColor(color);

      if (Native.RenderFillRect(Renderer, rectangle) is false && Logging.IsAvailable)
         Logging.Warning<SDL3NativeWindowDrawContext>($"Failed to draw a filled in rectangle in the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }

   /// <inheritdoc/>
   public override void DrawRectangleOutline(Rectangle rectangle, Color color)
   {
      SetRenderColor(color);

      if (Native.RenderRect(Renderer, rectangle) is false && Logging.IsAvailable)
         Logging.Warning<SDL3NativeWindowDrawContext>($"Failed to draw a rectangle outline in the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})");
   }
   #endregion

   #region Helpers
   private void SetRenderColor(Color color, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
   {
      if (Native.SetRenderDrawColor(Renderer, color) is false && Logging.IsAvailable)
      {
         string message = $"Failed to set the draw color for the window, Id = ({Id}), WindowId = ({WindowId}). ({Native.LastError})";
         Logging.Warning<SDL3NativeWindowDrawContext>(message, member, file, line);
      }
   }
   #endregion
}
