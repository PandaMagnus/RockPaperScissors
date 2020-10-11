using RockPaperScissors.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Api
{
    public static class RockPaperScissors
    {
        public static Option ValidatePlayerInput(string userInput)
        {
            if (userInput == null) userInput = "";

            return userInput.ToLower() switch
            {
                "rock" => Option.Rock,
                "r" => Option.Rock,
                "paper" => Option.Paper,
                "p" => Option.Paper,
                "scissors" => Option.Scissors,
                "scissor" => Option.Scissors,
                "s" => Option.Scissors,
                _ => Option.Invalid
            };
        }

        public static Game ProcessPlayerInput(Game game)
        {
            //Option userChoice = ValidateUserChoice(userInput);
            //if (userChoice == Option.Invalid)
            //{
            //    return "Invalid input. Please choose 'Rock', 'Paper', or 'Scissors'";
            //}
            // Probably extract this out into a couple of different APIs so that I have better control over this.
            // E.G. one API that calls validate input, one that calls for the computer decision, and one that processes the outcome
            //Console.WriteLine("Computer chose...");
            game.ComputerChoice = RandomComputerPick();
            //Console.WriteLine(computerChoice);
            Outcome didPlayerWin = DetermineIfPlayerWon(game.PlayerChoice, game.ComputerChoice);
            //ComputerOpponent.RecordOutcome(userChoice, didHumanWin);

            // Might make sense to move this logic into DetermineIfHumanWon
            if (didPlayerWin == Outcome.Indeterminate)
            {
                game.GameResult = "Unable to determine the victory. Please try again.";
                return game;
            }
            else if (didPlayerWin == Outcome.Draw)
            {
                game.GameResult = "A draw occurred. Try again!";
                return game;
            }
            else if (didPlayerWin == Outcome.Win)
            {
                game.GameResult = "Congratulations, you won!";
                return game;
            }
            else
            {
                game.GameResult = "Sorry, the computer won. Please try again!";
                return game;
            }
        }

        private static Option RandomComputerPick()
        {
            Random rand = new Random();
            var decision = rand.Next(3);
            return decision switch
            {
                0 => Option.Rock,
                1 => Option.Paper,
                2 => Option.Scissors,
                _ => Option.Rock,
            };
        }

        private static Outcome DetermineIfPlayerWon(Option playerChoice, Option computerChoice)
        {
            // Feels like this might be able to be done more efficiently

            switch (playerChoice)
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

        public static Option Decision()
        {
            OptionWeights = new Dictionary<Option, int> { { Option.Rock, 0 }, { Option.Paper, 0 }, { Option.Scissors, 0 } };
            // Eventually check for pattern recognition... E.G.:
            // If user plays R(W), then weight P + 1
            // If user plays R(W), R(W), then weight P + 2
            // If user playls R(L), then weight R + 1
            // If user plays R(L), R(L), then weight R + 2
            // If user plays R(W), S(L), R(W), then weight P + 1
            // If user plays R(W), S(L), R(L), then weight R + 2? Same as option 3?
            // What's the max sample size needed?

            //ComparePreviousGameAndAssignWeight();
            CheckForPatternsAndAssignWeight();
            //CheckForImmediateRepeatsAndAddWeight();
            //CheckForNumberOfPicksAndAssignWeight();

            List<KeyValuePair<Option, int>> results = OptionWeights.OrderByDescending(k => k.Value).ToList();

            if (results[0].Value == results[1].Value && results[1].Value == results[2].Value)
            {
                //Instead of this, how about assigning a small random value to each weight?
                return RandomComputerPick();
            }

            return results.First().Key;

            // If no pattern, and count > some value OR the last play was a loss...
            //if (GetLastResult().outcome == Outcome.Lose)
            //{
            //    return PickExpectedWinner(PickExpectedWinner(GetLastResult().userPick));
            //}

            //return RandomDecision();
        }

        public static void CheckForPatternsAndAssignWeight()
        {
            var segments = new List<List<(Option UserPick, Outcome Outcome)>>();
            for (int i = 0; i < PriorHumanChoices.Count; i += _IterationsToAnalyzeForPattern)
            {
                var group = new List<(Option UserChoice, Outcome Outcome)>();

                for (int j = 0; j < _IterationsToAnalyzeForPattern; j++)
                {
                    if (PriorHumanChoices.Count <= i + j)
                    {
                        break;
                    }
                    group.Add((PriorHumanChoices[i + j].UserPick, PriorHumanChoices[i + j].Outcome));
                }

                segments.Add(group);
            }

            // Another option: take most recent segment, and compare against each of the prior segments
            // Assign weights that way

            // Another option: write methods for most common strategies and randomly pick one eaach game
            // Common strategies: 
            // Pick whatever beats what just beat you and stay with winning pick
            // Pick the same as what just beat you

            // Iterate over segments and add weights based on the chunks of 3 likelihood for victory
            // Maybe increase weights the closer to the most recent game?
            // Apply weights to current iteration and make decision for next game

            // Could probably increase efficiency here by not re-analyzing games we've already analyzed.
            // Also, this disproportionally weights in favor of repeats instead of victories/losses
            int gamesAnalyzed = 1;
            foreach (var group in segments)
            {
                foreach (var r in group)
                {
                    gamesAnalyzed++;
                    OptionWeights[PickWinningOption(r.UserPick)] += 1;
                    if (r.Outcome == Outcome.Win)
                    {
                        OptionWeights[PickWinningOption(r.UserPick)] += (1 * gamesAnalyzed);
                    }
                    else if (r.Outcome == Outcome.Lose)
                    {
                        OptionWeights[PickWinningOption(PickWinningOption(r.UserPick))] += (1 * gamesAnalyzed);
                    }
                    // calculate repeat usage
                    // calculate last victory
                    // calculate average usage

                    // Assign small amount of randomness?
                }
            }





            //if(_PriorHumanChoices.Count == 0)
            //{
            //    return;
            //}

            //if(_PriorHumanChoices.Count < 3)
            //{
            //    if (_PriorHumanChoices.Last().Outcome == Outcome.Win)
            //    {
            //        _OptionWeights[PickExpectedWinner(_PriorHumanChoices.Last().UserPick)] += 2;
            //    }
            //    else if (_PriorHumanChoices.Last().Outcome == Outcome.Lose)
            //    {
            //        _OptionWeights[PickExpectedWinner(PickExpectedWinner(_PriorHumanChoices.Last().UserPick))] += 2;
            //    }
            //    return;
            //}

            // Probably break history down into chunks of four or five, and look for patterns that way instead of analyzing whole list?

        }

        private static Option PickWinningOption(Option userSelection)
        {
            return userSelection switch
            {
                Option.Rock => Option.Paper,
                Option.Paper => Option.Scissors,
                Option.Scissors => Option.Rock,
                _ => throw new ArgumentException("No expected user selection passed into "),
            };
        }

        public static void RecordOutcome(Option humanChoice, Outcome gameResult)
        {
            PriorHumanChoices.Add((humanChoice, gameResult));
        }

        private static Dictionary<Option, int> OptionWeights { get; set; }
        private static readonly int _IterationsToAnalyzeForPattern = 3;
        private static List<(Option UserPick, Outcome Outcome)> PriorHumanChoices { get; set; } = new List<(Option, Outcome)>();
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
