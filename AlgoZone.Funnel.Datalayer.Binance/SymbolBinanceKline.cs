using System;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class SymbolBinanceKline
    {
        #region Properties

        /// <summary>
        /// The close price of the candlestick.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// The close time of the candlestick.
        /// </summary>
        public DateTime CloseTime { get; set; }

        /// <summary>
        /// The highest price of the candlestick.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// The lowest price of the candlestick.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// The open price of the candlestick.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// The open time of the candlestick.
        /// </summary>
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// The volume of the candlestick.
        /// </summary>
        public decimal Volume { get; set; }

        #endregion
    }
}