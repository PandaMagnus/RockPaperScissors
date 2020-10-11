using RockPaperScissors.Api;
using RockPaperScissors.Api.Controllers;
using RockPaperScissors.Api.Models;
using System;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            do
            {
                Console.WriteLine("Make your choice: Rock, Paper, or Scissors?");
                var userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")
                {
                    return;
                }
                RockPaperScissorsController controller = new RockPaperScissorsController();
                Game gameResult = controller.ValidateChoiceAsync(userInput);
                if (!gameResult.IsPlayerSelectionValid)
                {
                    Console.WriteLine(gameResult.ErrorMessage);
                    continue;
                }
                gameResult = controller.SendChoiceAsync(gameResult);
                Console.WriteLine("Computer chose...");
                Console.WriteLine(gameResult.ComputerChoice);

                Console.WriteLine(gameResult.GameResult);
            } while (true);
        }
    }
}
