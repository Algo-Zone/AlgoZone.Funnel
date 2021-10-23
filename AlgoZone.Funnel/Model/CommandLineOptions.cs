using System.Collections.Generic;
using CommandLine;

namespace AlgoZone.Funnel.Model
{
    public class CommandLineOptions
    {
        #region Properties

        [Option('e', "exchange", Required = true, HelpText = "The exchange to use for this funnel")]
        public string Exchange { get; set; }

        [Option('s', "symbols", Required = true, HelpText = "The symbols for which to retrieve data")]
        public ICollection<string> Symbols { get; set; }

        #endregion
    }
}