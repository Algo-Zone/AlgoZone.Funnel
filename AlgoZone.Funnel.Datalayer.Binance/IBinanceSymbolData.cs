namespace AlgoZone.Funnel.Datalayer.Binance
{
    public interface IBinanceSymbolData<TSymbolData> : IBinanceEvent
    {
        #region Properties

        /// <summary>
        /// The received data deserialized into an object
        /// </summary>
        TSymbolData Data { get; set; }

        #endregion
    }
}