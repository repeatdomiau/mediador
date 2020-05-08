namespace Observer
{
    public interface IHandler<T>
    {
        void OnEvent(T eventData);
    }
}
