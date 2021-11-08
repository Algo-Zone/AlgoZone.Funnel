using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public sealed class BinanceDal : IDisposable
    {
        #region Fields

        private readonly BinanceClient _client;
        private readonly BinanceSocketClient _socketClient;

        #endregion

        #region Constructors

        public BinanceDal()
        {
            _client = new BinanceClient(new BinanceClientOptions());
            _socketClient = new BinanceSocketClient(new BinanceSocketClientOptions());
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
            _socketClient?.Dispose();
        }

        /// <summary>
        /// Subscribes to the symbol tick updates.
        /// </summary>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToAllSymbolTicker(Action<BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>>> onTick)
        {
            return _socketClient.Spot.SubscribeToAllSymbolTickerUpdatesAsync(eventData => { onTick.Invoke(MapSymbolTicks(eventData)); }).Result.Success;
        }

        /// <summary>
        /// Subscribes to the symbol order book updates.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="onUpdate">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<BinanceSymbolEvent<SymbolBinanceOrderBook>> onUpdate)
        {
            return _socketClient.Spot.SubscribeToOrderBookUpdatesAsync(symbol, interval, eventData => { onUpdate.Invoke(MapSymbolOrderBook(eventData)); }).Result.Success;
        }

        /// <summary>
        /// Subscribes to the symbol tick updates.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolTicker(string symbol, Action<BinanceSymbolEvent<SymbolBinanceTick>> onTick)
        {
            return _socketClient.Spot.SubscribeToSymbolTickerUpdatesAsync(symbol, eventData => { onTick.Invoke(MapSymbolTick(eventData)); }).Result.Success;
        }

        #region Static Methods

        private static BinanceSymbolEvent<SymbolBinanceOrderBook> MapSymbolOrderBook(DataEvent<IBinanceEventOrderBook> binanceOrderBook)
        {
            return new BinanceSymbolEvent<SymbolBinanceOrderBook>
            {
                Data = new SymbolBinanceOrderBook
                {
                    Asks = binanceOrderBook.Data.Asks.Select(MapSymbolOrderBookEntry).ToList(),
                    Bids = binanceOrderBook.Data.Bids.Select(MapSymbolOrderBookEntry).ToList(),
                    Symbol = binanceOrderBook.Data.Symbol,
                    FirstUpdatedId = binanceOrderBook.Data.FirstUpdateId,
                    LastUpdateId = binanceOrderBook.Data.LastUpdateId,
                    EventTime = binanceOrderBook.Data.EventTime
                },
                Timestamp = binanceOrderBook.Timestamp,
                Topic = binanceOrderBook.Topic,
                OriginalData = binanceOrderBook.OriginalData
            };
        }

        private static SymbolOrderBookEntry MapSymbolOrderBookEntry(BinanceOrderBookEntry orderBookEntry)
        {
            return new SymbolOrderBookEntry
            {
                Price = orderBookEntry.Price,
                Quantity = orderBookEntry.Quantity
            };
        }

        private static BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>> MapSymbolTicks(DataEvent<IEnumerable<IBinanceTick>> binanceTick)
        {
            return new BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>>
            {
                Data = binanceTick.Data.Select(bt =>
                                                   new SymbolBinanceTick
                                                   {
                                                       AskPrice = bt.AskPrice,
                                                       AskQuantity = bt.AskQuantity,
                                                       BidPrice = bt.BidPrice,
                                                       BidQuantity = bt.BidQuantity,
                                                       PrevDayClosePrice = bt.PrevDayClosePrice,
                                                       Symbol = bt.Symbol
                                                   }
                ),
                Timestamp = binanceTick.Timestamp,
                Topic = binanceTick.Topic,
                OriginalData = binanceTick.OriginalData
            };
        }

        private static BinanceSymbolEvent<SymbolBinanceTick> MapSymbolTick(DataEvent<IBinanceTick> binanceTick)
        {
            return new BinanceSymbolEvent<SymbolBinanceTick>
            {
                Data = new SymbolBinanceTick
                {
                    AskPrice = binanceTick.Data.AskPrice,
                    AskQuantity = binanceTick.Data.AskQuantity,
                    BidPrice = binanceTick.Data.BidPrice,
                    BidQuantity = binanceTick.Data.BidQuantity,
                    PrevDayClosePrice = binanceTick.Data.PrevDayClosePrice,
                    Symbol = binanceTick.Data.Symbol
                },
                Timestamp = binanceTick.Timestamp,
                Topic = binanceTick.Topic,
                OriginalData = binanceTick.OriginalData
            };
        }

        #endregion

        #endregion
    }
}