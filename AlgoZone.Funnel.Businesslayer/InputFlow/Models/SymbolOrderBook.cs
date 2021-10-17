using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Datalayer.Binance;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Models
{
    public class SymbolOrderBook
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