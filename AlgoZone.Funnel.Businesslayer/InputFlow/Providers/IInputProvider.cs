using System;
using AlgoZone.Core.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        bool SubscribeToAllSymbolTickerUpdates(Action<ISymbolEventData<SymbolTick>> onTick);

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<ISymbolEventData<SymbolOrderBook>> onUpdate);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<ISymbolEventData<SymbolTick>> onTick);

        #endregion
    }
}