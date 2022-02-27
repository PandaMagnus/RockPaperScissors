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
        private static readonly int _IterationsToAnalyzeForPattern = 3;

        // Does IIS parallelize this correctly? Or does this need to be an instance?
        // Also, do we need to keep track of outcome?
        // Presumably the pattern is to guess the player's next choice, regardless of outcome from prior games.
        private static List<(Option PlayerPick, Outcome Outcome)> _PriorHumanChoices = new List<(Option, Outcome)>(); 
        private static List<List<(Option PlayerPick, Outcome Outcome)>> _SingleGameModel = new(); // Is this worth it? Seems like random chance might be most straighforward. UNLESS a player likes to play one option... A LOT. And then randomly picking from here would weight towards beating that.
        private static List<List<(Option PlayerPick, Outcome Outcome)>> _BestOfThreeModel = new();
        private static List<List<(Option PlayerPick, Outcome Outcome)>> _BestOfFiveModel = new();
        private static bool _SetComplete; // Does IIS parallelize this correctly? Or does this need to be an instance?

        // Probably extract these adds out for clarity
        // Also figure out how to configure this for games that last for other than 3 turns
        // Also find a better way to do this other than hard coding. Maybe play something like 1,000 random choice games and then replace as real games are played?
        // Or have three different training sets, one for single games, best of 3 and best of 5?
        // Or, hell, a combination... a dataset that mixes of best of 1, best of 3, best of 5, best of 7, etc.?
        static RockPaperScissors()
        {
            // Check the performance impact of Tuples. May be more performant to use a list of KeyValue pairs?
            // Although we'd have to assess how important performance is for this.
            // Also, does _TrainingModel need to be a List<List<>>? Seems like we could use something faster for the outer one like a HashSet or Dictionary if we don't duplicate game results.
            // OR do we ditch that altogether? Is there a better way to handle this that doesn't also require the number of games specified in a set?
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Lose),
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Draw),
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Rock, Outcome.Draw),
                    (Option.Paper, Outcome.Draw),
                    (Option.Scissors, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Lose),
                    (Option.Rock, Outcome.Win),
                    (Option.Rock, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Draw),
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Paper, Outcome.Draw),
                    (Option.Scissors, Outcome.Draw),
                    (Option.Rock, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Lose),
                    (Option.Paper, Outcome.Win),
                    (Option.Paper, Outcome.Win)
                }
            );
            _BestOfThreeModel.Add(
                new List<(Option PlayerPick, Outcome Outcome)>
                {
                    (Option.Scissors, Outcome.Draw),
                    (Option.Scissors, Outcome.Win),
                    (Option.Scissors, Outcome.Lose)
                }
            );
            _BestOfThreeModel.Add(
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

            // if(SetIsComplete)
            // {
            //    _PriorHumanChoices = new();
            // }
            _PriorHumanChoices.Add((game.PlayerChoice, didPlayerWin));
            //_BestOfThreeModel.Add((game.PlayerChoice, didPlayerWin));
            return game;
        }

        private static Option DetermineComputerChoice()
        {
            return Decision();
        }


        // Don't need this method. Just handle it as part of determining the computer choice.
        public static Option Decision()
        {
            Option computerPick = Option.Invalid;

            if (_PriorHumanChoices.Count is 0)
            {
                computerPick = RandomPick();
            }
            else
            {
                // if(game.BestOf == 3)
                List<List<(Option PlayerPick, Outcome Outcome)>> possibleSegments = new();
                possibleSegments.AddRange(_BestOfThreeModel.Where(m => m.Take(_PriorHumanChoices.Count).SequenceEqual(_PriorHumanChoices)).ToList());
                if(possibleSegments.Count is 0)
                {
                    // Does this make sense if there's not enough training data?
                    computerPick = RandomPick();
                }
                else
                {
                    // Would it be worth weighting *these* instead of random choice?
                    int rand = new Random().Next(0, possibleSegments.Count);
                    var guessNextPick = possibleSegments[rand][_PriorHumanChoices.Count];
                    computerPick = PickWinningOption(guessNextPick.PlayerPick);
                }
            }




            // Instead of all this, should we just append human choices to the training model?
            // Maybe drop one hard-coded one from the list?
            //var segments = new List<List<(Option PlayerPick, Outcome Outcome)>>();
            //if (_PriorHumanChoices.Count >= _IterationsToAnalyzeForPattern)
            //{
            //    for (int i = 0; i < _PriorHumanChoices.Count; i += _IterationsToAnalyzeForPattern)
            //    {
            //        // if _PriorHumanChoices.Count < index + _IterationsToAnalyzeForPattern then return
            //        segments.Add(_PriorHumanChoices.GetRange(i, _IterationsToAnalyzeForPattern));
            //    }
            //}
            //else
            //{
            //    segments.Add(_PriorHumanChoices.GetRange(0, _PriorHumanChoices.Count));
            //}

            //List<List<(Option PlayerPick, Outcome Outcome)>> trainingSegments = new();
            //foreach (var seg in segments)
            //{
            //    trainingSegments.AddRange(_BestOfThreeModel.Where(tm => tm.Take(seg.Count).SequenceEqual(seg)).ToList());
            //}

            
            //// Basically, if our training model doesn't have enough data yet, default to random pick.
            //// Probably not the best idea, but... here we are.
            //if (trainingSegments.Count == 0)
            //{
            //    computerPick = RandomPick();
            //}
            //else if (trainingSegments.Count == 1)
            //{
            //    //return tempList[0]. ??? Parse the segment to determine winner.
            //    // E.G. If we're on game 2, pick the game 3 option?
            //    // IDEA: instead of breaking the player history and training model down into chunks,
            //    // Maybe just use this to match n number of matches, and once that data is obtained we can just pick whatever is next in the training model.
            //    // That *might* be good enough for demo purposes.
            //    computerPick = trainingSegments[0][segments[0].Count + 1].PlayerPick;
            //    //computerPick = RandomPick();
            //    //var c = segments.Last().Count;
            //}
            //else
            //{
            //    // Randomly choose one of the segments?
            //    // Or calculate probability of winning based off all available training segments
            //    // Then if we're on game 2 of a series, pick the game 3 option?
            //    computerPick = RandomPick();
            //}

            // Add prior human choice segments to training model here
            return computerPick;
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
    }
}
