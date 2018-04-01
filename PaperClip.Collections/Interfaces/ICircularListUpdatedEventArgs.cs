namespace PaperClip.Collections.Interfaces
{
    public interface ICircularListUpdatedEventArgs<out T>
    {
        T AddedElement { get; }
        bool WasElementRemoved { get; }
        T RemovedElement { get; }
    }
}