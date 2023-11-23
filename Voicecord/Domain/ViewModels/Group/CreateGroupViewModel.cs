using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Voicecord.Models;

namespace Voicecord.Domain.ViewModels.Group
{
    public class CreateGroupViewModel
    {
        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string NameGroup { get; set; }

        [Required]
        public string CreatorId { get; set; }


        [Required(ErrorMessage = "Ссылка на сервер")]
        public string GroupLink { get; set; }
    }
}
