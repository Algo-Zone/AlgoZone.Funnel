using System;
using AlgoZone.Core.EventData;

namespace AlgoZone.Funnel.Businesslayer.OutputFlow
{
    public interface IOutputManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Publishes the event data.
        /// </summary>
        /// <param name="eventData">The event to send.</param>
        /// <returns></returns>
        bool PublishEvent<TEventData>(TEventData eventData) where TEventData : IEventData;

        #endregion
    }
}