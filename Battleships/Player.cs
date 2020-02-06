using System;
using System.Collections.Generic;

namespace Battleships
{
    public class Player
    {
        public string PlayerName { get; }
        public bool HasLost { get; set; }
        public  BoardBlock[,] Board { get; }
        public  BoardBlock[,] AttackingBoard { get; }
        public List<Ship> PlayerShips { get; }
        private ISettings Settings { get; }
        private IInput Input { get; }
        

        public Player(string playerName, ISettings settings, IInput input)
        {
            PlayerName = playerName;
            Settings = settings;
            Input = input;
            Board = InitialiseBoard(settings.BoardSize);
            AttackingBoard = InitialiseBoard(settings.BoardSize);
            PlayerShips = CreateShips();
        }


        public void SetShips()
        {
            foreach (var ship in PlayerShips)
            {
                while (true)
                {
                    var (row, column, orientation) = GetValidUserShipInputCoordinates();
                    if (orientation == "H")
                    {
                        if (Utilities.CheckLengthAgainstBoardSize(row, Settings.BoardSize, ship.Length))
                        {
                            if (Utilities.CheckIfPositionIsEmpty(Board, ship, row,column, orientation))
                            {
                                for (var i = 0; i < ship.Length; i++)
                                {
                                    var position = Board[row + i, column];
                                    position.MarkerType = ship.MarkerCharacter;
                                }
                                break;
                            };
                        }
                    }
                
                    if (orientation == "V")
                    {
                        if (Utilities.CheckLengthAgainstBoardSize(column, Settings.BoardSize, ship.Length))
                        {
                            if (Utilities.CheckIfPositionIsEmpty(Board, ship, row,column, orientation))
                            {
                                for (var i = 0; i < ship.Length; i++)
                                {
                                    var position = Board[row, column + i];
                                    position.MarkerType = ship.MarkerCharacter;
                                }
                                break;
                            };
                        }
                    }      
                }
            }
        }
        
        private (int row, int column, string orientation) GetValidUserShipInputCoordinates()
        {
            try
            {
                var userCoordinates = Input.AskUserForShipPlacementCoordinates();
                if (userCoordinates == null) throw new Exception("Invalid coordinates");
                
                var coordinates = Array.ConvertAll(userCoordinates.ToCharArray(), char.ToString);
                if (coordinates[2] == "H" || coordinates[2] == "V")    
                    return (int.Parse(coordinates[0]), int.Parse(coordinates[1]), coordinates[2]);

                throw new Exception("Invalid coordinates");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
        private static BoardBlock[,] InitialiseBoard(int boardSize)
        {
            var board = new BoardBlock[boardSize, boardSize];
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                {
                    board[i,j] = new BoardBlock(i, j);   
                }
            }
            return board;
        }
        
        private static List<Ship> CreateShips()
        {
            return new List<Ship>
            {
                new PatrolBoat(),
                new Submarine(),
                new Destroyer(),
                new Battleship(),
                new Carrier(),
            };
        }
    }
}