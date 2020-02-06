namespace Battleships
{
    public class Ship
    {
        public string MarkerCharacter { get; protected set; }
        public string Description { get; protected set; }
        public int Length { get; protected set; }   
        public int Hits { get; private set; }
        public bool Sunk { get; set; }

        public void UpdateHits()
        {
            Hits = Hits + 1;
        }
    }
    
    public class PatrolBoat : Ship
    {
        public PatrolBoat()
        {
            MarkerCharacter = "P";
            Description = "Patrol Boat";
            Length = 2;
        }
    }
    
    public class Submarine : Ship
    {
        public Submarine()
        {
            MarkerCharacter = "S";
            Description = "Submarine";
            Length = 3;
        }
    }
    
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            MarkerCharacter = "D";
            Description = "Destroyer";
            Length = 3;
        }
    }
    
    public class Battleship : Ship
    {
        public Battleship()
        {
            MarkerCharacter = "B";
            Description = "Battleship";
            Length = 4;
        }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            MarkerCharacter = "C";
            Description = "Carrier";
            Length = 5;
        }
    }
    
    
}