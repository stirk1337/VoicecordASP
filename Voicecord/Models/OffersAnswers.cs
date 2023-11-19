using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class OffersAnswers
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Offer { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
