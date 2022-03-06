using System;
using System.Threading.Tasks;
using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Datalayer.RabbitMQ;
using NLog;

namespace AlgoZone.Funnel.Businesslayer.OutputFlow
{
    public class OutputManager : IOutputManager
    {
        #region Fields

        private readonly RabbitMqDal _dal;
        
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public OutputManager()
        {
            _dal = new RabbitMqDal("rabbitmq.lan", "admin", "admin");
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
                _dal.Publish(eventData);
                return true;
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Fatal, e);
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
                _logger.Log(LogLevel.Fatal, e);
            }

            return false;
        }

        #endregion
    }
}