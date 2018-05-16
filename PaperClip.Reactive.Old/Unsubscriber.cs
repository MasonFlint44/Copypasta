using System;
using System.Collections.Generic;

namespace PaperClip.Reactive
{
    internal class Unsubscriber<T>: IDisposable
    {
        private readonly HashSet<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(HashSet<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            _observers.Remove(_observer);
        }
    }
}
