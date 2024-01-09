using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Voicecord.Models;

namespace Voicecord.ViewModels.Group
{
    public class CreateGroupViewModel
    {
        [Required(ErrorMessage = "Укажите название группы")]
        [MaxLength(20, ErrorMessage = "Название должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Название должно иметь длину больше 3 символов")]
        public string NameGroup { get; set; }

        [Required]
        public string CreatorId { get; set; }


        [Required(ErrorMessage = "Ссылка на сервер")]
        public string GroupLink { get; set; }
    }
}
