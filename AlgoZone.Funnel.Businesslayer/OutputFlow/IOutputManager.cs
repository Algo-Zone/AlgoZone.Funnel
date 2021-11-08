using System;
using AlgoZone.Funnel.Businesslayer.EventData;

namespace AlgoZone.Funnel.Businesslayer.OutputFlow
{
    public interface IOutputManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Publishes the event data.
        /// </summary>
        /// <param name="symbolEventData">The event data to publish.</param>
        /// <typeparam name="TSymbolData">The symbol data type.</typeparam>
        /// <returns></returns>
        bool PublishEvent<TSymbolData>(ISymbolEventData<TSymbolData> symbolEventData) where TSymbolData : ISymbolData;

        #endregion
    }
}