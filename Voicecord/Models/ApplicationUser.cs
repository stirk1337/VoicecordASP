using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set;}
        public string Password { get; set;}
        public string Email { get; set;}
    }
}
