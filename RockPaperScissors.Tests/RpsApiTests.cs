﻿using RockPaperScissors.Api;
using System;
using System.Collections.Generic;
using System.Text;
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
            Assert.Equal(expectedResult, RockPaperScissors.Api.RockPaperScissors.ValidatePlayerInput(playerInput));
        }
    }
}
