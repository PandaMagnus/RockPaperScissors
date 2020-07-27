using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RockPaperScissors.Api.Controllers
{
    [Route("api/rockpaperscissors")]
    [ApiController]
    public class RockPaperScissorsController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Welcome to the Rock Paper Scissors API!" };
        }

        // POST: api/rockpaperscissors/validate/<choice>
        [HttpPost("validate/{choice}")]
        public Game ValidateChoiceAsync(string choice)
        {
            var formattedChoice = new Game
            {
                PlayerChoice = RockPaperScissors.ValidateUserInput(choice)
            };
            if (formattedChoice.PlayerChoice is Option.Invalid)
            {
                return new Game
                {
                    IsPlayerSelectionValid = false,
                    ErrorMessage = "Invalid input. Please choose 'Rock', 'Paper', or 'Scissors'"
                };
            }
            formattedChoice.IsPlayerSelectionValid = true;
            return formattedChoice;
        }

        // POST: api/rockpaperscissors/play
        [HttpPost("play")]
        public Game SendChoiceAsync([FromBody]Game content)
        {
            return RockPaperScissors.ProcessUserInput(content);
        }
    }
}
