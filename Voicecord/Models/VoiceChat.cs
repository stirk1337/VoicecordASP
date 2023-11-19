using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class VoiceChat
    {
        [Key]
        public int Id { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public string Offer {  get; set; }
        public string Answer { get; set; }
        public List<string> Answers { get; set; }
    }
}
