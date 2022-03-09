using System;
using System.Collections.Generic;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public class InputManager : IInputManager
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

        /// <inheritdoc />
        public void Dispose()
        {
            _inputProvider.Dispose();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetAllSymbols()
        {
            return _inputProvider.GetAllSymbols();
        }

        /// <inheritdoc />
        public bool SubscribeToAllSymbolTickerUpdates(Action<SymbolTickEventData> onTick)
        {
            return _inputProvider.SubscribeToAllSymbolTickerUpdates(onTick);
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<SymbolOrderBookEventData> onUpdate)
        {
            return _inputProvider.SubscribeToSymbolOrderBookUpdates(symbol, interval, onUpdate);
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolsCandlesticksOneMinute(IEnumerable<string> symbols, Action<SymbolCandlestickEventData> onCandlestick)
        {
            return _inputProvider.SubscribeToSymbolsCandlesticksOneMinute(symbols, onCandlestick);
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<SymbolTickEventData> onTick)
        {
            return _inputProvider.SubscribeToSymbolTickerUpdates(symbol, onTick);
        }

        #endregion
    }
}