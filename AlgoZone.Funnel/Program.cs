﻿using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading;
using AlgoZone.Funnel.Businesslayer.Enums;
using AlgoZone.Funnel.Businesslayer.Funnel;
using AlgoZone.Funnel.Businesslayer.InputFlow;
using AlgoZone.Funnel.Businesslayer.OutputFlow;
using AlgoZone.Funnel.Commands;
using AlgoZone.Funnel.Exceptions;
using LightInject;
using NLog;
using LogLevel = NLog.LogLevel;

namespace AlgoZone.Funnel
{
    public static class Program
    {
        #region Fields

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        #endregion

        #region Methods

        #region Static Methods

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            try
            {
                var container = BuildServiceContainer();
                var parser = BuildParser(container);
                parser.Invoke(args);
            }
            catch (NoExchangeProvidedException e0)
            {
                Console.WriteLine($"No exchange available for input: {e0.ExchangeInput}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong. Please try again later");
                _logger.Log(LogLevel.Fatal, e);
            }

            QuitEvent.WaitOne();
        }

        private static Parser BuildParser(IServiceFactory services)
        {
            var rootCommand = new RootCommand();

            rootCommand.Name = "funnel";
            rootCommand.Description = "Commands to run funnel";

            foreach (var command in services.GetAllInstances<Command>())
                rootCommand.AddCommand(command);

            return new CommandLineBuilder(rootCommand).UseDefaults().Build();
        }

        private static ServiceContainer BuildServiceContainer()
        {
            var container = new ServiceContainer();

            container.RegisterSingleton<IInputManager, BinanceInputManager>(Exchange.Binance.ToString());
            container.RegisterSingleton<IOutputManager, OutputManager>();
            container.RegisterSingleton<IFunnelManager, FunnelManager>();
            container.RegisterSingleton<RunCommand>();

            return container;
        }

        #endregion

        #endregion
    }
}