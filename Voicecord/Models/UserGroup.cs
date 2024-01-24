using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class UserGroup : Interfaces.IModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LinkImageGroup { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Chat> Chats { get; set; }
        public List<VoiceChat> Voices { get; set; }
    }
}
