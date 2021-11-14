using System;

namespace PriceSupplier
{
    public class ObserverInfo : IDisposable
    {
        public ObserverInfo(IObserver<decimal> observer)
        {
            this.Observer = observer;
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }

        public IObserver<decimal> Observer { get; }

        public void Dispose()
        {
            Observer.OnCompleted();
        }
    }
}
