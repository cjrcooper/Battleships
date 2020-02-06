using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Battleships.Tests
{
    [TestFixture]
    public class BattleshipsTest
    {
        [Test]
        [TestCase(10)]
        [TestCase(5)]
        [TestCase(3)]
        public void BattleshipsPlayerBoards_WhenInitialised_ShouldEqualTheSquareOfTheSettingsBoardSizeValue(int boardSize)
        {
            //arrange
            var mockSetting = SetupSettings(boardSize);
            var mockInput = SetupInput("12V");
            var battleship = new Battleships(mockSetting, mockInput);
            battleship.CreatePlayer("Chris");
            
            //act
            var player = battleship.GetPlayer("Chris");

            //assert
            player.Board.Should().HaveCount(boardSize * boardSize);
            player.AttackingBoard.Should().HaveCount(boardSize * boardSize);
        }
        
        [Test]
        [TestCase("Chris")]
        [TestCase("John")]
        public void BattleshipsGetPlayer_ShouldReturnPlayer_IfThatPlayerWasCreated(string playerName)
        {
            //arrange
            BattleshipsSetup(out var battleship);
            battleship.CreatePlayer(playerName);
            
            //act
            var result = battleship.GetPlayer(playerName);

            //assert
            result.PlayerName.Should().Be(playerName);
        }
        
        [Test]
        [TestCase("Chris")]
        public void BattleshipsGetPlayer_ShouldThrowException_IfThatPlayerDoesNotExist(string playerName)
        {
            //arrange
            BattleshipsSetup(out var battleship);
            battleship.CreatePlayer(playerName);
            
            //act
            Action act = () => battleship.GetPlayer("John");

            //assert
            act.Should().Throw<Exception>().WithMessage("Unable to find player John");
        }
        
        [Test]
        [TestCase("Chris")]
        public void BattleshipsGetOpponent_ShouldThrowException_IfThatAnOpponentDoesNotExist(string playerName)
        {
            //arrange
            BattleshipsSetup(out var battleship);
            battleship.CreatePlayer(playerName);
            
            //act
            Action act = () => battleship.GetOpponentOf("Chris");

            //assert
            act.Should().Throw<Exception>().WithMessage("Sequence contains no matching element");
        }

        [Test]
        [TestCase("P", 2)]
        [TestCase("S", 3)]
        [TestCase("D", 3)]
        [TestCase("B", 4)]
        [TestCase("C", 5)]
        public void BattleshipsPlayerShipSetUp_WhenSettingThePlayerShipsOnTheBoard_MarkTypesCountShouldEqualShipLength
            (string battleshipMarker, int shipLength)
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            var battleships = new Battleships(mockSetting, mockInput.Object);
            const string mockPlayerName = "Chris";
            battleships.CreatePlayer(mockPlayerName);

            //act
            var player = battleships.GetPlayer(mockPlayerName);
            player.SetShips();
            
            //assert
            var result = player.Board.Cast<BoardBlock>().Count(boardBlock => boardBlock.MarkerType == battleshipMarker);
            result.Should().Be(shipLength);
        }

        [Test]
        public void
            BattleshipsPlayerShipSetUp_WhenSettingThePlayerShipsOnTheBoard_PlayerShouldHaveMarkersEqualToTheNumberOfShipsAndTheirLength()
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            var battleships = new Battleships(mockSetting, mockInput.Object);
            const string mockPlayerName = "Chris";
            battleships.CreatePlayer(mockPlayerName);

            //act
            var player = battleships.GetPlayer(mockPlayerName);
            player.SetShips();
            var result = player.Board.Cast<BoardBlock>().Count(boardBlock => boardBlock.MarkerType == "E");

            
            //assert
            result.Should().Be(player.Board.Length - CountShips(player));
        }
        
        [Test]
        public void
            BattleshipsPlayerStartGame_WhenInvoked_ShouldThrowExceptionsIfNoPlayersHaveBeenCreated()
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            var battleships = new Battleships(mockSetting, mockInput.Object);

            //act
            Action act = () => battleships.StartGame();
            
            //assert
            act.Should().Throw<Exception>().WithMessage("No players created");
        }
        
        [Test]
        public void
            BattleshipsPlayerStartGame_WhenInvoked_ShouldThrowExceptionsIfOnlyOnePlayerHasBeenCreated()
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            var battleships = new Battleships(mockSetting, mockInput.Object);
            const string mockPlayerName = "Chris";
            battleships.CreatePlayer(mockPlayerName);

            //act
            Action act = () => battleships.StartGame();
            
            //assert
            act.Should().Throw<Exception>().WithMessage("Not enough players created");
        }
        
        [Test]
        public void
            BattleshipsPlayerStartGame_WhenInvoked_ShouldThrowExceptionsIfTooMany()
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            var battleships = new Battleships(mockSetting, mockInput.Object);
            const string mockPlayerName = "Chris";
            battleships.CreatePlayer(mockPlayerName);
            
            const string mockPlayerNameTwo = "John";
            battleships.CreatePlayer(mockPlayerNameTwo);
            
            const string mockPlayerNameThree = "Bill";
            battleships.CreatePlayer(mockPlayerNameThree);

            //act
            Action act = () => battleships.StartGame();
            
            //assert
            act.Should().Throw<Exception>().WithMessage("Too many players add");
        }
        
        [Test]
        public void BattleshipsGameStart_WhenStartingGame_AWinnerIsFoundAndSet()
        {
            //arrange
            var mockSetting = SetupSettings(10);
            var mockInput = new Mock<IInput>();
            mockInput.Setup(x => x.AskUserForShipPlacementCoordinates())
                .Returns(() => GenerateMockSetUpShipPlacementCoordinates());
            mockInput.Setup(x => x.AskUserForAttackingCoordinates())
                .Returns(() => GenerateMockSetUpAttackingCoordinates());
            
            var battleships = new Battleships(mockSetting, mockInput.Object);
            
            const string mockPlayerName = "Chris";
            const string mockOpponentName = "John";
            
            battleships.CreatePlayer(mockPlayerName);
            battleships.CreatePlayer(mockOpponentName);
            
            //act
            var player = battleships.GetPlayer(mockPlayerName);
            var opponent = battleships.GetPlayer(mockOpponentName);
            
            player.SetShips();
            opponent.SetShips();
            battleships.StartGame();
            
            //assert
            battleships.GetWinner().Should().NotBe(null);
            battleships.GetWinner().Should().BeOneOf("Chris", "John", "Draw");
        }
        
        private static void BattleshipsSetup(out Battleships battleship)
        {
            var mockSetting = SetupSettings(10);
            var mockInput = SetupInput("12V");
            battleship = new Battleships(mockSetting, mockInput);
        }

        private static int CountShips(Player player)
        {
            return player.PlayerShips.Sum(ships => ships.Length);
        }

        private static string GenerateMockSetUpShipPlacementCoordinates()
        {
            return Utilities.GenerateRandomShipPlacementCoordinates(10);
        }
        
        private static string GenerateMockSetUpAttackingCoordinates()
        {
            return Utilities.GenerateRandomBoardAttackingCoordinates(10);
        }
        
        private static FakeSettings SetupSettings(int boardSize)
        {
            return new FakeSettings
            {
                BoardSize = boardSize,
            };
        }
        
        private static FakeInput SetupInput(string str)
        {
            return new FakeInput(str);
        }   

        private class FakeSettings : ISettings
        {
            public int BoardSize { get; set; }
            public bool Ai { get; set; }
        }

        private class FakeInput : IInput
        {
            private readonly string _input;

            public FakeInput(string input)
            {
                _input = input;
            }
            
            public string AskUserForShipPlacementCoordinates()
            {
                return _input;
            }

            public string AskUserForAttackingCoordinates()
            {
                return _input;
            }
        }
    }
}