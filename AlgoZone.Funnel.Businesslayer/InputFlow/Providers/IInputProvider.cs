using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow.Providers
{
    public interface IInputProvider : IDisposable
    {
        #region Methods

        bool SubscribeToSymbolTickerUpdates(string symbol, Action<EventData<SymbolTick>> onTick);

        #endregion
    }
}