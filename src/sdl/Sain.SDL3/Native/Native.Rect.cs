namespace Sain.SDL3;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL3_Rect
{
   #region Fields
   public readonly int X;
   public readonly int Y;
   public readonly int Width;
   public readonly int Height;
   #endregion
}

static partial class Native
{

}
