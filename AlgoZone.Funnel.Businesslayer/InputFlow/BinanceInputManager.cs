using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public class BinanceInputManager : InputManager
    {
        #region Constructors

        public BinanceInputManager() : base(new BinanceInputProvider()) { }

        #endregion
    }
}