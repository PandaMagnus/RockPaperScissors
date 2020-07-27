using System.ComponentModel.DataAnnotations;

namespace BlazorServerApp.Models
{
    public class RockPaperScissorsModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Selection is too long.")]
        public string PlayerChoice { get; set; }
    }
}
