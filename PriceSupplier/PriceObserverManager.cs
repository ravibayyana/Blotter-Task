using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceSupplier
{
    public class PriceObserverManager : IPriceObserverManager
    {
        public Dictionary<string, ObserverInfo> observersDictionary;
        private object lockDictionary;

        public PriceObserverManager()
        {
            this.observersDictionary = new Dictionary<string, ObserverInfo>();
            lockDictionary = new object();
        }

        private void AddInternal(ObserverInfo info)
        {
            lock (this.lockDictionary)
            {
                observersDictionary.Add(info.Id, info);
            }
        }

        private void RemoveInternal(string id)
        {
            lock (this.lockDictionary)
            {
                if (!observersDictionary.ContainsKey(id))
                    return;

                var observer = observersDictionary[id];
                observer.Dispose();
                observersDictionary.Remove(id);
            }
        }

        public void Notify(decimal value)
        {
            lock (this.lockDictionary) 
            {
                observersDictionary.Values.ToList()
                    .ForEach(v => v.Observer.OnNext(value));
            }
        }

        public IDisposable Register(IObserver<decimal> observer)
        {
            var info  = new ObserverInfo(observer);
            this.AddInternal(info);

            return new UnRegisterPriceObserver(RemoveInternal, info.Id);
        }

        private class UnRegisterPriceObserver : IDisposable
        {            
            private readonly Action<string> action;
            private readonly string id;
            
            public UnRegisterPriceObserver(Action<string> action, string id)
            {
                this.action = action;
                this.id = id;
            }

            public void Dispose()
            {
                this.action(this.id);
            }
        }
    }
}
