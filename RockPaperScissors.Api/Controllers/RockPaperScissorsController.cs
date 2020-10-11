using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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
                PlayerChoice = RockPaperScissors.ValidatePlayerInput(choice)
            };
            if (formattedChoice.PlayerChoice is Option.Invalid)
            {
                formattedChoice.IsPlayerSelectionValid = false;
                formattedChoice.ErrorMessage = "Invalid input. Please choose 'Rock', 'Paper', or 'Scissors'.";
                return formattedChoice;
            }
            formattedChoice.IsPlayerSelectionValid = true;
            return formattedChoice;
        }

        // POST: api/rockpaperscissors/play
        [HttpPost("play")]
        public Game SendChoiceAsync([FromBody]Game content)
        {
            return RockPaperScissors.ProcessPlayerInput(content);
        }

        [HttpGet("memoryLeak")]
        public Game ExampleMemoryLeak()
        {
            SimulatedMemoryLeak();
            // Or should I just continually assign a large variable here to run myself out of memory?
            return new Game { };
        }

        private async Task SimulatedMemoryLeak()
        {
            while (true)
            {
                FakeLeak.Add(new byte[1024]);
                await Task.Delay(50);
            }
        }

        private static List<byte[]> FakeLeak { get; set; } = new List<byte[]>();
    }
}
