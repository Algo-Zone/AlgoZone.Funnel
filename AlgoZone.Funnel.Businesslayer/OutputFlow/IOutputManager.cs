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
        /// <param name="eventData">The event data to publish.</param>
        /// <typeparam name="TSymbolData">The symbol data type.</typeparam>
        /// <returns></returns>
        bool PublishEvent<TSymbolData>(IEventData<TSymbolData> eventData) where TSymbolData : ISymbolData;

        #endregion
    }
}