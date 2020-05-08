namespace Observer
{
    public class KeyEventHandler : IHandler<GameKeyEventData>
    {
        private readonly StateEventDispatcher stateChangeDispatcher;
        private readonly GameWonDispatcher gameWonDispatcher;
        private readonly GameOverDispatcher gameOverDispatcher;

        public KeyEventHandler(
            StateEventDispatcher stateChangeDispatcher,
            GameWonDispatcher gameWonDispatcher,
            GameOverDispatcher gameOverDispatcher
            )
        {
            this.stateChangeDispatcher = stateChangeDispatcher;
            this.gameWonDispatcher = gameWonDispatcher;
            this.gameOverDispatcher = gameOverDispatcher;
        }

        public void OnEvent(GameKeyEventData eventData)
        {
            eventData.GameState.Try(eventData.KeyInfo.KeyChar);
            stateChangeDispatcher.NotifyAll(eventData.GameState);
            if (eventData.GameState.GameOver) this.gameOverDispatcher.NotifyAll(eventData.GameState);
            else if (eventData.GameState.Won) this.gameWonDispatcher.NotifyAll(eventData.GameState);
        }
    }
}
 