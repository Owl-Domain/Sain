namespace Sain.SDL3.Contexts;

internal unsafe interface ISDL3Context : IContext
{
   #region Properties
   SDL3_InitFlags Flags { get; }
   #endregion

   #region Methods
   void OnEvent(in SDL3_Event ev);
   #endregion
}
