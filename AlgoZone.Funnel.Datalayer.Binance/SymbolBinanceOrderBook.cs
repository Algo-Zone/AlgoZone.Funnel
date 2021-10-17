using System;
using System.Collections.Generic;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class SymbolBinanceOrderBook
    {
        #region Properties

        public ICollection<SymbolOrderBookEntry> Asks { get; set; }

        public ICollection<SymbolOrderBookEntry> Bids { get; set; }

        public DateTime EventTime { get; set; }

        public long? FirstUpdatedId { get; set; }

        public long LastUpdateId { get; set; }

        public string Symbol { get; set; }

        #endregion
    }
}