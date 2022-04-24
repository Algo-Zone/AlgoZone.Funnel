using System;
using System.Collections.Generic;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public interface IInputManager : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the exchange for this input manager.
        /// </summary>
        Exchange Exchange { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the trading pairs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SymbolTradingPairEventData> GetAllTradingPairs();

        /// <summary>
        /// Gets the candlestick with
        /// </summary>
        /// <param name="symbol">The symbol for which to get the candlesticks.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        ICollection<Candlestick> GetCandlesticks(string symbol, DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Gets the candlestick with
        /// </summary>
        /// <param name="symbol">The symbol for which to get the candlesticks.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="limit">The limit of the response. Max is 1000</param>
        /// <returns></returns>
        ICollection<Candlestick> GetCandlesticks(string symbol, DateTime startDateTime, int limit = 1000);

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