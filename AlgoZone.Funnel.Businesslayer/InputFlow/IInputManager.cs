using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public interface IInputManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Subscribes to the order book update event.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="onUpdate">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<EventData<SymbolOrderBook>> onUpdate);

        /// <summary>
        /// Subscribes to the tick event of a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolTickerUpdates(string symbol, Action<EventData<SymbolTick>> onTick);

        #endregion
    }
}