using System;
using System.Collections.Generic;
using AlgoZone.Funnel.Businesslayer.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        bool SubscribeToAllSymbolTickerUpdates(Action<IEventData<IEnumerable<SymbolTick>>> onTick);

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<IEventData<SymbolOrderBook>> onUpdate);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<IEventData<SymbolTick>> onTick);

        #endregion
    }
}