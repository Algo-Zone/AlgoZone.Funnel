using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using AlgoZone.Funnel.Businesslayer.Funnel;

namespace AlgoZone.Funnel.Commands
{
    public sealed class RunCommand : Command
    {
        private IFunnelManager _funnelManager;
        
        #region Constructors

        /// <inheritdoc />
        public RunCommand(IFunnelManager funnelManager) : base("run")
        {
            _funnelManager = funnelManager;
            
            AddOption(CreateExchangeOption());
            AddOption(CreateSymbolOption());

            Handler = CommandHandler.Create((string exchange, string symbols) =>
            {
                _funnelManager.SetExchange(exchange);
                
                if(string.IsNullOrWhiteSpace(symbols))
                    _funnelManager.RunFunnel();
                else
                    _funnelManager.RunFunnel(symbols.Split(','));
            });
        }

        #endregion

        #region Methods

        #region Static Methods

        private static Option CreateSymbolOption()
        {
            var option = new Option<string>(new[] { "--symbols", "-s" })
            {
                Name = "Symbols",
                Description = "Sets the symbols to use comma seperated",
                IsRequired = false
            };
            return option;
        }

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

        #endregion

        #endregion
    }
}