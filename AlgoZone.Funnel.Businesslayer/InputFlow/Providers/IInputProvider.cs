using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<EventData<SymbolOrderBook>> onUpdate);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<EventData<SymbolTick>> onTick);

        #endregion
    }
}