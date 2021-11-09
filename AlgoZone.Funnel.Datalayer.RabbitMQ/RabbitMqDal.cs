using System;
using System.Threading.Tasks;
using EasyNetQ;

namespace AlgoZone.Funnel.Datalayer.RabbitMQ
{
    public class RabbitMqDal : IDisposable
    {
        #region Fields

        private readonly IBus _bus;

        #endregion

        #region Constructors

        public RabbitMqDal(string hostname)
        {
            _bus = RabbitHutch.CreateBus($"host={hostname}");
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            _bus.Dispose();
        }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        public async Task Publish<TMessageType>(TMessageType message)
        {
            await _bus.PubSub.PublishAsync(message);
        }

        #endregion
    }
}