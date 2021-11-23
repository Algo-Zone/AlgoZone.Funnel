using System;
using System.Linq;
using AlgoZone.Core.EventData;
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
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<ISymbolEventData<SymbolOrderBook>> onUpdate)
        {
            return _binanceDal.SubscribeToSymbolOrderBookUpdates(symbol, interval, eventData => { onUpdate.Invoke(MapBinanceSymbolOrderBook(eventData)); });
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<ISymbolEventData<SymbolTick>> onTick)
        {
            return _binanceDal.SubscribeToSymbolTicker(symbol, eventData => { onTick.Invoke(MapBinanceSymbolTick(eventData)); });
        }

        /// <inheritdoc />
        public bool SubscribeToAllSymbolTickerUpdates(Action<ISymbolEventData<SymbolTick>> onTick)
        {
            return _binanceDal.SubscribeToAllSymbolTicker(eventData =>
            {
                foreach (var eventDataObject in eventData.Data)
                {
                    onTick.Invoke(MapBinanceSymbolTick(eventData, eventDataObject));
                }
            });
        }

        #region Static Methods

        private static ISymbolEventData<SymbolOrderBook> MapBinanceSymbolOrderBook(BinanceSymbolEvent<SymbolBinanceOrderBook> binanceSymbolOrderBook)
        {
            return new SymbolEventData<SymbolOrderBook>
            {
                Data = new SymbolOrderBook
                {
                    Asks = binanceSymbolOrderBook.Data.Asks.Select(MapSymbolOrderBookEntry).ToList(),
                    Bids = binanceSymbolOrderBook.Data.Bids.Select(MapSymbolOrderBookEntry).ToList(),
                    Symbol = binanceSymbolOrderBook.Data.Symbol,
                    FirstUpdatedId = binanceSymbolOrderBook.Data.FirstUpdatedId,
                    LastUpdateId = binanceSymbolOrderBook.Data.LastUpdateId,
                    EventTime = binanceSymbolOrderBook.Data.EventTime
                },
                Timestamp = binanceSymbolOrderBook.Timestamp,
                Topic = binanceSymbolOrderBook.Topic,
                OriginalData = binanceSymbolOrderBook.OriginalData
            };
        }

        private static ISymbolEventData<SymbolTick> MapBinanceSymbolTick(IBinanceEvent eventData, SymbolBinanceTick binanceTick)
        {
            return new SymbolEventData<SymbolTick>
            {
                Data = new SymbolTick
                {
                    AskPrice = binanceTick.AskPrice,
                    AskQuantity = binanceTick.AskQuantity,
                    BidPrice = binanceTick.BidPrice,
                    BidQuantity = binanceTick.BidQuantity,
                    PrevDayClosePrice = binanceTick.PrevDayClosePrice,
                    Symbol = binanceTick.Symbol
                },
                Timestamp = eventData.Timestamp,
                Topic = eventData.Topic,
                OriginalData = eventData.OriginalData
            };
        }

        private static ISymbolEventData<SymbolTick> MapBinanceSymbolTick(BinanceSymbolEvent<SymbolBinanceTick> binanceSymbolBinanceSymbolEvent)
        {
            return new SymbolEventData<SymbolTick>
            {
                Data = new SymbolTick
                {
                    AskPrice = binanceSymbolBinanceSymbolEvent.Data.AskPrice,
                    AskQuantity = binanceSymbolBinanceSymbolEvent.Data.AskQuantity,
                    BidPrice = binanceSymbolBinanceSymbolEvent.Data.BidPrice,
                    BidQuantity = binanceSymbolBinanceSymbolEvent.Data.BidQuantity,
                    PrevDayClosePrice = binanceSymbolBinanceSymbolEvent.Data.PrevDayClosePrice,
                    Symbol = binanceSymbolBinanceSymbolEvent.Topic
                },
                Timestamp = binanceSymbolBinanceSymbolEvent.Timestamp,
                Topic = binanceSymbolBinanceSymbolEvent.Topic,
                OriginalData = binanceSymbolBinanceSymbolEvent.OriginalData
            };
        }

        private static SymbolOrderBookEntry MapSymbolOrderBookEntry(SymbolBinanceOrderBookEntry orderBookEntry)
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