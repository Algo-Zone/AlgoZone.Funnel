using System;
using System.Collections.Generic;

namespace AlgoZone.Funnel.Businesslayer.EventData
{
    public class EventDataCollection<TSymbolData> : IEventData<TSymbolData>
        where TSymbolData : IEnumerable<ISymbolData>
    {
        #region Properties

        /// <inheritdoc />
        public TSymbolData Data { get; set; }

        /// <inheritdoc />
        public EventDataType EventDataType { get; set; }

        /// <inheritdoc />
        public string OriginalData { get; set; }

        /// <inheritdoc />
        public DateTime Timestamp { get; set; }

        /// <inheritdoc />
        public string Topic { get; set; }

        #endregion
    }
}