using System;
using System.Collections.Generic;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

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

        bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick);

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate);

        bool SubscribeToSymbolsCandlesticksOneMinute(IEnumerable<string> symbols, Action<SymbolCandlestickEventData> onCandlestick);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick);

        #endregion
    }
}