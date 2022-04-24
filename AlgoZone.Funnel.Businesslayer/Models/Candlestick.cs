using System;

namespace AlgoZone.Funnel.Businesslayer.Models
{
    public class Candlestick
    {
        #region Properties

        public decimal Close { get; set; }

        public DateTime CloseTime { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Open { get; set; }

        public DateTime OpenTime { get; set; }

        public string Symbol { get; set; }

        public decimal Volume { get; set; }

        #endregion
    }
}