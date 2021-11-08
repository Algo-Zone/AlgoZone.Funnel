using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Businesslayer.InputFlow;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager
    {
        #region Fields

        private readonly IInputManager _inputManager;

        #endregion

        #region Constructors

        public FunnelManager(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }

        #endregion

        #region Methods

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
            });
        }

        #endregion
    }
}