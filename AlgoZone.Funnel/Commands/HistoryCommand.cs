using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;
using AlgoZone.Funnel.Businesslayer.Funnel;

namespace AlgoZone.Funnel.Commands
{
    public sealed class HistoryCommand : Command
    {
        #region Fields

        private readonly IFunnelManager _funnelManager;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public HistoryCommand(IFunnelManager funnelManager) : base("history")
        {
            _funnelManager = funnelManager;

            AddOption(CreateExchangeOption());
            AddOption(CreateSymbolOption());

            Handler = CommandHandler.Create((string exchange, string symbol) =>
            {
                _funnelManager.SetExchange(exchange);

                if (!string.IsNullOrWhiteSpace(symbol))
                    _funnelManager.ImportHistory(symbol);
                
                System.Environment.Exit(0);
            });
        }

        #endregion

        #region Methods

        #region Static Methods

        private static Option CreateExchangeOption()
        {
            var option = new Option<string>(new[] { "--exchange", "-e" })
            {
                Name = "Exchange",
                Description = "Sets the exchange to use for price updates",
                IsRequired = true
            };
            return option;
        }

        private static Option CreateSymbolOption()
        {
            var option = new Option<string>(new[] { "--symbol", "-s" })
            {
                Name = "Symbol",
                Description = "Sets the symbol to use for import",
                IsRequired = true
            };
            return option;
        }

        #endregion

        #endregion
    }
}