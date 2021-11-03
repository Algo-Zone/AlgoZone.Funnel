using System;

namespace AlgoZone.Funnel.Businesslayer.EventData
{
    public interface IEventData<TSymbolData>
        where TSymbolData : ISymbolData
    {
        #region Properties

        /// <summary>
        /// The received data deserialized into an object
        /// </summary>
        TSymbolData Data { get; set; }

        /// <summary>
        /// The event data type.
        /// </summary>
        EventDataType EventDataType { get; set; }

        /// <summary>
        /// The original data that was received, only available when OutputOriginalData is set to true in the client options
        /// </summary>
        string OriginalData { get; set; }

        /// <summary>
        /// The timestamp the data was received
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// The topic of the update, what symbol/asset etc..
        /// </summary>
        string Topic { get; set; }

        #endregion
    }
}