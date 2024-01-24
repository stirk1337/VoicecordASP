using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Voicecord.Models
{
    public class ApplicationUser : Interfaces.IModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<UserGroup> UserGroups { get; set; }
    }
}
