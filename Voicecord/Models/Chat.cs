using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
    }
}
