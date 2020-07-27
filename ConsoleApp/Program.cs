using RockPaperScissors.Api;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                System.Threading.Tasks.Task.Delay(1000).Wait();
                Console.WriteLine("Make your choice: Rock, Paper, or Scissors?");
                var userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")
                {
                    return;
                }
                Option playerChoice = RockPaperScissors.Api.RockPaperScissors.ValidateUserInput(userInput);
                if (playerChoice == Option.Invalid)
                {
                    continue;
                }
                var gameResult = RockPaperScissors.Api.RockPaperScissors.ProcessUserInput(new RockPaperScissors.Api.Models.Game { PlayerChoice = playerChoice });
                Console.WriteLine("Computer chose...");
                Console.WriteLine(gameResult.ComputerChoice);
                //ComputerOpponent.RecordOutcome(playerChoice, didHumanWin);

                Console.WriteLine(gameResult.GameResult);
            } while (true);
        }
    }
}
