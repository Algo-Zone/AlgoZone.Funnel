using System;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class EventData<T>
    {
        #region Properties

        /// <summary>
        /// The received data deserialized into an object
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// The original data that was received, only available when OutputOriginalData is set to true in the client options
        /// </summary>
        public string OriginalData { get; set; }

        /// <summary>
        /// The timestamp the data was received
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The topic of the update, what symbol/asset etc..
        /// </summary>
        public string Topic { get; set; }

        #endregion
    }
}