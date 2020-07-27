using System.Text.Json.Serialization;

namespace RockPaperScissors.Api.Models
{
    public class Game
    {
        [JsonPropertyName("isPlayerSelectionValid")]
        public bool IsPlayerSelectionValid { get; set; }
        [JsonPropertyName("playerChoice")]
        public Option PlayerChoice { get; set; }
        [JsonPropertyName("computerChoice")]
        public Option ComputerChoice { get; set; }
        [JsonPropertyName("gameResult")]
        public string GameResult { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
