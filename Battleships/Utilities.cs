using System;

namespace Battleships
{
    public static class Utilities
    {
        public static bool CheckIfPositionIsEmpty(BoardBlock[,] board, Ship ship, int coordinateX, int coordinateY, string orientation)
        {
            for (var i = 0; i < ship.Length; i++)
            {
                switch (orientation)
                {
                    case "H":
                    {
                        var position = coordinateX + i;
                        if (board[position, coordinateY].MarkerType != "E") return false;
                        break;
                    }
                    case "V":
                    {
                        var position = coordinateY + i;
                        if (board[coordinateX, position].MarkerType != "E") return false;
                        break;
                    }
                }
            }
            return true;
        }
                
        public static bool CheckLengthAgainstBoardSize(int value, int boardSize, int additionalLengthValue = 0)
        {
            return value + additionalLengthValue < boardSize;
        }

        public static (int positionX, int positionY) ConvertCoordinates(string userCoordinates)
        {
            var coordinates = Array.ConvertAll(userCoordinates.ToCharArray(), char.ToString);
            return (int.Parse(coordinates[0]), int.Parse(coordinates[1]));
        } 
        
        public static string GenerateRandomShipPlacementCoordinates(int boardSize)
        {
            var rowRandomNumber = GetRandomBoardNumber(boardSize).ToString();
            var columnRandomNumber = GetRandomBoardNumber(boardSize).ToString();
            var orientationRandomNumber = GetRandomOrientation();
            return rowRandomNumber + columnRandomNumber + orientationRandomNumber;
        }
        
        public static string GenerateRandomBoardAttackingCoordinates(int boardSize)
        {
            var rowRandomNumber = GetRandomBoardNumber(boardSize).ToString();
            var columnRandomNumber = GetRandomBoardNumber(boardSize).ToString();
            return rowRandomNumber + columnRandomNumber;
        }

        private static string GetRandomOrientation()
        {
            var value = new Random().Next(1);
            return ConvertRandOrientationNumberToString(value);
        }

        private static int GetRandomBoardNumber(int boardSize)
        {
            return new Random().Next(boardSize - 1);
        }
        
        private static string ConvertRandOrientationNumberToString(int value)
        {
            switch (value)
            {
                case 0:
                {
                    return "H";
                }
                case 1:
                {
                    return "V";
                }
                default:
                {
                    throw new Exception("Unable to covert orientation number to a string value");
                }
            }
        }
    }
}