using System;
using System.Collections.Generic;
using System.Linq;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Mappers;
using AlgoZone.Funnel.Datalayer.Binance;
using AutoMapper;
using NLog;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public class BinanceInputProvider : IInputProvider
    {
        #region Fields

        private readonly BinanceDal _binanceDal;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public BinanceInputProvider()
        {
            _binanceDal = new BinanceDal();

            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<CandlestickProfile>(); });
            _mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _binanceDal.Dispose();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetAllSymbols()
        {
            return _binanceDal.GetAllSymbols();
        }

        /// <inheritdoc />
        public bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick)
        {
            return _binanceDal.SubscribeToAllSymbolTicker(eventData =>
            {
                Console.WriteLine($"{eventData.Data.Count()} tick events retrieved");

                foreach (var eventDataObject in eventData.Data)
                {
                    onTick.Invoke(MapBinanceSymbolTick(eventData, eventDataObject));
                }
            });
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate)
        {
            return _binanceDal.SubscribeToSymbolOrderBookUpdates(symbol, interval, eventData => { onUpdate.Invoke(MapBinanceSymbolOrderBook(eventData)); });
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolsCandlesticksOneMinute(IEnumerable<string> symbols, Action<SymbolCandlestickEventData> onCandlestick)
        {
            return _binanceDal.SubscribeToSymbolsOneMinuteCandlesticks(symbols, eventData => { onCandlestick.Invoke(MapBinanceSymbolCandlestick(eventData)); });
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick)
        {
            return _binanceDal.SubscribeToSymbolTicker(symbol, eventData => { onTick.Invoke(MapBinanceSymbolTick(eventData)); });
        }

        private SymbolCandlestickEventData MapBinanceSymbolCandlestick(BinanceSymbolEvent<SymbolBinanceKline> binanceSymbolKline)
        {
            return _mapper.Map<SymbolCandlestickEventData>(binanceSymbolKline);
        }

        #region Static Methods

        private static SymbolOrderBookEventData MapBinanceSymbolOrderBook(BinanceSymbolEvent<SymbolBinanceOrderBook> binanceSymbolOrderBook)
        {
            return new SymbolOrderBookEventData
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

        private static SymbolTickEventData MapBinanceSymbolTick(IBinanceEvent eventData, SymbolBinanceTick binanceTick)
        {
            return new SymbolTickEventData
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

        private static SymbolTickEventData MapBinanceSymbolTick(BinanceSymbolEvent<SymbolBinanceTick> binanceSymbolBinanceSymbolEvent)
        {
            return new SymbolTickEventData
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