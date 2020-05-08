using System;

namespace Observer
{
    public class GameWonHandler : IHandler<GameState>
    {
        public void OnEvent(GameState eventData)
        {
            Console.WriteLine("\nYou have WON!!!");
            Console.WriteLine("\nPress ESC to leave...");
        }
    }
}
 