using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public class BinanceInputManager : InputManager
    {
        #region Properties

        /// <inheritdoc />
        public override Exchange Exchange => Exchange.Binance;

        #endregion

        #region Constructors

        public BinanceInputManager() : base(new BinanceInputProvider()) { }

        #endregion
    }
}