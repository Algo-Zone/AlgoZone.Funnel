using System;
using System.Collections.Generic;
using AlgoZone.Core.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public interface IInputManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Gets all the trading pairs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SymbolTradingPairEventData> GetAllTradingPairs();

        /// <summary>
        /// Subscribes to tick events for all symbols.
        /// </summary>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick);

        /// <summary>
        /// Subscribes to the order book update event.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="onUpdate">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate);

        /// <summary>
        /// Subscribes to the candlestick events for a list of symbols.
        /// </summary>
        /// <param name="symbols">The symbol to subscribe to.</param>
        /// <param name="onCandlestick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolsCandlesticksOneMinute(IEnumerable<string> symbols, Action<SymbolCandlestickEventData> onCandlestick);

        /// <summary>
        /// Subscribes to the tick event of a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onTick">The event callback.</param>
        /// <returns></returns>
        bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick);

        #endregion
    }
}