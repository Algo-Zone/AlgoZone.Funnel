using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;
using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public class InputManager : IDisposable
    {
        #region Fields

        private readonly IInputProvider _inputProvider;

        #endregion

        #region Constructors

        protected InputManager(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        #endregion

        #region Methods

        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<EventData<SymbolTick>> onTick)
        {
            return _inputProvider.SubscribeToSymbolTickerUpdates(symbol, onTick);
        }

        #endregion

        /// <inheritdoc />
        public void Dispose()
        {
            _inputProvider.Dispose();
        }
    }
}