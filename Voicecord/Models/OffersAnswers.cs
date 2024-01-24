using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class OffersAnswers : Interfaces.IModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Offer { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
