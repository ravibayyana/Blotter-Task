using System;

namespace PriceSupplier
{
    public class PriceObserver: IObserver<decimal>
    {
        private readonly Action<decimal> onNext;
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;

        public PriceObserver(Action<decimal> onNext, Action onCompleted = null, Action<Exception> onError = null)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.onError = onError;
        }

        public void OnCompleted()
        {
            if (onCompleted != null)
            {
                this.onCompleted();
            }

            Console.WriteLine($"OnCompleted");
        }

        public void OnError(Exception e)
        {
            if (onError != null)
            {
                this.OnError(e);
            }

            Console.WriteLine($"OnError ErrorMsg: " + e.Message);
        }

        public void OnNext(decimal value)
        {
            onNext(value);
            Console.WriteLine($"OnNext Price: {value}");
        }
    }
}
