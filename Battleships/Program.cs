using System;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            ISettings settings = new Settings
            {
                BoardSize = 10
            };
            
            IInput input = new Input();
            
            var game = new Battleships(settings, input);
            
            game.CreatePlayer("Chris");
            game.CreatePlayer("John");

            var p1 = game.GetPlayer("Chris");
            var p2 = game.GetPlayer("John");

            p1.SetShips();
            p2.SetShips();
            
            game.StartGame();
        }
    }
}