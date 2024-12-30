using System.Buffers;

namespace Sain.SDL3;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_Renderer;

static unsafe partial class Native
{
   #region General functions
   [LibraryImport(LibName, EntryPoint = "SDL_CreateRenderer", StringMarshalling = StringMarshalling.Utf8)]
   public static partial SDL3_Renderer* CreateRenderer(SDL3_Window* window, string? name);

   [LibraryImport(LibName, EntryPoint = "SDL_CreateWindowAndRenderer", StringMarshalling = StringMarshalling.Utf8)]
   [return: MarshalAs(Bool)]
   public static partial bool CreateWindowAndRenderer(string name, int width, int height, SDL3_WindowFlags flags, out SDL3_Window* window, out SDL3_Renderer* renderer);

   [LibraryImport(LibName, EntryPoint = "SDL_DestroyRenderer")]
   public static partial void DestroyRenderer(SDL3_Renderer* renderer);

   [LibraryImport(LibName, EntryPoint = "SDL_RenderPresent")]
   [return: MarshalAs(Bool)]
   public static partial bool RenderPresent(SDL3_Renderer* renderer);

   [LibraryImport(LibName, EntryPoint = "SDL_SetRenderDrawColor")]
   [return: MarshalAs(Bool)]
   public static partial bool SetRenderDrawColor(SDL3_Renderer* renderer, byte red, byte green, byte blue, byte alpha);
   public static bool SetRenderDrawColor(SDL3_Renderer* renderer, Color color) => SetRenderDrawColor(renderer, color.Red, color.Green, color.Blue, color.Alpha);

   [LibraryImport(LibName, EntryPoint = "SDL_RenderClear")]
   [return: MarshalAs(Bool)]
   public static partial bool RenderClear(SDL3_Renderer* renderer);
   #endregion

   #region Render points
   [LibraryImport(LibName, EntryPoint = "SDL_RenderPoint")]
   [return: MarshalAs(Bool)]
   public static partial bool RenderPoint(SDL3_Renderer* renderer, float x, float y);
   public static bool RenderPoint(SDL3_Renderer* renderer, Point point) => RenderPoint(renderer, (float)point.X, (float)point.Y);

