using System;
using System.Threading;
using AlgoZone.Funnel.Businesslayer.Funnel;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Exceptions;
using AlgoZone.Funnel.Model;
using CommandLine;

namespace AlgoZone.Funnel
{
    public static class Program
    {
        #region Fields

        private static readonly ManualResetEvent _quitEvent = new ManualResetEvent(false);

        #endregion

        #region Methods

        #region Static Methods

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            IInputManager inputManager = null;
            try
            {
                Parser.Default.ParseArguments<CommandLineOptions>(args)
                      .WithParsed(o =>
                      {
                          var exchange = GetExchange(o.Exchange);
                          if (exchange == Exchange.Unknown || string.IsNullOrWhiteSpace(o.Exchange))
                              throw new NoExchangeProvidedException(o.Exchange);

                          inputManager = GetInputManager(exchange);
                          if (inputManager == null)
                              throw new Exception();

                          var funnelManager = new FunnelManager(inputManager);

                          if (o.AllSymbols)
                              funnelManager.RunFunnel();
                          else
                              funnelManager.RunFunnel(o.Symbols);
                      });
            }
            catch (NoExchangeProvidedException e0)
            {
                Console.WriteLine($"No exchange availabile for input: {e0.ExchangeInput}");
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong. Please try again later");
            }

            if (inputManager != null)
            {
                _quitEvent.WaitOne();

                inputManager?.Dispose();
            }
        }

        /// <summary>
        /// Gets the exchange based on it's name.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <returns></returns>
        private static Exchange GetExchange(string exchangeName)
        {
            if (Constants.BinanceNames.Contains(exchangeName.ToLower()))
                return Exchange.Binance;

            return Exchange.Unknown;
        }

        /// <summary>
        /// Gets the input manager for an exchange.
        /// </summary>
        /// <param name="exchange">The exchange for which to get the input manager.</param>
        /// <returns></returns>
        private static IInputManager GetInputManager(Exchange exchange)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return new BinanceInputManager();
            }

            return new BinanceInputManager();
        }

        #endregion

        #endregion
    }
}