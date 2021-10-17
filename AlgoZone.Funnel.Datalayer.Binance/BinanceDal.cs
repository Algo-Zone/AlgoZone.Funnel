﻿using System;
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
        /// Subscribes to the symbol order book updates.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="onUpdate">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<EventData<SymbolBinanceOrderBook>> onUpdate)
        {
            return _socketClient.Spot.SubscribeToOrderBookUpdatesAsync(symbol, interval, eventData => { onUpdate.Invoke(MapSymbolOrderBook(eventData)); }).Result.Success;
        }

        /// <summary>
        /// Subscribes to the symbol tick updates.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolTicker(string symbol, Action<EventData<SymbolBinanceTick>> onTick)
        {
            return _socketClient.Spot.SubscribeToSymbolTickerUpdatesAsync(symbol, eventData => { onTick.Invoke(MapSymbolTick(eventData)); }).Result.Success;
        }

        #region Static Methods

        private static EventData<SymbolBinanceOrderBook> MapSymbolOrderBook(DataEvent<IBinanceEventOrderBook> binanceOrderBook)
        {
            return new EventData<SymbolBinanceOrderBook>
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

        private static EventData<SymbolBinanceTick> MapSymbolTick(DataEvent<IBinanceTick> binanceTick)
        {
            return new EventData<SymbolBinanceTick>
            {
                Data = new SymbolBinanceTick
                {
                    AskPrice = binanceTick.Data.AskPrice,
                    AskQuantity = binanceTick.Data.AskQuantity,
                    BidPrice = binanceTick.Data.BidPrice,
                    BidQuantity = binanceTick.Data.BidQuantity,
                    PrevDayClosePrice = binanceTick.Data.PrevDayClosePrice
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