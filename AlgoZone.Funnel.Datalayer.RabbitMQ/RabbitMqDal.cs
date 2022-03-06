using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using NLog;

namespace AlgoZone.Funnel.Datalayer.RabbitMQ
{
    public class RabbitMqDal : IDisposable
    {
        #region Fields

        private readonly IBus _bus;
        
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public RabbitMqDal(string hostname, string username, string password)
        {
            _bus = RabbitHutch.CreateBus($"host={hostname};username={username};password={password};publisherConfirms=true;timeout=10");
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
        public void Publish<TMessageType>(TMessageType message)
        {
            _bus.PubSub.Publish(message);
        }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        public async Task PublishAsync<TMessageType>(TMessageType message)
        {
            await _bus.PubSub.PublishAsync(message)
                      .ContinueWith(task =>
                      {
                          if (task.IsFaulted)
                              _logger.Fatal(task.Exception);
                      });
        }

        #endregion
    }
}