using System;
using System.Collections.Generic;
using System.Linq;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;
using AlgoZone.Funnel.Businesslayer.Models;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public abstract class InputManager : IInputManager
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
        public abstract Exchange Exchange { get; }

        /// <inheritdoc />
        public IEnumerable<SymbolTradingPairEventData> GetAllTradingPairs()
        {
            return _inputProvider.GetAllTradingPairs();
        }

        /// <inheritdoc />
        public ICollection<Candlestick> GetCandlesticks(string symbol, DateTime startDateTime, DateTime endDateTime)
        {
            return _inputProvider.GetCandlesticks(symbol, startDateTime, endDateTime);
        }

        /// <inheritdoc />
        public ICollection<Candlestick> GetCandlesticks(string symbol, DateTime startDateTime, int limit = 1000)
        {
            return _inputProvider.GetCandlesticks(symbol, startDateTime, limit);
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