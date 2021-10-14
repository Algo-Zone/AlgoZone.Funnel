using System;
using System.Threading;
using AlgoZone.Funnel.Businesslayer.InputFlow;

namespace AlgoZone.Funnel
{
    public class Program
    {
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        
        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };
                
            var inputManager = new BinanceInputManager();
            inputManager.SubscribeToSymbolTickerUpdates("BTCUSDT", tick =>
            {
                Console.WriteLine($"{tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");
            });

            _quitEvent.WaitOne();
            
            inputManager.Dispose();
        }
    }
}