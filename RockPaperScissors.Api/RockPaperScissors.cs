using RockPaperScissors.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Api
{
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

    public static class RockPaperScissors
    {
        private static Dictionary<Option, int> _OptionWeights = new Dictionary<Option, int> { { Option.Rock, 0 }, { Option.Paper, 0 }, { Option.Scissors, 0 } };
        private static readonly int _IterationsToAnalyzeForPattern = 3;
        private static List<(Option PlayerPick, Outcome Outcome)> _PriorHumanChoices = new List<(Option, Outcome)>();
        private static List<List<(Option PlayerPick, Outcome Outcome)>> _TrainingModel = new List<List<(Option PlayerPick, Outcome Outcome)>>();
        //private static List<(Option PlayerPick, Outcome Outcome)[]> _TrainingModel = new List<(Option PlayerPick, Outcome Outcome)[]>();

        // Probably extract these adds out for clarity
        // Also figure out how to configure this for games that last for other than 3 turns
        // Also find a better way to do this other than hard coding. Maybe play something like 1,000 random choice games?
        static RockPaperScissors()
        {
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Draw),
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Draw),
                    (Option.Paper, Outcome.Draw),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Draw),
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Draw),
                    (Option.Scissors, Outcome.Draw),
                    (Option.Rock, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Win)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Draw),
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose)
                }
            );
            _TrainingModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Draw),
                    (Option.Rock, Outcome.Draw),
                    (Option.Paper, Outcome.Win)
                }
            );
        }

        public static Option ValidatePlayerInput(string userInput)
        {
            if (userInput is null) userInput = "";

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
            game.ComputerChoice = DetermineComputerChoice();
            Outcome didPlayerWin = DetermineIfPlayerWon(game);

            // Might make sense to move this logic into DetermineIfHumanWon
            if (didPlayerWin is Outcome.Indeterminate)
            {
                game.GameResult = "Unable to determine the victory. Please try again.";
            }
            else if (didPlayerWin is Outcome.Draw)
            {
                game.GameResult = "A draw occurred. Try again!";
            }
            else if (didPlayerWin is Outcome.Win)
            {
                game.GameResult = "Congratulations, you won!";
            }
            else
            {
                game.GameResult = "Sorry, the computer won. Please try again!";
            }

            _PriorHumanChoices.Add((game.PlayerChoice, didPlayerWin));
            return game;
        }

        private static Option DetermineComputerChoice()
        {
            Option decision;
            if (_PriorHumanChoices.Count == 0)
            {
                decision = RandomPick();
            }
            else
            {
                decision = Decision();
            }

            return decision;
        }

        private static Outcome DetermineIfPlayerWon(Game game)
        {
            // Feels like this might be able to be done more efficiently

            switch (game.PlayerChoice)
            {
                case Option.Rock:
                    if (game.ComputerChoice is Option.Rock)
                        return Outcome.Draw;
                    if (game.ComputerChoice is Option.Scissors)
                        return Outcome.Win;
                    if (game.ComputerChoice is Option.Paper)
                        return Outcome.Lose;
                    break;
                case Option.Paper:
                    if (game.ComputerChoice is Option.Rock)
                        return Outcome.Win;
                    if (game.ComputerChoice is Option.Scissors)
                        return Outcome.Lose;
                    if (game.ComputerChoice is Option.Paper)
                        return Outcome.Draw;
                    break;
                case Option.Scissors:
                    if (game.ComputerChoice is Option.Rock)
                        return Outcome.Lose;
                    if (game.ComputerChoice is Option.Scissors)
                        return Outcome.Draw;
                    if (game.ComputerChoice is Option.Paper)
                        return Outcome.Win;
                    break;
                default:
                    break;
            }
            return Outcome.Indeterminate;
        }

        public static Option Decision()
        {
            var segments = new List<List<(Option PlayerPick, Outcome Outcome)>>();
            if (_PriorHumanChoices.Count >= _IterationsToAnalyzeForPattern)
            {
                for (int i = 0; i < _PriorHumanChoices.Count; i += _IterationsToAnalyzeForPattern)
                {
                    segments.Add(_PriorHumanChoices.GetRange(i, _IterationsToAnalyzeForPattern));
                }
            }
            else
            {
                segments.Add(_PriorHumanChoices.GetRange(0, _PriorHumanChoices.Count));
            }

            List<List<(Option PlayerPick, Outcome Outcome)>> trainingSegments = new();
            var tempList = _TrainingModel;
            foreach (var seg in segments)
            {
                for (int i = 0; i < seg.Count; i++)
                {
                    tempList.RemoveAll(m => m[i] != seg[i]);

                }
            }

            Option computerPick;
            // Basically, if our training model doesn't have enough data yet, default to random pick.
            // Probably not the best idea, but... here we are.
            if (tempList.Count == 0)
            {
                computerPick = RandomPick();
            }
            else if (tempList.Count == 1)
            {
                //return tempList[0]. ??? Parse the segment to determine winner.
                // E.G. If we're on game 2, pick the game 3 option?
                // IDEA: instead of breaking the player history and training model down into chunks,
                // Maybe just use this to match n number of matches, and once that data is obtained we can just pick whatever is next in the training model.
                // That *might* be good enough for demo purposes.
                computerPick = RandomPick();
            }
            else
            {
                // Randomly choose one of the segments?
                // Then if we're on game 2 of a series, pick the game 3 option?
                computerPick = RandomPick();
            }

            // Add prior human choice segments to training model here
            return RandomPick();
        }

        private static Option RandomPick()
        {
            int rand = new Random()
                .Next(3);
            return rand switch
            {
                0 => Option.Rock,
                1 => Option.Paper,
                2 => Option.Scissors,
                _ => Option.Rock,
            };
        }

        private static Option PickWinningOption(Option userSelection)
        {
            return userSelection switch
            {
                Option.Rock => Option.Paper,
                Option.Paper => Option.Scissors,
                Option.Scissors => Option.Rock,
                _ => Option.Invalid
            };
        }

        // Need to train the program with useful data. E.G.
        // R - W
        // R - L
        // S - W

        // R - W
        // R - L
        // S - L

        // S - W
        // etc.
        // THEN, on game 1, choose random
        // On game 2, compare the first choice with the training segments and pick ones that started the same. Then... randomly pick one?
        // On game 3, compare the first two choices with segments and pick ones, same as above.
        // After game 3, replace or add to the training model? Probably add.
    }
}
