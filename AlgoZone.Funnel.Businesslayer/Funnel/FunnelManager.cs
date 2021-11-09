using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Datalayer.RabbitMQ;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public class FunnelManager : IFunnelManager, IDisposable
    {
        #region Fields

        private readonly IInputManager _inputManager;

        private readonly RabbitMqDal _dal;

        #endregion

        #region Constructors

        public FunnelManager(IInputManager inputManager)
        {
            _inputManager = inputManager;
            _dal = new RabbitMqDal("localhost");
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
            _inputManager.SubscribeToAllSymbolTickerUpdates(async tick =>
            {
                Console.WriteLine($"[{tick.Data.Symbol}] Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");
                await _dal.Publish(tick);
            });
        }

        #endregion

        /// <inheritdoc />
        public void Dispose()
        {
            _inputManager?.Dispose();
            _dal?.Dispose();
        }
    }
}