namespace Sain.SDL3.Contexts;

internal interface ISDL3EventHandler<T>
{
   #region Methods
   void OnEvent(in T ev);
   #endregion
}

internal interface ISDL3EventHandler : ISDL3EventHandler<SDL3_Event> { }


internal static class ISDL3EventHandlerExtensions
{
   #region Methods
   public static void RouteEvent<T>(this ISDL3EventHandler<T> handler, in T ev)
   {
      handler.OnEvent(ev);
   }
   #endregion
}
