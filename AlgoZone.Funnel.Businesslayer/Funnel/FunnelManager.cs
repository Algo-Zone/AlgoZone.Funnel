using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;
using AlgoZone.Funnel.Exceptions;
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

        #endregion

        #region Constructors

        public FunnelManager(IEnumerable<IInputManager> inputManagers, IOutputManager outputManager)
        {
            _inputManagers = inputManagers;
            _outputManager = outputManager;

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
        public void RunFunnel(IEnumerable<string> symbols)
        {
            var tradingPairs = _selectedInputManager.GetAllTradingPairs().ToList();
            PublishTradingPairs(tradingPairs);

            // foreach (var symbol in symbols)
            // {
            // _inputManager.SubscribeToSymbolTickerUpdates(symbol, async tick => await HandleTick(tick));
            // _inputManager.SubscribeToSymbolOrderBookUpdates(symbol, 1000, orderBook => { Console.WriteLine($"Order book: {orderBook.Data.Asks.Count}:{orderBook.Data.Bids.Count}"); });
            // }
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