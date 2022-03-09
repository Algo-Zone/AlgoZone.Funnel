﻿using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Spot.Socket;
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
        /// Gets a string list of all the symbols currently on binance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllSymbols()
        {
            var response = _client.SpotApi.ExchangeData.GetExchangeInfoAsync().Result;
            if (response == null || !response.Success)
                return new List<string>();

            return response.Data.Symbols.Select(s => s.Name);
        }

        /// <summary>
        /// Subscribes to the symbol tick updates.
        /// </summary>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToAllSymbolTicker(Action<BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>>> onTick)
        {
            return _socketClient.SpotStreams.SubscribeToAllTickerUpdatesAsync(eventData => { onTick.Invoke(MapSymbolTicks(eventData)); }).Result.Success;
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
            return _socketClient.SpotStreams.SubscribeToOrderBookUpdatesAsync(symbol, interval, eventData => { onUpdate.Invoke(MapSymbolOrderBook(eventData)); }).Result.Success;
        }

        /// <summary>
        /// Subscribes to the one minute candlesticks for a list of symbols.
        /// </summary>
        /// <param name="symbols">The list of symbols.</param>
        /// <param name="onCandlestick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolsOneMinuteCandlesticks(IEnumerable<string> symbols, Action<BinanceSymbolEvent<SymbolBinanceKline>> onCandlestick)
        {
            return _socketClient.SpotStreams.SubscribeToKlineUpdatesAsync(symbols, KlineInterval.OneMinute, eventData => { onCandlestick.Invoke(MapSymbolKline(eventData)); }).Result.Success;
        }

        /// <summary>
        /// Subscribes to the symbol tick updates.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        public bool SubscribeToSymbolTicker(string symbol, Action<BinanceSymbolEvent<SymbolBinanceTick>> onTick)
        {
            return _socketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbol, eventData => { onTick.Invoke(MapSymbolTick(eventData)); }).Result.Success;
        }

        #region Static Methods

        private static BinanceSymbolEvent<SymbolBinanceKline> MapSymbolKline(DataEvent<IBinanceStreamKlineData> binanceKline)
        {
            var data = binanceKline.Data.Data as BinanceStreamKline;
            return new BinanceSymbolEvent<SymbolBinanceKline>
            {
                Data = new SymbolBinanceKline
                {
                    Open = data.OpenPrice,
                    High = data.HighPrice,
                    Low = data.LowPrice,
                    Close = data.ClosePrice,
                    Volume = data.Volume,
                    Timestamp = data.OpenTime
                },
                Timestamp = binanceKline.Timestamp,
                Topic = binanceKline.Topic,
                OriginalData = binanceKline.OriginalData
            };
        }

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

        private static SymbolBinanceOrderBookEntry MapSymbolOrderBookEntry(BinanceOrderBookEntry orderBookEntry)
        {
            return new SymbolBinanceOrderBookEntry
            {
                Price = orderBookEntry.Price,
                Quantity = orderBookEntry.Quantity
            };
        }

        private static BinanceSymbolEvent<SymbolBinanceTick> MapSymbolTick(DataEvent<IBinanceTick> binanceTick)
        {
            return new BinanceSymbolEvent<SymbolBinanceTick>
            {
                Data = new SymbolBinanceTick
                {
                    AskPrice = binanceTick.Data.BestAskPrice,
                    AskQuantity = binanceTick.Data.BestAskQuantity,
                    BidPrice = binanceTick.Data.BestBidPrice,
                    BidQuantity = binanceTick.Data.BestBidQuantity,
                    PrevDayClosePrice = binanceTick.Data.PrevDayClosePrice,
                    Symbol = binanceTick.Data.Symbol
                },
                Timestamp = binanceTick.Timestamp,
                Topic = binanceTick.Topic,
                OriginalData = binanceTick.OriginalData
            };
        }

        private static BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>> MapSymbolTicks(DataEvent<IEnumerable<IBinanceTick>> binanceTick)
        {
            return new BinanceSymbolEvent<IEnumerable<SymbolBinanceTick>>
            {
                Data = binanceTick.Data.Select(bt =>
                                                   new SymbolBinanceTick
                                                   {
                                                       AskPrice = bt.BestAskPrice,
                                                       AskQuantity = bt.BestAskQuantity,
                                                       BidPrice = bt.BestBidPrice,
                                                       BidQuantity = bt.BestBidQuantity,
                                                       PrevDayClosePrice = bt.PrevDayClosePrice,
                                                       Symbol = bt.Symbol
                                                   }
                ),
                Timestamp = binanceTick.Timestamp,
                Topic = binanceTick.Topic,
                OriginalData = binanceTick.OriginalData
            };
        }

        #endregion

        #endregion
    }
}