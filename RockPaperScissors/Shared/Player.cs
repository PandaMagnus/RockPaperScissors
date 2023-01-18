using System.ComponentModel.DataAnnotations;

namespace RockPaperScissors.Shared
{
    public class Player
    {
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Input must be between 1 and 10 characters.")]
        public string? PlayerInput { get; set; }
    }
}
