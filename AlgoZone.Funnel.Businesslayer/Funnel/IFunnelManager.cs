using System.Collections.Generic;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public interface IFunnelManager
    {
        #region Methods

        /// <summary>
        /// Imports all candlesticks for a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol for which to import data.</param>
        void ImportHistory(string symbol);

        /// <summary>
        /// Runs the funnel for specific symbols.
        /// </summary>
        /// <returns></returns>
        void RunFunnel(IEnumerable<string> symbols);

        /// <summary>
        /// Runs the funnel for all symbols.
        /// </summary>
        /// <returns></returns>
        void RunFunnel();

        /// <summary>
        /// Sets the correct exchange to use.
        /// </summary>
        /// <param name="exchange">The exchange.</param>
        void SetExchange(string exchange);

        #endregion
    }
}