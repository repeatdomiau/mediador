namespace Observer
{
    public class GameStartHandler : IHandler<GameState>
    {
        private StateEventDispatcher stateChangeDispatcher;
        public GameStartHandler(StateEventDispatcher stateChangeDispatcher)
        {
            this.stateChangeDispatcher = stateChangeDispatcher;
        }

        public void OnEvent(GameState eventData)
        {
            this.stateChangeDispatcher.NotifyAll(eventData);
        }
    }
}
 