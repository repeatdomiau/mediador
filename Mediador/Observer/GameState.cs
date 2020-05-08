using System;
using System.Collections.Generic;

namespace Observer
{
    public class GameState
    {
        public string Word { get; set; }
        public char[] Letters { get; private set; }
        public char[] Result { get; private set; }
        public int Attempts { get; private set; }
        public bool Won { get; private set; }
        public int MaxAttempts { get; private set; }
        public bool GameOver { get; private set; }

        public List<char> UsedLetters { get; set; }

        public GameState(string word, int maxAttempts)
        {
            this.Word = word;
            this.Letters = word.ToCharArray();
            this.Result = new char[word.Length];
            this.UsedLetters = new List<char>();
            Array.Fill(this.Result, '_'); 
            this.Attempts = 0;
            this.Won = false;
            this.MaxAttempts = maxAttempts;
            this.GameOver = false;
        }

        public void Try(char c)
        {
            if (!this.Won && !this.GameOver)
            {
                if (!this.UsedLetters.Contains(c)) this.UsedLetters.Add(c);

                bool goodMove = false;
                
                for (int i = 0; i < this.Letters.Length; i++)
                {
                    if (this.Letters[i] == c)
                    {
                        this.Result[i] = c;
                        goodMove = true;
                    }
                }
                
                if (!goodMove) this.Attempts++;

                if (this.Attempts >= this.MaxAttempts) this.GameOver = true;

                if (String.Join("", this.Word) == String.Join("", this.Result))
                {
                    this.Won = true;
                }
            }
        }

    }
}
 