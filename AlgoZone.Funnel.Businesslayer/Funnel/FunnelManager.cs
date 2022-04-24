using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;
using AlgoZone.Funnel.Exceptions;
using AutoMapper;
using NLog;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager, IDisposable
    {
        #region Fields

        private readonly IEnumerable<IInputManager> _inputManagers;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IOutputManager _outputManager;
        private IInputManager _selectedInputManager;
        private IMapper _mapper;

        #endregion

        #region Constructors

        public FunnelManager(IEnumerable<IInputManager> inputManagers, IOutputManager outputManager, IMapper mapper)
        {
            _inputManagers = inputManagers;
            _outputManager = outputManager;
            _mapper = mapper;

            _selectedInputManager = _inputManagers.FirstOrDefault();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _selectedInputManager?.Dispose();
        }

        /// <inheritdoc />
        public void ImportHistory(string symbol)
        {
            var startDate = new DateTime(2017, 1, 1);
            var endDate = DateTime.Now;
            var days = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                      .Select(offset => startDate.AddDays(offset))
                      .ToArray(); 
            foreach(var day in days)
            {
                var candlesticks = _selectedInputManager.GetCandlesticks(symbol, day, day.AddDays(1).AddTicks(-1));
                var mappedCandlesticks = candlesticks.Select(c =>
                {
                    var candlestick = _mapper.Map<SymbolCandlestick>(c);
                    candlestick.Symbol = symbol;
                    return new SymbolCandlestickEventData
                    {
                        Data = candlestick,
                        EventDataType = EventDataType.Tick,
                        Timestamp = DateTime.Now,
                        Topic = symbol
                    };
                }).ToList();
                foreach (var candlestickEventData in mappedCandlesticks)
                    OnCandlestick(candlestickEventData);
                
                _logger.Info($"Imported {candlesticks.Count} candlesticks for date {day:dd-MM-yyyy}");
            }
        }

        /// <inheritdoc />
        public void RunFunnel(IEnumerable<string> symbols)
        {
            _logger.Info($"Running funnel for symbols: {string.Join(", ", symbols)}");
            var tradingPairs = _selectedInputManager.GetAllTradingPairs().ToList();
            PublishTradingPairs(tradingPairs);
            
            _selectedInputManager.SubscribeToSymbolsCandlesticksOneMinute(symbols, OnCandlestick);
        }

        /// <inheritdoc />
        public void RunFunnel()
        {
            var tradingPairs = _selectedInputManager.GetAllTradingPairs().ToList();
            PublishTradingPairs(tradingPairs);

            var symbols = tradingPairs.Select(tp => tp.Topic);
            symbols = symbols.Where(s => s.StartsWith("BTC") || s.EndsWith("BTC"));
            _selectedInputManager.SubscribeToSymbolsCandlesticksOneMinute(symbols, OnCandlestick);
        }

        /// <inheritdoc />
        public void SetExchange(string exchange)
        {
            if (string.IsNullOrWhiteSpace(exchange))
                throw new NoExchangeProvidedException(exchange);

            var parsedExchange = GetExchange(exchange);
            if (parsedExchange == Exchange.Unknown)
                throw new NoExchangeProvidedException(exchange);

            _selectedInputManager = _inputManagers.FirstOrDefault(i => i.Exchange == parsedExchange);
        }

        /// <summary>
        /// Handles a tick event.
        /// </summary>
        /// <param name="tick">The tick event to handle.</param>
        private async Task HandleTick(SymbolTickEventData tick)
        {
            try
            {
                await _outputManager.PublishEventAsync(tick);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Fatal, e);
            }
        }

        private async void OnCandlestick(SymbolCandlestickEventData candlestick)
        {
            await _outputManager.PublishEventAsync(candlestick);
        }

        private void PublishTradingPairs(IEnumerable<SymbolTradingPairEventData> tradingPairs)
        {
            foreach (var tradingPair in tradingPairs)
            {
                _outputManager.PublishEvent(tradingPair);
            }
        }

        #region Static Methods

        /// <summary>
        /// Gets the exchange based on it's name.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <returns></returns>
        private static Exchange GetExchange(string exchangeName)
        {
            if (Constants.Constants.BinanceNames.Contains(exchangeName.ToLower()))
                return Exchange.Binance;

            return Exchange.Unknown;
        }

        #endregion

        #endregion
    }
}