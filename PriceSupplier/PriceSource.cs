using System;
using System.Threading;
using System.Threading.Tasks;

namespace PriceSupplier
{
    public class PriceSource : IObservable<decimal>
    {
        private IPriceObserverManager  priceObserverManager = new PriceObserverManager();
        private readonly Random _rng = new Random();
        private Timer _timer;
        private readonly int _rounding = 4;
        public EventHandler PriceUpdate { get; set; }

        public decimal Price { get; set; }
        public string CurrencyPair { get; private set; }

        public PriceSource(string ccypair, decimal px)
        {
            if (ccypair.Equals("USDJPY", StringComparison.OrdinalIgnoreCase))
            {
                _rounding = 2;
            }

            CurrencyPair = ccypair;
            Price = px;

            Task.Factory.StartNew(UpdatePrice);
        }

        public IDisposable Subscribe(IObserver<decimal> observer)
        {
            return priceObserverManager.Register(observer);
        }

        private void UpdatePrice()
        {
            Price = Math.Round(Price * (1 + ((decimal) _rng.NextDouble() - 0.5m) / 100), _rounding);

            PriceUpdate?.Invoke(this, EventArgs.Empty);
            priceObserverManager.Notify(Price);

            _timer = new Timer(_ => UpdatePrice(), null, _rng.Next(50, 2000), int.MaxValue);
            Console.WriteLine($"{CurrencyPair} updated to {Price} at {DateTime.Now}");
        }
    }
}
