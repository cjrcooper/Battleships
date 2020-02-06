using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships
{   
    public class Battleships
    {
        private readonly ISettings _settings;
        private readonly IInput _input;
        private readonly List<Player> _players;
        private string _winner; 

        public Battleships(ISettings settings, IInput input)
        {
            _settings = settings;
            _input = input;
            _players = new List<Player>();
        }
        
        public void StartGame()
        {
            CheckPlayerCount();

            var player1 = _players[0];
            var player2 = _players[1];
            
            while (!player1.HasLost && !player2.HasLost)
            {
                Turn(player1.PlayerName);
                Turn(player2.PlayerName);
            }
            
            SetWinner(GameResult(player1, player2));
        }

        public void CreatePlayer(string playerName)
        {
            try
            {
                _players.Add(new Player(playerName, _settings, _input));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Player GetPlayer(string playerName)
        {
            try
            {
               var player = _players.Find(p => p.PlayerName == playerName);
               if (player == null) throw new Exception($"Unable to find player {playerName}");
               return player;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

        public Player GetOpponentOf(string playerName)
        {
            try
            {
                var opponent = _players.First(p => p.PlayerName != playerName);
                return opponent;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public string GetWinner()
        {
            return _winner;
        }
        
        private void CheckPlayerCount()
        {
            if (_players.Count == 0)
                throw new Exception("No players created");
            if (_players.Count == 1) 
                throw new Exception("Not enough players created");
            if (_players.Count > 2)
                throw new Exception("Too many players add");
        }

        private void SetWinner(string player)
        {
            _winner = player;
        }

        private void Turn(string attackingPlayer)
        {
            var player = GetPlayer(attackingPlayer);
            var opponent = GetOpponentOf(attackingPlayer);
            var (rowPosition, columnPosition) = GetPlayerAttackingCoordinates(player);
            var marker = ConfirmHitOrMiss(opponent.Board[rowPosition, columnPosition].MarkerType);
            UpdatePlayerBoards(player, opponent, rowPosition, columnPosition, marker);
            CheckIfPlayerHasWon(opponent);
        }

        private (int positionX, int positionY) GetPlayerAttackingCoordinates(Player player)
        {
            (int positionRow, int positionColumn) attackingCoordinates;
            bool validCoordinates;
            do
            {
                attackingCoordinates = Utilities.ConvertCoordinates(_input.AskUserForAttackingCoordinates());
                validCoordinates = ConfirmCoordinatesAreNew(attackingCoordinates.positionRow,
                    attackingCoordinates.positionColumn, player);
            } while (!validCoordinates);
            return attackingCoordinates;
        }


        private static bool ConfirmCoordinatesAreNew(int rowPosition, int columnPosition, Player player)
        {
            return player.AttackingBoard[rowPosition, columnPosition].MarkerType == "E";
        }

        private static string ConfirmHitOrMiss(string marker)
        {
            return marker.Equals("E") ? "M" : "H";
        }


        private static void 
            UpdatePlayerBoards(Player player, Player opponent, int rowPosition, int columnPosition, string marker)
        {
            switch (marker)
            {
                case "H":
                {
                    player.AttackingBoard[rowPosition, columnPosition].MarkerType = "H";
                
                    var opponentMarker = opponent.Board[rowPosition, columnPosition].MarkerType;
                    var ship = opponent.PlayerShips.Find(x => x.MarkerCharacter == opponentMarker );
                    ship.UpdateHits();
                    CheckForSunkBattleShips(ship);
                    break;
                }
                case "M":
                    player.AttackingBoard[rowPosition, columnPosition].MarkerType = "M";
                    break;
            }
        }


        private static void CheckForSunkBattleShips(Ship ship)
        {
            if (ship.Length != ship.Hits || ship.Sunk) return;
            ship.Sunk = true;
        }

        private static void CheckIfPlayerHasWon(Player opponent)
        {
            if (opponent.PlayerShips.All(x => x.Sunk))
            {
                opponent.HasLost = true;
            }
        }

        private static string GameResult(Player player1, Player player2)
        {
            if (player1.HasLost) 
                return player2.PlayerName;
            if (player2.HasLost)
                return player1.PlayerName;
            return "Draw";
        }
    }
}