using System;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Datalayer.RabbitMQ;

namespace AlgoZone.Funnel.Businesslayer.OutputFlow
{
    public class OutputManager : IOutputManager
    {
        #region Fields

        private readonly RabbitMqDal _dal;

        #endregion

        #region Constructors

        public OutputManager()
        {
            _dal = new RabbitMqDal("localhost");
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _dal?.Dispose();
        }

        /// <inheritdoc />
        public bool PublishEvent<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            try
            {
                _dal.Publish(eventData).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong. Please try again later. {e}");
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> PublishEventAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            try
            {
                await _dal.PublishAsync(eventData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong. Please try again later. {e}");
            }

            return false;
        }

        #endregion
    }
}