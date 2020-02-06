using System;

namespace Battleships
{
    public interface IInput
    {
        string AskUserForShipPlacementCoordinates();
        string AskUserForAttackingCoordinates();
    }
    
    public class Input: IInput
    {
        public string AskUserForShipPlacementCoordinates()
        {
            return Console.ReadLine();
        }
        
        public string AskUserForAttackingCoordinates()
        {
            return Console.ReadLine();
        }
    }
}