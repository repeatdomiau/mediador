namespace Observer
{
    public interface IDispatcher<TListener, TParam> where TListener : IHandler<TParam>
    {
        void Register(params TListener[] listener);

        void NotifyAll(TParam info);
    }
}
