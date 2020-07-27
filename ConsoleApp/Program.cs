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
                Console.WriteLine("Make your choice: Rock, Paper, or Scissors?");
                var userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")
                {
                    return;
                }
                Option playerChoice = RockPaperScissors.Api.RockPaperScissors.ValidateUserInput(userInput);
                if (playerChoice == Option.Invalid)
                {
                    Console.WriteLine("Your choice could not be understood. Please choose \"Rock\", \"Paper\", or \"Scissors\".");
                    continue;
                }
                var gameResult = RockPaperScissors.Api.RockPaperScissors.ProcessUserInput(new RockPaperScissors.Api.Models.Game { PlayerChoice = playerChoice });
                Console.WriteLine("Computer chose...");
                Console.WriteLine(gameResult.ComputerChoice);

                Console.WriteLine(gameResult.GameResult);
            } while (true);
        }
    }
}
