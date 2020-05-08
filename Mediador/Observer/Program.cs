using System;
using Observer;

namespace Mediador.UI
{
    class Program
    {
        static void Main()
        {
            var state = GameStateFactory.CreateGameState();

            StateEventDispatcher stateEventDispatcher = new StateEventDispatcher();
            stateEventDispatcher.Register(new StateEventHandler());

            GameStartDispatcher gameStartDispatcher = new GameStartDispatcher();
            gameStartDispatcher.Register(new GameStartHandler(stateEventDispatcher));

            GameWonDispatcher gameWonDispatcher = new GameWonDispatcher();
            gameWonDispatcher.Register(new GameWonHandler());

            GameOverDispatcher gameOverDispatcher = new GameOverDispatcher();
            gameOverDispatcher.Register(new GameOverHandler());

            KeyEventDispatcher keyEventDispatcher = new KeyEventDispatcher();
            keyEventDispatcher.Register(new KeyEventHandler(stateEventDispatcher,
                                                         gameWonDispatcher,
                                                         gameOverDispatcher));

            bool stop = false;
            gameStartDispatcher.NotifyAll(state);

            while (!stop)
            {
                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape) stop = true;
                else keyEventDispatcher.NotifyAll(new GameKeyEventData { KeyInfo = info, GameState = state });
            }
        }
    }

}