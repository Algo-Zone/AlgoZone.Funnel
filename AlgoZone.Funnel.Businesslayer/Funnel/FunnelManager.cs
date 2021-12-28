using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;
using AlgoZone.Funnel.Datalayer.Elasticsearch;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager, IDisposable
    {
        #region Fields

        private readonly ElasticsearchDal _elasticsearchDal;

        private readonly IInputManager _inputManager;
        private readonly IOutputManager _outputManager;

        #endregion

        #region Constructors

        public FunnelManager(IInputManager inputManager, IOutputManager outputManager)
        {
            _inputManager = inputManager;
            _outputManager = outputManager;
            _elasticsearchDal = new ElasticsearchDal("elastic.lan", "80", "funnel");
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
            foreach (var symbol in symbols)
            {
                _inputManager.SubscribeToSymbolTickerUpdates(symbol, async tick => await HandleTick(tick));
                _inputManager.SubscribeToSymbolOrderBookUpdates(symbol, 1000, orderBook => { Console.WriteLine($"Order book: {orderBook.Data.Asks.Count}:{orderBook.Data.Bids.Count}"); });
            }
        }

        /// <inheritdoc />
        public void RunFunnel()
        {
            _inputManager.SubscribeToAllSymbolTickerUpdates(async tick => await HandleTick(tick));
        }

        /// <summary>
        /// Handles a tick event.
        /// </summary>
        /// <param name="tick">The tick event to handle.</param>
        private async Task HandleTick(SymbolTickEventData tick)
        {
            Console.WriteLine($"[{tick.Data.Symbol}] Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");

            try
            {
                await _elasticsearchDal.AddDocumentAsync(tick);
                await _outputManager.PublishEventAsync(tick);
            }
            catch (Exception)
            {
                Console.WriteLine($"Something went wrong while getting tick for [{tick.Data.Symbol}]");
            }
        }

        #endregion
    }
}