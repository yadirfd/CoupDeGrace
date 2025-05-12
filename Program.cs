using System;
using lasthope;

namespace lasthope
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new CoupDeGrace();
            game.Run();
        }
    }
}