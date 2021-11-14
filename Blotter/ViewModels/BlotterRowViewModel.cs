using Blotter.Utils;
using PriceSupplier;
using System;

namespace Blotter.ViewModels
{
    public class BlotterRowViewModel : PropertyChangedBase
    {
        private PriceSource priceSource;
        private string currencyPair = string.Empty;
        private decimal? price;
        private string error;
        private readonly IPriceSourceFactory priceSourceFactory;
        private readonly SubscriptionType subscriptionType;

        public BlotterRowViewModel(
            string ccypair,
            IPriceSourceFactory priceSourceFactory,
            SubscriptionType subscriptionType)
        {
            this.priceSourceFactory = priceSourceFactory;
            this.subscriptionType = subscriptionType;
            CurrencyPair = ccypair;
        }

        IDisposable priceDisposable;

        private void DisposeSubscription()
        {
            if (subscriptionType == SubscriptionType.Rx)
            {
                priceDisposable?.Dispose();
                priceDisposable = null;
                return;
            }

            if (priceSource != null)
                priceSource.PriceUpdate -= OnPriceUpdated;
        }

        private void SubscibeToPriceEventUpdates()
        {
            DisposeSubscription();
            Error = string.Empty;

            priceSource = priceSourceFactory.GetSource(CurrencyPair);
            if (priceSource == null)
            {
                Price = null;
                Error = $"Invalid Currency Pair: '{CurrencyPair}'";
                return;
            }

            if (subscriptionType == SubscriptionType.Rx)
            {
                priceDisposable = priceSource.Subscribe(new PriceObserver(
                    value => Price = value, 
                    () => Console.WriteLine($"'{CurrencyPair} Pricer Stream' ****COMPLETED***"),
                    (e) => Console.WriteLine($"'{CurrencyPair}' Price Stream error ErrorMSg: {e?.Message}")));

                return;
            }

            priceSource.PriceUpdate += OnPriceUpdated;
        }

        private void OnPriceUpdated(object sender, EventArgs e)
        {
            Price = priceSource.Price;
        }

        public string CurrencyPair
        {
            get => currencyPair;
            set
            {
                currencyPair = value;
                OnPropertyChanged(nameof(CurrencyPair));
                SubscibeToPriceEventUpdates();
            }
        }

        public decimal? Price
        {
            get => price;
            private set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public string Error
        {
            get => error;
            private set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
    }
}
