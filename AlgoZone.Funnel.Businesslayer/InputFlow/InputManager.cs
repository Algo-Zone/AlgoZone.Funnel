﻿using System;
using AlgoZone.Funnel.Businesslayer.InputFlow.Models;
using AlgoZone.Funnel.Businesslayer.InputFlow.Providers;

namespace AlgoZone.Funnel.Businesslayer.InputFlow
{
    public class InputManager : IInputManager, IDisposable
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
        public bool SubscribeToSymbolOrderBookUpdates(string symbol, int interval, Action<EventData<SymbolOrderBook>> onUpdate)
        {
            return _inputProvider.SubscribeToSymbolOrderBookUpdates(symbol, interval, onUpdate);
        }

        /// <inheritdoc />
        public bool SubscribeToSymbolTickerUpdates(string symbol, Action<EventData<SymbolTick>> onTick)
        {
            return _inputProvider.SubscribeToSymbolTickerUpdates(symbol, onTick);
        }

        #endregion
    }
}