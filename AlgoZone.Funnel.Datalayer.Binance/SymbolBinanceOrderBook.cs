using System;
using System.Collections.Generic;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class SymbolBinanceOrderBook
    {
        #region Properties

        public ICollection<SymbolBinanceOrderBookEntry> Asks { get; set; }

        public ICollection<SymbolBinanceOrderBookEntry> Bids { get; set; }

        public DateTime EventTime { get; set; }

        public long? FirstUpdatedId { get; set; }

        public long LastUpdateId { get; set; }

        public string Symbol { get; set; }

        #endregion
    }
}