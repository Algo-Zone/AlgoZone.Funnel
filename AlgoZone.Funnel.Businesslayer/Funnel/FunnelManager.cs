using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager, IDisposable
    {
        #region Fields

        private readonly IInputManager _inputManager;
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
            foreach (var symbol in symbols)
            {
                _inputManager.SubscribeToSymbolTickerUpdates(symbol, tick => { Console.WriteLine($"Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}"); });
                _inputManager.SubscribeToSymbolOrderBookUpdates(symbol, 1000, orderBook => { Console.WriteLine($"Order book: {orderBook.Data.Asks.Count}:{orderBook.Data.Bids.Count}"); });
            }
        }

        /// <inheritdoc />
        public void RunFunnel()
        {
            _inputManager.SubscribeToAllSymbolTickerUpdates(tick =>
            {
                Console.WriteLine($"[{tick.Data.Symbol}] Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");
                _outputManager.PublishEvent(tick);
            });
        }

        #endregion
    }
}