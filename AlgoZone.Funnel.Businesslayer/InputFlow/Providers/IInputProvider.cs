using System;
using System.Collections.Generic;
using AlgoZone.Core.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        IEnumerable<string> GetAllSymbols();

        bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick);

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate);

        bool SubscribeToSymbolsCandlesticksOneMinute(IEnumerable<string> symbols, Action<SymbolCandlestickEventData> onCandlestick);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick);

        #endregion
    }
}