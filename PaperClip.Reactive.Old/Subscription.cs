using System;
using System.Collections.Generic;

namespace PaperClip.Reactive
{
    public class Subscription<T>: IObservable<T>
    {
        public readonly HashSet<IObserver<T>> Subscribers = new HashSet<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!Subscribers.Contains(observer))
            {
                Subscribers.Add(observer);
            }
            return new Unsubscriber<T>(Subscribers, observer);
        }
    }
}
