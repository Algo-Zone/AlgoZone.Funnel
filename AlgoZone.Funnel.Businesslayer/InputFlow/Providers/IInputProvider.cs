using System;
using AlgoZone.Core.EventData;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick);

        bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate);

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick);

        #endregion
    }
}