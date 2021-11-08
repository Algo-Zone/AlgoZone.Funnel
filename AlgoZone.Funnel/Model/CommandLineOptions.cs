using System.Collections.Generic;
using CommandLine;

namespace AlgoZone.Funnel.Model
{
    public class CommandLineOptions
    {
        #region Properties

        [Option('a', "all", Required = false, HelpText = "Whether to get all the symbols")]
        public bool AllSymbols { get; set; }

        [Option('e', "exchange", Required = true, HelpText = "The exchange to use for this funnel")]
        public string Exchange { get; set; }

        [Option('s', "symbols", Required = false, HelpText = "The symbols for which to retrieve data")]
        public ICollection<string> Symbols { get; set; }

        #endregion
    }
}