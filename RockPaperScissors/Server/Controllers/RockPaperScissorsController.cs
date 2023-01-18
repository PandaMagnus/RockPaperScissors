using RockPaperScissors.Shared;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Server;

namespace ExampleBlazorApp.Server.Controllers;

[ApiController]
[Route("api/rockpaperscissors")]
public class RockPaperScissorsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Welcome to the Rock Paper Scissors API!" };
    }

    [HttpPost("Validate/{choice}")]
    public Game ValidateChoiceAsync(string choice)
    {
        var formattedChoice = new Game
        {
            PlayerChoice = GameEngine.ValidatePlayerInput(choice)
        };
        if (formattedChoice.PlayerChoice is Option.Invalid)
        {
            formattedChoice.IsPlayerSelectionValid = false;
            formattedChoice.GameResult = "Invalid input. Please choose 'Rock', 'Paper', or 'Scissors'.";
            return formattedChoice;
        }
        formattedChoice.IsPlayerSelectionValid = true;
        return formattedChoice;
    }

    [HttpPost("play")]
    public Game SendChoiceAsync([FromBody] Game content)
    {
        return GameEngine.ProcessPlayerInput(content);
    }
}