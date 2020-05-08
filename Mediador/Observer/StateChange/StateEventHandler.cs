using System;

namespace Observer
{
    public class StateEventHandler : IHandler<GameState>
    {
        
        public void OnEvent(GameState eventData)
        {
            Console.Clear();
            Console.WriteLine(DrawHangMan.GenerateHangMan(eventData.Attempts));
            Console.WriteLine();
            Console.Write("Word: " + String.Join("", eventData.Result));
            Console.Write(" | Failed: " + eventData.Attempts + "/" + eventData.MaxAttempts);
            Console.WriteLine(" | Tried: " + String.Join("", eventData.UsedLetters));
        }
    }
}
 