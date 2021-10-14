using System;
using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace AlgoZone.Funnel.Datalayer.Binance
{
    public class BinanceDal : IDisposable
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

        public bool SubscribeToSymbolTicker(string symbol, Action<EventData<SymbolBinanceTick>> onTick)
        {
            return _socketClient.Spot.SubscribeToSymbolTickerUpdatesAsync(symbol, eventData => { onTick.Invoke(MapSymbolTick(eventData)); }).Result.Success;
        }

        #region Static Methods

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

        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
            _socketClient?.Dispose();
        }
    }
}