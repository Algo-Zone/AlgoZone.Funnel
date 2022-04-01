using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;
using AlgoZone.Funnel.Datalayer.Elasticsearch;
using NLog;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager, IDisposable
    {
        #region Fields

        private readonly ElasticsearchDal _elasticsearchDal;

        private readonly IInputManager _inputManager;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IOutputManager _outputManager;

        #endregion

        #region Constructors

        public FunnelManager(IInputManager inputManager, IOutputManager outputManager)
        {
            _inputManager = inputManager;
            _outputManager = outputManager;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _inputManager?.Dispose();
        }

        /// <inheritdoc />
        public void RunFunnel(IEnumerable<string> symbols)
        {
            var tradingPairs = _inputManager.GetAllTradingPairs().ToList();
            PublishTradingPairs(tradingPairs);

            // foreach (var symbol in symbols)
            // {
            // _inputManager.SubscribeToSymbolTickerUpdates(symbol, async tick => await HandleTick(tick));
            // _inputManager.SubscribeToSymbolOrderBookUpdates(symbol, 1000, orderBook => { Console.WriteLine($"Order book: {orderBook.Data.Asks.Count}:{orderBook.Data.Bids.Count}"); });
            // }
            _inputManager.SubscribeToSymbolsCandlesticksOneMinute(symbols, OnCandlestick);
        }

        /// <inheritdoc />
        public void RunFunnel()
        {
            var tradingPairs = _inputManager.GetAllTradingPairs().ToList();
            PublishTradingPairs(tradingPairs);

            var symbols = tradingPairs.Select(tp => tp.Topic);
            symbols = symbols.Where(s => s.StartsWith("BTC") || s.EndsWith("BTC"));
            _inputManager.SubscribeToSymbolsCandlesticksOneMinute(symbols, OnCandlestick);
        }

        /// <summary>
        /// Handles a tick event.
        /// </summary>
        /// <param name="tick">The tick event to handle.</param>
        private async Task HandleTick(SymbolTickEventData tick)
        {
            //_logger.Log(LogLevel.Info, $"[{tick.Data.Symbol}] Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");

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

        #endregion
    }
}