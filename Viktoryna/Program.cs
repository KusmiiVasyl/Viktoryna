using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viktoryna
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Game.ShowFirstScreen();

            Game game = new Game();
            while (!game.IsExit)
            {
                game.VerifyLoginPassword();
            }
            Console.Clear();
            Console.WriteLine("\n\n\n*****BYE*****\n\n\n");
        }
    }
}
