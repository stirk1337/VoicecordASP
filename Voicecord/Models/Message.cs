using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class Message : Interfaces.IModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public ApplicationUser Owner { get; set; }
        public string TextMessage { get; set; }
        public DateTime Date { get; set; }
    }
}
