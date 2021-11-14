using System;

namespace PriceSupplier
{
    public interface IPriceObserverManager
    {
        IDisposable Register(IObserver<decimal> observer);

        void Notify(decimal value);
    }
}
