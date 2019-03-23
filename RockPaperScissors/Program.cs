using System;

namespace RockPaperScissors
{
    public class Program
    {
        public static void Main(string[] args)
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
                Option userChoice = ValidateUserChoice(userInput);
                if (userChoice == Option.Invalid)
                {
                    continue;
                }
                Console.WriteLine("Computer chose...");
                Option computerChoice = ComputerOpponent.Decision();
                Console.WriteLine(computerChoice);
                Outcome didHumanWin = DetermineIfHumanWon(userChoice, computerChoice);
                ComputerOpponent.RecordOutcome(userChoice, didHumanWin);

                // Might make sense to move this logic into DetermineIfHumanWon
                if (didHumanWin == Outcome.Indeterminate)
                {
                    Console.WriteLine("Unable to determine the victory. Please try again.");
                }
                else if (didHumanWin == Outcome.Draw)
                {
                    Console.WriteLine("A draw occurred. Try again!");
                }
                else if (didHumanWin == Outcome.Win)
                {
                    Console.WriteLine("Congratulations, you won!");
                }
                else
                {
                    Console.WriteLine("Sorry, the computer won. Please try again!");
                }
            } while (true);
        }

        public static Option ValidateUserChoice(string choice)
        {
            if (choice == null)
                choice = string.Empty;

            switch (choice.ToLower())
            {
                case "rock":
                    return Option.Rock;
                case "paper":
                    return Option.Paper;
                case "scissors":
                    return Option.Scissors;
                case "scissor":
                    return Option.Scissors;
                default:
                    Console.WriteLine("Invalid choice.");
                    return Option.Invalid;
            }
        }



        //private static void ComparePreviousGameAndAssignWeight()
        //{
        //    if(_PriorHumanChoices.Last().Outcome == Outcome.Win)
        //    {
        //        _OptionWeights[PickExpectedWinner(GetLastResult().userPick)] += 2;
        //    }
        //    else if(_PriorHumanChoices.Last().Outcome == Outcome.Lose)
        //    {
        //        _OptionWeights[PickExpectedWinner(PickExpectedWinner(GetLastResult().userPick))] += 2;
        //    }
        //}


        //private static void CheckForImmediateRepeatsAndAddWeight()
        //{
        //    int numberOfRepeats = 0;
        //    for(int i = _PriorHumanChoices.Count - 2; i >= 0; i--)
        //    {
        //        if(_PriorHumanChoices[i].UserPick == _PriorHumanChoices.Last().UserPick)
        //        {
        //            numberOfRepeats++;
        //        }
        //    }
        //    switch (numberOfRepeats)
        //    {
        //        case 0:
        //            break;
        //        case 1:
        //            _OptionWeights[GetLastResult().userPick] += 1;
        //            break;
        //        case 2:
        //            _OptionWeights[GetLastResult().userPick] += 2;
        //            break;
        //        default:
        //            _OptionWeights[GetLastResult().userPick] += 3;
        //            break;
        //    }
        //}

        //private static void CheckForNumberOfPicksAndAssignWeight()
        //{
        //    var totalCount = _PriorHumanChoices.Count;
        //    var rockCount = _PriorHumanChoices.FindAll(c => c.UserPick == Option.Rock).Count;
        //    var paperCount = _PriorHumanChoices.FindAll(c => c.UserPick == Option.Paper).Count;
        //    var scissorsCount = _PriorHumanChoices.FindAll(c => c.UserPick == Option.Scissors).Count;
        //    // Probably could eventually do more logic around here by looking at how much more one option is picked over others
        //    if (rockCount > paperCount && rockCount > scissorsCount)
        //        _OptionWeights[Option.Rock] += 1;
        //    if (paperCount > rockCount && paperCount > scissorsCount)
        //        _OptionWeights[Option.Paper] += 1;
        //    if (scissorsCount > paperCount && scissorsCount > rockCount)
        //        _OptionWeights[Option.Scissors] += 1;
        //}



        //private static Option PickExpectedWinner(Option expectedUserSelection)
        //{
        //    switch (expectedUserSelection)
        //    {
        //        case Option.Rock:
        //            return Option.Paper;
        //        case Option.Paper:
        //            return Option.Scissors;
        //        case Option.Scissors:
        //            return Option.Rock;
        //        default:
        //            throw new ArgumentException("No expected user selection passed into ");
        //    }
        //}



        //private static (Option userPick, Outcome outcome) GetLastResult()
        //{
        //    if (_PriorHumanChoices.Count > 0)
        //    {
        //        return _PriorHumanChoices[_PriorHumanChoices.Count - 1];
        //    }
        //    throw new ArgumentException("Attempted to invoke GetLastResult() when there were no existing results");
        //}

        public static Outcome DetermineIfHumanWon(Option userChoice, Option computerChoice)
        {
            // Feels like this might be able to be done more efficiently

            switch (userChoice)
            {
                case Option.Rock:
                    if (computerChoice == Option.Rock)
                        return Outcome.Draw;
                    if (computerChoice == Option.Scissors)
                        return Outcome.Win;
                    if (computerChoice == Option.Paper)
                        return Outcome.Lose;
                    break;
                case Option.Paper:
                    if (computerChoice == Option.Rock)
                        return Outcome.Win;
                    if (computerChoice == Option.Scissors)
                        return Outcome.Lose;
                    if (computerChoice == Option.Paper)
                        return Outcome.Draw;
                    break;
                case Option.Scissors:
                    if (computerChoice == Option.Rock)
                        return Outcome.Lose;
                    if (computerChoice == Option.Scissors)
                        return Outcome.Draw;
                    if (computerChoice == Option.Paper)
                        return Outcome.Win;
                    break;
                default:
                    break;
            }
            return Outcome.Indeterminate;
        }
    }

    public enum Option
    {
        Invalid,
        Rock,
        Paper,
        Scissors
    }

    public enum Outcome
    {
        Indeterminate,
        Win,
        Lose,
        Draw
    }
}
