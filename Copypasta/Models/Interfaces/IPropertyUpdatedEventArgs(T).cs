namespace Copypasta.Models.Interfaces
{
    public interface IPropertyUpdatedEventArgs<out T>
    {
        T OldValue { get; }
        T NewValue { get; }
    }
}