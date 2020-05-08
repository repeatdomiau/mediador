using System;

namespace Observer
{
    public struct GameKeyEventData
    {
        public ConsoleKeyInfo KeyInfo { get; set; }
        public GameState GameState { get; set; }
    }
}
 