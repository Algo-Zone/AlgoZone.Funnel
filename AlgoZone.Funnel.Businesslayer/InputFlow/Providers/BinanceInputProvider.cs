using System;
using System.Linq;
using AlgoZone.Funnel.Businesslayer.EventData;
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
        public void Dispose()
        {
            _binanceDal.Dispose();
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<IEventData<SymbolOrderBook>> onUpdate)
        {
            return _binanceDal.SubscribeToSymbolOrderBookUpdates(symbol, interval, eventData => { onUpdate.Invoke(MapBinanceSymbolOrderBook(eventData)); });
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<IEventData<SymbolTick>> onTick)
        {
            return _binanceDal.SubscribeToSymbolTicker(symbol, eventData => { onTick.Invoke(MapBinanceSymbolTick(eventData)); });
        }

        #region Static Methods

        private static IEventData<SymbolTick> MapBinanceSymbolTick(Datalayer.Binance.EventData<SymbolBinanceTick> binanceEventData)
        {
            return new EventData.EventData<SymbolTick>
            {
                Data = new SymbolTick
                {
                    AskPrice = binanceEventData.Data.AskPrice,
                    AskQuantity = binanceEventData.Data.AskQuantity,
                    BidPrice = binanceEventData.Data.BidPrice,
                    BidQuantity = binanceEventData.Data.BidQuantity,
                    PrevDayClosePrice = binanceEventData.Data.PrevDayClosePrice,
                    Symbol = binanceEventData.Topic
                },
                Timestamp = binanceEventData.Timestamp,
                Topic = binanceEventData.Topic,
                OriginalData = binanceEventData.OriginalData
            };
        }

        private static IEventData<SymbolOrderBook> MapBinanceSymbolOrderBook(Datalayer.Binance.EventData<SymbolBinanceOrderBook> binanceOrderBook)
        {
            return new EventData.EventData<SymbolOrderBook>
            {
                Data = new SymbolOrderBook
                {
                    Asks = binanceOrderBook.Data.Asks.Select(MapSymbolOrderBookEntry).ToList(),
                    Bids = binanceOrderBook.Data.Bids.Select(MapSymbolOrderBookEntry).ToList(),
                    Symbol = binanceOrderBook.Data.Symbol,
                    FirstUpdatedId = binanceOrderBook.Data.FirstUpdatedId,
                    LastUpdateId = binanceOrderBook.Data.LastUpdateId,
                    EventTime = binanceOrderBook.Data.EventTime
                },
                Timestamp = binanceOrderBook.Timestamp,
                Topic = binanceOrderBook.Topic,
                OriginalData = binanceOrderBook.OriginalData
            };
        }

        private static SymbolOrderBookEntry MapSymbolOrderBookEntry(SymbolOrderBookEntry orderBookEntry)
        {
            return new SymbolOrderBookEntry
            {
                Price = orderBookEntry.Price,
                Quantity = orderBookEntry.Quantity
            };
        }

        #endregion

        #endregion
    }
}