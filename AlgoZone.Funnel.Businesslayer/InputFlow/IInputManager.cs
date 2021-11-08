using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Businesslayer.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public interface IInputManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Subscribes to tick events for all symbols.
        /// </summary>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToAllSymbolTickerUpdates(Action<IEventData<IEnumerable<SymbolTick>>> onTick);

        /// <summary>
        /// Subscribes to the order book update event.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="onUpdate">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<IEventData<SymbolOrderBook>> onUpdate);

        /// <summary>
        /// Subscribes to the tick event of a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolTickerUpdates(string symbol, Action<IEventData<SymbolTick>> onTick);

        #endregion
    }
}