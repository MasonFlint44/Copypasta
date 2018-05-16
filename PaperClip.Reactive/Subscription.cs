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

        public void Broadcast(T notification)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnNext(notification);
            }
        }

        public void BroadcastError(Exception error)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnError(error);
            }
        }

        public void EndBroadcast()
        {
            foreach(var subscriber in Subscribers)
            {
                subscriber.OnCompleted();
            }
        }
    }
}
