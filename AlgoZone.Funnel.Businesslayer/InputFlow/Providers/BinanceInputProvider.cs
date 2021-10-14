using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;
using AlgoZone.Funnel.Datalayer.Binance;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public class BinanceInputProvider : IInputProvider
    {
        #region Fields

        private readonly BinanceDal _binanceDal;

        #endregion

        #region Constructors

        public BinanceInputProvider()
        {
            _binanceDal = new BinanceDal();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<Models.EventData<SymbolTick>> onTick)
        {
            return _binanceDal.SubscribeToSymbolTicker(symbol, eventData => { onTick.Invoke(MapBinanceSymbolTick(eventData)); });
        }

        #region Static Methods

        private static Models.EventData<SymbolTick> MapBinanceSymbolTick(Datalayer.Binance.EventData<SymbolBinanceTick> binanceEventData)
        {
            return new Models.EventData<SymbolTick>
            {
                Data = new SymbolTick
                {
                    AskPrice = binanceEventData.Data.AskPrice,
                    AskQuantity = binanceEventData.Data.AskQuantity,
                    BidPrice = binanceEventData.Data.BidPrice,
                    BidQuantity = binanceEventData.Data.BidQuantity,
                    PrevDayClosePrice = binanceEventData.Data.PrevDayClosePrice
                },
                Timestamp = binanceEventData.Timestamp,
                Topic = binanceEventData.Topic,
                OriginalData = binanceEventData.OriginalData
            };
        }

        #endregion

        #endregion

        /// <inheritdoc />
        public void Dispose()
        {
            _binanceDal.Dispose();
        }
    }
}