using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors
{
    public static class ComputerOpponent
    {
        public static void CheckForPatternsAndAssignWeight()
        {
            var segments = new List<List<(Option UserPick, Outcome Outcome)>>();
            for (int i = 0; i < _PriorHumanChoices.Count; i += _IterationsToAnalyzeForPattern)
            {
                var group = new List<(Option UserChoice, Outcome Outcome)>();

                for (int j = 0; j < _IterationsToAnalyzeForPattern; j++)
                {
                    if (_PriorHumanChoices.Count <= i + j)
                    {
                        break;
                    }
                    group.Add((_PriorHumanChoices[i + j].UserPick, _PriorHumanChoices[i + j].Outcome));
                }

                segments.Add(group);
            }

            // Iterate over segments and add weights based on the chunks of 3
            // Maybe increase weights the closer to the most recent game?
            foreach (var group in segments)
            {
                foreach (var r in group)
                {
                    // calculate repeat usage
                    // calculate last victory
                    // calculate average usage
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

        public static Option Decision()
        {
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

            List<KeyValuePair<Option, int>> results = _OptionWeights.OrderByDescending(k => k.Value).ToList();

            if (results[0].Value == results[1].Value && results[1].Value == results[2].Value)
            {
                return RandomDecision();
            }

            return _OptionWeights.First().Key;

            // If no pattern, and count > some value OR the last play was a loss...
            //if (GetLastResult().outcome == Outcome.Lose)
            //{
            //    return PickExpectedWinner(PickExpectedWinner(GetLastResult().userPick));
            //}

            //return RandomDecision();
        }

        public static Option RandomDecision()
        {
            Random rand = new Random();
            var decision = rand.Next(3);
            switch (decision)
            {
                case 0:
                    return Option.Rock;
                case 1:
                    return Option.Paper;
                case 2:
                    return Option.Scissors;
                default:
                    return Option.Rock;
            }
        }

        public static void RecordOutcome(Option humanChoice, Outcome gameResult)
        {
            _PriorHumanChoices.Add((humanChoice, gameResult));
        }

        // Find out a way to assess if it makes sense to restart the weighting every iteration
        private static Dictionary<Option, int> _OptionWeights { get; set; } = new Dictionary<Option, int> { { Option.Rock, 0 }, { Option.Paper, 0 }, { Option.Scissors, 0 } };
        private static readonly int _IterationsToAnalyzeForPattern = 3;
        private static List<(Option UserPick, Outcome Outcome)> _PriorHumanChoices { get; set; } = new List<(Option, Outcome)>();
    }
}
