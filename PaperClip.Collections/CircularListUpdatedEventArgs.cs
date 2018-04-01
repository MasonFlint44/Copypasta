using PaperClip.Collections.Interfaces;

namespace PaperClip.Collections
{
    public class CircularListUpdatedEventArgs<T>: ICircularListUpdatedEventArgs<T>
    {
        public T AddedElement { get; }
        public bool WasElementRemoved { get; }
        public T RemovedElement { get; }

        public CircularListUpdatedEventArgs(T addedElement, bool wasElementRemoved, T removedElement)
        {
            AddedElement = addedElement;
            WasElementRemoved = wasElementRemoved;
            RemovedElement = removedElement;
        }
    }
}
