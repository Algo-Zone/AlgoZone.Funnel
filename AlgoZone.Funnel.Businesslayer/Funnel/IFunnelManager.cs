using System.Collections.Generic;

namespace AlgoZone.Funnel.Businesslayer.Funnel
{
    public interface IFunnelManager
    {
        #region Methods

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