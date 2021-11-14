using Blotter.Utils;
using PriceSupplier;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blotter.ViewModels
{
    public class MainWindowViewModel: PropertyChangedBase
    {
        private IPriceSourceFactory priceSourceFactory;

        public ObservableCollection<BlotterRowViewModel> EventSubscriptionItems { get; set; }

        public ObservableCollection<BlotterRowViewModel> RxSubscriptionItems { get; set; }

        private List<string> ccyPairs;
        public MainWindowViewModel()
        {
            this.priceSourceFactory = new PriceSourceFactory();
            this.EventSubscriptionItems = new ObservableCollection<BlotterRowViewModel>();
            this.RxSubscriptionItems = new ObservableCollection<BlotterRowViewModel>();

            this.ccyPairs = new List<string>
            {
                "EURUSD",
                "GBPUSD",
                "USDJPY",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };

            this.InitializeItems();
        }

        private void InitializeItems()
        {
            ccyPairs.ForEach(ccy =>
            {
                EventSubscriptionItems.Add(new BlotterRowViewModel(ccy, priceSourceFactory, SubscriptionType.Event));
                RxSubscriptionItems.Add(new BlotterRowViewModel(ccy, priceSourceFactory, SubscriptionType.Rx));
            });
        }
    }
}
