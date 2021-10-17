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
                Console.WriteLine($"Tick: {tick.Data.BidPrice}:{tick.Data.AskPrice} {tick.Data.BidQuantity}:{tick.Data.AskQuantity}");
            });
            
            inputManager.SubscribeToSymbolOrderBookUpdates("BTCUSDT", 1000, orderBook =>
            {
                Console.WriteLine($"Order book: {orderBook.Data.Asks.Count}:{orderBook.Data.Bids.Count}");
            });

            _quitEvent.WaitOne();
            
            inputManager.Dispose();
        }
    }
}