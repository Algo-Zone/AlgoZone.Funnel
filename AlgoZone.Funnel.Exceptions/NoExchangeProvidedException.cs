using System;

namespace AlgoZone.Funnel.Exceptions
{
    public class NoExchangeProvidedException : Exception
    {
        #region Properties

        public string ExchangeInput { get; private set; }

        #endregion

        #region Constructors

        public NoExchangeProvidedException(string exchangeInput)
        {
            ExchangeInput = exchangeInput;
        }

        #endregion
    }
}