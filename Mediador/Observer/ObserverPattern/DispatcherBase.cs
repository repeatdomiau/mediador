using System.Collections.Generic;

namespace Observer
{
    public abstract class DispatcherBase<TParam> : IDispatcher<IHandler<TParam>, TParam>
    {
        protected List<IHandler<TParam>> Listeners { get; set; } = new List<IHandler<TParam>>();

        public void NotifyAll(TParam info)
        {
            Listeners.ForEach(l => l.OnEvent(info));
        }

        public void Register(params IHandler<TParam>[] listeners)
        {
            Listeners.AddRange(listeners);
        }
    }
}
