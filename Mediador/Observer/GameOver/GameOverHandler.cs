using System;

namespace Observer
{
    public class GameOverHandler : IHandler<GameState>
    {
        public void OnEvent(GameState eventData)
        {
            Console.WriteLine("\nGame Over");
            Console.WriteLine($"\n{eventData.Word}");
            Console.WriteLine("\nPress ESC to leave...");
        }
    }
}
 