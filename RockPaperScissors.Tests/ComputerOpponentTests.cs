using RockPaperScissors.Api;
using RockPaperScissors.Api.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class ComputerOpponentTests
    {
        [Fact]
        public void RandomDecisionChoosesEachOption()
        {
            var results = new List<Option>();
            var rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                Option randomPlayerPick = (Option)rand.Next(1, 3);
                results.Add(
                    Api.RockPaperScissors.ProcessPlayerInput(
                        new Game { PlayerChoice = randomPlayerPick })
                    .ComputerChoice
                );
            }

            Assert.Contains(results,
                p => p == Option.Rock);
            Assert.Contains(results,
                p => p == Option.Paper);
            Assert.Contains(results,
                p => p == Option.Scissors);
        }
    }
}
