﻿using RockPaperScissors.Server;
using RockPaperScissors.Shared;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class RpsApiTests
    {
        [Theory]
        [InlineData("rock", Option.Rock)]
        [InlineData("ROCK", Option.Rock)]
        [InlineData("paper", Option.Paper)]
        [InlineData("PAPER", Option.Paper)]
        [InlineData("scissor", Option.Scissors)]
        [InlineData("SCISSOR", Option.Scissors)]
        [InlineData("scissors", Option.Scissors)]
        [InlineData("SCISSORS", Option.Scissors)]
        [InlineData("quit", Option.Invalid)]
        [InlineData("QUIT", Option.Invalid)]
        [InlineData("exit", Option.Invalid)]
        [InlineData("EXIT", Option.Invalid)]
        [InlineData("Blaaargh", Option.Invalid)]
        [InlineData("1234", Option.Invalid)]
        [InlineData("123abc", Option.Invalid)]
        [InlineData("abc123", Option.Invalid)]
        [InlineData(null, Option.Invalid)]
        [InlineData("", Option.Invalid)]
        [InlineData(" ", Option.Invalid)]
        public void ValidateUserChoice(string playerInput, Option expectedResult)
        {
            Assert.Equal(expectedResult, GameEngine.ValidatePlayerInput(playerInput));
        }

        [Fact]
        public void Test()
        {
            Random rand = new();
            int randPick = rand.Next(1, 3);
            Game g = GameEngine.ProcessPlayerInput(new Game { PlayerChoice = (Option)randPick });
            randPick = rand.Next(1, 3);
            g = GameEngine.ProcessPlayerInput(new Game { PlayerChoice = (Option)randPick });
            randPick = rand.Next(1, 3);
            g = GameEngine.ProcessPlayerInput(new Game { PlayerChoice = (Option)randPick });
            Assert.NotNull(g);
        }
    }
}
