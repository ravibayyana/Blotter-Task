using System.Collections.Generic;

namespace PriceSupplier
{
    public class PriceSourceFactory : IPriceSourceFactory
    {
        private readonly Dictionary<string, decimal> availableCurrencyPairs;
        private readonly Dictionary<string, PriceSource> priceSourceManager;

        public PriceSourceFactory()
        {
            priceSourceManager = new Dictionary<string, PriceSource>();
            availableCurrencyPairs = new Dictionary<string, decimal>
            {
                {"AUDUSD", 0.73835m},
                {"EURUSD", 1.1985m},
                {"GBPUSD", 1.3472m},
                {"USDCAD", 1.3003m},
                {"USDCHF", 0.9052m},
                {"USDJPY", 105.845m},
                {"USDNOK", 8.7062m},
                {"USDNZD", 0.6764m},
                {"USDSEK", 8.6407m}
            };
        }

        private bool IsCcyValid(string ccyPair)
        {
            return availableCurrencyPairs.ContainsKey(ccyPair);
        }

        public PriceSource GetSource(string ccyPair)
        {
            if (string.IsNullOrEmpty(ccyPair))
                return null;

            ccyPair = ccyPair.ToUpper();
            if (!IsCcyValid(ccyPair))
            {
                return null;
            }

            if (!priceSourceManager.ContainsKey(ccyPair))
            {
                priceSourceManager.Add(ccyPair, new PriceSource(ccyPair, availableCurrencyPairs[ccyPair]));
            }

            return priceSourceManager[ccyPair];
        }
    }
}