   [LibraryImport(LibName, EntryPoint = "SDL_RenderPoints")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderPoints(SDL3_Renderer* renderer, SDL3_FPoint* points, int count);
   public static bool RenderPoints(SDL3_Renderer* renderer, params ReadOnlySpan<Point> points)
   {
      static void Convert(ReadOnlySpan<Point> src, Span<SDL3_FPoint> dest)
      {
         Debug.Assert(src.Length == dest.Length);
         for (int i = 0; i < src.Length; i++)
            dest[i] = src[i];
      }

      if (points.Length <= 128)
      {
         Span<SDL3_FPoint> span = stackalloc SDL3_FPoint[points.Length];
         Convert(points, span);

         fixed (SDL3_FPoint* pointer = span)
            return _RenderPoints(renderer, pointer, points.Length);
      }
      else
      {
         SDL3_FPoint[] buffer = ArrayPool<SDL3_FPoint>.Shared.Rent(points.Length);

         try
         {
            Span<SDL3_FPoint> span = new(buffer, 0, points.Length);
            Convert(points, span);

            fixed (SDL3_FPoint* pointer = span)
               return _RenderPoints(renderer, pointer, points.Length);
         }
         finally
         {
            ArrayPool<SDL3_FPoint>.Shared.Return(buffer);
         }
      }
   }
   #endregion

   #region Render rectangle outlines
   [LibraryImport(LibName, EntryPoint = "SDL_RenderRect")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderRect(SDL3_Renderer* renderer, SDL3_FRect* rect);
   public static bool RenderRect(SDL3_Renderer* renderer, Rectangle rectangle)
   {
      SDL3_FRect rect = rectangle;
      return _RenderRect(renderer, &rect);
   }
   public static bool RenderRect(SDL3_Renderer* renderer) => _RenderRect(renderer, null);

   [LibraryImport(LibName, EntryPoint = "SDL_RenderRects")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderRects(SDL3_Renderer* renderer, SDL3_FRect* rects, int count);
   public static bool RenderRects(SDL3_Renderer* renderer, params ReadOnlySpan<Rectangle> rectangles)
   {
      static void Convert(ReadOnlySpan<Rectangle> src, Span<SDL3_FRect> dest)
      {
         Debug.Assert(src.Length == dest.Length);
         for (int i = 0; i < src.Length; i++)
            dest[i] = src[i];
      }

      if (rectangles.Length <= 64)
      {
         Span<SDL3_FRect> span = stackalloc SDL3_FRect[rectangles.Length];
         Convert(rectangles, span);

         fixed (SDL3_FRect* pointer = span)
            return _RenderRects(renderer, pointer, rectangles.Length);
      }
      else
      {
         SDL3_FRect[] buffer = ArrayPool<SDL3_FRect>.Shared.Rent(rectangles.Length);

         try
         {
            Span<SDL3_FRect> span = new(buffer, 0, rectangles.Length);
            Convert(rectangles, span);

            fixed (SDL3_FRect* pointer = span)
               return _RenderRects(renderer, pointer, rectangles.Length);
         }
         finally
         {
            ArrayPool<SDL3_FRect>.Shared.Return(buffer);
         }
      }
   }
   #endregion

   #region Render filled rectangles
   [LibraryImport(LibName, EntryPoint = "SDL_RenderFillRect")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderFillRect(SDL3_Renderer* renderer, SDL3_FRect* rect);
   public static bool RenderFillRect(SDL3_Renderer* renderer, Rectangle rectangle)
   {
      SDL3_FRect rect = rectangle;
      return _RenderFillRect(renderer, &rect);
   }
   public static bool RenderFillRect(SDL3_Renderer* renderer) => _RenderFillRect(renderer, null);

   [LibraryImport(LibName, EntryPoint = "SDL_RenderFillRects")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderFillRects(SDL3_Renderer* renderer, SDL3_FRect* rects, int count);
   public static bool RenderFillRects(SDL3_Renderer* renderer, params ReadOnlySpan<Rectangle> rectangles)
   {
      static void Convert(ReadOnlySpan<Rectangle> src, Span<SDL3_FRect> dest)
      {
         Debug.Assert(src.Length == dest.Length);
         for (int i = 0; i < src.Length; i++)
            dest[i] = src[i];
      }

      if (rectangles.Length <= 64)
      {
         Span<SDL3_FRect> span = stackalloc SDL3_FRect[rectangles.Length];
         Convert(rectangles, span);

         fixed (SDL3_FRect* pointer = span)
            return _RenderFillRects(renderer, pointer, rectangles.Length);
      }
      else
      {
         SDL3_FRect[] buffer = ArrayPool<SDL3_FRect>.Shared.Rent(rectangles.Length);

         try
         {
            Span<SDL3_FRect> span = new(buffer, 0, rectangles.Length);
            Convert(rectangles, span);

            fixed (SDL3_FRect* pointer = span)
               return _RenderFillRects(renderer, pointer, rectangles.Length);
         }
         finally
         {
            ArrayPool<SDL3_FRect>.Shared.Return(buffer);
         }
      }
   }
   #endregion

   #region Render lines
   [LibraryImport(LibName, EntryPoint = "SDL_RenderLine")]
   [return: MarshalAs(Bool)]
   public static partial bool RenderLine(SDL3_Renderer* renderer, float x1, float y1, float x2, float y2);
   public static bool RenderLine(SDL3_Renderer* renderer, Point pt1, Point pt2)
   {
      float
         x1 = (float)pt1.X,
         y1 = (float)pt1.Y,
         x2 = (float)pt2.X,
         y2 = (float)pt2.Y;

      return RenderLine(renderer, x1, y1, x2, y2);
   }

   [LibraryImport(LibName, EntryPoint = "SDL_RenderLines")]
   [return: MarshalAs(Bool)]
   private static partial bool _RenderLines(SDL3_Renderer* renderer, SDL3_FPoint* points, int count);
   public static bool RenderLines(SDL3_Renderer* renderer, params ReadOnlySpan<Point> points)
   {
      static void Convert(ReadOnlySpan<Point> src, Span<SDL3_FPoint> dest)
      {
         Debug.Assert(src.Length == dest.Length);
         for (int i = 0; i < src.Length; i++)
            dest[i] = src[i];
      }

      if (points.Length <= 128)
      {
         Span<SDL3_FPoint> span = stackalloc SDL3_FPoint[points.Length];
         Convert(points, span);

         fixed (SDL3_FPoint* pointer = span)
            return _RenderLines(renderer, pointer, points.Length);
      }
      else
      {
         SDL3_FPoint[] buffer = ArrayPool<SDL3_FPoint>.Shared.Rent(points.Length);

         try
         {
            Span<SDL3_FPoint> span = new(buffer, 0, points.Length);
            Convert(points, span);

            fixed (SDL3_FPoint* pointer = span)
               return _RenderLines(renderer, pointer, points.Length);
         }
         finally
         {
            ArrayPool<SDL3_FPoint>.Shared.Return(buffer);
         }
      }
   }
   #endregion
}
