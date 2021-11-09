using System;

namespace AlgoZone.Funnel.Businesslayer.EventData
{
    [Serializable]
    public class OrderBookEntry
    {
        #region Properties

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        #endregion
    }
}