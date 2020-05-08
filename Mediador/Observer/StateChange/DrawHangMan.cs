using System;

namespace Observer
{
    public class DrawHangMan
    {
        public static string GenerateHangMan(int step)
        {
            return step switch
            {
                0 => "_____\n|   |  \n|      \n|      \n|      ",
                1 => "_____\n|   |  \n|   O  \n|      \n|      ",
                2 => "_____\n|   |  \n|   O  \n|   |  \n|      ",
                3 => "_____\n|   |  \n|   O  \n|  /|  \n|      ",
                4 => "_____\n|   |  \n|   O  \n|  /|\\ \n|      ",
                5 => "_____\n|   |  \n|   O  \n|  /|\\ \n|  /   ",
                6 => "_____\n|   |  \n|   O  \n|  /|\\ \n|  / \\ ",
                _ => throw new ArgumentException("fora do intervalo."),
            };
        }
    }
}
 