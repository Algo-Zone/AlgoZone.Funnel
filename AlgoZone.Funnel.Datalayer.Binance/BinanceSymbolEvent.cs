using System;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class BinanceSymbolEvent<TSymbolData> : IBinanceEvent, IBinanceSymbolData<TSymbolData>
    {
        #region Properties

        /// <inheritdoc />
        public TSymbolData Data { get; set; }

        /// <inheritdoc />
        public string OriginalData { get; set; }

        /// <inheritdoc />
        public DateTime Timestamp { get; set; }

        /// <inheritdoc />
        public string Topic { get; set; }

        #endregion
    }
}